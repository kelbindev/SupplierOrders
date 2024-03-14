using Contracts.Repository;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Context;

namespace Repository;
internal sealed class UserRefreshTokenRepository(SupplierOrdersContext context) : IUserRefreshTokenRepository
{
    private readonly SupplierOrdersContext _context = context;
    public async Task Add(UserRefreshToken userToken)
    {
        await _context.UsersRefreshToken.AddAsync(userToken);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(UserRefreshToken userToken)
    {
        _context.UsersRefreshToken.Remove(userToken);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Exists(UserRefreshToken userToken, bool trackChanges = false)
    {
        return await _context.UsersRefreshToken.AnyAsync(ut => ut.UserId == userToken.UserId && ut.RefreshToken == userToken.RefreshToken);
    }

    public async Task<UserRefreshToken> Get(int userId, bool trackChanges = false)
    {
        return trackChanges ? await _context.UsersRefreshToken.FirstOrDefaultAsync(ut => ut.UserId == userId)
            : await _context.UsersRefreshToken.AsNoTracking().FirstOrDefaultAsync(ut => ut.UserId == userId);
    }

    public async Task<UserRefreshToken> GetByUserIdAndRefreshToken(int userId, string refreshToken, bool trackChanges = false)
    {
        return trackChanges ? await _context.UsersRefreshToken.FirstOrDefaultAsync(ut => ut.UserId == userId && ut.RefreshToken == refreshToken)
            : await _context.UsersRefreshToken.AsNoTracking().FirstOrDefaultAsync(ut => ut.UserId == userId && ut.RefreshToken == refreshToken);
    }



    public async Task<IEnumerable<UserRefreshToken>> GetAll(bool trackChanges = false)
    {
        return trackChanges ? await _context.UsersRefreshToken.ToListAsync()
           : await _context.UsersRefreshToken.AsNoTracking().ToListAsync();
    }

    public async Task Update(UserRefreshToken userToken)
    {
        var existing = await Get(userToken.Id, true);

        existing.RefreshToken = userToken.RefreshToken;
        existing.RefreshTokenExpiry = userToken.RefreshTokenExpiry;

        _context.Entry(existing).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }
}
