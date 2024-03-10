using Shared;

namespace Service.Contracts;
public interface IDataService<TEntity>
{
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity> Get(int id);
    Task<ApiResponse> Add(TEntity entity);
    Task<ApiResponse> Update(TEntity entity);
    Task<ApiResponse> Delete(TEntity entity);
}
