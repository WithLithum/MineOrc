// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest.Libraries;
using MineOrc.Foundation.Runtime;
using MineOrc.Foundation.Utilities;
using MineOrc.Instancing.Runtime;

namespace MineOrc.Instancing.Operations;

internal sealed class RestoreLibrariesAction : QueueDispatchActionWrapper<LibraryArtefactInfo>
{
    private readonly IReadOnlyCollection<LibraryInfo> _libraries;

    public RestoreLibrariesAction(IReadOnlyCollection<LibraryInfo> libraries)
        : base("restoreLibraries")
    {
        _libraries = libraries;
    }

    protected override async ValueTask<IReadOnlyCollection<LibraryArtefactInfo>?> GetPayloadsAsync(CancellationToken cancellationToken)
    {
        return await LibraryEvaluator.EvaluateAsync(_libraries, cancellationToken).ConfigureAwait(false);
    }

    protected override QueueDispatchAction<LibraryArtefactInfo> CreateAction(IReadOnlyCollection<LibraryArtefactInfo> payload)
    {
        return new LibrariesRestorer(payload);
    }
}