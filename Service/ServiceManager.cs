using Contracts.Repository;
using Service.Contracts;

namespace Service;
public class ServiceManager(IRepositoryManager repository) : IServiceManager
{
    private readonly Lazy<ISupplierService> _supplier = new(() => new SupplierService(repository.Supplier));
    private readonly Lazy<ICountryService> _country = new(() => new CountryService(repository.Country));

    public ISupplierService Supplier => _supplier.Value;
    public ICountryService Country => _country.Value;
}
