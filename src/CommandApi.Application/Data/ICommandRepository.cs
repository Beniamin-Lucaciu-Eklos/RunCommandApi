namespace CommandApi.Application.Data;

public interface ICommandRepository
{
    Task<bool> SaveChangesAsync();

    Task<IEnumerable<Command>> GetAllAsync();
    Task<Command?> GetByIdAsync(int id);
    Task<IEnumerable<Command>> GetAllByPlatformIdAsync(int platformId);
    Task CreateAsync(Command command);
    Task UpdateAsync(Command command);
    Task DeleteAsync(int id);
}