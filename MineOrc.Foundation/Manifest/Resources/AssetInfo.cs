// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Manifest.Resources;

public sealed record AssetInfo
{
    public required string Hash { get; init; }
    
    public required int Size { get; init; }
}