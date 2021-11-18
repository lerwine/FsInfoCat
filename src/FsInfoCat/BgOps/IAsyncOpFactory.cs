using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FsInfoCat.BgOps
{
    public interface IAsyncOpFactory
    {
        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation and produces the result value.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="ITimedAsyncProducer{TState, TResult}"/> object that represents the asynchronous operation.</returns>
        public ITimedAsyncProducer<TState, TResult> TimedFromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state, IObserver<ITimedAsyncOpEventArgs<TState>> observer,
            Func<ITimedAsyncProducer<TState, TResult>, string> getFinalStatusMessage);

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
            Func<IAsyncProducer<TState, TResult>, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation and produces the result value.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <returns>A <see cref="ITimedAsyncProducer{TState, TTResult}"/> object that represents the asynchronous operation.</returns>
        public ITimedAsyncProducer<TState, TResult> TimedFromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state, IObserver<ITimedAsyncOpEventArgs<TState>> observer);

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
        public IAsyncProducer<TState, TResult> FromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state, IObserver<ITimedAsyncOpEventArgs<TState>> observer);

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation and produces the result value.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="ITimedAsyncProducer{TState, TResult}"/> object that represents the asynchronous operation.</returns>
        public ITimedAsyncProducer<TState, TResult> TimedFromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state,
            Func<ITimedAsyncProducer<TState, TResult>, string> getFinalStatusMessage);

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
        public IAsyncProducer<TState, TResult> FromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state, Func<IAsyncProducer<TState, TResult>, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation and produces the result value.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <returns>A <see cref="ITimedAsyncProducer{TState, TTResult}"/> object that represents the asynchronous operation.</returns>
        public ITimedAsyncProducer<TState, TResult> TimedFromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state);

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
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation and produces the result value.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="ITimedAsyncProducer{TResult}"/> object that represents the asynchronous operation.</returns>
        public ITimedAsyncProducer<TResult> TimedFromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, IObserver<ITimedAsyncOpEventArgs> observer,
            Func<ITimedAsyncProducer<TResult>, string> getFinalStatusMessage);

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
        public IAsyncProducer<TResult> FromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, IObserver<IAsyncOpEventArgs> observer, Func<IAsyncProducer<TResult>, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation and produces the result value.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <returns>A <see cref="ITimedAsyncProducer{TResult}"/> object that represents the asynchronous operation.</returns>
        public ITimedAsyncProducer<TResult> TimedFromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, IObserver<ITimedAsyncOpEventArgs> observer);

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
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation and produces the result value.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="ITimedAsyncProducer{TResult}"/> object that represents the asynchronous operation.</returns>
        public ITimedAsyncProducer<TResult> TimedFromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, Func<ITimedAsyncProducer<TResult>, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation and produces the result value.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="IAsyncProducer{TResult}"/> object that represents the asynchronous operation.</returns>
        public IAsyncProducer<TResult> FromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, Func<IAsyncProducer<TResult>, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation and produces the result value.</param>
        /// <returns>A <see cref="ITimedAsyncProducer{TResult}"/> object that represents the asynchronous operation.</returns>
        public ITimedAsyncProducer<TResult> TimedFromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate);

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
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TOperation">The type of the result operation object.</typeparam>
        /// <typeparam name="TEvent">The type of the operation progress event.</typeparam>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="operationFactory">The delegate method that creates the result operation object.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <returns>A <see cref="ITimedAsyncOperation{TState}"/>, <see cref="ICustomAsyncProducer{TState, TEvent, TResult}"/> object that represents the asynchronous operation.</returns>
        public TOperation TimedFromAsync<TOperation, TEvent, TState, TResultEvent, TResult>(IFuncOperationFactory<TOperation, ITimedAsyncOpProgress<TState>, TEvent, TResultEvent, TResult> operationFactory, IObserver<TEvent> observer, TState state)
            where TEvent : ITimedAsyncOpEventArgs<TState>
            where TResultEvent : TEvent, ITimedAsyncOpResultArgs<TState, TResult>
            where TOperation : ICustomAsyncProducer<TState, TEvent, TResult>, ITimedAsyncOperation<TState>;

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <typeparam name="TOperation">The type of the result operation object.</typeparam>
        /// <typeparam name="TEvent">The type of the operation progress event.</typeparam>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="operationFactory">The delegate method that creates the result operation object.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <returns>A <see cref="ICustomAsyncProducer{TState, TEvent, TResult}"/> object that represents the asynchronous operation.</returns>
        public TOperation FromAsync<TOperation, TEvent, TState, TResultEvent, TResult>(IFuncOperationFactory<TOperation, IAsyncOpProgress<TState>, TEvent, TResultEvent, TResult> operationFactory, IObserver<TEvent> observer, TState state)
            where TEvent : IAsyncOpEventArgs<TState>
            where TResultEvent : TEvent, ITimedAsyncOpResultArgs<TState, TResult>
            where TOperation : ICustomAsyncProducer<TState, TEvent, TResult>;

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TOperation">The type of the result operation object.</typeparam>
        /// <typeparam name="TEvent">The type of the operation progress event.</typeparam>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="operationFactory">The delegate method that creates the result operation object.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <returns>A <see cref="ITimedAsyncOperation{TState}"/>, <see cref="ICustomAsyncProducer{TState, TEvent, TResult}"/> object that represents the asynchronous operation.</returns>
        public TOperation TimedFromAsync<TOperation, TEvent, TState, TResultEvent, TResult>(IFuncOperationFactory<TOperation, ITimedAsyncOpProgress<TState>, TEvent, TResultEvent, TResult> operationFactory, TState state)
            where TEvent : ITimedAsyncOpEventArgs<TState>
            where TResultEvent : TEvent, ITimedAsyncOpResultArgs<TState, TResult>
            where TOperation : ICustomAsyncProducer<TState, TEvent, TResult>, ITimedAsyncOperation<TState>;

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <typeparam name="TOperation">The type of the result operation object.</typeparam>
        /// <typeparam name="TEvent">The type of the operation progress event.</typeparam>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="operationFactory">The delegate method that creates the result operation object.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <returns>A <see cref="ICustomAsyncProducer{TState, TEvent, TResult}"/> object that represents the asynchronous operation.</returns>
        public TOperation FromAsync<TOperation, TEvent, TState, TResultEvent, TResult>(IFuncOperationFactory<TOperation, IAsyncOpProgress<TState>, TEvent, TResultEvent, TResult> operationFactory, TState state)
            where TEvent : IAsyncOpEventArgs<TState>
            where TResultEvent : TEvent, ITimedAsyncOpResultArgs<TState, TResult>
            where TOperation : ICustomAsyncProducer<TState, TEvent, TResult>;

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TOperation">The type of the result operation object.</typeparam>
        /// <typeparam name="TEvent">The type of the operation progress event.</typeparam>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="operationFactory">The delegate method that creates the result operation object.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <returns>A <see cref="ITimedAsyncOperation"/>, <see cref="ICustomAsyncProducer{TEvent, TResult}"/> object that represents the asynchronous operation.</returns>
        public TOperation TimedFromAsync<TOperation, TEvent, TResultEvent, TResult>(IFuncOperationFactory<TOperation, ITimedAsyncOpProgress, TEvent, TResultEvent, TResult> operationFactory, IObserver<TEvent> observer)
            where TEvent : ITimedAsyncOpEventArgs
            where TResultEvent : TEvent, ITimedAsyncOpResultArgs<TResult>
            where TOperation : ICustomAsyncProducer<TEvent, TResult>, ITimedAsyncOperation;

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <typeparam name="TOperation">The type of the result operation object.</typeparam>
        /// <typeparam name="TEvent">The type of the operation progress event.</typeparam>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="operationFactory">The delegate method that creates the result operation object.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <returns>A <see cref="ICustomAsyncProducer{TEvent, TResult}"/> object that represents the asynchronous operation.</returns>
        public TOperation FromAsync<TOperation, TEvent, TResultEvent, TResult>(IFuncOperationFactory<TOperation, IAsyncOpProgress, TEvent, TResultEvent, TResult> operationFactory, IObserver<TEvent> observer)
            where TEvent : IAsyncOpEventArgs
            where TResultEvent : TEvent, ITimedAsyncOpResultArgs<TResult>
            where TOperation : ICustomAsyncProducer<TEvent, TResult>;

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TOperation">The type of the result operation object.</typeparam>
        /// <typeparam name="TEvent">The type of the operation progress event.</typeparam>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="operationFactory">The delegate method that creates the result operation object.</param>
        /// <returns>A <see cref="ITimedAsyncOperation"/>, <see cref="ICustomAsyncProducer{TEvent, TResult}"/> object that represents the asynchronous operation.</returns>
        public TOperation TimedFromAsync<TOperation, TEvent, TResultEvent, TResult>(IFuncOperationFactory<TOperation, ITimedAsyncOpProgress, TEvent, TResultEvent, TResult> operationFactory)
            where TEvent : ITimedAsyncOpEventArgs
            where TResultEvent : TEvent, ITimedAsyncOpResultArgs<TResult>
            where TOperation : ICustomAsyncProducer<TEvent, TResult>, ITimedAsyncOperation;

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <typeparam name="TOperation">The type of the result operation object.</typeparam>
        /// <typeparam name="TEvent">The type of the operation progress event.</typeparam>
        /// <typeparam name="TResult">The type of value returned by the asynchronous operation.</typeparam>
        /// <param name="operationFactory">The delegate method that creates the result operation object.</param>
        /// <returns>A <see cref="ICustomAsyncProducer{TState, TEvent, TResult}"/> object that represents the asynchronous operation.</returns>
        public TOperation FromAsync<TOperation, TEvent, TResultEvent, TResult>(IFuncOperationFactory<TOperation, IAsyncOpProgress, TEvent, TResultEvent, TResult> operationFactory)
            where TEvent : IAsyncOpEventArgs
            where TResultEvent : TEvent, ITimedAsyncOpResultArgs<TResult>
            where TOperation : ICustomAsyncProducer<TEvent, TResult>;

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <param name="activity"></param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="ITimedAsyncOperation{TState}"/> object that represents the asynchronous operation.</returns>
        public ITimedAsyncOperation<TState> TimedFromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state, IObserver<ITimedAsyncOpEventArgs<TState>> observer,
            Func<ITimedAsyncOperation<TState>, string> getFinalStatusMessage);
        
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
        public IAsyncOperation<TState> FromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state, IObserver<IAsyncOpEventArgs<TState>> observer,
            Func<IAsyncOperation<TState>, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <returns>A <see cref="ITimedAsyncOperation{TState}"/> object that represents the asynchronous operation.</returns>
        public ITimedAsyncOperation<TState> TimedFromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state, IObserver<ITimedAsyncOpEventArgs<TState>> observer);

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
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="ITimedAsyncOperation{TState}"/> object that represents the asynchronous operation.</returns>
        public ITimedAsyncOperation<TState> TimedFromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state, Func<ITimedAsyncOperation<TState>, string> getFinalStatusMessage);

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
        public IAsyncOperation<TState> FromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state, Func<IAsyncOperation<TState>, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <returns>A <see cref="ITimedAsyncOperation{TState}"/> object that represents the asynchronous operation.</returns>
        public ITimedAsyncOperation<TState> TimedFromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state);

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
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="ITimedAsyncOperation"/> object that represents the asynchronous operation.</returns>
        public ITimedAsyncOperation TimedFromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, IObserver<ITimedAsyncOpEventArgs> observer, Func<ITimedAsyncOperation, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="IAsyncOperation"/> object that represents the asynchronous operation.</returns>
        public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, IObserver<IAsyncOpEventArgs> observer, Func<IAsyncOperation, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <returns>A <see cref="ITimedAsyncOperation"/> object that represents the asynchronous operation.</returns>
        public ITimedAsyncOperation TimedFromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, IObserver<ITimedAsyncOpEventArgs> observer);

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
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="ITimedAsyncOperation"/> object that represents the asynchronous operation.</returns>
        public ITimedAsyncOperation TimedFromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, Func<ITimedAsyncOperation, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <param name="getFinalStatusMessage">Gets the final <see cref="IAsyncOpInfo.StatusDescription"/> value.</param>
        /// <returns>A <see cref="IAsyncOperation"/> object that represents the asynchronous operation.</returns>
        public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, Func<IAsyncOperation, string> getFinalStatusMessage);

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <returns>A <see cref="ITimedAsyncOperation"/> object that represents the asynchronous operation.</returns>
        public ITimedAsyncOperation TimedFromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate);

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <param name="activity">Describes the activity being peformed by the background operation.</param>
        /// <param name="initialStatusMessage">The initial status message to display for the background operation.</param>
        /// <param name="asyncMethodDelegate">A delegate that refers to the asynchonous method that performs the background operation.</param>
        /// <returns>A <see cref="IAsyncOperation"/> object that represents the asynchronous operation.</returns>
        public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate);

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TOperation">The type of the result operation object.</typeparam>
        /// <typeparam name="TEvent">The type of the operation progress event.</typeparam>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <param name="operationFactory">The delegate method that creates the result operation object.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <returns>A <see cref="ITimedAsyncOperation{TState}"/>, <see cref="ICustomAsyncOperation{TState, TEvent}"/> object that represents the asynchronous operation.</returns>
        public TOperation TimedFromAsync<TOperation, TEvent, TState, TFinalEvent>(IActionOperationNotifyCompleteFactory<TOperation, ITimedAsyncOpProgress<TState>, TEvent> operationFactory, IObserver<TEvent> observer, TState state)
            where TEvent : ITimedAsyncOpEventArgs<TState>
            where TFinalEvent : TEvent
            where TOperation : ICustomAsyncOperation<TState, TEvent>, ITimedAsyncOperation<TState>;

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TOperation">The type of the result operation object.</typeparam>
        /// <typeparam name="TEvent">The type of the operation progress event.</typeparam>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <param name="operationFactory">The delegate method that creates the result operation object.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <returns>A <see cref="ICustomAsyncOperation{TState, TEvent}"/> object that represents the asynchronous operation.</returns>
        public TOperation FromAsync<TOperation, TEvent, TState>(IActionOperationNotifyCompleteFactory<TOperation, IAsyncOpProgress<TState>, TEvent> operationFactory, IObserver<TEvent> observer, TState state)
            where TEvent : IAsyncOpEventArgs<TState>
            where TOperation : ICustomAsyncOperation<TState, TEvent>;

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TOperation">The type of the result operation object.</typeparam>
        /// <typeparam name="TEvent">The type of the operation progress event.</typeparam>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <param name="operationFactory">The delegate method that creates the result operation object.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <returns>A <see cref="ITimedAsyncOperation{TState}"/>, <see cref="ICustomAsyncOperation{TState, TEvent}"/> object that represents the asynchronous operation.</returns>
        public TOperation TimedFromAsync<TOperation, TEvent, TState>(IActionOperationNotifyCompleteFactory<TOperation, ITimedAsyncOpProgress<TState>, TEvent> operationFactory, TState state)
            where TEvent : ITimedAsyncOpEventArgs<TState>
            where TOperation : ICustomAsyncOperation<TState, TEvent>, ITimedAsyncOperation<TState>;

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TOperation">The type of the result operation object.</typeparam>
        /// <typeparam name="TEvent">The type of the operation progress event.</typeparam>
        /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
        /// <param name="operationFactory">The delegate method that creates the result operation object.</param>
        /// <param name="state">The user-defined object that qualifies or contains information about the asynchronous operation.</param>
        /// <returns>A <see cref="ICustomAsyncOperation{TState, TEvent}"/> object that represents the asynchronous operation.</returns>
        public TOperation FromAsync<TOperation, TEvent, TState>(IActionOperationNotifyCompleteFactory<TOperation, IAsyncOpProgress<TState>, TEvent> operationFactory, TState state)
            where TEvent : IAsyncOpEventArgs<TState>
            where TOperation : ICustomAsyncOperation<TState, TEvent>;

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TOperation">The type of the result operation object.</typeparam>
        /// <typeparam name="TEvent">The type of the operation progress event.</typeparam>
        /// <param name="operationFactory">The delegate method that creates the result operation object.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <returns>A <see cref="ITimedAsyncOperation"/>, <see cref="ICustomAsyncOperation{TEvent}"/> object that represents the asynchronous operation.</returns>
        public TOperation TimedFromAsync<TOperation, TEvent>(IActionOperationNotifyCompleteFactory<TOperation, ITimedAsyncOpProgress, TEvent> operationFactory, IObserver<TEvent> observer)
            where TEvent : ITimedAsyncOpEventArgs
            where TOperation : ICustomAsyncOperation<TEvent>, ITimedAsyncOperation;

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TOperation">The type of the result operation object.</typeparam>
        /// <typeparam name="TEvent">The type of the operation progress event.</typeparam>
        /// <param name="operationFactory">The delegate method that creates the result operation object.</param>
        /// <param name="observer">Observer for asynchronous operation status update events.</param>
        /// <returns>A <see cref="ICustomAsyncOperation{TEvent}"/> object that represents the asynchronous operation.</returns>
        public TOperation FromAsync<TOperation, TEvent>(IActionOperationNotifyCompleteFactory<TOperation, IAsyncOpProgress, TEvent> operationFactory, IObserver<TEvent> observer)
            where TEvent : IAsyncOpEventArgs
            where TOperation : ICustomAsyncOperation<TEvent>;

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TOperation">The type of the result operation object.</typeparam>
        /// <typeparam name="TEvent">The type of the operation progress event.</typeparam>
        /// <param name="operationFactory">The delegate method that creates the result operation object.</param>
        /// <returns>A <see cref="ITimedAsyncOperation"/>, <see cref="ICustomAsyncOperation{TEvent}"/> object that represents the asynchronous operation.</returns>
        public TOperation TimedFromAsync<TOperation, TEvent>(IActionOperationNotifyCompleteFactory<TOperation, ITimedAsyncOpProgress, TEvent> operationFactory)
            where TEvent : ITimedAsyncOpEventArgs
            where TOperation : ICustomAsyncOperation<TEvent>, ITimedAsyncOperation;

        /// <summary>
        /// Starts a new timed asynchronous operation.
        /// </summary>
        /// <typeparam name="TOperation">The type of the result operation object.</typeparam>
        /// <typeparam name="TEvent">The type of the operation progress event.</typeparam>
        /// <param name="operationFactory">The delegate method that creates the result operation object.</param>
        /// <returns>A <see cref="ICustomAsyncOperation{TEvent}"/> object that represents the asynchronous operation.</returns>
        public TOperation FromAsync<TOperation, TEvent>(IActionOperationNotifyCompleteFactory<TOperation, IAsyncOpProgress, TEvent> operationFactory)
        where TEvent : IAsyncOpEventArgs
        where TOperation : ICustomAsyncOperation<TEvent>;
    }
}
