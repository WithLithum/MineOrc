// Natverk - application server for Minecraft: Java Edition
// Copyright (C) 2025 WithLithum
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

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

    public static bool MatchPlatform(RuntimePlatformRule osRule)
    {
        return MatchSystem(osRule) && MatchArchitecture(osRule);
    }

    private static bool MatchSystem(RuntimePlatformRule osRule)
    {
        return osRule.Name != null &&
               osRule.Name == ManifestPlatformUtil.GetSystemName();
    }

    private static bool MatchArchitecture(RuntimePlatformRule osRule)
    {
        return osRule.Arch != null &&
               osRule.Arch == ManifestPlatformUtil.GetSystemArch();
    }

    #endregion
}