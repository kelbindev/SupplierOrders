using Shared;

namespace Service.Contracts;
public interface IDataService<TEntity>
{
    Task<ApiResponse> GetAll();
    Task<ApiResponse> Get(int id);
    Task<ApiResponse> Add(TEntity entity);
    Task<ApiResponse> Update(TEntity entity);
    Task<ApiResponse> Delete(TEntity entity);
}
