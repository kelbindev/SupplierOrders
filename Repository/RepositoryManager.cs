using Contracts.Repository;
using Repository.Context;

namespace Repository;
public class RepositoryManager(SupplierOrdersContext context) : IRepositoryManager
{
    private readonly Lazy<ISupplierRepository> _supplier = new(() => new SupplierRepository(context));
    private readonly Lazy<ICountryRepository> _country = new(() => new CountryRepository(context));

    public ISupplierRepository Supplier => _supplier.Value;
    public ICountryRepository Country => _country.Value;
}
