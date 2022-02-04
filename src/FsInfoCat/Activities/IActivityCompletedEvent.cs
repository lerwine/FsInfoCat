namespace FsInfoCat.Activities
{
    public interface IActivityCompletedEvent : IActivityEvent, IActivityStatusInfo { }

    public interface IActivityCompletedEvent<TState> : IActivityEvent<TState>, IActivityStatusInfo<TState>, IActivityCompletedEvent { }
}
