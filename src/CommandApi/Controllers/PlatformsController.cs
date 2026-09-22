using CommandApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Platform>>> GetAllAsync()
    {
        var platforms = await dbContext.Platforms.ToListAsync();

        return Ok(platforms);
    }

    [HttpGet("{id}", Name = "GetById")]
    public async Task<ActionResult<Platform>> GetById(int id)
    {
        var platform = await dbContext.Platforms.FirstOrDefaultAsync(p => p.Id == id);
        if (platform is null)
            return NotFound();

        return Ok(platform);
    }

    [HttpPost]
    public async Task<ActionResult<Platform>> Create(Platform platform)
    {
        if (platform is null)
            return BadRequest();


        await dbContext.Platforms.AddAsync(platform);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { Id = platform.Id }, platform);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, Platform platform)
    {
        var platformFromContext = await dbContext.Platforms.FirstOrDefaultAsync(p => p.Id == id);
        if (platformFromContext is null)
            return NotFound();

        platformFromContext!.PlatformName = platform.PlatformName;
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