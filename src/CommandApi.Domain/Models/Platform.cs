using System.Text.Json.Serialization;

namespace CommandApi.Domain.Models;

public class Platform
{
    public int Id { get; set; } = default!;

    public string PlatformName { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
}