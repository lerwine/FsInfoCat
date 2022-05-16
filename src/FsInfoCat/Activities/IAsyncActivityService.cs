using Microsoft.Extensions.Hosting;
using System;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents a hosted service that can be used to start asynchronous activities.
    /// </summary>
    /// <seealso cref="IHostedService" />
    /// <seealso cref="IObservable{bool}" />
    /// <seealso cref="IAsyncActivityProvider" />
    /// <remarks>This is also an <seealso cref="IObservable{bool}" /> provider that sends boolean notifications whereby a <see langword="true"/> is pushed when the service
    /// transitions from having no activities; and a <see langword="false"/> is pushed when the service transitions to having no more activities.</remarks>
    public interface IAsyncActivityService : IHostedService, IObservable<bool>, IAsyncActivityProvider
    {
        /// <summary>
        /// Gets a value indicating whether there is at least one active <see cref="IAsyncActivity"/>.
        /// </summary>
        /// <value><see langword="true"/></c> if there is at least one active <see cref="IAsyncActivity"/>; otherwise, <see langword="false"/>.</value>
        bool IsActive { get; }
    }
}
