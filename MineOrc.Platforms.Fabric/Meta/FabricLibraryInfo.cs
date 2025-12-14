// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Utilities.Maven;

namespace MineOrc.Platforms.Fabric.Meta;

public sealed record FabricLibraryInfo
{
    public required MavenCoordinate Name { get; init; }
    
    /// <summary>
    /// Gets the URL to the root of the maven repository containing the artefact.
    /// </summary>
    /// <value>
    /// The maven repository. If <see langword="null"/>, this artefact is contained in the
    /// <c>libraries.minecraft.net</c> repository.
    /// </value>
    public Uri? Url { get; init; }
    
    public string? Sha1 { get; init; }
    
    public int Size { get; init; }
}