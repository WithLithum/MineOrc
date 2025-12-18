// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Instancing;

/// <summary>
/// Specifies the subdirectories under the profile directory.
/// </summary>
/// <seealso href="https://minecraft.wiki/w/.minecraft#Game_directory"/>
public enum ProfileFolder
{
    /// <summary>
    /// The profile root.
    /// </summary>
    Root,
    /// <summary>
    /// The <c>config</c> directory, where mod configuration files are stored.
    /// </summary>
    ModConfiguration,
    /// <summary>
    /// The <c>debug</c> directory, where profiler sessions and other debug dumps are generated and
    /// stored in.
    /// </summary>
    Debug,
    /// <summary>
    /// The <c>logs</c> directory, where log files are stored in.
    /// </summary>
    Logs,
    /// <summary>
    /// The <c>mods</c> directory, where common mod loaders searches for mod files.
    /// </summary>
    Mods,
    /// <summary>
    /// The <c>resourcepacks</c> directory, where resource packs are put in.
    /// </summary>
    ResourcePacks,
    /// <summary>
    /// The <c>saves</c> directory, where worlds are stored.
    /// </summary>
    Saves,
    /// <summary>
    /// The <c>screenshots</c> directory, where screenshots are stored.
    /// </summary>
    Screenshots,
}