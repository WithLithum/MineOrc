// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json.Serialization;
using MineOrc.Foundation.Json;
using MineOrc.Foundation.Manifest.Libraries;
using MineOrc.Foundation.Manifest.Network;
using MineOrc.Foundation.Manifest.Options;
using MineOrc.Foundation.Manifest.Resources;

namespace MineOrc.Foundation.Manifest;

[JsonSourceGenerationOptions(
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        Converters =
        [
            typeof(GameArgumentEntryConverter),
            typeof(JvmArgumentEntryConverter)
        ]
    )
]
[JsonSerializable(typeof(DateTime))]
[JsonSerializable(typeof(MatchRuleAction))]
[JsonSerializable(typeof(GameArgumentRule))]
[JsonSerializable(typeof(GameArgumentEntry))]
[JsonSerializable(typeof(VersionType))]
[JsonSerializable(typeof(VersionExcerpt))]
[JsonSerializable(typeof(VersionManifest))]
[JsonSerializable(typeof(RuntimePlatformRule))]
[JsonSerializable(typeof(RuntimeRule))]
[JsonSerializable(typeof(JvmArgumentEntry))]
[JsonSerializable(typeof(VersionArguments))]
[JsonSerializable(typeof(ArtefactInfo))]
[JsonSerializable(typeof(LogConfigurationArtefactInfo))]
[JsonSerializable(typeof(AssetIndexArtefactInfo))]
[JsonSerializable(typeof(VersionLoggingInfo))]
[JsonSerializable(typeof(VersionLoggingManifest))]
[JsonSerializable(typeof(LibraryDownloadInfo))]
[JsonSerializable(typeof(NativeExtractionOptions))]
[JsonSerializable(typeof(LibraryInfo))]
[JsonSerializable(typeof(RuntimeVersionInfo))]
[JsonSerializable(typeof(ClientManifest))]
[JsonSerializable(typeof(AssetInfo))]
public sealed partial class VersionManifestJsonContext : JsonSerializerContext;