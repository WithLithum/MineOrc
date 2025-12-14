// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Concurrent;
using MineOrc.Foundation.Manifest.Libraries;
using MineOrc.Foundation.Platforms;
using MineOrc.Foundation.Runtime.Arguments;

namespace MineOrc.Foundation.Runtime;

public static class LibraryEvaluator
{
    public static async Task<IReadOnlyCollection<LibraryArtefactInfo>> EvaluateAsync(
        IEnumerable<LibraryInfo> libraries,
        CancellationToken cancellationToken = default)
    {
        var bag = new ConcurrentBag<LibraryArtefactInfo>();
        
        await Parallel.ForEachAsync(libraries,
                cancellationToken,
                async (x, _) => await EvaluateInternal(x, bag)
                    .ConfigureAwait(false))
            .ConfigureAwait(false);

        return bag.ToArray();
    }

    private static ValueTask EvaluateInternal(LibraryInfo library,
        ConcurrentBag<LibraryArtefactInfo> bag)
    {
        var osName = ManifestPlatformUtil.GetSystemName();
        
        if (library.Rules != null
            && !ArgumentConditions.CheckApplies(library.Rules))
        {
            return ValueTask.CompletedTask;
        }

        bag.Add(library.Downloads.Artifact);
        
        if (library is { Natives: not null, Downloads.Classifiers: not null }
            && library.Natives.TryGetValue(osName, out var nativeId)
            && library.Downloads.Classifiers.TryGetValue(nativeId, out var nativeLibrary))
        {
            bag.Add(nativeLibrary);
        }

        return ValueTask.CompletedTask;
    }
}