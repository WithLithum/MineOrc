// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Manifest.Network;

public sealed record LogConfigurationArtefactInfo : ArtefactInfo
{
    public required string Id { get; init; }
}