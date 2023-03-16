using Domain.Entities;

namespace Infrastructure.Repositories.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAsync();
    Task<TEntity> GetByIdAsync(int id);
    Task<TEntity> CreateAsync(TEntity entity);
    TEntity Update(TEntity entity);
    Task<TEntity> DeleteAsync(int id);
}