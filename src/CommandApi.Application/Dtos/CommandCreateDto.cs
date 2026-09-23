namespace CommandApi.Application.Dtos;

public record CommandCreateDto
(
    string HowTo,
    string CommandLine,
    int PlatformId
);

public record CommandUpdateDto
(
    string HowTo,
    string CommandLine
);