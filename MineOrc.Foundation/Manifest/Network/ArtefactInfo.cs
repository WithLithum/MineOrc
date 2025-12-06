// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Manifest.Network;

public record ArtefactInfo
{
    /// <summary>
    /// Gets the path, relative to the directory associated with the type of the file,
    /// to store the downloaded artefact.
    /// </summary>
    /// <value>
    /// The relative path to store the file at. If <see langword="null"/>, the location of this
    /// file is known to the client.
    /// </value>
    public string? Path { get; init; }
    
    public required string Sha1 { get; init; }
    
    public required int Size { get; init; }
    
    public required Uri Url { get; init; }
}