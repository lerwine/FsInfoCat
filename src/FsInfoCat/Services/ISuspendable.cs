using System;
using System.ComponentModel;

namespace FsInfoCat.Services
{
    /// <summary>
    /// Interface for an object that supports a suspended state
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public interface ISuspendable : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when this object enters a suspended state.
        /// </summary>
        event EventHandler BeginSuspension;

        /// <summary>
        /// Occurs when this object enters a suspended state.
        /// </summary>
        event EventHandler EndSuspension;

        /// <summary>
        /// Gets an object that can be used to synchronize access to this object.
        /// </summary>
        /// <value>
        /// An object that can be used to synchronize access to this object.
        /// </value>
        object SyncRoot { get; }

        /// <summary>
        /// Gets a value indicating whether this object is in a suspended state.
        /// </summary>
        /// <value>
        /// <c>true</c> if this object is in a suspended state; otherwise, <c>false</c>.
        /// </value>
        bool IsSuspended { get; }

        /// <summary>
        /// Asserts that this object is not in a suspended state.
        /// </summary>
        /// <exception cref="InvalidOperationException">This object is in a suspended state.</exception>
        void AssertNotSuspended();

        /// <summary>
        /// Invokes a delegate method, asserting that this object is not in a suspended state.
        /// </summary>
        /// <param name="action"><see cref="Action"/> to invoke.</param>
        /// <exception cref="InvalidOperationException">This object is in a suspended state.</exception>
        void AssertNotSuspended(Action action);

        /// <summary>
        /// Calls a delegate method, asserting that this object is not in a suspended state.
        /// </summary>
        /// <typeparam name="TResult">The type of the value returned by the delegate method.</typeparam>
        /// <param name="function">The <see cref="Func{TResult}"/> to call.</param>
        /// <returns>The value returned by the delegate <paramref name="function"/>.</returns>
        TResult AssertNotSuspended<TResult>(Func<TResult> function);

        /// <summary>
        /// Invokes a delegate method if this object is not in a suspended state.
        /// </summary>
        /// <param name="ifNotSuspended"><see cref="Action"/> to invoke if this object is not in a suspended state.</param>
        /// <returns>
        /// <c>true</c> if the <c><paramref name="ifNotSuspended"/></c> delegate method was invoked; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>A thread lock is obtained on <see cref="SyncRoot"/> while the delegate method is being invoked.</remarks>
        bool IfNotSuspended(Action ifNotSuspended);

        /// <summary>
        /// Invokes a delegate method if this object is in a suspended state.
        /// </summary>
        /// <param name="ifSuspended"><see cref="Action"/> to invoke if this object is in a suspended state.</param>
        /// <returns>
        /// <c>true</c> if the <c><paramref name="ifSuspended"/></c> delegate method was invoked; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>A thread lock is obtained on <see cref="SyncRoot"/> while the delegate method is being invoked.</remarks>
        bool IfSuspended(Action ifSuspended);

        /// <summary>
        /// Invokes either of a pair of delegate methods, depending upon the suspension state.
        /// </summary>
        /// <param name="ifNotSuspended"><see cref="Action"/> to invoke if this object is not in a suspended state.</param>
        /// <param name="ifSuspended"><see cref="Action"/> to invoke if this object is in a suspended state.</param>
        /// <remarks>A thread lock is obtained on <see cref="SyncRoot"/> while the delegate method is being invoked.</remarks>
        void IfNotSuspended(Action ifNotSuspended, Action ifSuspended);

        /// <summary>
        /// Calls either of a pair of delegate methods, depending upon the suspension state.
        /// </summary>
        /// <typeparam name="TResult">The type of the result value returned by the delegate method.</typeparam>
        /// <param name="ifNotSuspended">The <see cref="Func{TResult}"/> to call if this object is not in a suspended state.</param>
        /// <param name="ifSuspended">The <see cref="Func{TResult}"/> to call if this object is in a suspended state.</param>
        /// <returns>The return value of the <c><paramref name="ifNotSuspended"/></c> delegate method that was invoked if this object was in a suspended state;
        /// otherwise, the return value of the <c><paramref name="ifNotSuspended"/></c> delegate method.</returns>
        TResult IfNotSuspended<TResult>(Func<TResult> ifNotSuspended, Func<TResult> ifSuspended);

        /// <summary>
        /// Calls a delegate method or returns a given value, depending upon the suspended state.
        /// </summary>
        /// <typeparam name="TResult">The type of the result value returned by the delegate method.</typeparam>
        /// <param name="ifNotSuspended">The <see cref="Func{TResult}"/> to call if this object is not in a suspended state.</param>
        /// <param name="ifSuspended">The value to return if this object is in a suspended state.</param>
        /// <returns>The return value of the <c><paramref name="ifNotSuspended"/></c> delegate method that was invoked if this object was in a suspended state; otherwise, the value of <paramref name="ifSuspended"/>.</returns>
        TResult IfNotSuspended<TResult>(Func<TResult> ifNotSuspended, TResult ifSuspended);

        /// <summary>
        /// Returns a given value or calls a delegate method, depending upon the suspended state..
        /// </summary>
        /// <typeparam name="TResult">The type of the result value returned by the delegate method.</typeparam>
        /// <param name="ifNotSuspended">The value to return if this object is not in a suspended state.</param>
        /// <param name="ifSuspended">The <see cref="Func{TResult}"/> to call if this object is in a suspended state.</param>
        /// <returns>The value of <c><paramref name="ifNotSuspended"/></c> if this object was in a suspended state; otherwise, the return value of the <c><paramref name="ifNotSuspended"/></c> delegate method.</returns>
        TResult IfNotSuspended<TResult>(TResult ifNotSuspended, Func<TResult> ifSuspended);

        /// <summary>
        /// Puts this object into the suspended state and obtains a thread lock on <see cref="SyncRoot"/>.
        /// </summary>
        /// <returns>A <see cref="ISuspension"/> object that maintains the suspended state of this object until it is disposed. If this method is subsequently called, this object will remain in a suspended state
        /// untile there are no instances of <see cref="ISuspension"/>, that were created by this object, which have not been disposed.</returns>
        ISuspension Suspend();

        /// <summary>
        /// Puts this object into the suspended state, optionally obtaining a thread lock on <see cref="SyncRoot"/>.
        /// </summary>
        /// <param name="noThreadLock"><c>true</c> enter the suspended state with no thread lock; otherwise <see langword="false"/>.</param>
        /// <returns>A <see cref="ISuspension"/> object that maintains the suspended state of this object until it is disposed. If this method is subsequently called, this object will remain in a suspended state
        /// untile there are no instances of <see cref="ISuspension"/>, that were created by this object, which have not been disposed.</returns>
        ISuspension Suspend(bool noThreadLock);
    }
}
