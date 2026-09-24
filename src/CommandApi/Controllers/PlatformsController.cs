using Mapster;

namespace CommandApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController(
    IPlatformRepository platformRepository,
    ICommandRepository commandRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<PlatformReadDto>>> GetAllAsync([FromQuery] PaginationParams paginationParams)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var platforms = await platformRepository.GetAllAsync(paginationParams);
        var platformDtos = platforms.Items.Select(p => p.Adapt<PlatformReadDto>()).ToList();

        var result = new PaginatedList<PlatformReadDto>(platformDtos,
        platforms.Count,
        platforms.Index,
        platforms.PageSize);

        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetById")]
    public async Task<ActionResult<PlatformReadDto>> GetById(int id)
    {
        var platform = await platformRepository.GetByIdAsync(id);
        if (platform is null)
            return NotFound();

        var platformDto = platform.Adapt<PlatformReadDto>();
        return Ok(platformDto);
    }

    [HttpGet("{platformId:int}/commands")]
    public async Task<ActionResult<CommandReadDto>> GetCommandsByPlatformId(int platformId, [FromQuery] PaginationParams paginationParams)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var platform = await platformRepository.GetByIdAsync(platformId);
        if (platform is null)
            return NotFound();

        var commands = await commandRepository.GetAllByPlatformIdAsync(platformId, paginationParams);
        var commandsDto = commands.Items.Select(c => c.Adapt<CommandReadDto>()).ToList();

        var result = new PaginatedList<CommandReadDto>(
            commandsDto,
            commands.Count,
            commands.Index,
            commands.PageSize
        );
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> Create(PlatformReadDto platformCreateDto)
    {
        if (platformCreateDto is null)
            return BadRequest();

        var platform = platformCreateDto.Adapt<Platform>();

        await platformRepository.CreateAsync(platform);
        await platformRepository.SaveChangesAsync();

        var platformReadDto = platform.Adapt<PlatformReadDto>();
        return CreatedAtRoute(nameof(GetById), new { Id = platform.Id }, platformReadDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, PlatformUpdateDto platformUpdateDto)
    {
        var platformFromContext = await platformRepository.GetByIdAsync(id);
        if (platformFromContext is null)
            return NotFound();

        platformFromContext!.PlatformName = platformUpdateDto.PlatformName;
        await platformRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await platformRepository.DeleteAsync(id);

        return NoContent();
    }
}