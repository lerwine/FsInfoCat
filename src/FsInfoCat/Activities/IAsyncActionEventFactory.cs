using System;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Describes a provider for creating progress event objects.
    /// </summary>
    /// <typeparam name="TEvent">The base type of progress event objects.</typeparam>
    /// <typeparam name="TProgressEvent">The type of the operational progress event object.</typeparam>
    /// <typeparam name="TFinalEvent">The type of the final event object.</typeparam>
    /// <typeparam name="TOperationInfo">The type of the object that contains current operation information.</typeparam>
    public interface IAsyncActionEventFactory<TEvent, TProgressEvent, TFinalEvent, TOperationInfo>
        where TEvent : IOperationEvent
        where TProgressEvent : TEvent, IActivityProgressEvent
        where TFinalEvent : TEvent, IOperationEvent
        where TOperationInfo : IOperationInfo
    {
        /// <summary>
        /// Creates the initial progress event object.
        /// </summary>
        /// <param name="operation">The object that contains the initial operation information.</param>
        /// <returns>The initial event object of type <typeparamref name="TProgressEvent"/>.</returns>
        TProgressEvent CreateInitialEvent(TOperationInfo operation);

        /// <summary>
        /// Creates an operational progress event object.
        /// </summary>
        /// <param name="operation">The object that contains current operation information.</param>
        /// <param name="percentComplete">The percentage completion value or <see langword="null"/> if there is no completion value.</param>
        /// <returns>An operational event object of type <typeparamref name="TProgressEvent"/>.</returns>
        TProgressEvent CreateOperationEvent(TOperationInfo operation, int? percentComplete);

        /// <summary>
        /// Creates the final event progress object.
        /// </summary>
        /// <param name="operation">The object that contains the final operation information.</param>
        /// <param name="percentComplete">The percentage completion value or <see langword="null"/> if there is no completion value.</param>
        /// <param name="isCanceled"><see langword="true"/> if the activity had been canceled; otherwise, <see langword="false"/>.</param>
        /// <returns>The final event object of type <typeparamref name="TFinalEvent"/>.</returns>
        TFinalEvent CreateFinalEvent(TOperationInfo operation, int? percentComplete, bool isCanceled);

        /// <summary>
        /// Creates the final event progress object which indicates the activity was terminated due to an unhandled exception.
        /// </summary>
        /// <param name="operation">The object that contains the final operation information.</param>
        /// <param name="exception">The unhandled exception that terminated the activity.</param>
        /// <param name="percentComplete">The percentage completion value or <see langword="null"/> if there is no completion value.</param>
        /// <returns>The final event object of type <typeparamref name="TFinalEvent"/>.</returns>
        TFinalEvent CreateFaultedEvent(TOperationInfo operation, Exception exception, int? percentComplete);
    }
}
