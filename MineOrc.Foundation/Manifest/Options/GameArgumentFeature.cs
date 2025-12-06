// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Manifest.Options;

/// <summary>
/// Defines a standard set of game argument boolean conditions. 
/// </summary>
public static class GameArgumentFeature
{
    /// <summary>
    /// Determines whether Minecraft should be launched into demo mode.
    /// </summary>
    public const string IsDemoUser = "is_demo_user";
    /// <summary>
    /// Determines whether Minecraft is configured to start with a custom resolution.
    /// </summary>
    public const string HasCustomResolution = "has_custom_resolution";
    /// <summary>
    /// Determines whether the launcher supports quick play.
    /// </summary>
    public const string HasQuickPlaysSupport = "has_quick_plays_support";
    /// <summary>
    /// Determines whether Minecraft is configured to start directly into a singleplayer world.
    /// </summary>
    public const string IsQuickPlaySingleplayer = "is_quick_play_singleplayer";
    /// <summary>
    /// Determines whether Minecraft is configured to start directly into a multiplayer session
    /// or server.
    /// </summary>
    public const string IsQuickPlayMultiplayer = "is_quick_play_multiplayer";
    /// <summary>
    /// Determines whether Minecraft is configured to start directly into a realm.
    /// </summary>
    public const string IsQuickPlayRealms = "is_quick_play_realms";
}