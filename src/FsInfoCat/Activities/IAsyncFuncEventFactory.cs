using System;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Describes a provider for creating progress event objects.
    /// </summary>
    /// <typeparam name="TEvent">The base type of progress event objects.</typeparam>
    /// <typeparam name="TOperationEvent">The type of the operational progress event object.</typeparam>
    /// <typeparam name="TResult">The type of the result value produced by the associated activity.</typeparam>
    /// <typeparam name="TFinalEvent">The type of the final event object.</typeparam>
    /// <typeparam name="TOperationInfo">The type of the object that contains current operation information.</typeparam>
    public interface IAsyncFuncEventFactory<TEvent, TOperationEvent, TResult, TFinalEvent, TOperationInfo>
        where TEvent : IOperationEvent
        where TOperationEvent : TEvent, IActivityProgressEvent
        where TFinalEvent : TEvent, IActivityResultEvent<TResult>
        where TOperationInfo : IOperationInfo
    {
        /// <summary>
        /// Creates the initial progress event object.
        /// </summary>
        /// <param name="operation">The object that contains the initial operation information.</param>
        /// <returns>The initial event object of type <typeparamref name="TOperationEvent"/>.</returns>
        TOperationEvent CreateInitialEvent(TOperationInfo operation);

        /// <summary>
        /// Creates an operational progress event object.
        /// </summary>
        /// <param name="operation">The object that contains current operation information.</param>
        /// <param name="percentComplete">The percentage completion value or <see langword="null"/> if there is no completion value.</param>
        /// <returns>An operational event object of type <typeparamref name="TOperationEvent"/>.</returns>
        TOperationEvent CreateOperationEvent(TOperationInfo operation, int? percentComplete);

        /// <summary>
        /// Creates the final event progress object that contains the result value.
        /// </summary>
        /// <param name="operation">The object that contains the final operation information.</param>
        /// <param name="result">The value produced by the asynchronous activity.</param>
        /// <returns>The final event object of type <typeparamref name="TFinalEvent"/>.</returns>
        TFinalEvent CreateResultEvent(TOperationInfo operation, TResult result);

        /// <summary>
        /// Creates the final progress event object which indicates the activity was canceled.
        /// </summary>
        /// <param name="operation">The object that contains the final operation information.</param>
        /// <param name="percentComplete">The percentage completion value or <see langword="null"/> if there is no completion value.</param>
        /// <returns>The final event object of type <typeparamref name="TFinalEvent"/>.</returns>
        TFinalEvent CreateCanceledEvent(TOperationInfo operation, int? percentComplete);

        /// <summary>
        /// Creates the final event progress object which indicates the activity was terminated due to an unhandled exception.
        /// </summary>
        /// <param name="operation">The object that contains the final operation information.</param>
        /// <param name="exception">The unhandled exception that terminated the activity.</param>~
        /// <param name="percentComplete">The percentage completion value or <see langword="null"/> if there is no completion value.</param>
        /// <returns>The final event object of type <typeparamref name="TFinalEvent"/>.</returns>
        TFinalEvent CreateFaultedEvent(TOperationInfo operation, Exception exception, int? percentComplete);
    }
}
