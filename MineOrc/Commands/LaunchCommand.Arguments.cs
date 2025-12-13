// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using MineOrc.Resources;

namespace MineOrc.Commands;

internal partial class LaunchCommand
{
    #region Argument definitions
    private static readonly Argument<string> ArgumentName = CommandHelper.StringArgument("name",
        Texts.CommandLaunchArgumentName);

    private static readonly Option<bool> OptionNoRestore = CommandHelper.Switch("-N",
        "--no-restore",
        Texts.CommandLaunchOptionNoRestore);
    
    #if DEBUG
    private static readonly Option<bool> DevAuth = CommandHelper.Switch("--dev-auth",
        Texts.CommandLaunchArgumentDevAuth);
    #endif
    
    private static readonly Option<bool> OptionDemo = CommandHelper.Switch("-D",
        "--demo",
        Texts.CommandLaunchOptionDemo);

    private static readonly Option<int> OptionMinMemory = new("--min-memory")
    {
        Description = Texts.CommandLaunchOptionMinMemory,
    };

    private static readonly Option<int> OptionMaxMemory = new("--max-memory")
    {
        Description = Texts.CommandLaunchOptionMaxMemory,
        DefaultValueFactory = _ => DefaultMaxMemory,
    };

    private static readonly Option<string?> OptionJavaName = new("-j", "--java")
    {
        Description = Texts.CommandLaunchOptionJavaName,
    };

    private static readonly Option<string?> OptionJavaExecutable = new("-J",
        "--java-file")
    {
        Description = Texts.CommandLaunchOptionJavaFile,
    };
    #endregion

    #region Argument value properties
    internal required string ProfileName { get; init; }
    #if DEBUG
    private bool UseDevAuth { get; init; }
    #endif
    private bool Demo { get; init; }
    private bool NoRestore { get; init; }
    private int MinMemory { get; init; }
    private int MaxMemory { get; init; }
    private string? UserJavaName { get; init; }
    private string? UserJavaExecutable { get; init; }
    #endregion
}