using CommandApi.Domain;

namespace CommandApi.Infrastructure.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
 DbContext(options), IApplicationDbContext
{
    public DbSet<Platform> Platforms => Set<Platform>();

    public DbSet<Command> Commands => Set<Command>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is ICreatedAtTrackable
                     && e.State == EntityState.Added);

        foreach (var entry in entries)
        {
            ((ICreatedAtTrackable)entry.Entity).CreatedAt = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}