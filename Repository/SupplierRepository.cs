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