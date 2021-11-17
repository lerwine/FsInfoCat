using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FsInfoCat.BgOps
{
    public interface IAsyncOpFactory
    {
        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation and produces the result value.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="IAsyncProducer{TState, TResult}"/> object that represents the asynchronous operation.</returns>
        public IAsyncProducer<TState, TResult> FromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state, IObserver<IAsyncOpEventArgs<TState>> observer,
            Func<IAsyncOpEventArgs<TState>, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation and produces the result value.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <returns>A <see cref="IAsyncProducer{TState, TTResult}"/> object that represents the asynchronous operation.</returns>
        public IAsyncProducer<TState, TResult> FromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state, IObserver<IAsyncOpEventArgs<TState>> observer);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation and produces the result value.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="IAsyncProducer{TState, TResult}"/> object that represents the asynchronous operation.</returns>
        public IAsyncProducer<TState, TResult> FromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state, Func<IAsyncOpEventArgs<TState>, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation and produces the result value.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <returns>A <see cref="IAsyncProducer{TState, TTResult}"/> object that represents the asynchronous operation.</returns>
        public IAsyncProducer<TState, TResult> FromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation and produces the result value.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="IAsyncProducer{TResult}"/> object that represents the asynchronous operation.</returns>
        public IAsyncProducer<TResult> FromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, IObserver<IAsyncOpEventArgs> observer, Func<IAsyncOpEventArgs, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation and produces the result value.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <returns>A <see cref="IAsyncProducer{TResult}"/> object that represents the asynchronous operation.</returns>
        public IAsyncProducer<TResult> FromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, IObserver<IAsyncOpEventArgs> observer);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation and produces the result value.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="IAsyncProducer{TResult}"/> object that represents the asynchronous operation.</returns>
        public IAsyncProducer<TResult> FromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, Func<IAsyncOpEventArgs, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation and produces the result value.</param>
        /// <returns>A <see cref="IAsyncProducer{TResult}"/> object that represents the asynchronous operation.</returns>
        public IAsyncProducer<TResult> FromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <param name="activity"></param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="IAsyncOperation{TState}"/> object that represents the asynchronous operation.</returns>
        public IAsyncOperation<TState> FromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state, IObserver<IAsyncOpEventArgs<TState>> observer, Func<IAsyncOpEventArgs<TState>, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <returns>A <see cref="IAsyncOperation{TState}"/> object that represents the asynchronous operation.</returns>
        public IAsyncOperation<TState> FromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state, IObserver<IAsyncOpEventArgs<TState>> observer);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="IAsyncOperation{TState}"/> object that represents the asynchronous operation.</returns>
        public IAsyncOperation<TState> FromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state, Func<IAsyncOpEventArgs<TState>, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <returns>A <see cref="IAsyncOperation{TState}"/> object that represents the asynchronous operation.</returns>
        public IAsyncOperation<TState> FromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="IAsyncOperation"/> object that represents the asynchronous operation.</returns>
        public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, IObserver<IAsyncOpEventArgs> observer, Func<IAsyncOpEventArgs, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <returns>A <see cref="IAsyncOperation"/> object that represents the asynchronous operation.</returns>
        public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, IObserver<IAsyncOpEventArgs> observer);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="IAsyncOperation"/> object that represents the asynchronous operation.</returns>
        public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, Func<IAsyncOpEventArgs, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <returns>A <see cref="IAsyncOperation"/> object that represents the asynchronous operation.</returns>
        public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate);
    }
}
