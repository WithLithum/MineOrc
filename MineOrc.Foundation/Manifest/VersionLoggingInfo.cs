// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest.Network;

namespace MineOrc.Foundation.Manifest;

public sealed record VersionLoggingInfo
{
    public required string Argument { get; init; }
    
    public required LogConfigurationArtefactInfo File { get; init; }
}