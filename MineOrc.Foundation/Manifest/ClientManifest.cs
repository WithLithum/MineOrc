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
    
    /// <summary>
    /// Gets the information leading to the download of the asset index.
    /// </summary>
    /// <remarks>
    /// The asset index file is a dictionary with a string (asset file name) as the key and a
    /// value of <see cref="Resources.AssetInfo"/> as the value.
    /// </remarks>
    public required AssetIndexArtefactInfo AssetIndex { get; init; }
    
    public required string Assets { get; init; }
    
    public int ComplianceLevel { get; init; }
    
    public required IReadOnlyDictionary<string, ArtefactInfo> Downloads { get; init; }
    
    public RuntimeVersionInfo? JavaVersion { get; init; }
    
    public VersionLoggingManifest? Logging { get; init; }

    public ArtefactInfo? GetClientDownload()
    {
        return Downloads.GetValueOrDefault("client");
    }
}