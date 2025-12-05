using System.Text.Json.Serialization;

namespace MineOrc.Foundation.Manifest.Options;

[JsonConverter(typeof(JsonStringEnumConverter<MatchRuleAction>))]
public enum MatchRuleAction
{
    /// <summary>
    /// The argument should be specified.
    /// </summary>
    Allow,
    /// <summary>
    /// The argument should not be specified.
    /// </summary>
    Disallow
}