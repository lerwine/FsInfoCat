namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents a timed asyncronous activity that does not return a value.
    /// </summary>
    /// <seealso cref="ITimedOperationEvent" />
    /// <seealso cref="IAsyncAction" />
    public interface ITimedAsyncAction : ITimedOperationEvent, IAsyncAction { }

    /// <summary>
    /// Represents a timed asyncronous activity that does not return a value and is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <seealso cref="ITimedOperationEvent{TState}" />
    /// <seealso cref="IAsyncAction{TState}" />
    /// <seealso cref="ITimedAsyncAction" />
    public interface ITimedAsyncAction<TState> : ITimedOperationEvent<TState>, IAsyncAction<TState>, ITimedAsyncAction { }
}
