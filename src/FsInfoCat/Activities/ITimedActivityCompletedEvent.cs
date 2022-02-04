namespace FsInfoCat.Activities
{
    public interface ITimedActivityCompletedEvent : ITimedActivityEvent, IActivityCompletedEvent { }

    public interface ITimedActivityCompletedEvent<TState> : ITimedActivityEvent<TState>, IActivityCompletedEvent<TState>, ITimedActivityCompletedEvent { }
}
