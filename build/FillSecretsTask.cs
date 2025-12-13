// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build;

[TaskName("FillSecrets")]
public sealed class FillSecretsTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var node = new JsonObject
        {
            ["EntraAppId"] = context.EntraAppId ?? "missingno",
        };

        var secretsPath = FilePath.FromString("../MineOrc/Resources/Secrets.json");
        var file = context.FileSystem.GetFile(secretsPath);
        
        using var stream = file.Open(FileMode.Create, FileAccess.Write);
        JsonSerializer.Serialize(stream, node);
    }
}