// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using Cake.Common.Build;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Frosting;

namespace Build;

[TaskName("Build")]
[IsDependentOn(typeof(FillSecretsTask))]
[IsDependentOn(typeof(RestoreTask))]
public class BuildTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        string? overrideVersionSuffix = null;

        if (context.BuildSystem().IsRunningOnGitHubActions)
        {
            var runId = context.BuildSystem().GitHubActions.Environment.Workflow.RunId;
            overrideVersionSuffix = $"gh.{runId}";
        }

        context.DotNetBuild(CommonPaths.Solution,
            new DotNetBuildSettings
            {
                NoLogo = true,
                NoRestore = true,
                Configuration = context.BuildConfiguration,
                VersionSuffix = overrideVersionSuffix,
            });
    }
}