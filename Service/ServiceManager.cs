using Contracts.Repository;
using Entities;
using Microsoft.Extensions.Options;
using Service.Contracts;

namespace Service;
public class ServiceManager(IRepositoryManager repository, IOptions<AppSettings> appSettings) : IServiceManager
{
    private readonly Lazy<ISupplierService> _supplier = new(() => new SupplierService(repository.Supplier));
    private readonly Lazy<ICountryService> _country = new(() => new CountryService(repository.Country));
    private readonly Lazy<IUserService> _user = new(() => new UserService(repository.User, repository.UserRefreshToken, appSettings));

    public ISupplierService Supplier => _supplier.Value;
    public ICountryService Country => _country.Value;
    public IUserService User => _user.Value;
}
