namespace MineOrc.Foundation.Manifest.Options;

public sealed record RuntimePlatformRule
{
    public string? Name { get; init; }
    
    public string? Version { get; init; }
    
    public string? Arch { get; init; }
}