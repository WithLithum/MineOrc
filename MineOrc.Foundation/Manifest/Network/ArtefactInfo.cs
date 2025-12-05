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