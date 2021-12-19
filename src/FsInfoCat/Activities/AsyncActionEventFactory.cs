using System;

namespace FsInfoCat.Activities
{
    public class AsyncActionEventFactory : IAsyncActionEventFactory<IOperationEvent, IActivityProgressEvent, IOperationEvent, IOperationInfo>
    {
        public IOperationEvent CreateFaultedEvent(IOperationInfo operation, Exception exception, int? percentComplete)
        {
            throw new NotImplementedException();
        }

        public IOperationEvent CreateFinalEvent(IOperationInfo operation, int? percentComplete, bool isCanceled)
        {
            throw new NotImplementedException();
        }

        public IActivityProgressEvent CreateInitialEvent(IOperationInfo operation)
        {
            throw new NotImplementedException();
        }

        public IActivityProgressEvent CreateOperationEvent(IOperationInfo operation, int? percentComplete)
        {
            throw new NotImplementedException();
        }
    }

    public class AsyncActionEventFactory<TState> : IAsyncActionEventFactory<IOperationEvent<TState>, IActivityProgressEvent<TState>, IOperationEvent<TState>, IOperationInfo<TState>>
    {
        public IOperationEvent<TState> CreateFaultedEvent(IOperationInfo<TState> operation, Exception exception, int? percentComplete)
        {
            throw new NotImplementedException();
        }

        public IOperationEvent<TState> CreateFinalEvent(IOperationInfo<TState> operation, int? percentComplete, bool isCanceled)
        {
            throw new NotImplementedException();
        }

        public IActivityProgressEvent<TState> CreateInitialEvent(IOperationInfo<TState> operation)
        {
            throw new NotImplementedException();
        }

        public IActivityProgressEvent<TState> CreateOperationEvent(IOperationInfo<TState> operation, int? percentComplete)
        {
            throw new NotImplementedException();
        }
    }
}
