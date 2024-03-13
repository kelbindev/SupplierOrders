using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Context;

namespace Contracts.Repository;
internal sealed class CountryRepository(SupplierOrdersContext context) : ICountryRepository
{
    readonly SupplierOrdersContext _context = context;

    public async Task Add(Country country)
    {
        await _context.AddAsync(country);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Country country)
    {
        _context.Remove(country);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Exists(Country country, bool trackChanges = false)
    {
        return await Get(country.Id, trackChanges) is not null || await _context.Countries.AnyAsync(c => c.CountryName == country.CountryName);
    }

    public async Task<Country> Get(int id, bool trackChanges = false)
    {
        return 
            trackChanges ? await _context.Countries.FirstOrDefaultAsync(ct => ct.Id == id)
            : await _context.Countries.AsNoTracking().FirstOrDefaultAsync(ct => ct.Id == id);
    }

    public async Task<IEnumerable<Country>> GetAll(bool trackChanges = false)
    {
        return
            trackChanges ? await _context.Countries.ToListAsync()
            : await _context.Countries.AsNoTracking().ToListAsync();
    }

    public async Task Update(Country updatedCountry)
    {
        var existing = await Get(updatedCountry.Id, true);

        existing.CountryCode = updatedCountry.CountryCode;
        existing.CountryName = updatedCountry.CountryName;

        _context.Entry(existing).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }
}
