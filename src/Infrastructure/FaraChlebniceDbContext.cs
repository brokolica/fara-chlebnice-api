using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class FaraChlebniceDbContext : DbContext
{
    public virtual DbSet<Announcement> Announcements { get; set; }

    public FaraChlebniceDbContext(DbContextOptions<FaraChlebniceDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.LogTo(Console.WriteLine).EnableSensitiveDataLogging();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.HasDefaultSchema("fch");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FaraChlebniceDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}