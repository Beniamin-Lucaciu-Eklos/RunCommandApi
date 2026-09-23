namespace CommandApi.Domain.Models;

public class Platform : ICreatedAtTrackable
{
    public int Id { get; set; } = default!;

    public string PlatformName { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public ICollection<Command> Commands { get; set; } = [];
}