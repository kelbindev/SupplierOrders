namespace Contracts.Repository;
public interface IDataRepository<TEntity>
{
    Task<IEnumerable<TEntity>> GetAll(bool trackChanges = false);
    Task<TEntity> Get(int id, bool trackChanges = false);
    Task Add(TEntity entity);
    Task Update(TEntity entity);
    Task Delete(TEntity entity);
    Task<bool> Exists(TEntity entity, bool trackChanges = false);
}
