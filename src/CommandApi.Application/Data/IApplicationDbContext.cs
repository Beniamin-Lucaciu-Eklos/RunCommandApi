namespace CommandApi.Application.Data;

public interface IApplicationDbContext
{
    DbSet<Platform> Platforms { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}