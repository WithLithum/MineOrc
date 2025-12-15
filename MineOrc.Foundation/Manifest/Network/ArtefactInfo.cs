// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Manifest.Network;

public record ArtefactInfo
{
    public string? Sha1 { get; init; }
    
    public int? Size { get; init; }
    
    public required Uri Url { get; init; }
}