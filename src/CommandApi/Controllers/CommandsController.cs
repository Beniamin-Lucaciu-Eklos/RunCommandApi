using Mapster;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace CommandApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommandsController(ICommandRepository commandRepository) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommandReadDto>>> GetAllAsync()
    {
        var commands = await commandRepository.GetAllAsync();

        var commandsDto = commands.Select(c => c.Adapt<CommandReadDto>());

        return Ok(commandsDto);
    }

    [HttpGet("{id}", Name = "GetCommandById")]
    public async Task<ActionResult<CommandReadDto>> GetCommandById(int id)
    {
        var command = await commandRepository.GetByIdAsync(id);
        if (command is null)
            return NotFound();

        var commandDto = command.Adapt<CommandReadDto>();
        return Ok(commandDto);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CommandCreateDto dto)
    {
        var command = dto.Adapt<Command>();

        await commandRepository.CreateAsync(command);
        await commandRepository.SaveChangesAsync();

        var commandReadDto = command.Adapt<CommandReadDto>();

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

    [HttpPatch("{id:int}")]
    public async Task<ActionResult> PartialUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDocument)
    {
        var command = await commandRepository.GetByIdAsync(id);
        if (command is null)
            return NotFound("you must supply a valid command id in the route");

        var commandPatch = command.Adapt<CommandUpdateDto>();
        patchDocument.ApplyTo(commandPatch, JsonPatchError =>
        {
            var key = JsonPatchError.AffectedObject.GetType().Name;
            ModelState.AddModelError(key, JsonPatchError.ErrorMessage);
        });

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        commandPatch.Adapt(command);

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