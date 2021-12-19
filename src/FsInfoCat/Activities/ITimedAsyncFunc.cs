namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents a timed asyncronous activity that produces a result value.
    /// </summary>
    /// <typeparam name="TResult">The type of value produced by the activity.</typeparam>
    /// <seealso cref="ITimedAsyncAction" />
    /// <seealso cref="IAsyncFunc{TResult}" />
    public interface ITimedAsyncFunc<TResult> : ITimedAsyncAction, IAsyncFunc<TResult> { }

    /// <summary>
    /// Represents a timed asyncronous activity that produces a result value and is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <typeparam name="TResult">The type of value produced by the activity.</typeparam>
    /// <seealso cref="ITimedAsyncAction{TState}" />
    /// <seealso cref="IAsyncFunc{TState, TResult}" />
    /// <seealso cref="ITimedAsyncFunc{TResult}" />
    public interface ITimedAsyncFunc<TState, TResult> : ITimedAsyncAction<TState>, IAsyncFunc<TState, TResult>, ITimedAsyncFunc<TResult> { }
}
