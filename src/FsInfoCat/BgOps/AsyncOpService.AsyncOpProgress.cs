using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.BgOps
{
    public partial class AsyncOpService
    {
        class AsyncOpProgress<TState> : IAsyncOpProgress<TState>, IObserver<IAsyncOpEventArgs>
        {
            private readonly object _syncRoot = new();
            private readonly IDisposable _subscription;
            private readonly Action<IAsyncOpEventArgs<TState>> _report;

            public CancellationToken Token { get; }

            public TState AsyncState { get; }

            public Guid Id { get; } = Guid.NewGuid();

            public string Activity { get; }

            public string StatusDescription { get; private set; }

            public string CurrentOperation { get; private set; }

            public IAsyncOpEventArgs ParentOperation { get; private set; }

            IAsyncOpInfo IAsyncOpInfo.ParentOperation => ParentOperation;

            internal AsyncOpProgress([DisallowNull] Action<IAsyncOpEventArgs<TState>> report, [DisallowNull] string activity, [DisallowNull] string statusDescription, string currentOperation, TState state, IAsyncOpEventArgs parentOperation, CancellationToken token) =>
                (_report, Activity, StatusDescription, CurrentOperation, ParentOperation, AsyncState, Token) =
                    (report ?? throw new ArgumentNullException(nameof(report)), activity ?? throw new ArgumentNullException(nameof(activity)), statusDescription ?? throw new ArgumentNullException(nameof(statusDescription)), currentOperation ?? "", parentOperation, state, token);

            void IObserver<IAsyncOpEventArgs>.OnCompleted() { }

            void IObserver<IAsyncOpEventArgs>.OnError(Exception error) { }

            void IObserver<IAsyncOpEventArgs>.OnNext(IAsyncOpEventArgs value) => ParentOperation = value;

            public IAsyncProducer<TState2, TResult> FromAsync<TState2, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState2>, Task<TResult>> asyncMethodDelegate, TState2 state, IObserver<IAsyncOpEventArgs<TState2>> observer, Func<IAsyncOpEventArgs<TState2>, string> getFinalStatusMessage)
            {
                throw new NotImplementedException();
            }

            public IAsyncProducer<TState2, TResult> FromAsync<TState2, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState2>, Task<TResult>> asyncMethodDelegate, TState2 state, IObserver<IAsyncOpEventArgs<TState2>> observer)
            {
                throw new NotImplementedException();
            }

            public IAsyncProducer<TState2, TResult> FromAsync<TState2, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState2>, Task<TResult>> asyncMethodDelegate, TState2 state, Func<IAsyncOpEventArgs<TState2>, string> getFinalStatusMessage)
            {
                throw new NotImplementedException();
            }

            public IAsyncProducer<TState2, TResult> FromAsync<TState2, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState2>, Task<TResult>> asyncMethodDelegate, TState2 state)
            {
                throw new NotImplementedException();
            }

            public IAsyncProducer<TResult> FromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, IObserver<IAsyncOpEventArgs> observer, Func<IAsyncOpEventArgs, string> getFinalStatusMessage)
            {
                throw new NotImplementedException();
            }

            public IAsyncProducer<TResult> FromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, IObserver<IAsyncOpEventArgs> observer)
            {
                throw new NotImplementedException();
            }

            public IAsyncProducer<TResult> FromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, Func<IAsyncOpEventArgs, string> getFinalStatusMessage)
            {
                throw new NotImplementedException();
            }

            public IAsyncProducer<TResult> FromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate)
            {
                throw new NotImplementedException();
            }

            public IAsyncOperation<TState2> FromAsync<TState2>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState2>, Task> asyncMethodDelegate, TState2 state, IObserver<IAsyncOpEventArgs<TState2>> observer, Func<IAsyncOpEventArgs<TState2>, string> getFinalStatusMessage)
            {
                throw new NotImplementedException();
            }

            public IAsyncOperation<TState2> FromAsync<TState2>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState2>, Task> asyncMethodDelegate, TState2 state, IObserver<IAsyncOpEventArgs<TState2>> observer)
            {
                throw new NotImplementedException();
            }

            public IAsyncOperation<TState2> FromAsync<TState2>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState2>, Task> asyncMethodDelegate, TState2 state, Func<IAsyncOpEventArgs<TState2>, string> getFinalStatusMessage)
            {
                throw new NotImplementedException();
            }

            public IAsyncOperation<TState2> FromAsync<TState2>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState2>, Task> asyncMethodDelegate, TState2 state)
            {
                throw new NotImplementedException();
            }

            public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, IObserver<IAsyncOpEventArgs> observer, Func<IAsyncOpEventArgs, string> getFinalStatusMessage)
            {
                throw new NotImplementedException();
            }

            public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, IObserver<IAsyncOpEventArgs> observer)
            {
                throw new NotImplementedException();
            }

            public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, Func<IAsyncOpEventArgs, string> getFinalStatusMessage)
            {
                throw new NotImplementedException();
            }

            public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate)
            {
                throw new NotImplementedException();
            }

            public void Report(string value)
            {
                AsyncOpEventArgs<TState> args;
                lock (_syncRoot)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        if (string.IsNullOrEmpty(CurrentOperation))
                            return;
                        CurrentOperation = "";
                    }
                    else
                    {
                        if (CurrentOperation == value)
                            return;
                        CurrentOperation = value;
                    }
                    args = new AsyncOpEventArgs<TState>(this, null);
                }
                _report(args);
            }

            public void Report(Exception value)
            {
                AsyncOpEventArgs<TState> args;
                lock (_syncRoot)
                    args = new AsyncOpEventArgs<TState>(this, value ?? throw new ArgumentNullException(nameof(value)));
                _report(args);
            }

            public void ReportStatus(string statusDescription, string currentOperation, Exception exception)
            {
                AsyncOpEventArgs<TState> args;
                lock (_syncRoot)
                {
                    if (exception is null)
                    {
                        if (string.IsNullOrEmpty(currentOperation))
                        {
                            if (string.IsNullOrEmpty(CurrentOperation))
                            {
                                if (string.IsNullOrEmpty(statusDescription))
                                {
                                    if (string.IsNullOrEmpty(StatusDescription))
                                        return;
                                    StatusDescription = "";
                                }
                                else
                                {
                                    if (StatusDescription == statusDescription)
                                        return;
                                    StatusDescription = statusDescription;
                                }
                            }
                            CurrentOperation = "";
                        }
                        else
                        {
                            if (CurrentOperation == currentOperation)
                            {
                                if (string.IsNullOrEmpty(statusDescription))
                                {
                                    if (string.IsNullOrEmpty(StatusDescription))
                                        return;
                                    StatusDescription = "";
                                }
                                else
                                {
                                    if (StatusDescription == statusDescription)
                                        return;
                                    StatusDescription = statusDescription;
                                }
                            }
                            else
                                StatusDescription = statusDescription ?? "";
                            CurrentOperation = currentOperation;
                        }
                    }
                    else
                    {
                        StatusDescription = statusDescription ?? "";
                        CurrentOperation = currentOperation ?? "";
                    }
                    args = new AsyncOpEventArgs<TState>(this, exception);
                }
                _report(args);
            }

            public void ReportStatus(string statusDescription, string currentOperation)
            {
                AsyncOpEventArgs<TState> args;
                lock (_syncRoot)
                {
                    if (string.IsNullOrEmpty(currentOperation))
                    {
                        if (string.IsNullOrEmpty(CurrentOperation))
                        {
                            if (string.IsNullOrEmpty(statusDescription))
                            {
                                if (string.IsNullOrEmpty(StatusDescription))
                                    return;
                                StatusDescription = "";
                            }
                            else
                            {
                                if (StatusDescription == statusDescription)
                                    return;
                                StatusDescription = statusDescription;
                            }
                        }
                        CurrentOperation = "";
                    }
                    else
                    {
                        if (CurrentOperation == currentOperation)
                        {
                            if (string.IsNullOrEmpty(statusDescription))
                            {
                                if (string.IsNullOrEmpty(StatusDescription))
                                    return;
                                StatusDescription = "";
                            }
                            else
                            {
                                if (StatusDescription == statusDescription)
                                    return;
                                StatusDescription = statusDescription;
                            }
                        }
                        else
                            StatusDescription = statusDescription ?? "";
                        CurrentOperation = currentOperation;
                    }
                    args = new AsyncOpEventArgs<TState>(this, null);
                }
                _report(args);
            }

            public void ReportStatus(string statusDescription)
            {
                AsyncOpEventArgs<TState> args;
                lock (_syncRoot)
                {
                    if (string.IsNullOrEmpty(CurrentOperation))
                    {
                        if (string.IsNullOrEmpty(statusDescription))
                        {
                            if (string.IsNullOrEmpty(StatusDescription))
                                return;
                            StatusDescription = "";
                        }
                        else
                        {
                            if (StatusDescription == statusDescription)
                                return;
                            StatusDescription = statusDescription;
                        }
                    }
                    else
                    {
                        CurrentOperation = "";
                        StatusDescription = statusDescription ?? "";
                    }

                    args = new AsyncOpEventArgs<TState>(this, null);
                }
                _report(args);
            }

            public void Report(string currentOperation, Exception exception)
            {
                if (exception is null)
                    throw new ArgumentNullException(nameof(exception));
                AsyncOpEventArgs<TState> args;
                lock (_syncRoot)
                {
                    CurrentOperation = currentOperation ?? "";
                    args = new AsyncOpEventArgs<TState>(this, exception);
                }
                _report(args);
            }
        }
    }
}
