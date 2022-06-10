using System;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using System.Threading.Tasks;
using System.Threading;

namespace DevUtil.Wrappers
{
    public delegate Task AsyncTaskAction<TOutput>([DisallowNull] Action<TOutput> writeOutput, CancellationToken cancellationToken);

    public delegate Task AsyncTaskAction3<TOutput>([DisallowNull] Action<ProgressRecord> writeProgress, [DisallowNull] Action<TOutput> writeOutput, CancellationToken cancellationToken);

    public delegate Task AsyncTaskAction5<TOutput>([DisallowNull] Action<ProgressRecord> writeProgress, [DisallowNull] Action<WarningRecord> writeWarning, [DisallowNull] Action<ErrorRecord> writeError,
        [DisallowNull] Action<TOutput> writeOutput, CancellationToken cancellationToken);

    public delegate Task AsyncTaskAction7<TOutput>([DisallowNull] Action<ProgressRecord> writeProgress, [DisallowNull] Action<InformationRecord> writeInformation, [DisallowNull] Action<VerboseRecord> writeVerbose,
        [DisallowNull] Action<WarningRecord> writeWarning, [DisallowNull] Action<ErrorRecord> writeError, [DisallowNull] Action<TOutput> writeOutput, CancellationToken cancellationToken);

    public delegate Task AsyncTaskAction8<TOutput>([DisallowNull] Action<ProgressRecord> writeProgress, [DisallowNull] Action<InformationRecord> writeInformation, [DisallowNull] Action<VerboseRecord> writeVerbose,
        [DisallowNull] Action<DebugRecord> writeDebug, [DisallowNull] Action<WarningRecord> writeWarning, [DisallowNull] Action<ErrorRecord> writeError, [DisallowNull] Action<TOutput> writeOutput,
        CancellationToken cancellationToken);

    public delegate Task<TResult> AsyncTaskFunc<TResult>(CancellationToken cancellationToken);

    public delegate Task<TResult> AsyncTaskFunc2<TResult>([DisallowNull] Action<ProgressRecord> writeProgress, CancellationToken cancellationToken);

    public delegate Task<TResult> AsyncTaskFunc4<TResult>([DisallowNull] Action<ProgressRecord> writeProgress, [DisallowNull] Action<WarningRecord> writeWarning, [DisallowNull] Action<ErrorRecord> writeError,
        CancellationToken cancellationToken);

    public delegate Task<TResult> AsyncTaskFunc6<TResult>([DisallowNull] Action<ProgressRecord> writeProgress, [DisallowNull] Action<InformationRecord> writeInformation,
        [DisallowNull] Action<VerboseRecord> writeVerbose, [DisallowNull] Action<WarningRecord> writeWarning, [DisallowNull] Action<ErrorRecord> writeError, CancellationToken cancellationToken);

    public delegate Task<TResult> AsyncTaskFunc7<TResult>([DisallowNull] Action<ProgressRecord> writeProgress, [DisallowNull] Action<InformationRecord> writeInformation,
        [DisallowNull] Action<VerboseRecord> writeVerbose, [DisallowNull] Action<DebugRecord> writeDebug, [DisallowNull] Action<WarningRecord> writeWarning, [DisallowNull] Action<ErrorRecord> writeError,
        CancellationToken cancellationToken);
}
