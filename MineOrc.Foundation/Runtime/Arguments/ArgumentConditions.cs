// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest.Options;
using MineOrc.Foundation.Platforms;

namespace MineOrc.Foundation.Runtime.Arguments;

public static class ArgumentConditions
{
    public static bool CheckApplies(IEnumerable<RuntimeRule> rules)
    {
        return rules.All(rule =>
            DoesAllow(action: rule.Action,
                conditionMatch:
                rule.Os == null || MatchPlatform(rule.Os)));
    }
    
    public static bool CheckApplies(IEnumerable<GameArgumentRule> rules,
        IReadOnlyCollection<string> ownFeatures)
    {
        return rules.All(rule =>
            DoesAllow(action: rule.Action,
                conditionMatch:
                rule.Features == null || MatchFeatures(ownFeatures, rule.Features)));
    }
    
    private static bool DoesAllow(MatchRuleAction action, bool conditionMatch)
    {
        return action switch
        {
            MatchRuleAction.Allow => conditionMatch,
            MatchRuleAction.Disallow => !conditionMatch,
            _ => conditionMatch
        };
    }

    public static bool MatchFeatures(IReadOnlyCollection<string> ownFeatures,
        IReadOnlyDictionary<string, bool> matchers)
    {
        foreach (var (name, flag) in matchers)
        {
            var exists = ownFeatures.Contains(name);
            if ((flag && !exists) || (!flag && exists))
            {
                return false;
            }
        }

        return true;
    }

    #region JVM - Platform

    public static bool MatchPlatform(RuntimePlatformRule osRule,
        string? systemName = null,
        string? systemArch = null)
    {
        return MatchSystem(osRule, systemName) && MatchArchitecture(osRule, systemArch);
    }

    private static bool MatchSystem(RuntimePlatformRule osRule,
        string? systemName = null)
    {
        return osRule.Name == null ||
               osRule.Name == (systemName ?? ManifestPlatformUtil.GetSystemName());
    }

    private static bool MatchArchitecture(RuntimePlatformRule osRule,
        string? systemArch = null)
    {
        return osRule.Arch == null ||
               osRule.Arch == (systemArch ?? ManifestPlatformUtil.GetSystemArch());
    }

    #endregion
}