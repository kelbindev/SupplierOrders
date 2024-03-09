namespace Contracts.Repository;
public interface IDataRepository<TEntity>
{
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity> Get(int id);
    Task Add(TEntity entity);
    Task Update(TEntity entity);
    Task Delete(TEntity entity);
    Task<bool> Exists(TEntity entity);
}
