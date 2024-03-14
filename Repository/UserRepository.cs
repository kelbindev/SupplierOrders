using Contracts.Repository;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Context;

namespace Repository;
internal sealed class UserRepository(SupplierOrdersContext context) : IUserRepository
{
    private readonly SupplierOrdersContext _context = context;
    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(User user)
    {
       _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Exists(User user, bool trackChanges = false)
    {
        return await Get(user.UserId, trackChanges) is not null
           || await _context.Users.AnyAsync(s => s.UserName.ToLower() == user.UserName);
    }

    public async Task<bool> UserNameInUse(string userName)
    {
        return await _context.Users.AnyAsync(s => s.UserName.ToLower() == userName.ToLower());
    }

    public async Task<bool> UserEmailInUse(string userEmail)
    {
        return await _context.Users.AnyAsync(s => s.UserEmail.ToLower() == userEmail.ToLower());
    }

    public async Task<User> Get(int id, bool trackChanges = false)
    {
        return
           trackChanges ? await _context.Users.FirstOrDefaultAsync(x => x.UserId == id)
           : await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == id);
    }

    public async Task<User> GetByUserName(string userName, bool trackChanges = false)
    {
        return
           trackChanges ? await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName)
           : await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserName == userName);
    }

    public async Task<User> GetByEmail(string userEmail, bool trackChanges = false)
    {
        return
           trackChanges ? await _context.Users.FirstOrDefaultAsync(x => x.UserEmail == userEmail)
           : await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserEmail == userEmail);
    }

    public async Task<IEnumerable<User>> GetAll(bool trackChanges = false)
    {
        return
          trackChanges ? await _context.Users.ToListAsync()
          : await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task Update(User user)
    {
        var existing = await Get(user.UserId, true);

        existing.UserEmail = user.UserEmail;
        existing.UserName = user.UserName;
        existing.PasswordSalt = user.PasswordSalt;
        existing.Password = user.Password;
        existing.UpdatedBy = user.UpdatedBy;
        existing.UpdatedDate = user.UpdatedDate;

        _context.Entry(existing).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }
}
