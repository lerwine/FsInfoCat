using System;

namespace FsInfoCat.Activities
{
    public class TimedAsyncActionEventFactory : IAsyncActionEventFactory<ITimedOperationEvent, ITimedActivityProgressEvent, ITimedOperationEvent, ITimedOperationInfo>
    {
        public ITimedOperationEvent CreateFaultedEvent(ITimedOperationInfo operation, Exception exception, int? percentComplete)
        {
            throw new NotImplementedException();
        }

        public ITimedOperationEvent CreateFinalEvent(ITimedOperationInfo operation, int? percentComplete, bool isCanceled)
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
    }

    public class TimedAsyncActionEventFactory<TState> : IAsyncActionEventFactory<ITimedOperationEvent<TState>, ITimedActivityProgressEvent<TState>, ITimedOperationEvent<TState>, ITimedOperationInfo<TState>>
    {
        public ITimedOperationEvent<TState> CreateFaultedEvent(ITimedOperationInfo<TState> operation, Exception exception, int? percentComplete)
        {
            throw new NotImplementedException();
        }

        public ITimedOperationEvent<TState> CreateFinalEvent(ITimedOperationInfo<TState> operation, int? percentComplete, bool isCanceled)
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
    }
}
