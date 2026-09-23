namespace CommandApi.Application.Dtos;

public record CommandReadDto(
    int Id,
    string HowTo,
    string CommandLine,
    int PlatformId,
    DateTime CreatedAt);