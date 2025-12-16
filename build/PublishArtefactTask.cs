// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.IO;
using Cake.Common.Build;
using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;
using Path = System.IO.Path;

namespace Build;

[TaskName("PublishArtefact")]
[IsDependentOn(typeof(ZipArtefactTask))]
public sealed class PublishArtefactTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        if (!context.GitHubActions().IsRunningOnGitHubActions)
        {
            throw new CakeReportException("Not running on GitHub Actions");
        }
        
        var zipFile = Path.GetFullPath($"MineOrc/bin/{context.BuildConfiguration}.zip", CommonPaths.RepositoryRoot);
        if (!File.Exists(zipFile))
        {
            throw new CakeReportException($"Expected zip file {zipFile} to exist");
        }
        
        context.Information("Publishing artefact file '{0}'", zipFile);
        context.BuildSystem().GitHubActions.Commands.UploadArtifact(new FilePath(zipFile),
            "app.zip").Wait();
    }
}