namespace CommandApi.Domain.Models;

public class Command : ICreatedAtTrackable
{
    public int Id { get; set; }

    public string HowTo { get; set; } = default!;

    public string CommandLine { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = default!;

    public int PlatformId { get; set; } = default!;
}