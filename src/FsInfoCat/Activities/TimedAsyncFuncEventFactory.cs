using System;

namespace FsInfoCat.Activities
{
    public class TimedAsyncFuncEventFactory<TResult> : IAsyncFuncEventFactory<ITimedOperationEvent, ITimedActivityProgressEvent, TResult, ITimedActivityResultEvent<TResult>, ITimedOperationInfo>
    {
        public ITimedActivityResultEvent<TResult> CreateCanceledEvent(ITimedOperationInfo operation, int? percentComplete)
        {
            throw new NotImplementedException();
        }

        public ITimedActivityResultEvent<TResult> CreateFaultedEvent(ITimedOperationInfo operation, Exception exception, int? percentComplete)
        {
            throw new NotImplementedException();
        }

        public ITimedActivityProgressEvent CreateInitialEvent(ITimedOperationInfo operation)
        {
            throw new NotImplementedException();
        }

        public ITimedActivityProgressEvent CreateOperationEvent(ITimedOperationInfo operation, int? percentComplete)
        {
            throw new NotImplementedException();
        }

        public ITimedActivityResultEvent<TResult> CreateResultEvent(ITimedOperationInfo operation, TResult result)
        {
            throw new NotImplementedException();
        }
    }

    public class TimedAsyncFuncEventFactory<TState, TResult> : IAsyncFuncEventFactory<ITimedOperationEvent<TState>, ITimedActivityProgressEvent<TState>, TResult, ITimedActivityResultEvent<TState, TResult>, ITimedOperationInfo<TState>>
    {
        public ITimedActivityResultEvent<TState, TResult> CreateCanceledEvent(ITimedOperationInfo<TState> operation, int? percentComplete)
        {
            throw new NotImplementedException();
        }

        public ITimedActivityResultEvent<TState, TResult> CreateFaultedEvent(ITimedOperationInfo<TState> operation, Exception exception, int? percentComplete)
        {
            throw new NotImplementedException();
        }

        public ITimedActivityProgressEvent<TState> CreateInitialEvent(ITimedOperationInfo<TState> operation)
        {
            throw new NotImplementedException();
        }

        public ITimedActivityProgressEvent<TState> CreateOperationEvent(ITimedOperationInfo<TState> operation, int? percentComplete)
        {
            throw new NotImplementedException();
        }

        public ITimedActivityResultEvent<TState, TResult> CreateResultEvent(ITimedOperationInfo<TState> operation, TResult result)
        {
            throw new NotImplementedException();
        }
    }
}
