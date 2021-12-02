using FsInfoCat.AsyncOps;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace FsInfoCat.Services
{
    /// <summary>
    /// Defines a service for reporting the progress of background processes.
    /// </summary>
    /// <remarks>
    /// Pushes a <see cref="IBackgroundProgressStartedEvent"/> notification when a background operation is started and a <see cref="IBackgroundOperationCompletedEvent"/>
    /// notification when a background operation is complete. All other events are pushed from the associated asynchronous <see cref="Task"/> through an <see cref="IBackgroundProgress{TEvent}"/> object.
    /// <para><see langword="true"/> value notifications are pushed to indicate when the service transitions from having no active background operations, and a <see langword="false"/> value notification
    /// after it transitions to having no active background operations.</para>
    /// </remarks>
    public interface IBackgroundProgressService : IBackgroundProgressFactory, IHostedService, IObservable<IBackgroundProgressEvent>, IObservable<bool>, IReadOnlyCollection<IBackgroundOperation>
    {
        /// <summary>
        /// Determines whether there are any active <see cref="IBackgroundOperation">background operations</see>.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Notifies the current <c>IBackgroundProgressService</c> that an observer is to receive <see cref="IBackgroundProgressEvent"/> notifications.
        /// </summary>
        /// <param name="observer">The object that is to receive <see cref="IBackgroundProgressEvent"/> notifications.</param>
        /// <param name="getActiveOperationsOnObserving">Delegate which gets called immediately before the <paramref name="observer"/> begins receiving notifications, providing the currently active <see cref="IBackgroundOperation"/> objects.</param>
        /// <returns>A reference to an interface that allows observers to stop receiving notifications before the current <c>IBackgroundProgressService</c> has finished sending them.</returns>
        IDisposable Subscribe(IObserver<IBackgroundProgressEvent> observer, Action<IReadOnlyList<IBackgroundOperation>> getActiveOperationsOnObserving);
    }
}
