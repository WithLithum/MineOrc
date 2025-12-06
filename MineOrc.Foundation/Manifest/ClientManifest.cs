// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest.Network;
using MineOrc.Foundation.Manifest.Options;

namespace MineOrc.Foundation.Manifest;

public sealed record ClientManifest
{
    public required string Id { get; init; }
    
    public required string MainClass { get; init; }
    
    public int MinimumLauncherVersion { get; init; }
    
    public VersionType Type { get; init; }
    
    public DateTime Time { get; init; }
    
    public DateTime ReleaseTime { get; init; }
    
    public required VersionArguments Arguments { get; init; }
    
    public required AssetIndexArtefactInfo AssetIndex { get; init; }
    
    public required string Assets { get; init; }
    
    public int ComplianceLevel { get; init; }
    
    public required IReadOnlyDictionary<string, ArtefactInfo> Downloads { get; init; }
    
    public RuntimeVersionInfo? JavaVersion { get; init; }
    
    public VersionLoggingManifest? Logging { get; init; }
}