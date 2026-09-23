using System.Runtime.CompilerServices;

namespace CommandApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommandsController(ICommandRepository commandRepository) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommandReadDto>>> GetAllAsync()
    {
        var commands = await commandRepository.GetAllAsync();

        var commandsDto = commands.Select(x => new CommandReadDto(x.Id, x.HowTo, x.CommandLine, x.PlatformId, x.CreatedAt));

        return Ok(commandsDto);
    }

    [HttpGet("{id}", Name = "GetCommandById")]
    public async Task<ActionResult<CommandReadDto>> GetCommandById(int id)
    {
        var command = await commandRepository.GetByIdAsync(id);
        if (command is null)
            return NotFound();

        var commandDto = new CommandReadDto(command.Id, command.HowTo, command.CommandLine, command.PlatformId, command.CreatedAt);
        return Ok(commandDto);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CommandCreateDto dto)
    {
        var command = new Command
        {
            HowTo = dto.HowTo,
            CommandLine = dto.CommandLine,
            PlatformId = dto.PlatformId
        };

        await commandRepository.CreateAsync(command);
        await commandRepository.SaveChangesAsync();

        var commandReadDto = new CommandReadDto(command.Id, command.HowTo, command.CommandLine, command.PlatformId, command.CreatedAt);

        return CreatedAtAction(nameof(GetCommandById), new { Id = commandReadDto.Id }, commandReadDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, CommandUpdateDto dto)
    {
        var command = await commandRepository.GetByIdAsync(id);
        if (command is null)
            return NotFound();

        command.HowTo = dto.HowTo;
        command.CommandLine = dto.CommandLine;

        await commandRepository.UpdateAsync(command);
        await commandRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var commandFromRepo = await commandRepository.GetByIdAsync(id);
        if (commandFromRepo is null)
        {
            return NotFound();
        }

        await commandRepository.DeleteAsync(commandFromRepo.Id);
        await commandRepository.SaveChangesAsync();

        return NoContent();
    }

}