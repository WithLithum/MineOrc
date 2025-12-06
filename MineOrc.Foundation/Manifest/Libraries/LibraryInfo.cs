// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest.Options;

namespace MineOrc.Foundation.Manifest.Libraries;

public sealed record LibraryInfo
{
    public required string Name { get; init; }
    
    public required LibraryDownloadInfo Downloads { get; init; }
    
    /// <summary>
    /// Gets a dictionary of the platform name that maps to a key in the classifier dictionary
    /// within <see cref="LibraryDownloadInfo"/>.
    /// </summary>
    public IReadOnlyDictionary<string, string>? Natives { get; init; }
    
    /// <summary>
    /// Gets the options that configures the behaviour when extracting the native libraries.
    /// </summary>
    /// <value>
    /// The extraction options. If <see langword="null"/>, the libraries are not extracted.
    /// </value>
    public NativeExtractionOptions? Extract { get; init; }
    
    /// <summary>
    /// Gets the rules that needs to be satisfied.
    /// </summary>
    public IReadOnlyList<RuntimeRule>? Rules { get; init; }
}