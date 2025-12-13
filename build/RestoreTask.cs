// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using Cake.Common.Tools.DotNet;
using Cake.Frosting;

namespace Build;

[TaskName("Restore")]
public class RestoreTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        if (context.NoRestore)
        {
            return;
        }
        
        context.DotNetRestore(CommonPaths.Solution);
    }
}