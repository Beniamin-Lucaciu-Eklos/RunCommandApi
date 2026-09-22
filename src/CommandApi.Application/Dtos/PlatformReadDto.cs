namespace CommandApi.Application.Dtos;

public record PlatformReadDto(
    int Id,
    string PlatformName,
    DateTime CreatedAt
);