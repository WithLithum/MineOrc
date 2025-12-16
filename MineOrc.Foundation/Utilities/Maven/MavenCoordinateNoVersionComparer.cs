// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Utilities.Maven;

public sealed class MavenCoordinateNoVersionComparer : IEqualityComparer<MavenCoordinate>
{
    public static readonly MavenCoordinateNoVersionComparer Instance = new();
    
    private MavenCoordinateNoVersionComparer()
    {
    }

    public bool Equals(MavenCoordinate? x, MavenCoordinate? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null) return false;
        if (y is null) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Group == y.Group && x.Artefact == y.Artefact && x.Classifier == y.Classifier;
    }

    public int GetHashCode(MavenCoordinate obj)
    {
        return HashCode.Combine(obj.Group, obj.Artefact, obj.Classifier);
    }
}