using Domain.Entities;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class GenericRepository<TDbContext, TEntity> : IBaseRepository<TEntity>
    where TDbContext : DbContext
    where TEntity : BaseEntity
{
    private TDbContext Context { get; }
    private DbSet<TEntity> Entities { get; }

    public GenericRepository(TDbContext context)
    {
        Context = context;
        Entities = Context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAsync()
    {
        return await Entities.ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        var entry = await Entities.SingleOrDefaultAsync(x => x.Id == id);
        
        if (entry is null)
        {
            throw new KeyNotFoundException();
        }

        return entry;
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await Entities.AddAsync(entity);
        return entity;
    }

    public TEntity Update(TEntity entity)
    {
        Context.Update(entity);
        return entity;
    }

    public async Task<TEntity> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        Delete(entity);
        return entity;
    }

    private TEntity Delete(TEntity entity)
    {
        Context.Remove(entity);
        return entity;
    }
}