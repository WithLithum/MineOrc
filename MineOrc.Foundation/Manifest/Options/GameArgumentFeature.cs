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