// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Manifest;

/// <summary>
/// Represents a version listed on the version manifest.
/// </summary>
public sealed record VersionExcerpt
{
    public required string Id { get; init; }
    
    public required VersionType Type { get; init; }
    
    public required Uri Url { get; init; }

    public required DateTime Time { get; init; }
    
    public required DateTime ReleaseTime { get; init; }
    
    public required string Sha1 { get; init; }
    
    public int ComplianceLevel { get; init; }
}