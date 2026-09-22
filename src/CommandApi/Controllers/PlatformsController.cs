using CommandApi.Application.Dtos;
using CommandApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetAllAsync()
    {
        var platforms = await dbContext.Platforms.ToListAsync();
        var platformDtos = platforms.Select(p => new PlatformReadDto(p.Id, p.PlatformName, p.CreatedAt));

        return Ok(platformDtos);
    }

    [HttpGet("{id}", Name = "GetById")]
    public async Task<ActionResult<PlatformReadDto>> GetById(int id)
    {
        var platform = await dbContext.Platforms.FirstOrDefaultAsync(p => p.Id == id);
        if (platform is null)
            return NotFound();

        var platformDto = new PlatformReadDto(platform.Id, platform.PlatformName, platform.CreatedAt);
        return Ok(platformDto);
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> Create(PlatformReadDto platformCreateDto)
    {
        if (platformCreateDto is null)
            return BadRequest();

        var platform = new Platform
        {
            PlatformName = platformCreateDto.PlatformName
        };

        await dbContext.Platforms.AddAsync(platform);
        await dbContext.SaveChangesAsync();

        var platformReadDto = new PlatformReadDto(platform.Id, platform.PlatformName, platform.CreatedAt);

        return CreatedAtRoute(nameof(GetById), new { Id = platform.Id }, platformReadDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, PlatformUpdateDto platformUpdateDto)
    {
        var platformFromContext = await dbContext.Platforms.FirstOrDefaultAsync(p => p.Id == id);
        if (platformFromContext is null)
            return NotFound();

        platformFromContext!.PlatformName = platformUpdateDto.PlatformName;
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await dbContext.Platforms
          .Where(p => p.Id == id)
          .ExecuteDeleteAsync();

        return NoContent();
    }
}