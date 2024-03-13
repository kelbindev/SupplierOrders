namespace Contracts.Repository;
public interface IRepositoryManager
{
    ISupplierRepository Supplier {  get; }
    ICountryRepository Country { get; }
    IUserRepository User { get; }
    IUserRefreshTokenRepository UserRefreshToken { get; }
}
