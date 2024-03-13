using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Shared.Pagination;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace Contracts.Repository;
internal sealed class SupplierRepository(SupplierOrdersContext context) : ISupplierRepository
{
    readonly SupplierOrdersContext _context = context;

    public async Task Add(Supplier supplier)
    {
        await _context.Suppliers.AddAsync(supplier);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Supplier supplier)
    {
        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Exists(Supplier supplier, bool trackChanges = false)
    {
        return await Get(supplier.Id, trackChanges) is not null
            || await _context.Suppliers.AnyAsync(s => s.SupplierName.ToLower() == supplier.SupplierName);
    }

    public async Task<bool> SupplierNameAlreadyExists(Supplier supplier)
    {
        return await _context.Suppliers.AnyAsync(s => s.SupplierName.ToLower() == supplier.SupplierName.ToLower() && s.Id != supplier.Id);
    }

    public async Task<Supplier> Get(int id, bool trackChanges = false)
    {
        return
            trackChanges ? await _context.Suppliers.Include(s => s.Country).FirstOrDefaultAsync(x => x.Id == id)
            : await _context.Suppliers.Include(s => s.Country).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Supplier>> GetAll(bool trackChanges = false)
    {
        return
            trackChanges ? await _context.Suppliers.Include(s => s.Country).ToListAsync()
            : await _context.Suppliers.Include(s => s.Country).AsNoTracking().ToListAsync();
    }

    public async Task Update(Supplier updatedSupplier)
    {
        var existing = await Get(updatedSupplier.Id, true);

        existing.SupplierEmail = updatedSupplier.SupplierEmail;
        existing.SupplierName = updatedSupplier.SupplierName;
        existing.UpdatedBy = updatedSupplier.UpdatedBy;
        existing.UpdatedDate = updatedSupplier.UpdatedDate;
        existing.CountryId = updatedSupplier.CountryId;

        _context.Entry(existing).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    public async Task<PagedList<Supplier>> GetAllPaged(SupplierRequestParameter param, bool trackChanges = false)
    {
        string globalSearchValue = param.search.value is null ? string.Empty : param.search.value.ToLower();

        var query = _context.Suppliers.Where(_ => 1 == 1);

        query = GenerateColumnWhereCondition(query, param);

        query = GenerateGlobalSearchWhereCondition(query, globalSearchValue);

        string orderByQuery = GenerateOrderByCondition(param.order);

        var count = await query.CountAsync();

        var resultQuery = query
            .Include(s => s.Country)
            .OrderBy(orderByQuery)
            .Skip((param.PageNumber - 1) * param.PageSize)
            .Take(param.PageSize);

        if (!trackChanges) resultQuery = resultQuery.AsNoTracking();

        var content = await resultQuery.ToListAsync();

        return new PagedList<Supplier>(content, count, param.PageNumber, param.PageSize);
    }

    public async Task<List<Supplier>> GetAllPagedExportToExcel(SupplierRequestParameter param)
    {
        string globalSearchValue = param.search.value is null ? string.Empty : param.search.value.ToLower();

        var query = _context.Suppliers.Include(s => s.Country).Where(s => s.Id == s.Id);

        query = GenerateColumnWhereCondition(query, param);

        query = GenerateGlobalSearchWhereCondition(query, globalSearchValue);

        return await query.AsNoTracking().ToListAsync();
    }


    private static IQueryable<Supplier> GenerateColumnWhereCondition(IQueryable<Supplier> query, SupplierRequestParameter param)
    {
        return query.Where(s => string.IsNullOrWhiteSpace(param.SearchSupplierName) || s.SupplierName.ToLower().Contains(param.SearchSupplierName.ToLower()))
            .Where(s => string.IsNullOrWhiteSpace(param.SearchSupplierEmail) || s.SupplierEmail.ToLower().Contains(param.SearchSupplierEmail.ToLower()))
            .Where(s => string.IsNullOrWhiteSpace(param.SearchCountry) || s.Country.CountryCode.ToLower().Contains(param.SearchCountry.ToLower()))
            .Where(s => string.IsNullOrWhiteSpace(param.SearchCountry) || s.Country.CountryName.ToLower().Contains(param.SearchCountry.ToLower()));
    }

    private static IQueryable<Supplier> GenerateGlobalSearchWhereCondition(IQueryable<Supplier> query, string globalSearchValue)
    {
        if (string.IsNullOrWhiteSpace(globalSearchValue)) return query;

        return query.Where(s => s.SupplierName.Contains(globalSearchValue)
                    || s.SupplierEmail.Contains(globalSearchValue)
                    || s.Country.CountryName.Contains(globalSearchValue)
                    || s.Country.CountryCode.Contains(globalSearchValue));
    }

    private static string GenerateOrderByCondition(IEnumerable<Order> orders)
    {
        var orderByQueryBuilder = new StringBuilder();

        var propertyInfos = typeof(Supplier).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var columnMappings = new Dictionary<int, string> {
            { 0, "Id" },
            { 1, "SupplierName" },
            { 2, "SupplierEmail" },
            { 4, "CountryName" },
            { 5, "UpdatedDate" },
            { 6, "UpdatedBy" }
        };

        foreach (var order in orders)
        {
            if (!columnMappings.TryGetValue(order.column, out var colName))
            {
                continue;
            }

            var objectProperty = Array.Find(propertyInfos, pi => pi.Name.Equals(colName, StringComparison.InvariantCultureIgnoreCase));

            if (objectProperty is null) continue;

            var direction = order.dir.Equals("asc", StringComparison.OrdinalIgnoreCase) ? "ascending" : "descending";

            orderByQueryBuilder.Append($"{objectProperty.Name} {direction}, ");
        }

        var orderByQuery = orderByQueryBuilder.ToString().TrimEnd(',', ' ');

        return string.IsNullOrWhiteSpace(orderByQuery) ? "Id ascending" : orderByQuery;
    }
}