// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using Cake.Core;
using Cake.Frosting;
using JetBrains.Annotations;

namespace Build;

[UsedImplicitly]
public class BuildContext : FrostingContext
{
    public string? EntraAppId { get; private set; }
    
    public bool NoRestore { get; private set; }
    
    public string BuildConfiguration { get; }

    public BuildContext(ICakeContext context)
        : base(context)
    {
        EntraAppId = context.Environment.GetEnvironmentVariable("MINEORC_BUILD_ENTRA_APP_ID");
        NoRestore = context.Arguments.HasArgument("noRestore");
        BuildConfiguration = context.Arguments.GetArgument("configuration")
            ?? "Release";
    }
}