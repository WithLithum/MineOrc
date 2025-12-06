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

using System.CommandLine;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using MineOrc.Foundation.Manifest;
using MineOrc.Resources;
using Spectre.Console;

namespace MineOrc.Commands;

[UsedImplicitly]
internal static class SearchVersionCommand
{
    private static readonly Argument<string> TermArgument = new("term")
    {
        Description = Texts.VersionSearchTermArgument
    };

    private static readonly Option<bool> RegexOption = new("-r", "--regex")
    {
        Description = Texts.VersionSearchRegexOption
    };

    private static readonly Option<bool> SnapshotOption = CommandHelper.Switch(
        shortName: "-S",
        longName: "--include-snapshots",
        description: Texts.VersionSearchSnapshotOption);
    
    private static readonly Option<bool> OldBetaOption = CommandHelper.Switch(
        shortName: "-B",
        longName: "--include-old-beta",
        description: Texts.VersionSearchBetaOption);
    
    private static readonly Option<bool> OldAlphaOption = CommandHelper.Switch(
        shortName: "-A",
        longName: "--include-old-alpha",
        description: Texts.VersionSearchAlphaOption);

    public static Command CreateCommand()
    {
        var command = new Command("search")
        {
            TermArgument,
            RegexOption,
            SnapshotOption,
            OldBetaOption,
            OldAlphaOption
        };

        command.Description = Texts.VersionSearchCommand;
        command.SetAction(ExecuteAsync);
        return command;
    }
    
    private static async Task<int> ExecuteAsync(ParseResult parseResult,
        CancellationToken cancellationToken)
    {
        var term = parseResult.GetRequiredValue(TermArgument);
        var isRegex = parseResult.GetValue(RegexOption);
        var includeSnapshot = parseResult.GetValue(SnapshotOption);
        var includeOldBeta = parseResult.GetValue(OldBetaOption);
        var includeOldAlpha = parseResult.GetValue(OldAlphaOption);
        
        if (string.IsNullOrWhiteSpace(term))
        {
            MyOutput.Error("missing or empty search term");
            return 1;
        }

        var manifest = await MineOrcApp.PistonMetaClient.GetVersionManifest(cancellationToken);
        var versions = FilterByOptions(manifest.Versions,
            includeSnapshot,
            includeOldBeta,
            includeOldAlpha);

        versions = isRegex
            ? FindByRegex(term, versions)
            : FindByTerm(term, versions);

        if (versions == null)
        {
            return 1;
        }

        PrintVersions(versions);
        return 0;
    }

    private static IEnumerable<VersionExcerpt> FindByTerm(string term,
        IEnumerable<VersionExcerpt> from)
    {
        return from.Where(x => x.Id.Contains(term));
    }

    private static IEnumerable<VersionExcerpt>? FindByRegex(string regex,
            IEnumerable<VersionExcerpt> from)
    {
        Regex expression;
        try
        {
            expression = new Regex(regex);
        }
        catch (Exception ex)
        {
            MyOutput.Error($"failed to parse regex: {ex}");
            return null;
        }

        return from.Where(x => expression.IsMatch(x.Id));
    }

    private static IEnumerable<VersionExcerpt> FilterByOptions(IEnumerable<VersionExcerpt> versions,
        bool includeSnapshot,
        bool includeBeta,
        bool includeAlpha)
    {
        var result = versions;

        if (!includeSnapshot)
        {
            result = result.Where(x => x.Type != VersionType.Snapshot);
        }

        if (!includeBeta)
        {
            result = result.Where(x => x.Type != VersionType.OldBeta);
        }

        if (!includeAlpha)
        {
            result = result.Where(x => x.Type != VersionType.OldAlpha);
        }

        return result;
    }

    private static void PrintVersions(IEnumerable<VersionExcerpt> versions)
    {
        foreach (var version in versions)
        {
            AnsiConsole.MarkupLineInterpolated($"[bold white]{version.Id}[/] [slateblue3]{version.Type}[/]");
            AnsiConsole.MarkupLineInterpolated($"[grey]released[/] [grey66]{version.ReleaseTime}[/]");
            if (version.ReleaseTime != version.Time)
            {
                AnsiConsole.MarkupLineInterpolated($"[grey]updated[/] [grey66]{version.Time}[/]");
            }

            AnsiConsole.WriteLine();
        }
    }
}