using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TheCollabSys.Backend.Data.Interfaces;

namespace TheCollabSys.Backend.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly TheCollabsysContext _context;
    public Repository(TheCollabsysContext context)
    {
        _context = context;
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity> GetByIdAsync(string id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public virtual IQueryable<TEntity> GetAllQueryable()
    {
        return _context.Set<TEntity>().AsQueryable();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }
    public async Task<IEnumerable<TResult>> GetAllProjectedAsync<TResult>(Expression<Func<TEntity, TResult>> selector)
    {
        return await _context.Set<TEntity>()
            .AsNoTracking()
            .Select(selector)
            .ToListAsync();
    }

    public IAsyncEnumerable<TResult> GetAllProjectedAsAsyncEnumerable<TResult>(Expression<Func<TEntity, TResult>> selector)
    {
        return _context.Set<TEntity>()
            .AsNoTracking()
            .Select(selector)
            .AsAsyncEnumerable();
    }
    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    public void Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }
}
