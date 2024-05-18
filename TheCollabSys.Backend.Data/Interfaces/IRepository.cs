using System.Linq.Expressions;

namespace TheCollabSys.Backend.Data.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(int id);
    Task<TEntity> GetByIdAsync(string id);
    IQueryable<TEntity> GetAllQueryable();
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TResult>> GetAllProjectedAsync<TResult>(Expression<Func<TEntity, TResult>> selector);
    IAsyncEnumerable<TResult> GetAllProjectedAsAsyncEnumerable<TResult>(Expression<Func<TEntity, TResult>> selector);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
