using Entities;

namespace Contracts.Repository;
public interface IUserRefreshTokenRepository : IDataRepository<UserRefreshToken>
{
    Task<UserRefreshToken> GetByUserIdAndRefreshToken(int userId, string refreshToken, bool trackChanges = false);
}
