namespace TheCollabSys.Backend.Data.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(int id);
    Task<TEntity> GetByIdAsync(string id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
