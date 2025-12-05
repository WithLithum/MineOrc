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

using System.ComponentModel;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using MineOrc.Foundation.Manifest;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MineOrc.Commands;

[UsedImplicitly]
[Description("Search for client versions")]
internal sealed class SearchVersionCommand : AsyncCommand<SearchVersionCommand.Settings>
{
    [UsedImplicitly(ImplicitUseKindFlags.Assign, ImplicitUseTargetFlags.Members)]
    public sealed class Settings : CommandSettings
    {
        [Description("The term to search. If '--regex' is specified, interperts the term as a regex.")]
        [CommandArgument(0, "<term>")]
        public required string Term { get; init; }

        [Description("If specified, treats the term as regex.")]
        [CommandOption("-r|--regex")]
        public bool Regex { get; init; }

        [Description("If specified, includes development versions")]
        [CommandOption("-s|--snapshots")]
        public bool IncludeSnapshots { get; init; }

        [Description("If specified, includes 'old_beta' versions")]
        [CommandOption("-B|--old-beta")]
        public bool IncludeOldBeta { get; init; }

        [Description("If specified, includes 'old_alpha' versions")]
        [CommandOption("-A|--old-alpha")]
        public bool IncludeOldAlpha { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context,
        Settings settings,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(settings.Term))
        {
            MyOutput.Error("missing or empty search term");
            return 1;
        }

        var manifest = await MineOrcApp.PistonMetaClient.GetVersionManifest(cancellationToken);
        var versions = FilterByOptions(manifest.Versions, settings);

        versions = settings.Regex
            ? FindByRegex(settings.Term, versions)
            : FindByTerm(settings.Term, versions);

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
        Settings settings)
    {
        var result = versions;

        if (!settings.IncludeSnapshots)
        {
            result = result.Where(x => x.Type != VersionType.Snapshot);
        }

        if (!settings.IncludeOldBeta)
        {
            result = result.Where(x => x.Type != VersionType.OldBeta);
        }

        if (!settings.IncludeOldAlpha)
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