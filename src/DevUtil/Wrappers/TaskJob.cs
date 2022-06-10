using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using System.Threading;

namespace DevUtil.Wrappers
{
    public class TaskJob : Job
    {
        private readonly CancellationTokenSource _tokenSource = new();

        internal Task BackingTask { get; }

        public override bool HasMoreData => Output.Count > 0 || Error.Count > 0 || Progress.Count > 0 || Verbose.Count > 0 || Debug.Count > 0 || Warning.Count > 0 || Information.Count > 0;

        public override string Location => "PowerShell";

        public override string StatusMessage => string.Empty;

        public static TaskJob Create<TIntermediate, TResult>([DisallowNull] AsyncTaskFunc<TIntermediate> func, [DisallowNull] Func<TIntermediate, TResult> factory)
            where TResult : class
        {
            return new(async t =>
            {
                TIntermediate i = await func(t);
                t.ThrowIfCancellationRequested();
                TResult result = factory(i);
                t.ThrowIfCancellationRequested();
                return (result is null) ? null : PSObject.AsPSObject(result);
            });
        }

        public static TaskJob Create<TIntermediate, TOutput>([DisallowNull] AsyncTaskAction5<TIntermediate> handler, [DisallowNull] Func<TIntermediate, TOutput> factory)
            where TOutput : class
        {
            return new(async (writeProgress, writeWarning, writeError, writeOutput, token) =>
            {
                await handler(writeProgress, writeWarning, writeError, i =>
                {
                    TOutput result = factory(i);
                    writeOutput((result is null) ? null : PSObject.AsPSObject(result));
                }, token);
            });
        }

        public static TaskJob Create<TIntermediate, TOutput>([DisallowNull] AsyncTaskAction8<TIntermediate> handler, [DisallowNull] Func<TIntermediate, TOutput> factory)
            where TOutput : class
        {
            return new(async (writeProgress, writeInformation, writeVerbose, writeDebug, writeWarning, writeError, writeOutput, token) =>
            {
                await handler(writeProgress, writeInformation, writeVerbose, writeDebug, writeWarning, writeError, i =>
                {
                    TOutput result = factory(i);
                    writeOutput((result is null) ? null : PSObject.AsPSObject(result));
                }, token);
            });
        }

        public static TaskJob Create<T>(Func<CancellationToken, T> func) where T : class
        {
            return new(t =>
            {
                T result = func(t);
                return (result is null) ? null : PSObject.AsPSObject(result);
            });
        }

        public static TaskJob Create<T>(Func<Action<ProgressRecord>, CancellationToken, T> func)
            where T : class
        {
            return new((progress, warning, error, t) =>
            {
                T result = func(progress, t);
                return (result is null) ? null : PSObject.AsPSObject(result);
            });
        }

        public static TaskJob Create<T>(Func<Action<ProgressRecord>, Action<WarningRecord>, Action<ErrorRecord>, CancellationToken, T> func)
            where T : class
        {
            return new((progress, warning, error, t) =>
            {
                T result = func(progress, warning, error, t);
                return (result is null) ? null : PSObject.AsPSObject(result);
            });
        }

        public static TaskJob Create<T>(Func<Action<ProgressRecord>, Action<InformationRecord>, Action<VerboseRecord>, Action<WarningRecord>, Action<ErrorRecord>, CancellationToken, T> func)
            where T : class
        {
            return new((progress, information, verbose, warning, error, t) =>
            {
                T result = func(progress, information, verbose, warning, error, t);
                return (result is null) ? null : PSObject.AsPSObject(result);
            });
        }

        public static TaskJob Create<T>(Func<Action<ProgressRecord>, Action<InformationRecord>, Action<VerboseRecord>, Action<DebugRecord>, Action<WarningRecord>, Action<ErrorRecord>, CancellationToken, T> func)
            where T : class
        {
            return new((progress, information, verbose, debug, warning, error, t) =>
            {
                T result = func(progress, information, verbose, debug, warning, error, t);
                return (result is null) ? null : PSObject.AsPSObject(result);
            });
        }

        public static TaskJob Create<T>(Func<CancellationToken, Task<T>> func) where T : class
        {
            return new(async t =>
            {
                T result = await func(t);
                return (result is null) ? null : PSObject.AsPSObject(result);
            });
        }

        public static TaskJob Create<T>(Func<Action<ProgressRecord>, CancellationToken, Task<T>> func)
            where T : class
        {
            return new(async (progress, t) =>
            {
                T result = await func(progress, t);
                return (result is null) ? null : PSObject.AsPSObject(result);
            });
        }

        public static TaskJob Create<T>(Func<Action<ProgressRecord>, Action<WarningRecord>, Action<ErrorRecord>, CancellationToken, Task<T>> func)
            where T : class
        {
            return new(async (progress, warning, error, t) =>
            {
                T result = await func(progress, warning, error, t);
                return (result is null) ? null : PSObject.AsPSObject(result);
            });
        }

        public static TaskJob Create<T>(Func<Action<ProgressRecord>, Action<InformationRecord>, Action<VerboseRecord>, Action<WarningRecord>, Action<ErrorRecord>, CancellationToken, Task<T>> func)
            where T : class
        {
            return new(async (progress, information, verbose, warning, error, t) =>
            {
                T result = await func(progress, information, verbose, warning, error, t);
                return (result is null) ? null : PSObject.AsPSObject(result);
            });
        }

        public static TaskJob Create<T>(Func<Action<ProgressRecord>, Action<InformationRecord>, Action<VerboseRecord>, Action<DebugRecord>, Action<WarningRecord>, Action<ErrorRecord>, CancellationToken, Task<T>> func)
            where T : class
        {
            return new(async (progress, information, verbose, debug, warning, error, t) =>
            {
                T result = await func(progress, information, verbose, debug, warning, error, t);
                return (result is null) ? null : PSObject.AsPSObject(result);
            });
        }

        internal TaskJob(Func<CancellationToken, Task<PSObject>> func)
        {
            CancellationToken token = _tokenSource.Token;
            BackingTask = Task.Factory.StartNew(async () =>
            {
                if (token.IsCancellationRequested) return;
                SetJobState(JobState.Running);
                PSObject result = await func(token);
                if (result is not null && !token.IsCancellationRequested)
                    Output.Add(result);
            }, token).ContinueWith(OnContinue);
        }

        internal TaskJob(Func<Action<ProgressRecord>, CancellationToken, Task<PSObject>> func)
        {
            CancellationToken token = _tokenSource.Token;
            BackingTask = Task.Factory.StartNew(async () =>
            {
                if (token.IsCancellationRequested) return;
                SetJobState(JobState.Running);
                PSObject result = await func(Progress.Add, token);
                if (result is not null && !token.IsCancellationRequested)
                    Output.Add(result);
            }, token).ContinueWith(OnContinue);
        }

        internal TaskJob(Func<Action<ProgressRecord>, Action<WarningRecord>, Action<ErrorRecord>, CancellationToken, Task<PSObject>> func)
        {
            CancellationToken token = _tokenSource.Token;
            BackingTask = Task.Factory.StartNew(async () =>
            {
                if (token.IsCancellationRequested) return;
                SetJobState(JobState.Running);
                PSObject result = await func(Progress.Add, Warning.Add, Error.Add, token);
                if (result is not null && !token.IsCancellationRequested)
                    Output.Add(result);
            }, token).ContinueWith(OnContinue);
        }

        internal TaskJob(Func<Action<ProgressRecord>, Action<InformationRecord>, Action<VerboseRecord>, Action<WarningRecord>, Action<ErrorRecord>, CancellationToken, Task<PSObject>> func)
        {
            CancellationToken token = _tokenSource.Token;
            BackingTask = Task.Factory.StartNew(async () =>
            {
                if (token.IsCancellationRequested) return;
                SetJobState(JobState.Running);
                PSObject result = await func(Progress.Add, Information.Add, Verbose.Add, Warning.Add, Error.Add, token);
                if (result is not null && !token.IsCancellationRequested)
                    Output.Add(result);
            }, token).ContinueWith(OnContinue);
        }

        internal TaskJob(Func<Action<ProgressRecord>, Action<InformationRecord>, Action<VerboseRecord>, Action<DebugRecord>, Action<WarningRecord>, Action<ErrorRecord>, CancellationToken, Task<PSObject>> func)
        {
            CancellationToken token = _tokenSource.Token;
            BackingTask = Task.Factory.StartNew(async () =>
            {
                if (token.IsCancellationRequested) return;
                SetJobState(JobState.Running);
                PSObject result = await func(Progress.Add, Information.Add, Verbose.Add, Debug.Add, Warning.Add, Error.Add, token);
                if (result is not null && !token.IsCancellationRequested)
                    Output.Add(result);
            }, token).ContinueWith(OnContinue);
        }

        internal TaskJob(Func<CancellationToken, PSObject> func)
        {
            CancellationToken token = _tokenSource.Token;
            BackingTask = Task.Factory.StartNew(() =>
            {
                if (token.IsCancellationRequested) return;
                SetJobState(JobState.Running);
                PSObject result = func(token);
                if (result is not null && !token.IsCancellationRequested)
                    Output.Add(result);
            }, token).ContinueWith(OnContinue);
        }

        internal TaskJob(Func<Action<ProgressRecord>, CancellationToken, PSObject> func)
        {
            CancellationToken token = _tokenSource.Token;
            BackingTask = Task.Factory.StartNew(() =>
            {
                if (token.IsCancellationRequested) return;
                SetJobState(JobState.Running);
                PSObject result = func(Progress.Add, token);
                if (result is not null && !token.IsCancellationRequested)
                    Output.Add(result);
            }, token).ContinueWith(OnContinue);
        }

        internal TaskJob(Func<Action<ProgressRecord>, Action<WarningRecord>, Action<ErrorRecord>, CancellationToken, PSObject> func)
        {
            CancellationToken token = _tokenSource.Token;
            BackingTask = Task.Factory.StartNew(() =>
            {
                if (token.IsCancellationRequested) return;
                SetJobState(JobState.Running);
                PSObject result = func(Progress.Add, Warning.Add, Error.Add, token);
                if (result is not null && !token.IsCancellationRequested)
                    Output.Add(result);
            }, token).ContinueWith(OnContinue);
        }

        internal TaskJob(Func<Action<ProgressRecord>, Action<InformationRecord>, Action<VerboseRecord>, Action<WarningRecord>, Action<ErrorRecord>, CancellationToken, PSObject> func)
        {
            CancellationToken token = _tokenSource.Token;
            BackingTask = Task.Factory.StartNew(() =>
            {
                if (token.IsCancellationRequested) return;
                SetJobState(JobState.Running);
                PSObject result = func(Progress.Add, Information.Add, Verbose.Add, Warning.Add, Error.Add, token);
                if (result is not null && !token.IsCancellationRequested)
                    Output.Add(result);
            }, token).ContinueWith(OnContinue);
        }

        internal TaskJob(Func<Action<ProgressRecord>, Action<InformationRecord>, Action<VerboseRecord>, Action<DebugRecord>, Action<WarningRecord>, Action<ErrorRecord>, CancellationToken, PSObject> func)
        {
            CancellationToken token = _tokenSource.Token;
            BackingTask = Task.Factory.StartNew(() =>
            {
                if (token.IsCancellationRequested) return;
                SetJobState(JobState.Running);
                PSObject result = func(Progress.Add, Information.Add, Verbose.Add, Debug.Add, Warning.Add, Error.Add, token);
                if (result is not null && !token.IsCancellationRequested)
                    Output.Add(result);
            }, token).ContinueWith(OnContinue);
        }

        internal TaskJob(Action<Action<PSObject>, CancellationToken> action)
        {
            CancellationToken token = _tokenSource.Token;
            BackingTask = Task.Factory.StartNew(() =>
            {
                if (token.IsCancellationRequested) return;
                SetJobState(JobState.Running);
                action(Output.Add, token);
            }, token).ContinueWith(OnContinue);
        }

        internal TaskJob(Action<Action<ProgressRecord>, Action<PSObject>, CancellationToken> action)
        {
            CancellationToken token = _tokenSource.Token;
            BackingTask = Task.Factory.StartNew(() =>
            {
                if (token.IsCancellationRequested) return;
                SetJobState(JobState.Running);
                action(Progress.Add, Output.Add, token);
            }, token).ContinueWith(OnContinue);
        }

        internal TaskJob(Action<Action<ProgressRecord>, Action<WarningRecord>, Action<ErrorRecord>, Action<PSObject>, CancellationToken> action)
        {
            CancellationToken token = _tokenSource.Token;
            BackingTask = Task.Factory.StartNew(() =>
            {
                if (token.IsCancellationRequested) return;
                SetJobState(JobState.Running);
                action(Progress.Add, Warning.Add, Error.Add, Output.Add, token);
            }, token).ContinueWith(OnContinue);
        }

        internal TaskJob(Action<Action<ProgressRecord>, Action<InformationRecord>, Action<VerboseRecord>, Action<WarningRecord>, Action<ErrorRecord>, Action<PSObject>, CancellationToken> action)
        {
            CancellationToken token = _tokenSource.Token;
            BackingTask = Task.Factory.StartNew(() =>
            {
                if (token.IsCancellationRequested) return;
                SetJobState(JobState.Running);
                action(Progress.Add, Information.Add, Verbose.Add, Warning.Add, Error.Add, Output.Add, token);
            }, token).ContinueWith(OnContinue);
        }

        internal TaskJob(Action<Action<ProgressRecord>, Action<InformationRecord>, Action<VerboseRecord>, Action<DebugRecord>, Action<WarningRecord>, Action<ErrorRecord>, Action<PSObject>, CancellationToken> action)
        {
            CancellationToken token = _tokenSource.Token;
            BackingTask = Task.Factory.StartNew(() =>
            {
                if (token.IsCancellationRequested) return;
                SetJobState(JobState.Running);
                action(Progress.Add, Information.Add, Verbose.Add, Debug.Add, Warning.Add, Error.Add, Output.Add, token);
            }, token).ContinueWith(OnContinue);
        }

        private void OnContinue(Task task)
        {
            if (task.IsCanceled)
                SetJobState(JobState.Stopped);
            else if (task.IsFaulted)
            {
                AggregateException exception = task.Exception.Flatten();
                ErrorRecord[] errorRecords = exception.InnerExceptions.OfType<IContainsErrorRecord>().Select(e => e.ErrorRecord).Where(e => e is not null).ToArray();
                if (errorRecords.Length == exception.InnerExceptions.Count)
                {
                    foreach (ErrorRecord e in errorRecords)
                        Error.Add(e);
                }
                else if (exception.InnerExceptions.Count == 1)
                    Error.Add(new ErrorRecord(exception.InnerException, $"{nameof(TaskJob)}:UnexpectedException", ErrorCategory.NotSpecified, null));
                else
                    Error.Add(new ErrorRecord(task.Exception, $"{nameof(TaskJob)}:UnexpectedException", ErrorCategory.NotSpecified, null));
                SetJobState(JobState.Failed);
            }
            else
                SetJobState(JobState.Completed);
        }

        public override async void StopJob()
        {
            if (!BackingTask.IsCompleted)
            {
                if (!_tokenSource.IsCancellationRequested)
                {
                    SetJobState(JobState.Stopping);
                    _tokenSource.Cancel(true);
                }
                try { await BackingTask; } catch { /* okay to ignore */ }
            }
        }

        protected override async void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (!BackingTask.IsCompleted)
                    {
                        if (!_tokenSource.IsCancellationRequested)
                            _tokenSource.Cancel(true);
                        try { await BackingTask; }
                        finally { _tokenSource.Dispose(); }
                    }
                    else
                        _tokenSource.Dispose();
                }
            }
            finally { base.Dispose(disposing); }
        }
    }
}
