using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.BgOps
{
    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public class AsyncOpProgress : IAsyncOpProgress
    {
        private readonly IAsyncOpFactory _factory;

        public AsyncOpProgress(IAsyncOpFactory factory, string activity, string initialStatusDescriptiony, IAsyncOpEventArgs parentOperation, CancellationToken token)
        {
            _factory = factory;
            Activity = activity ?? "";
            StatusDescription = initialStatusDescriptiony ?? "";
            ParentOperation = parentOperation;
            Token = token;
        }

        public CancellationToken Token { get; }

        public IAsyncOpEventArgs ParentOperation { get; }

        public Guid Id { get; } = Guid.NewGuid();

        public string Activity { get; }

        public string StatusDescription { get; }

        public string CurrentOperation { get; }

        IAsyncOpInfo IAsyncOpInfo.ParentOperation => ParentOperation;

        public IAsyncProducer<TState, TResult> FromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state, IObserver<IAsyncOpEventArgs<TState>> observer, Func<IAsyncProducer<TState, TResult>, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

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

        public TOperation FromAsync<TOperation, TEvent, TState, TResultEvent, TResult>(IFuncOperationFactory<TOperation, IAsyncOpProgress<TState>, TEvent, TResultEvent, TResult> operationFactory, IObserver<TEvent> observer, TState state)
            where TOperation : ICustomAsyncProducer<TState, TEvent, TResult>
            where TEvent : IAsyncOpEventArgs<TState>
            where TResultEvent : ITimedAsyncOpResultArgs<TState, TResult>, TEvent
        {
            throw new NotImplementedException();
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

        public void Report(string currentOperation, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Report(string value)
        {
            throw new NotImplementedException();
        }

        public void Report(Exception value)
        {
            throw new NotImplementedException();
        }

        public void ReportStatus(string statusDescription, string currentOperation, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void ReportStatus(string statusDescription, string currentOperation)
        {
            throw new NotImplementedException();
        }

        public void ReportStatus(string statusDescription)
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
    }

    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public class AsyncOpProgress<TState> : AsyncOpProgress, IAsyncOpProgress<TState>
    {
        public AsyncOpProgress(IAsyncOpFactory factory, string activity, string initialStatusDescription, IAsyncOpEventArgs parentOperation, TState state, CancellationToken token)
            : base(factory, activity, initialStatusDescription, parentOperation, token)
        {
            AsyncState = state;
        }

        public TState AsyncState { get; }
    }
}
