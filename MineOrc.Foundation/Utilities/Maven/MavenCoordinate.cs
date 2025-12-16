// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MineOrc.Foundation.Json;

namespace MineOrc.Foundation.Utilities.Maven;

/// <summary>
/// Represents a short, Group-Artefact-Version (GAV) maven coordinate. This implementation does not
/// support additional fields such as classifiers.
/// </summary>
[JsonConverter(typeof(MavenCoordinateConverter))]
public sealed partial record MavenCoordinate : ISpanParsable<MavenCoordinate>
{
    [JsonConstructor]
    public MavenCoordinate()
    {
    }

    [SetsRequiredMembers]
    public MavenCoordinate(string group, string artefact, string version,
        string? classifier = null)
    {
        Group = group;
        Artefact = artefact;
        Version = version;
        Classifier = classifier;
    }

    public required string Group { get; init; }

    public required string Artefact { get; init; }

    public required string Version { get; init; }
    
    public string? Classifier { get; init; }

    public Uri ToUri(Uri root, string extension)
    {
        return new Uri(root, CreatePath(extension, '/'));
    }

    public string ToPath(string root, string extension)
    {
        return Path.GetFullPath(CreatePath(extension, Path.DirectorySeparatorChar),
            root);
    }

    public string ToArtefactPath(string extension)
    {
        return CreatePath(extension, '/');
    }

    private string CreatePath(string extension, char directorySeparator)
    {
        var sp = directorySeparator;
        var dotGroup = Group.Replace('.', '/');
        
        return Classifier == null
        ? $"{dotGroup}{sp}{Artefact}{sp}{Version}{sp}{Artefact}-{Version}.{extension}"
        : $"{dotGroup}{sp}{Artefact}{sp}{Version}{sp}{Artefact}-{Version}-{Classifier}.{extension}";
    }

    public override string ToString()
    {
        return $"{Group}:{Artefact}:{Version}";
    }
}