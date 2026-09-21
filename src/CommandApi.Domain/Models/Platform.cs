namespace CommandApi.Domain.Models;

public class Platform
{
    private Platform(string platformName)
    {
        PlatformName = platformName;
    }

    public int Id { get; private set; } = default!;

    public string PlatformName { get; private set; } = default!;

    private static Platform Create(string platformName)
        => new Platform(platformName);
}