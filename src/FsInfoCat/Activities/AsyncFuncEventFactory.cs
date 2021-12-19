using System;

namespace FsInfoCat.Activities
{
    public class AsyncFuncEventFactory<TResult> : IAsyncFuncEventFactory<IOperationEvent, IActivityProgressEvent, TResult, IActivityResultEvent<TResult>, IOperationInfo>
    {
        public IActivityResultEvent<TResult> CreateCanceledEvent(IOperationInfo operation, int? percentComplete)
        {
            throw new NotImplementedException();
        }

        public IActivityResultEvent<TResult> CreateFaultedEvent(IOperationInfo operation, Exception exception, int? percentComplete)
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

        public IActivityResultEvent<TResult> CreateResultEvent(IOperationInfo operation, TResult result)
        {
            throw new NotImplementedException();
        }
    }

    public class AsyncFuncEventFactory<TState, TResult> : IAsyncFuncEventFactory<IOperationEvent<TState>, IActivityProgressEvent<TState>, TResult, IActivityResultEvent<TState, TResult>, IOperationInfo<TState>>
    {
        public IActivityResultEvent<TState, TResult> CreateCanceledEvent(IOperationInfo<TState> operation, int? percentComplete)
        {
            throw new NotImplementedException();
        }

        public IActivityResultEvent<TState, TResult> CreateFaultedEvent(IOperationInfo<TState> operation, Exception exception, int? percentComplete)
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

        public IActivityResultEvent<TState, TResult> CreateResultEvent(IOperationInfo<TState> operation, TResult result)
        {
            throw new NotImplementedException();
        }
    }
}
