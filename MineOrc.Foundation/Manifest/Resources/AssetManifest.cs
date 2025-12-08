// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Manifest.Resources;

public sealed record AssetManifest
{
    public required AssetIndexDictionary Objects { get; init; }
}