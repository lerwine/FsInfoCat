using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    /// <summary>
    /// Allows background operations to safely update status information to the associated <see cref="IAsyncOpViewModel">background operation view model item</typeparamref>.
    /// </summary>
    public interface IStatusListener
    {
        /// <summary>
        /// Gets the cancellation token.
        /// </summary>
        /// <value>The cancellation token.</value>
        CancellationToken CancellationToken { get; }

        /// <summary>
        /// Gets the concurrency ID.
        /// </summary>
        /// <value>The <see cref="Guid"/> that uniquely identifies the background operation.</value>
        Guid ConcurrencyId { get; }

        /// <summary>
        /// Gets the application logger.
        /// </summary>
        /// <value>The <see cref="ILogger"/> that writes to the application log.</value>
        ILogger Logger { get; }

        /// <summary>
        /// Sets the values of the <see cref="StatusMessage"/> and <see cref="MessageLevel"/> properties for the current background operation.
        /// </summary>
        /// <param name="message">The value to apply to the <see cref="StatusMessage"/> property.</param>
        /// <param name="level">The value to apply to the <see cref="MessageLevel"/> property.</param>
        /// <param name="timeout">The maximum amount of time to wait for the UI operation to start. Once the UI operation has started, it will complete before this method returns.</param>
        /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
        /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
        /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke(Action, DispatcherPriority, CancellationToken, TimeSpan)"/> method
        /// that is used to update the properties in the UI thread.</remarks>
        void SetMessage([AllowNull] string message, StatusMessageLevel level, TimeSpan timeout);

        /// <summary>
        /// Sets the values of the <see cref="StatusMessage"/> and <see cref="MessageLevel"/> properties for the current background operation.
        /// </summary>
        /// <param name="message">The value to apply to the <see cref="StatusMessage"/> property.</param>
        /// <param name="level">The value to apply to the <see cref="MessageLevel"/> property.</param>
        /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
        /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
        /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke(Action, DispatcherPriority, CancellationToken)"/> method
        /// that is used to update the properties in the UI thread.</remarks>
        void SetMessage([AllowNull] string message, StatusMessageLevel level);

        /// <summary>
        /// Sets the value of the <see cref="StatusMessage"/> property for the current background operation.
        /// </summary>
        /// <param name="message">The value to apply to the <see cref="StatusMessage"/> property.</param>
        /// <param name="timeout">The maximum amount of time to wait for the UI operation to start. Once the UI operation has started, it will complete before this method returns.</param>
        /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
        /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
        /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke(Action, DispatcherPriority, CancellationToken, TimeSpan)"/> method
        /// that is used to update the property in the UI thread.</remarks>
        void SetMessage([AllowNull] string message, TimeSpan timeout);

        /// <summary>
        /// Asynchronously sets the values of the <see cref="StatusMessage"/> and <see cref="MessageLevel"/> properties for the current background operation.
        /// </summary>
        /// <param name="message">The value to apply to the <see cref="StatusMessage"/> property.</param>
        /// <param name="level">The value to apply to the <see cref="MessageLevel"/> property.</param>
        /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
        /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
        /// <returns>A <see cref="DispatcherOperation"/> object that can be used to wait for completion while the UI operation waits the event queue.</returns>
        /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.InvokeAsync(Action, DispatcherPriority, CancellationToken)"/> method
        /// that is used to update the properties in the UI thread.</remarks>
        Task BeginSetMessage([AllowNull] string message, StatusMessageLevel level);

        /// <summary>
        /// Sets the value of the <see cref="StatusMessage"/> property for the current background operation.
        /// </summary>
        /// <param name="message">The value to apply to the <see cref="StatusMessage"/> property.</param>
        /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
        /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
        /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke(Action, DispatcherPriority, CancellationToken)"/> method
        /// that is used to update the property in the UI thread.</remarks>
        void SetMessage([AllowNull] string message);

        /// <summary>
        /// Asynchronously sets the value of the <see cref="StatusMessage"/> property for the current background operation.
        /// </summary>
        /// <param name="message">The value to apply to the <see cref="StatusMessage"/> property.</param>
        /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
        /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
        /// <returns>A <see cref="DispatcherOperation"/> object that can be used to wait for completion while the UI operation waits the event queue.</returns>
        /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.InvokeAsync(Action, DispatcherPriority, CancellationToken)"/> method
        /// that is used to update the property in the UI thread.</remarks>
        Task BeginSetMessage([AllowNull] string message);
    }
}