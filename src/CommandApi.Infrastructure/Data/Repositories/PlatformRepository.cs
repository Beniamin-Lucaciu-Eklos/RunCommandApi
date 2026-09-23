namespace CommandApi.Infrastructure.Data.Repositories;

public class PlatformRepository(IApplicationDbContext dbContext) : IPlatformRepository
{
    public async Task<IEnumerable<Platform>> GetAllAsync()
    {
        var platforms = await dbContext.Platforms.AsNoTracking()
            .ToListAsync();
        return platforms;
    }

    public async Task<Platform?> GetByIdAsync(int id)
    {
        return await dbContext.Platforms.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task CreateAsync(Platform platform)
    {
        ArgumentNullException.ThrowIfNull(platform);

        await dbContext.Platforms.AddAsync(platform);
    }

    public async Task DeleteAsync(int id)
    {
        await dbContext.Platforms
          .Where(p => p.Id == id)
          .ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(Platform platform)
    {
        await dbContext.Platforms
         .Where(p => p.Id == platform.Id)
         .ExecuteUpdateAsync(setProp =>
         {
             setProp.SetProperty(x => x.PlatformName, platform.PlatformName);
             //setProp.SetProperty(x=>x.CreatedAt, platform.CreatedAt);
         });
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync() >= 0;
    }
}