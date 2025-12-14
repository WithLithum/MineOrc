// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Frozen;
using MineOrc.Foundation.Manifest;
using MineOrc.Foundation.Manifest.Network;
using MineOrc.Foundation.Manifest.Options;

namespace MineOrc.Tests.Utils;

public static class FakeData
{
    public static readonly AssetIndexArtefactInfo AssetIndexArtefact = new()
    {
        Id = "",
        Sha1 = "",
        Size = 0,
        Url = new Uri("https://contoso.com"),
        TotalSize = 0,
    };

    public static readonly VersionArguments EmptyVersionArguments = new()
    {
        Game = [],
        Jvm = [],
    };

    public const string ExampleMainClass = "com.contoso.example.MineOrcExample";
    
    public static readonly ClientManifest ExampleVersion = new()
    {
        MainClass = ExampleMainClass,
        Libraries = [],
        Id = "MockVersion",
        Arguments = EmptyVersionArguments,
        AssetIndex = AssetIndexArtefact,
        Downloads = FrozenDictionary<string, ArtefactInfo>.Empty,
        Assets = "",
        ComplianceLevel = 2,
    };
}