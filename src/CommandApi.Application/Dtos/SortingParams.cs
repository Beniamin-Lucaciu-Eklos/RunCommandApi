namespace CommandApi.Application.Dtos;

public record SortingParams(string? SortBy = null, bool desceding = false);