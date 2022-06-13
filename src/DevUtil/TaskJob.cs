using System;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace DevUtil
{
    public class TaskJob : Job
    {
        private readonly CancellationTokenSource _tokenSource = new();

        public override bool HasMoreData => Output.Count > 0 || Progress.Count > 0 || Error.Count > 0 || Warning.Count > 0 || Information.Count > 0 || Verbose.Count > 0 || Debug.Count > 0;

        public override string Location => "pwsh";

        public override string StatusMessage => "";

        public override void StopJob()
        {
            if (!_tokenSource.IsCancellationRequested)
            {
                try
                {
                    switch (JobStateInfo.State)
                    {
                        case JobState.Running:
                        case JobState.NotStarted:
                            SetJobState(JobState.Stopping);
                            break;
                    }
                }
                finally { _tokenSource.Cancel(true); }
            }
        }

        protected override void Dispose(bool disposing)
        {
            try { base.Dispose(disposing); }
            finally
            {
                if (disposing) _tokenSource.Dispose();
            }
        }

        public static TaskJob StartNew<TResult>(AsyncFunc<TResult> func)
            where TResult : class => new(async (writeOutput, cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                TResult result = await func(cancellationToken);
                if (result is not null) writeOutput(PSObject.AsPSObject(result));
            });

        public static TaskJob StartNew<TResult>(AsyncFunc3<TResult> func)
            where TResult : class => new(async (writeOutput, writeProgress, writeError, cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                TResult result = await func(writeProgress, writeError, cancellationToken);
                if (result is not null) writeOutput(PSObject.AsPSObject(result));
            });

        public static TaskJob StartNew<TResult>(AsyncFunc4<TResult> func)
            where TResult : class => new(async (writeOutput, writeProgress, writeError, writeWarning, cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                TResult result = await func(writeProgress, writeError, writeWarning, cancellationToken);
                if (result is not null) writeOutput(PSObject.AsPSObject(result));
            });

        public TaskJob(AsyncTaskJobAction asyncAction)
        {
            CancellationToken token = _tokenSource.Token;
            Task.Factory.StartNew(async () =>
            {
                token.ThrowIfCancellationRequested();
                SetJobState(JobState.Running);
                await asyncAction(Output.Add, token);
            }, token).ContinueWith(task =>
            {
                if (task.IsCanceled)
                    SetJobState(JobState.Stopped);
                else if (task.IsFaulted)
                    try
                    {
                        if (task.Exception is IContainsErrorRecord containsErrorRecord)
                            Error.Add(containsErrorRecord.ErrorRecord ?? new ErrorRecord(task.Exception.GetCausalException(), $"{nameof(TaskJob)}:UnexpectedException", ErrorCategory.NotSpecified, null));
                        else
                            Error.Add(new ErrorRecord(task.Exception.GetCausalException(), $"{nameof(TaskJob)}:UnexpectedException", ErrorCategory.NotSpecified, null));
                    }
                    finally { SetJobState(JobState.Failed); }
                else
                    SetJobState(JobState.Completed);
            });
        }

        public TaskJob(AsyncTaskJobAction4 asyncAction)
        {
            CancellationToken token = _tokenSource.Token;
            Task.Factory.StartNew(async () =>
            {
                token.ThrowIfCancellationRequested();
                SetJobState(JobState.Running);
                await asyncAction(Output.Add, Progress.Add, Error.Add, token);
            }, token).ContinueWith(task =>
            {
                if (task.IsCanceled)
                    SetJobState(JobState.Stopped);
                else if (task.IsFaulted)
                    try
                    {
                        if (task.Exception is IContainsErrorRecord containsErrorRecord)
                            Error.Add(containsErrorRecord.ErrorRecord ?? new ErrorRecord(task.Exception.GetCausalException(), $"{nameof(TaskJob)}:UnexpectedException", ErrorCategory.NotSpecified, null));
                        else
                            Error.Add(new ErrorRecord(task.Exception.GetCausalException(), $"{nameof(TaskJob)}:UnexpectedException", ErrorCategory.NotSpecified, null));
                    }
                    finally { SetJobState(JobState.Failed); }
                else
                    SetJobState(JobState.Completed);
            });
        }

        public TaskJob(AsyncTaskJobAction5 asyncAction)
        {
            CancellationToken token = _tokenSource.Token;
            Task.Factory.StartNew(async () =>
            {
                token.ThrowIfCancellationRequested();
                SetJobState(JobState.Running);
                await asyncAction(Output.Add, Progress.Add, Error.Add, Warning.Add, token);
            }, token).ContinueWith(task =>
            {
                if (task.IsCanceled)
                    SetJobState(JobState.Stopped);
                else if (task.IsFaulted)
                    try
                    {
                        if (task.Exception is IContainsErrorRecord containsErrorRecord)
                            Error.Add(containsErrorRecord.ErrorRecord ?? new ErrorRecord(task.Exception.GetCausalException(), $"{nameof(TaskJob)}:UnexpectedException", ErrorCategory.NotSpecified, null));
                        else
                            Error.Add(new ErrorRecord(task.Exception.GetCausalException(), $"{nameof(TaskJob)}:UnexpectedException", ErrorCategory.NotSpecified, null));
                    }
                    finally { SetJobState(JobState.Failed); }
                else
                    SetJobState(JobState.Completed);
            });
        }
    }

    public delegate Task<TResult> AsyncFunc<TResult>(CancellationToken cancellationToken);

    public delegate Task<TResult> AsyncFunc3<TResult>(Action<ProgressRecord> writeProgress, Action<ErrorRecord> writeError, CancellationToken cancellationToken);

    public delegate Task<TResult> AsyncFunc4<TResult>(Action<ProgressRecord> writeProgress, Action<ErrorRecord> writeError, Action<WarningRecord> writeWarning, CancellationToken cancellationToken);

    public delegate Task AsyncTaskJobAction(Action<PSObject> writeOutput, CancellationToken cancellationToken);

    public delegate Task AsyncTaskJobAction4(Action<PSObject> writeOutput, Action<ProgressRecord> writeProgress, Action<ErrorRecord> writeError, CancellationToken cancellationToken);

    public delegate Task AsyncTaskJobAction5(Action<PSObject> writeOutput, Action<ProgressRecord> writeProgress, Action<ErrorRecord> writeError, Action<WarningRecord> writeWarning, CancellationToken cancellationToken);
}
