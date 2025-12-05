namespace MineOrc.Foundation.Manifest.Options;

public sealed record RuntimeRule
{
    /// <summary>
    /// Gets the action to take if the conditions of this rule was satisfied.
    /// </summary>
    public required MatchRuleAction Action { get; init; }
    
    public RuntimePlatformRule? Os { get; init; }
}