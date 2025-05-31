namespace FsInfoCat.Activities;

/// <summary>
/// Represents timed operational information about the state of an activity.
/// </summary>
public interface ITimedOperationInfo : ITimedActivityInfo, IOperationInfo { }

/// <summary>
/// Represents timed operational information about the state of an activity that is associated with a user-specified value.
/// </summary>
/// <typeparam name="TState">The type of the user-defined value that is associated with this activity.</typeparam>
public interface ITimedOperationInfo<TState> : ITimedActivityInfo<TState>, IOperationInfo<TState>, ITimedOperationInfo { }
