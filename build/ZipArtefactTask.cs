// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.IO;
using System.IO.Compression;
using Cake.Common.Diagnostics;
using Cake.Frosting;

namespace Build;

[TaskName("ZipArtefact")]
public sealed class ZipArtefactTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var outputDir = Path.GetFullPath($"MineOrc/bin/{context.BuildConfiguration}", CommonPaths.RepositoryRoot);
        var outputFile = Path.GetFullPath($"MineOrc/bin/{context.BuildConfiguration}.zip", CommonPaths.RepositoryRoot);
        
        ZipFile.CreateFromDirectory(outputDir, outputFile);
        context.Information("Zipped artefact to '{0}'", outputFile);
    }
}