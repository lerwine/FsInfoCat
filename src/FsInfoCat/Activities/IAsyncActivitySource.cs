using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents an object that is the source of asynchronous activity events.
    /// </summary>
    /// <seealso cref="IReadOnlyCollection{IAsyncActivity}" />
    /// <remarks>This contains a collection of <see cref="IAsyncActivity"/> objects representing asynchronous activities that have not yet completed. Once an activity has completed, it will be automatically removed from this collection.</remarks>
    public interface IAsyncActivitySource : IReadOnlyCollection<IAsyncActivity>
    {
        /// <summary>
        /// Notifies this activity source that an observer is to receive activity start notifications, providing a list of existing activities.
        /// </summary>
        /// <param name="observer">The object that is to receive activity start notifications.</param>
        /// <param name="onObserving">The callback method that provides a list of existing activities immediately before the observer is registered to receive activity start notifications.</param>
        /// <returns>A reference to an interface that allows observers to stop receiving notifications before this has finished sending them.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> or <paramref name="onObserving"/> is <see langword="null"/>.</exception>
        IDisposable SubscribeChildActivityStart([DisallowNull] IObserver<IAsyncActivity> observer, [DisallowNull] Action<IAsyncActivity[]> onObserving);

        /// <summary>
        /// Gets the provider for activity start notifications.
        /// </summary>
        /// <value>The provider that can be used to subscribe for activity start notifications.</value>
        IObservable<IAsyncActivity> ActivityStartedObservable { get; }
    }
}
