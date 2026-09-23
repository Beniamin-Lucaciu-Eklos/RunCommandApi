namespace CommandApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController(
    IPlatformRepository platformRepository,
    ICommandRepository commandRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetAllAsync()
    {
        var platforms = await platformRepository.GetAllAsync();
        var platformDtos = platforms.Select(p => new PlatformReadDto(p.Id, p.PlatformName, p.CreatedAt));

        return Ok(platformDtos);
    }

    [HttpGet("{id}", Name = "GetById")]
    public async Task<ActionResult<PlatformReadDto>> GetById(int id)
    {
        var platform = await platformRepository.GetByIdAsync(id);
        if (platform is null)
            return NotFound();

        var platformDto = new PlatformReadDto(platform.Id, platform.PlatformName, platform.CreatedAt);
        return Ok(platformDto);
    }

    [HttpGet("{platformId:int}/commands")]
    public async Task<ActionResult<CommandReadDto>> GetCommandsByPlatformId(int platformId)
    {
        var platform = await platformRepository.GetByIdAsync(platformId);
        if (platform is null)
            return NotFound();

        var commands = await commandRepository.GetAllByPlatformIdAsync(platformId);
        var commandsDto = commands.Select(x => new CommandReadDto(x.Id, x.HowTo, x.CommandLine, x.PlatformId, x.CreatedAt));
        return Ok(commandsDto);
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

        await platformRepository.CreateAsync(platform);
        await platformRepository.SaveChangesAsync();

        var platformReadDto = new PlatformReadDto(platform.Id, platform.PlatformName, platform.CreatedAt);

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