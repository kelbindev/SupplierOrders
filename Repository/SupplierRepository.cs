using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Context;

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

    public async Task<bool> Exists(Supplier supplier)
    {
        return await SupplierNameExists(supplier.SupplierName)
        || supplier.Id == 0 ? false
            : await Get(supplier.Id) is not null; ;
    }

    public async Task<Supplier> Get(int id)
    {
        return await _context.Suppliers.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Supplier>> GetAll()
    {
        return await _context.Suppliers.ToListAsync();
    }

    public async Task Update(Supplier supplier)
    {
        _context.Attach(supplier);
        _context.Entry(supplier).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    private async Task<bool> SupplierNameExists(string supplierName) => await _context.Suppliers.AnyAsync(s => s.SupplierName.Equals(supplierName, StringComparison.CurrentCultureIgnoreCase));
}

