namespace CommandApi.Application.Data;

public interface IApplicationDbContext
{
    DbSet<Platform> Platforms { get; }

    DbSet<Command> Commands { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}