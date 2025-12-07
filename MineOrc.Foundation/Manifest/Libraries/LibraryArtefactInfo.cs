// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest.Network;

namespace MineOrc.Foundation.Manifest.Libraries;

public sealed record LibraryArtefactInfo : ArtefactInfo
{
    public required string Path { get; init; }
}