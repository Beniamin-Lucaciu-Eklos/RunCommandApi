namespace CommandApi.Application.Data;

public interface ICommandRepository
{
    Task<bool> SaveChangesAsync();

    Task<PaginatedList<Command>> GetAllAsync(
        PaginationParams paginationParams,
        FilteringParams filteringParams,
        SortingParams sortingParams);
    Task<Command?> GetByIdAsync(int id);
    Task<PaginatedList<Command>> GetAllByPlatformIdAsync(
        int platformId,
        PaginationParams paginationParams,
        FilteringParams filteringParams,
        SortingParams sortingParams);
    Task CreateAsync(Command command);
    Task UpdateAsync(Command command);
    Task DeleteAsync(int id);
}