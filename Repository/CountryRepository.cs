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

    public async Task<bool> Exists(Country country)
    {
        return await CountryNameExists(country.CountryName)
            || country.Id == 0 ? false 
            : await Get(country.Id) is not null;
    }

    public async Task<Country> Get(int id)
    {
        return await _context.Countries.FirstOrDefaultAsync(ct => ct.Id == id);
    }

    public async Task<IEnumerable<Country>> GetAll()
    {
        return await _context.Countries.ToListAsync();
    }

    public async Task Update(Country country)
    {
        _context.Attach(country);
        _context.Entry(country).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    private async Task<bool> CountryNameExists(string countryName) => await _context.Countries.AnyAsync(c => c.CountryName.Equals(countryName, StringComparison.OrdinalIgnoreCase));

}
