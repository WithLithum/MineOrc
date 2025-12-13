// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Test;
using Cake.Frosting;

namespace Build;

[TaskName("Test")]
[IsDependentOn(typeof(BuildTask))]
public sealed class TestTask : FrostingTask<BuildContext> 
{
    public override void Run(BuildContext context)
    {
        context.DotNetTest(CommonPaths.Solution, new DotNetTestSettings
        {
            NoLogo = true,
            Configuration = context.BuildConfiguration,
            NoBuild = true,
        });
    }
}