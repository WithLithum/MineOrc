// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Manifest;

public class VersionManifest
{
    public sealed record Link
    {
        public required string Release { get; init; }
        
        public required string Snapshot { get; init; }
    }
    
    public required Link Latest { get; init; }
    
    public required IReadOnlyList<VersionExcerpt> Versions { get; init; }
}