namespace CommandApi.Application.Data;

public interface ICommandRepository
{
    Task<bool> SaveChangesAsync();

    Task<PaginatedList<Command>> GetAllAsync(PaginationParams paginationParams);
    Task<Command?> GetByIdAsync(int id);
    Task<PaginatedList<Command>> GetAllByPlatformIdAsync(int platformId, PaginationParams paginationParams);
    Task CreateAsync(Command command);
    Task UpdateAsync(Command command);
    Task DeleteAsync(int id);
}