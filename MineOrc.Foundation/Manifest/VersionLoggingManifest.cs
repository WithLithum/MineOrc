// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Manifest;

public sealed record VersionLoggingManifest
{
    public required VersionLoggingInfo Client { get; init; }
}