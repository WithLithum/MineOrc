// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Drawing;
using MineOrc.Foundation.Manifest;
using MineOrc.Foundation.Network.Security;

namespace MineOrc.Foundation.Runtime.Launch;

public sealed record LaunchGameSettings
{
    public required string ClientId { get; init; }
    
    public required SessionWithProfile Session { get; init; }
    
    public required string AssetsVersion { get; init; }
    
    public required string AssetsRoot { get; init; }
    
    public required string GameDirectory { get; init; }
    
    public required string VersionName { get; init; }
    
    public required VersionType VersionType { get; init; }
    
    public bool IsDemoMode { get; init; }
    
    public Size? CustomResolution { get; init; }
    
    public string? QuickPlayPath { get; init; }
    
    public string? QuickPlaySingleplayer { get; init; }
    
    public string? QuickPlayMultiplayer { get; init; }
    
    public string? QuickPlayRealms { get; init; }
}