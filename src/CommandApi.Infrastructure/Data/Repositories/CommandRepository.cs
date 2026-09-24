using CommandApi.Application.Dtos;

namespace CommandApi.Infrastructure.Data.Repositories;

public class CommandRepository(IApplicationDbContext dbContext) : ICommandRepository
{
    public async Task<PaginatedList<Command>> GetAllAsync(PaginationParams paginationParams)
    {
        var commands = await dbContext.Commands.AsNoTracking()
           .OrderBy(c => c.Id)
           .Skip((paginationParams.PageIndex - 1) * paginationParams.PageSize)
           .Take(paginationParams.PageSize)
           .ToListAsync();

        var count = await dbContext.Commands.CountAsync();
        return new PaginatedList<Command>(commands, count, paginationParams.PageIndex, paginationParams.PageSize);
    }

    public async Task<PaginatedList<Command>> GetAllByPlatformIdAsync(int platformId, PaginationParams paginationParams)
    {
        var commands = await dbContext.Commands.AsNoTracking()
             .Where(x => x.PlatformId == platformId)
             .OrderBy(c => c.Id)
             .Skip((paginationParams.PageIndex - 1) * paginationParams.PageSize)
             .Take(paginationParams.PageSize)
             .ToListAsync();

        var count = await dbContext.Commands.AsNoTracking()
            .CountAsync(x => x.PlatformId == platformId);

        return new PaginatedList<Command>(commands, count, paginationParams.PageIndex, paginationParams.PageSize);
    }


    public async Task<Command?> GetByIdAsync(int id)
    {
        return await dbContext.Commands.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task CreateAsync(Command command)
    {
        ArgumentNullException.ThrowIfNull(command);

        await dbContext.Commands.AddAsync(command);
    }

    public async Task DeleteAsync(int id)
    {
        await dbContext.Commands
          .Where(p => p.Id == id)
          .ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(Command command)
    {
        await dbContext.Commands
         .Where(p => p.Id == command.Id)
         .ExecuteUpdateAsync(setProp =>
         {
             setProp.SetProperty(x => x.CommandLine, command.CommandLine);
             setProp.SetProperty(x => x.CreatedAt, command.CreatedAt);
             setProp.SetProperty(x => x.HowTo, command.HowTo);
             //setProp.SetProperty(x => x.PlatformId, command.PlatformId);
             //setProp.SetProperty(x=>x.CreatedAt, platform.CreatedAt);
         });
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync() >= 0;
    }
}