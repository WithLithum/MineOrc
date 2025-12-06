// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest.Network;

namespace MineOrc.Foundation.Manifest.Libraries;

public sealed record LibraryDownloadInfo
{
    public required ArtefactInfo Artifact { get; init; }
    
    public IReadOnlyDictionary<string, ArtefactInfo>? Classifiers { get; init; }
}