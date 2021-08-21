using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace FsInfoCat.Desktop
{

    /// <summary>
    /// Allows background operations to safely update status information to the associated <see cref="IAsyncOpViewModel">background operation view model item</typeparamref>.
    /// </summary>
    public interface IWindowsStatusListener : IStatusListener
    {
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
        new DispatcherOperation BeginSetMessage([AllowNull] string message, StatusMessageLevel level);

        /// <summary>
        /// Asynchronously sets the value of the <see cref="StatusMessage"/> property for the current background operation.
        /// </summary>
        /// <param name="message">The value to apply to the <see cref="StatusMessage"/> property.</param>
        /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
        /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
        /// <returns>A <see cref="DispatcherOperation"/> object that can be used to wait for completion while the UI operation waits the event queue.</returns>
        /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.InvokeAsync(Action, DispatcherPriority, CancellationToken)"/> method
        /// that is used to update the property in the UI thread.</remarks>
        new DispatcherOperation BeginSetMessage([AllowNull] string message);
    }
}
