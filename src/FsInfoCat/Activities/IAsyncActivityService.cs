using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace FsInfoCat.Activities
{
    public interface IAsyncActivityService : IHostedService, IObservable<IOperationEvent>, IObservable<bool>, IAsyncActivityProvider
    {
        /// <summary>
        /// Determines whether there are any active or pending <see cref="IAsyncAction">background operations</see>.
        /// </summary>
        /// <value><see langword="true"/> if there is at least one active or pending <see cref="IAsyncAction">background operation</see>, otherwise, <see langword="false"/>.</value>
        bool IsActive { get; }

        /// <summary>
        /// Notifies the current <c>IAsyncActivityService</c> that an observer is to receive <see cref="IOperationEvent"/> notifications.
        /// </summary>
        /// <param name="observer">The object that is to receive <see cref="IOperationEvent"/> notifications.</param>
        /// <param name="getActiveOperationsOnObserving">Delegate which gets called immediately before the <paramref name="observer"/> begins receiving notifications, providing <see cref="IOperationEvent"/>
        /// events for the currently active and pending <see cref="IAsyncAction">background operations</see>.
        /// <para>If there are no active or pending background operations, <paramref name="getActiveOperationsOnObserving"/> will be invoked with an empty list.</para></param>
        /// <returns>A reference to an interface that allows observers to stop receiving notifications before the current <c>IAsyncActivityService</c> has finished sending them.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is <see langword="null"/>.</exception>
        IDisposable Subscribe(IObserver<IOperationEvent> observer, Action<IReadOnlyList<IOperationEvent>> getActiveOperationsOnObserving);
    }
}
