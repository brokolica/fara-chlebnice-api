using Domain.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories;

public sealed class UnitOfWork : IDisposable
{
    private readonly FaraChlebniceDbContext _dbContext;

    private IBaseRepository<Announcement>? _announcements;

    public UnitOfWork(FaraChlebniceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IBaseRepository<Announcement> AnnouncementsRepository => _announcements ??= new GenericRepository<FaraChlebniceDbContext, Announcement>(_dbContext);

    public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();

    private bool _disposed = false;

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}