using Entities;
using Shared.Pagination;

namespace Service.Contracts;

public interface ISupplierService : IDataService<Supplier>
{
    Task<PagedList<Supplier>> GetAllPaged(SupplierRequestParameter param, bool trackChanges = false);
}