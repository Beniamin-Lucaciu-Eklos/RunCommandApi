using CommandApi.Application.Dtos;

namespace CommandApi.Application.Mappings;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Platform, PlatformReadDto>();
        config.NewConfig<PlatformCreateDto, Platform>();
        config.NewConfig<PlatformUpdateDto, Platform>();

        config.NewConfig<Command, CommandReadDto>();
        config.NewConfig<CommandCreateDto, Command>();
        config.NewConfig<CommandUpdateDto, Command>();
    }
}