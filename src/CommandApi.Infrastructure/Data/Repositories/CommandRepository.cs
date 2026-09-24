using System.Linq.Expressions;
using CommandApi.Application.Dtos;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CommandApi.Infrastructure.Data.Repositories;

public class CommandRepository(IApplicationDbContext dbContext) : ICommandRepository
{
    public async Task<PaginatedList<Command>> GetAllAsync(
        PaginationParams paginationParams,
        FilteringParams filteringParams,
        SortingParams sortingParams)
    {
        var query = dbContext.Commands.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filteringParams?.SearchBy))
        {
            string? searchBy = filteringParams?.SearchBy?.ToLower();
            query = query.Where(c => c.HowTo.ToLower().Contains(searchBy)
                             || c.CommandLine.ToLower().Contains(searchBy));
        }

        if (sortingParams is not null)
        {
            query = ApplySorting(query, sortingParams.desceding, sortingParams.SortBy);
        }

        var commands = await query.AsNoTracking()
           .Skip((paginationParams.PageIndex - 1) * paginationParams.PageSize)
           .Take(paginationParams.PageSize)
           .ToListAsync();

        var count = await query.CountAsync();
        return new PaginatedList<Command>(commands, count, paginationParams.PageIndex, paginationParams.PageSize);
    }

    private IQueryable<Command> ApplySorting(
        IQueryable<Command> query,
        bool descending,
        string? sortBy) => sortBy?.ToLower() switch
        {
            "howto" => descending ? query.OrderByDescending(c => c.HowTo)
                                  : query.OrderBy(c => c.HowTo),

            "commandline" => descending ? query.OrderByDescending(c => c.CommandLine)
                                        : query.OrderBy(c => c.CommandLine),

            "createdat" => descending ? query.OrderByDescending(c => c.CreatedAt)
                                      : query.OrderBy(c => c.CreatedAt),

            "platformid" => descending ? query.OrderByDescending(c => c.PlatformId)
                                      : query.OrderBy(c => c.PlatformId),

            _ => descending ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id)
        };

    public async Task<PaginatedList<Command>> GetAllByPlatformIdAsync(
        int platformId,
        PaginationParams paginationParams,
        FilteringParams filteringParams,
        SortingParams sortingParams)
    {
        var query = dbContext.Commands.Where(x => x.PlatformId == platformId);

        if (!string.IsNullOrWhiteSpace(filteringParams?.SearchBy))
        {
            string? searchBy = filteringParams?.SearchBy?.ToLower();
            query = query.Where(c => c.HowTo.ToLower().Contains(searchBy)
                             || c.CommandLine.ToLower().Contains(searchBy));
        }

        if (sortingParams is not null)
        {
            query = ApplySorting(query, sortingParams.desceding, sortingParams.SortBy);
        }

        var commands = await query.AsNoTracking()
             .Skip((paginationParams.PageIndex - 1) * paginationParams.PageSize)
             .Take(paginationParams.PageSize)
             .ToListAsync();

        var count = await query.AsNoTracking()
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