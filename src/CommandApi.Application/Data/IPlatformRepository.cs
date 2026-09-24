namespace CommandApi.Application.Data;

public interface IPlatformRepository
{
    Task<bool> SaveChangesAsync();

    Task<PaginatedList<Platform>> GetAllAsync(PaginationParams paginationParams);

    Task<Platform?> GetByIdAsync(int id);

    Task CreateAsync(Platform platform);

    Task UpdateAsync(Platform platform);

    Task DeleteAsync(int id);
}