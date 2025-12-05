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