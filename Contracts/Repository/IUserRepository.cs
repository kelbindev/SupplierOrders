using Entities;

namespace Contracts.Repository;
public interface IUserRepository : IDataRepository<User>
{
    Task<bool> UserNameInUse(string userName);
    Task<bool> UserEmailInUse(string userEmail);
    Task<User> GetByUserName(string userName, bool trackChanges = false);
    Task<User> GetByEmail(string userEmail, bool trackChanges = false);
}
