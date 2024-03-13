using Entities;
using Shared.Pagination;

namespace Contracts.Repository;
public interface ISupplierRepository : IDataRepository<Supplier>
{
    Task<PagedList<Supplier>> GetAllPaged(SupplierRequestParameter param,bool trackChanges = false);
    Task<List<Supplier>> GetAllPagedExportToExcel(SupplierRequestParameter param);
}
