using Contracts.Repository;
using Repository.Context;

namespace Repository;
public class RepositoryManager(SupplierOrdersContext context) : IRepositoryManager
{
    private readonly Lazy<ISupplierRepository> _supplier = new(() => new SupplierRepository(context));
    private readonly Lazy<ICountryRepository> _country = new(() => new CountryRepository(context));
    private readonly Lazy<IUserRepository> _user = new(() => new UserRepository(context));

    public ISupplierRepository Supplier => _supplier.Value;
    public ICountryRepository Country => _country.Value;
    public IUserRepository User => _user.Value;
}
