using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Shared.Pagination;

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
        var s = await Get(supplier.Id, trackChanges);
        return s is not null;
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

    public async Task<PagedList<Supplier>> GetAllPaged(SupplierRequestParameter param, bool trackChanges = false)
    {
        string globalSearchValue = param.search.value is null ? string.Empty : param.search.value.ToLower();

        var query = _context.Suppliers.Include(s => s.Country)
            //for column search
            .Where(s => string.IsNullOrWhiteSpace(param.SearchSupplierName) || s.SupplierName.ToLower().Contains(param.SearchSupplierName.ToLower()))
            .Where(s => string.IsNullOrWhiteSpace(param.SearchSupplierEmail) || s.SupplierEmail.ToLower().Contains(param.SearchSupplierEmail.ToLower()))
            .Where(s => string.IsNullOrWhiteSpace(param.SearchCountry) || s.Country.CountryCodeAndName.ToLower().Contains(param.SearchCountry.ToLower()));

        //for global search
        if (!string.IsNullOrWhiteSpace(param.search.value)) {
            query = query
            .Where(s => s.SupplierName.Contains(globalSearchValue) 
                        || s.SupplierEmail.Contains(globalSearchValue) 
                        || s.Country.CountryName.Contains(globalSearchValue) 
                        || s.Country.CountryCode.Contains(globalSearchValue));
        }

        var count = await query.CountAsync();

        var resultQuery = query
            .OrderBy(s => s.Id)
            .Skip((param.PageNumber - 1) * param.PageSize)
            .Take(param.PageSize);

        if (!trackChanges) resultQuery = resultQuery.AsNoTracking();

        var content = await resultQuery.ToListAsync();

        return new PagedList<Supplier>(content, count, param.PageNumber, param.PageSize);
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
}