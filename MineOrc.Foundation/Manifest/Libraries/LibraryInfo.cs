// Natverk - application server for Minecraft: Java Edition
// Copyright (C) 2025 WithLithum
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

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