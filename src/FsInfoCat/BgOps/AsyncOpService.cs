using FsInfoCat.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.BgOps
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public class AsyncOpService : BackgroundService, IAsyncOpService
    {
        private readonly object _syncRoot = new();
        private readonly ILogger<AsyncOpService> _logger;
        private readonly LinkedList<IAsyncAction> _backingList = new();

        public AsyncOpService(ILogger<AsyncOpService> logger)
        {
            _logger = logger;
        }

        int IReadOnlyCollection<IAsyncAction>.Count => throw new NotImplementedException();

        private TOperation FromAsync<TFactory, TTask, TOperation, TState>(TFactory operationFactory, TState state, IObserver<IAsyncOpEventArgs<TState>> observer,
            Func<TOperation, string> getFinalStatusMessage)
            where TTask : Task
            where TOperation : ICustomAsyncOperation<TState, AsyncOpEventArgs<TState>>
            where TFactory : IOperationFactory<TTask, TOperation, AsyncOpProgress<TState>, AsyncOpEventArgs<TState>>
        {
            CancellationTokenSource tokenSource = new();
            AsyncOpProgress<TState> progress = new(this, operationFactory.GetActivity(out string initialStatusMessage), initialStatusMessage, null, state, tokenSource.Token);
            TOperation asycnOp = operationFactory.CreateOperation(tokenSource, operationFactory.InvokeAsync(progress), progress, observer);
            _backingList.AddLast(asycnOp);
            if (getFinalStatusMessage is null)
            {
                if (observer is null)
                    asycnOp.Task.ContinueWith(task => _backingList.Remove(asycnOp));
                else
                    asycnOp.Task.ContinueWith(task =>
                    {
                        try
                        {
                            if (!task.IsCanceled && task.IsFaulted)
                                observer.OnError((task.Exception.InnerExceptions.Count == 1) ? task.Exception.InnerException : task.Exception);
                        }
                        finally
                        {
                            try { observer.OnCompleted(); }
                            finally { _backingList.Remove(asycnOp); }
                        }
                    });
            }
            else if (observer is null)
                asycnOp.Task.ContinueWith(task =>
                {
                    try
                    {
                        if (task.IsCanceled)
                            progress.ReportStatus(getFinalStatusMessage(asycnOp));
                        else if (task.IsFaulted)
                            progress.ReportStatus(getFinalStatusMessage(asycnOp), "", (task.Exception.InnerExceptions.Count == 1) ? task.Exception.InnerException : task.Exception);
                        else
                            progress.ReportStatus(getFinalStatusMessage(asycnOp));
                    }
                    finally { _backingList.Remove(asycnOp); }
                });
            else
                asycnOp.Task.ContinueWith(task =>
                {
                    try
                    {
                        if (task.IsCanceled)
                            progress.ReportStatus(getFinalStatusMessage(asycnOp));
                        else if (task.IsFaulted)
                            progress.ReportStatus(getFinalStatusMessage(asycnOp), "", (task.Exception.InnerExceptions.Count == 1) ? task.Exception.InnerException : task.Exception);
                        else
                            progress.ReportStatus(getFinalStatusMessage(asycnOp));
                    }
                    finally
                    {
                        try { observer.OnCompleted(); }
                        finally { _backingList.Remove(asycnOp); }
                    }
                });
            return asycnOp;
        }

        public IAsyncProducer<TState, TResult> FromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state,
            IObserver<IAsyncOpEventArgs<TState>> observer, Func<IAsyncProducer<TState, TResult>, string> getFinalStatusMessage) =>
            FromAsync<FuncOperationFactory<TState, TResult>, Task<TResult>, AsyncProducer<TState, TResult>, TState>(new(activity, initialStatusMessage, asyncMethodDelegate), state, observer,
                getFinalStatusMessage);

        public IAsyncProducer<TState, TResult> FromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state, IObserver<ITimedAsyncOpEventArgs<TState>> observer)
        {
            throw new NotImplementedException();
        }

        public IAsyncProducer<TState, TResult> FromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state, Func<IAsyncProducer<TState, TResult>, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public IAsyncProducer<TState, TResult> FromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state)
        {
            throw new NotImplementedException();
        }

        public IAsyncProducer<TResult> FromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, IObserver<IAsyncOpEventArgs> observer, Func<IAsyncProducer<TResult>, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public IAsyncProducer<TResult> FromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, IObserver<IAsyncOpEventArgs> observer)
        {
            throw new NotImplementedException();
        }

        public IAsyncProducer<TResult> FromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, Func<IAsyncProducer<TResult>, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public IAsyncProducer<TResult> FromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public TOperation FromAsync<TOperation, TEvent, TState, TResultEvent, TResult>(IFuncOperationFactory<TOperation, IAsyncOpProgress<TState>, TEvent, TResultEvent, TResult> operationFactory,
                IObserver<TEvent> observer, TState state)   
            where TOperation : ICustomAsyncProducer<TState, TEvent, TResult>
            where TEvent : IAsyncOpEventArgs<TState>
            where TResultEvent : ITimedAsyncOpResultArgs<TState, TResult>, TEvent
        {
            CancellationTokenSource tokenSource = new();
            AsyncOpProgress<TState> progress = new(this, operationFactory.GetActivity(out string initialStatusMessage), initialStatusMessage, null, state, tokenSource.Token);
            TOperation asycnOp = operationFactory.CreateOperation(tokenSource, operationFactory.InvokeAsync(progress), progress, observer);
            _backingList.AddLast(asycnOp);
            asycnOp.Task.ContinueWith(task =>
            {
                try
                {
                    if (task.IsCanceled)
                        progress.ReportStatus(operationFactory.OnCanceled(asycnOp));
                    else if (task.IsFaulted)
                    {
                        Exception exception = (task.Exception.InnerExceptions.Count == 1) ? task.Exception.InnerException : task.Exception;
                        progress.ReportStatus(operationFactory.OnFaulted(exception, asycnOp), "", exception);
                    }
                    else
                        progress.ReportStatus(operationFactory.OnRanToCompletion(asycnOp));
                }
                finally
                {
                    try { observer.OnCompleted(); }
                    finally { _backingList.Remove(asycnOp); }
                }
            });
            return asycnOp;
        }

        public TOperation FromAsync<TOperation, TEvent, TState, TResultEvent, TResult>(IFuncOperationFactory<TOperation, IAsyncOpProgress<TState>, TEvent, TResultEvent, TResult> operationFactory, TState state)
            where TOperation : ICustomAsyncProducer<TState, TEvent, TResult>
            where TEvent : IAsyncOpEventArgs<TState>
            where TResultEvent : ITimedAsyncOpResultArgs<TState, TResult>, TEvent
        {
            throw new NotImplementedException();
        }

        public TOperation FromAsync<TOperation, TEvent, TResultEvent, TResult>(IFuncOperationFactory<TOperation, IAsyncOpProgress, TEvent, TResultEvent, TResult> operationFactory, IObserver<TEvent> observer)
            where TOperation : ICustomAsyncProducer<TEvent, TResult>
            where TEvent : IAsyncOpEventArgs
            where TResultEvent : ITimedAsyncOpResultArgs<TResult>, TEvent
        {
            throw new NotImplementedException();
        }

        public TOperation FromAsync<TOperation, TEvent, TResultEvent, TResult>(IFuncOperationFactory<TOperation, IAsyncOpProgress, TEvent, TResultEvent, TResult> operationFactory)
            where TOperation : ICustomAsyncProducer<TEvent, TResult>
            where TEvent : IAsyncOpEventArgs
            where TResultEvent : ITimedAsyncOpResultArgs<TResult>, TEvent
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<TState> FromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state, IObserver<IAsyncOpEventArgs<TState>> observer, Func<IAsyncOperation<TState>, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<TState> FromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state, IObserver<IAsyncOpEventArgs<TState>> observer)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<TState> FromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state, Func<IAsyncOperation<TState>, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<TState> FromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, IObserver<IAsyncOpEventArgs> observer, Func<IAsyncOperation, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, IObserver<IAsyncOpEventArgs> observer)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, Func<IAsyncOperation, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public TOperation FromAsync<TOperation, TEvent, TState>(IActionOperationNotifyCompleteFactory<TOperation, IAsyncOpProgress<TState>, TEvent> operationFactory, IObserver<TEvent> observer, TState state)
            where TOperation : ICustomAsyncOperation<TState, TEvent>
            where TEvent : IAsyncOpEventArgs<TState>
        {
            throw new NotImplementedException();
        }

        public TOperation FromAsync<TOperation, TEvent, TState>(IActionOperationNotifyCompleteFactory<TOperation, IAsyncOpProgress<TState>, TEvent> operationFactory, TState state)
            where TOperation : ICustomAsyncOperation<TState, TEvent>
            where TEvent : IAsyncOpEventArgs<TState>
        {
            throw new NotImplementedException();
        }

        public TOperation FromAsync<TOperation, TEvent>(IActionOperationNotifyCompleteFactory<TOperation, IAsyncOpProgress, TEvent> operationFactory, IObserver<TEvent> observer)
            where TOperation : ICustomAsyncOperation<TEvent>
            where TEvent : IAsyncOpEventArgs
        {
            throw new NotImplementedException();
        }

        public TOperation FromAsync<TOperation, TEvent>(IActionOperationNotifyCompleteFactory<TOperation, IAsyncOpProgress, TEvent> operationFactory)
            where TOperation : ICustomAsyncOperation<TEvent>
            where TEvent : IAsyncOpEventArgs
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncProducer<TState, TResult> TimedFromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state, IObserver<ITimedAsyncOpEventArgs<TState>> observer, Func<ITimedAsyncProducer<TState, TResult>, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncProducer<TState, TResult> TimedFromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state, IObserver<ITimedAsyncOpEventArgs<TState>> observer)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncProducer<TState, TResult> TimedFromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state, Func<ITimedAsyncProducer<TState, TResult>, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncProducer<TState, TResult> TimedFromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncProducer<TResult> TimedFromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, IObserver<ITimedAsyncOpEventArgs> observer, Func<ITimedAsyncProducer<TResult>, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncProducer<TResult> TimedFromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, IObserver<ITimedAsyncOpEventArgs> observer)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncProducer<TResult> TimedFromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, Func<ITimedAsyncProducer<TResult>, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncProducer<TResult> TimedFromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public TOperation TimedFromAsync<TOperation, TEvent, TState, TResultEvent, TResult>(IFuncOperationFactory<TOperation, ITimedAsyncOpProgress<TState>, TEvent, TResultEvent, TResult> operationFactory, IObserver<TEvent> observer, TState state)
            where TOperation : ICustomAsyncProducer<TState, TEvent, TResult>, ITimedAsyncOperation<TState>
            where TEvent : ITimedAsyncOpEventArgs<TState>
            where TResultEvent : ITimedAsyncOpResultArgs<TState, TResult>, TEvent
        {
            throw new NotImplementedException();
        }

        public TOperation TimedFromAsync<TOperation, TEvent, TState, TResultEvent, TResult>(IFuncOperationFactory<TOperation, ITimedAsyncOpProgress<TState>, TEvent, TResultEvent, TResult> operationFactory, TState state)
            where TOperation : ICustomAsyncProducer<TState, TEvent, TResult>, ITimedAsyncOperation<TState>
            where TEvent : ITimedAsyncOpEventArgs<TState>
            where TResultEvent : ITimedAsyncOpResultArgs<TState, TResult>, TEvent
        {
            throw new NotImplementedException();
        }

        public TOperation TimedFromAsync<TOperation, TEvent, TResultEvent, TResult>(IFuncOperationFactory<TOperation, ITimedAsyncOpProgress, TEvent, TResultEvent, TResult> operationFactory, IObserver<TEvent> observer)
            where TOperation : ICustomAsyncProducer<TEvent, TResult>, ITimedAsyncOperation
            where TEvent : ITimedAsyncOpEventArgs
            where TResultEvent : ITimedAsyncOpResultArgs<TResult>, TEvent
        {
            throw new NotImplementedException();
        }

        public TOperation TimedFromAsync<TOperation, TEvent, TResultEvent, TResult>(IFuncOperationFactory<TOperation, ITimedAsyncOpProgress, TEvent, TResultEvent, TResult> operationFactory)
            where TOperation : ICustomAsyncProducer<TEvent, TResult>, ITimedAsyncOperation
            where TEvent : ITimedAsyncOpEventArgs
            where TResultEvent : ITimedAsyncOpResultArgs<TResult>, TEvent
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncOperation<TState> TimedFromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state, IObserver<ITimedAsyncOpEventArgs<TState>> observer, Func<ITimedAsyncOperation<TState>, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncOperation<TState> TimedFromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state, IObserver<ITimedAsyncOpEventArgs<TState>> observer)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncOperation<TState> TimedFromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state, Func<ITimedAsyncOperation<TState>, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncOperation<TState> TimedFromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncOperation TimedFromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, IObserver<ITimedAsyncOpEventArgs> observer, Func<ITimedAsyncOperation, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncOperation TimedFromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, IObserver<ITimedAsyncOpEventArgs> observer)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncOperation TimedFromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, Func<ITimedAsyncOperation, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncOperation TimedFromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public TOperation TimedFromAsync<TOperation, TEvent, TState, TFinalEvent>(IActionOperationNotifyCompleteFactory<TOperation, ITimedAsyncOpProgress<TState>, TEvent> operationFactory, IObserver<TEvent> observer, TState state)
            where TOperation : ICustomAsyncOperation<TState, TEvent>, ITimedAsyncOperation<TState>
            where TEvent : ITimedAsyncOpEventArgs<TState>
            where TFinalEvent : TEvent
        {
            throw new NotImplementedException();
        }

        public TOperation TimedFromAsync<TOperation, TEvent, TState>(IActionOperationNotifyCompleteFactory<TOperation, ITimedAsyncOpProgress<TState>, TEvent> operationFactory, TState state)
            where TOperation : ICustomAsyncOperation<TState, TEvent>, ITimedAsyncOperation<TState>
            where TEvent : ITimedAsyncOpEventArgs<TState>
        {
            throw new NotImplementedException();
        }

        public TOperation TimedFromAsync<TOperation, TEvent>(IActionOperationNotifyCompleteFactory<TOperation, ITimedAsyncOpProgress, TEvent> operationFactory, IObserver<TEvent> observer)
            where TOperation : ICustomAsyncOperation<TEvent>, ITimedAsyncOperation
            where TEvent : ITimedAsyncOpEventArgs
        {
            throw new NotImplementedException();
        }

        public TOperation TimedFromAsync<TOperation, TEvent>(IActionOperationNotifyCompleteFactory<TOperation, ITimedAsyncOpProgress, TEvent> operationFactory)
            where TOperation : ICustomAsyncOperation<TEvent>, ITimedAsyncOperation
            where TEvent : ITimedAsyncOpEventArgs
        {
            throw new NotImplementedException();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IAsyncAction> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
