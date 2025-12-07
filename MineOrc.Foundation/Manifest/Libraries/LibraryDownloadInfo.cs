// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Manifest.Libraries;

public sealed record LibraryDownloadInfo
{
    public required LibraryArtefactInfo Artifact { get; init; }
    
    public IReadOnlyDictionary<string, LibraryArtefactInfo>? Classifiers { get; init; }
}