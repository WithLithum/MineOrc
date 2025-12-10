// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Manifest;

public static class VersionTypes
{
    public static bool TryParse(string value,
        out VersionType result)
    {
        result = value switch
        {
            "release" => VersionType.Release,
            "snapshot" => VersionType.Snapshot,
            "unobfuscated" => VersionType.Unobfuscated,
            "old_beta" => VersionType.OldBeta,
            "old_alpha" => VersionType.OldAlpha,
            _ => VersionType.Invalid,
        };

        return result != VersionType.Invalid;
    }

    public static string ToString(VersionType value)
    {
        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        return value switch
        {
            VersionType.Release => "release",
            VersionType.Snapshot => "snapshot",
            VersionType.Unobfuscated => "unobfuscated",
            VersionType.OldBeta => "old_beta",
            VersionType.OldAlpha => "old_alpha",
            _ => throw new ArgumentException("The specified version value is not support."),
        };
    }
}