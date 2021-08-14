using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class AsyncOpManagerViewModel<TState, TTask, TItem, TListener>
    {
        public abstract partial class AsyncOpViewModel
        {
            /// <summary>
            /// Allows background operations to safely status information with the associated <typeparamref name="TItem">item</typeparamref>.
            /// </summary>
            public abstract class StatusListener
            {
                private readonly TItem _item;

                /// <summary>
                /// Gets the cancellation token.
                /// </summary>
                /// <value>The cancellation token.</value>
                public CancellationToken CancellationToken { get; }

                public Guid ConcurrencyId { get; }

                public ILogger<AsyncOpViewModel> Logger { get; }

                /// <summary>
                /// Initializes a new instance of the <see cref="StatusListener"/> class.
                /// </summary>
                /// <param name="item">The item representing the background operation.</param>
                protected StatusListener(TItem item)
                {
                    _item = item;
                    CancellationToken = item.GetCancellationToken();
                    ConcurrencyId = item.ConcurrencyId;
                    Logger = item.Logger;
                }

                /// <summary>
                /// Gets the current background operation state value with the status message and level.
                /// </summary>
                /// <param name="timeout">The maximum amount of time to wait for the UI operation to start. Once the UI operation has started, it will complete before this method returns.</param>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <returns>A <c><see cref="ValueTuple{T1, T2, T3}">ValueTuple&lt;<see cref="TState"/>, <see cref="string"/>, <see cref="StatusMessageLevel"/>&gt;</see></c>:
                /// <c>(
                /// <list type="bullet">
                /// <item><term><see cref="TState"/> State</term> <description>The value from the <see cref="State"/> property.</description></item>
                /// <item><term><see cref="string"/> Text</term> <description>The value from the <see cref="StatusMessage"/> property.</description></item>
                /// <item><term><see cref="StatusMessageLevel"/> Level</term> <description>The value from the <see cref="MessageLevel"/> property.</description></item>
                /// </list>
                /// )</c></returns>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke{TResult}(Func{TResult}, DispatcherPriority, CancellationToken, TimeSpan)"/> method
                /// that is used to update the property in the UI thread.</remarks>
                public (TState State, string Text, StatusMessageLevel Level) GetStateAndMessage(TimeSpan timeout, DispatcherPriority priority = DispatcherPriority.Background) =>
                    _item.Dispatcher.Invoke(() => (_item.State, _item.StatusMessage, _item.MessageLevel), priority, CancellationToken, timeout);

                /// <summary>
                /// Gets the current background operation state value with the status message and level.
                /// </summary>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <returns>A <c><see cref="ValueTuple{T1, T2, T3}">ValueTuple&lt;<see cref="TState"/>, <see cref="string"/>, <see cref="StatusMessageLevel"/>&gt;</see></c> as:
                /// <c>(
                /// <list type="bullet">
                /// <item><term><see cref="TState"/> State</term> <description>The value from the <see cref="State"/> property.</description></item>
                /// <item><term><see cref="string"/> Text</term> <description>The value from the <see cref="StatusMessage"/> property.</description></item>
                /// <item><term><see cref="StatusMessageLevel"/> Level</term> <description>The value from the <see cref="MessageLevel"/> property.</description></item>
                /// </list>
                /// )</c></returns>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/> method
                /// that is used to update the property in the UI thread.</remarks>
                public (TState State, string Text, StatusMessageLevel Level) GetStateAndMessage(DispatcherPriority priority = DispatcherPriority.Background) =>
                    _item.Dispatcher.Invoke(() => (_item.State, _item.StatusMessage, _item.MessageLevel), priority, CancellationToken);

                /// <summary>
                /// Asynchronously gets the current background operation state value with the status message and level.
                /// </summary>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <returns>A <c><see cref="DispatcherOperation{TResult}">DispatcherOperation&lt;<see cref="ValueTuple{T1, T2, T3}">ValueTuple&lt;<see cref="TState"/>,
                /// <see cref="string"/>, <see cref="StatusMessageLevel"/>&gt;</see>&gt;</see></c> that can be used to wait for the result value while it is pending execution in the UI thread event queue.
                /// The <see cref="DispatcherOperation{TResult}.Result">the result value</see> is returned as:
                /// <c>(
                /// <list type="bullet">
                /// <item><term><see cref="TState"/> State</term> <description>The value from the <see cref="State"/> property.</description></item>
                /// <item><term><see cref="string"/> Text</term> <description>The value from the <see cref="StatusMessage"/> property.</description></item>
                /// <item><term><see cref="StatusMessageLevel"/> Level</term> <description>The value from the <see cref="MessageLevel"/> property.</description></item>
                /// </list>
                /// )</c></returns>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.InvokeAsync{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/> method
                /// that is used to update the property in the UI thread.</remarks>
                public DispatcherOperation<(TState State, string Text, StatusMessageLevel Level)> BeginGetStateAndMessage(DispatcherPriority priority = DispatcherPriority.Background) =>
                    _item.Dispatcher.InvokeAsync(new Func<(TState state, string Text, StatusMessageLevel Level)>(() => (_item.State, _item.StatusMessage, _item.MessageLevel)), priority, CancellationToken);

                /// <summary>
                /// Sets the current background operation state value along with a status message and level.
                /// </summary>
                /// <param name="state">The value to apply to the <see cref="State"/> property.</param>
                /// <param name="message">The value to apply to the <see cref="StatusMessage"/> property.</param>
                /// <param name="level">The value to apply to the <see cref="MessageLevel"/> property.</param>
                /// <param name="timeout">The maximum amount of time to wait for the UI operation to start. Once the UI operation has started, it will complete before this method returns.</param>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke(Action, DispatcherPriority, CancellationToken, TimeSpan)"/> method
                /// that is used to update the properties in the UI thread.</remarks>
                public void SetStateAndMessage(TState state, [AllowNull] string message, StatusMessageLevel level, TimeSpan timeout, DispatcherPriority priority = DispatcherPriority.Background) => _item.Dispatcher.Invoke(() =>
                {
                    _item.State = state;
                    _item.MessageLevel = level;
                    _item.StatusMessage = message ?? "";
                }, priority, CancellationToken, timeout);

                /// <summary>
                /// Sets the current background operation state value along with a status message and level.
                /// </summary>
                /// <param name="state">The value to apply to the <see cref="State"/> property.</param>
                /// <param name="message">The value to apply to the <see cref="StatusMessage"/> property.</param>
                /// <param name="level">The value to apply to the <see cref="MessageLevel"/> property.</param>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke(Action, DispatcherPriority, CancellationToken)"/> method
                /// that is used to update the properties in the UI thread.</remarks>
                public void SetStateAndMessage(TState state, [AllowNull] string message, StatusMessageLevel level, DispatcherPriority priority = DispatcherPriority.Background) =>
                    _item.Dispatcher.Invoke(() =>
                    {
                        _item.State = state;
                        _item.MessageLevel = level;
                        _item.StatusMessage = message ?? "";
                    }, priority, CancellationToken);

                /// <summary>
                /// Sets the current background operation state value along with a status message using the current <see cref="MessageLevel"/>.
                /// </summary>
                /// <param name="state">The value to apply to the <see cref="State"/> property.</param>
                /// <param name="message">The value to apply to the <see cref="StatusMessage"/> property.</param>
                /// <param name="timeout">The maximum amount of time to wait for the UI operation to start. Once the UI operation has started, it will complete before this method returns.</param>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke(Action, DispatcherPriority, CancellationToken, TimeSpan)"/> method
                /// that is used to update the properties in the UI thread.</remarks>
                public void SetStateAndMessage(TState state, [AllowNull] string message, TimeSpan timeout, DispatcherPriority priority = DispatcherPriority.Background) => _item.Dispatcher.Invoke(() =>
                {
                    _item.State = state;
                    _item.StatusMessage = message ?? "";
                }, priority, CancellationToken, timeout);

                /// <summary>
                /// Sets the current background operation state value along with a status message using the current <see cref="MessageLevel"/>.
                /// </summary>
                /// <param name="state">The value to apply to the <see cref="State"/> property.</param>
                /// <param name="message">The value to apply to the <see cref="StatusMessage"/> property.</param>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke(Action, DispatcherPriority, CancellationToken)"/> method
                /// that is used to update the properties in the UI thread.</remarks>
                public void SetStateAndMessage(TState state, [AllowNull] string message, DispatcherPriority priority = DispatcherPriority.Background) =>
                    _item.Dispatcher.Invoke(() =>
                    {
                        _item.State = state;
                        _item.StatusMessage = message ?? "";
                    }, priority, CancellationToken);

                /// <summary>
                /// Asyncrhonously sets the current background operation state value along with a status message and level.
                /// </summary>
                /// <param name="state">The value to apply to the <see cref="State"/> property.</param>
                /// <param name="message">The value to apply to the <see cref="StatusMessage"/> property.</param>
                /// <param name="level">The value to apply to the <see cref="MessageLevel"/> property.</param>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <returns>A <see cref="DispatcherOperation"/> object that can be used to wait for completion while the UI operation waits the event queue.</returns>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.InvokeAsync(Action, DispatcherPriority, CancellationToken)"/> method
                /// that is used to update the properties in the UI thread.</remarks>
                public DispatcherOperation BeginSetStateAndMessage(TState state, [AllowNull] string message, StatusMessageLevel level, DispatcherPriority priority = DispatcherPriority.Background)
                    => _item.Dispatcher.InvokeAsync(() =>
                    {
                        _item.State = state;
                        _item.MessageLevel = level;
                        _item.StatusMessage = message ?? "";
                    }, priority, CancellationToken);

                /// <summary>
                /// Asyncrhonously sets the current background operation state value along with a status message using the current <see cref="MessageLevel"/>.
                /// </summary>
                /// <param name="state">The value to apply to the <see cref="State"/> property.</param>
                /// <param name="message">The value to apply to the <see cref="StatusMessage"/> property.</param>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <returns>A <see cref="DispatcherOperation"/> object that can be used to wait for completion while the UI operation waits the event queue.</returns>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.InvokeAsync(Action, DispatcherPriority, CancellationToken)"/> method
                /// that is used to update the properties in the UI thread.</remarks>
                public DispatcherOperation BeginSetStateAndMessage(TState state, [AllowNull] string message, DispatcherPriority priority) => _item.Dispatcher.InvokeAsync(() =>
                {
                    _item.State = state;
                    _item.StatusMessage = message ?? "";
                }, priority, CancellationToken);

                /// <summary>
                /// Gets the values of the <see cref="StatusMessage"/> and <see cref="MessageLevel"/> properties for the current background operation.
                /// </summary>
                /// <param name="timeout">The maximum amount of time to wait for the UI operation to start. Once the UI operation has started, it will complete before this method returns.</param>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <returns>A <c><see cref="ValueTuple{T1, T2}">ValueTuple&lt;<see cref="string"/>, <see cref="StatusMessageLevel"/>&gt;</see></c>:
                /// <c>(
                /// <list type="bullet">
                /// <item><term><see cref="string"/> Text</term> <description>The value from the <see cref="StatusMessage"/> property.</description></item>
                /// <item><term><see cref="StatusMessageLevel"/> Level</term> <description>The value from the <see cref="MessageLevel"/> property.</description></item>
                /// </list>
                /// )</c></returns>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke{TResult}(Func{TResult}, DispatcherPriority, CancellationToken, TimeSpan)"/> method
                /// that is used to update the property in the UI thread.</remarks>
                public (string Text, StatusMessageLevel Level) GetMessage(TimeSpan timeout, DispatcherPriority priority = DispatcherPriority.Background) =>
                    _item.Dispatcher.Invoke(() => (_item.StatusMessage, _item.MessageLevel), priority, CancellationToken, timeout);

                /// <summary>
                /// Gets the values of the <see cref="StatusMessage"/> and <see cref="MessageLevel"/> properties for the current background operation.
                /// </summary>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <returns>A <c><see cref="ValueTuple{T1, T2}">ValueTuple&lt;<see cref="string"/>, <see cref="StatusMessageLevel"/>&gt;</see></c>:
                /// <c>(
                /// <list type="bullet">
                /// <item><term><see cref="string"/> Text</term> <description>The value from the <see cref="StatusMessage"/> property.</description></item>
                /// <item><term><see cref="StatusMessageLevel"/> Level</term> <description>The value from the <see cref="MessageLevel"/> property.</description></item>
                /// </list>
                /// )</c></returns>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/> method
                /// that is used to update the property in the UI thread.</remarks>
                public (string Text, StatusMessageLevel Level) GetMessage(DispatcherPriority priority = DispatcherPriority.Background) =>
                    _item.Dispatcher.Invoke(() => (_item.StatusMessage, _item.MessageLevel), priority, CancellationToken);

                /// <summary>
                /// Asynchronously gets the values of the <see cref="StatusMessage"/> and <see cref="MessageLevel"/> properties for the current background operation.
                /// </summary>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <returns>A <c><see cref="DispatcherOperation{TResult}">DispatcherOperation&lt;<see cref="ValueTuple{T1, T2}">ValueTuple&lt;<see cref="string"/>,
                /// <see cref="StatusMessageLevel"/>&gt;</see>&gt;</see></c> that can be used to wait for the result value while it is pending execution in the UI thread event queue.
                /// The <see cref="DispatcherOperation{TResult}.Result">the result value</see> is returned as:
                /// <c>(
                /// <list type="bullet">
                /// <item><term><see cref="string"/> Text</term> <description>The value from the <see cref="StatusMessage"/> property.</description></item>
                /// <item><term><see cref="StatusMessageLevel"/> Level</term> <description>The value from the <see cref="MessageLevel"/> property.</description></item>
                /// </list>
                /// )</c></returns>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.InvokeAsync{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/> method
                /// that is used to update the property in the UI thread.</remarks>
                public DispatcherOperation<(string Text, StatusMessageLevel Level)> BeginGetMessage(DispatcherPriority priority = DispatcherPriority.Background) =>
                    _item.Dispatcher.InvokeAsync(new Func<(string Text, StatusMessageLevel Level)>(() => (_item.StatusMessage, _item.MessageLevel)), priority, CancellationToken);

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
                public void SetMessage([AllowNull] string message, StatusMessageLevel level, TimeSpan timeout, DispatcherPriority priority = DispatcherPriority.Background) =>
                    _item.Dispatcher.Invoke(() =>
                    {
                        _item.MessageLevel = level;
                        _item.StatusMessage = message ?? "";
                    }, priority, CancellationToken, timeout);

                /// <summary>
                /// Sets the values of the <see cref="StatusMessage"/> and <see cref="MessageLevel"/> properties for the current background operation.
                /// </summary>
                /// <param name="message">The value to apply to the <see cref="StatusMessage"/> property.</param>
                /// <param name="level">The value to apply to the <see cref="MessageLevel"/> property.</param>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke(Action, DispatcherPriority, CancellationToken)"/> method
                /// that is used to update the properties in the UI thread.</remarks>
                public void SetMessage([AllowNull] string message, StatusMessageLevel level, DispatcherPriority priority = DispatcherPriority.Background) =>
                    _item.Dispatcher.Invoke(() =>
                    {
                        _item.MessageLevel = level;
                        _item.StatusMessage = message ?? "";
                    }, priority, CancellationToken);

                /// <summary>
                /// Sets the value of the <see cref="StatusMessage"/> property for the current background operation.
                /// </summary>
                /// <param name="message">The value to apply to the <see cref="StatusMessage"/> property.</param>
                /// <param name="timeout">The maximum amount of time to wait for the UI operation to start. Once the UI operation has started, it will complete before this method returns.</param>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke(Action, DispatcherPriority, CancellationToken, TimeSpan)"/> method
                /// that is used to update the property in the UI thread.</remarks>
                public void SetMessage([AllowNull] string message, TimeSpan timeout, DispatcherPriority priority = DispatcherPriority.Background) =>
                    _item.Dispatcher.Invoke(() => _item.StatusMessage = message ?? "", priority, CancellationToken, timeout);

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
                public DispatcherOperation BeginSetMessage([AllowNull] string message, StatusMessageLevel level, DispatcherPriority priority = DispatcherPriority.Background) =>
                    _item.Dispatcher.InvokeAsync(() =>
                    {
                        _item.MessageLevel = level;
                        _item.StatusMessage = message ?? "";
                    }, priority, CancellationToken);

                /// <summary>
                /// Sets the value of the <see cref="StatusMessage"/> property for the current background operation.
                /// </summary>
                /// <param name="message">The value to apply to the <see cref="StatusMessage"/> property.</param>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke(Action, DispatcherPriority, CancellationToken)"/> method
                /// that is used to update the property in the UI thread.</remarks>
                public void SetMessage([AllowNull] string message, DispatcherPriority priority = DispatcherPriority.Background) =>
                    _item.Dispatcher.Invoke(() => _item.StatusMessage = message ?? "", priority, CancellationToken);

                /// <summary>
                /// Asynchronously sets the value of the <see cref="StatusMessage"/> property for the current background operation.
                /// </summary>
                /// <param name="message">The value to apply to the <see cref="StatusMessage"/> property.</param>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <returns>A <see cref="DispatcherOperation"/> object that can be used to wait for completion while the UI operation waits the event queue.</returns>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.InvokeAsync(Action, DispatcherPriority, CancellationToken)"/> method
                /// that is used to update the property in the UI thread.</remarks>
                public DispatcherOperation BeginSetMessage([AllowNull] string message, DispatcherPriority priority = DispatcherPriority.Background) =>
                    _item.Dispatcher.InvokeAsync(() => _item.StatusMessage = message ?? "", priority, CancellationToken);

                /// <summary>
                /// Gets the value of the <see cref="State"/> property for the current background operation.
                /// </summary>
                /// <param name="timeout">The maximum amount of time to wait for the UI operation to start. Once the UI operation has started, it will complete before this method returns.</param>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <returns>The <typeparamref name="TState"/> value retrieved from the <see cref="State"/> property.</returns>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke{TResult}(Func{TResult}, DispatcherPriority, CancellationToken, TimeSpan)"/> method
                /// that is used to update the property in the UI thread.</remarks>
                public TState GetState(TimeSpan timeout, DispatcherPriority priority = DispatcherPriority.Background) => _item.Dispatcher.Invoke(() => _item.State, priority, CancellationToken, timeout);

                /// <summary>
                /// Gets the current value of the <see cref="State"/> property for the current background operation.
                /// </summary>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <returns>The <typeparamref name="TState"/> value retrieved from the <see cref="State"/> property.</returns>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/> method
                /// that is used to update the property in the UI thread.</remarks>
                public TState GetState(DispatcherPriority priority = DispatcherPriority.Background) => _item.Dispatcher.Invoke(() => _item.State, priority, CancellationToken);

                /// <summary>
                /// Asynchronously gets the value of the <see cref="State"/> property for the current background operation.
                /// </summary>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <returns>A <c><see cref="DispatcherOperation{TResult}">DispatcherOperation&lt;<typeparamref name="TState"/>&gt;</see></c>
                /// that can be used to wait for the result value while it is pending execution in the UI thread event queue.</returns>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.InvokeAsync{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/> method
                /// that is used to update the property in the UI thread.</remarks>
                public DispatcherOperation<TState> BeginGetState(DispatcherPriority priority = DispatcherPriority.Background) => _item.Dispatcher.InvokeAsync(new Func<TState>(() => _item.State),
                    priority, CancellationToken);

                /// <summary>
                /// Sets the value of the <see cref="State"/> property for the current background operation.
                /// </summary>
                /// <param name="state">The value to apply to the <see cref="State"/> property.</param>
                /// <param name="timeout">The maximum amount of time to wait for the UI operation to start. Once the UI operation has started, it will complete before this method returns.</param>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke(Action, DispatcherPriority, CancellationToken, TimeSpan)"/> method
                /// that is used to update the property in the UI thread.</remarks>
                public void SetState(TState state, TimeSpan timeout, DispatcherPriority priority = DispatcherPriority.Background) =>
                    _item.Dispatcher.Invoke(() => _item.State = state, priority, CancellationToken, timeout);

                /// <summary>
                /// Sets the value of the <see cref="State"/> property for the current background operation.
                /// </summary>
                /// <param name="state">The value to apply to the <see cref="State"/> property.</param>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.Invoke(Action, DispatcherPriority, CancellationToken)"/> method
                /// that is used to update the property in the UI thread.</remarks>
                public void SetState(TState state, DispatcherPriority priority = DispatcherPriority.Background) => _item.Dispatcher.Invoke(() => _item.State = state, priority, CancellationToken);

                /// <summary>
                /// Asynchronously sets the value of the <see cref="State"/> property for the current background operation.
                /// </summary>
                /// <param name="state">The value to apply to the <see cref="State"/> property.</param>
                /// <param name="priority">The optional priority that determines the order in which the specified callback is invoked to the other pending operations in the <see cref="Dispatcher"/>.
                /// The default value is <see cref="DispatcherPriority.Background"/>.</param>
                /// <returns>A <see cref="DispatcherOperation"/> object that can be used to wait for completion while the UI operation waits the event queue.</returns>
                /// <remarks>The current <see cref="CancellationToken"/> is passed to the <see cref="Dispatcher.InvokeAsync(Action, DispatcherPriority, CancellationToken)"/> method
                /// that is used to update the property in the UI thread.</remarks>
                public DispatcherOperation BeginSetState(TState state, DispatcherPriority priority = DispatcherPriority.Background) =>
                    _item.Dispatcher.InvokeAsync(() => _item.State = state, priority, CancellationToken);

                /// <summary>
                /// This is the first call made by the <see cref="Task"/> delegate to put the associated <typeparamref name="TItem">item</typeparamref> into the <see cref="AsyncOpStatusCode.Running"/>
                /// state.
                /// </summary>
                internal void RaiseTaskStarted()
                {
                    if (_item.CheckAccess())
                        Logger.LogWarning("Task started UI dispatcher thread: Concurrency ID = {ConcurrencyId}; Task.CurrentId = {TaskId}", nameof(RaiseTaskStarted), ConcurrencyId, Task.CurrentId);
                    else
                        Logger.LogDebug("{MethodName}() invoked: Concurrency ID = {ConcurrencyId}; Task.CurrentId = {TaskId}", nameof(RaiseTaskStarted), ConcurrencyId, Task.CurrentId);
                    DateTime started;
                    lock (_item._stopWatch)
                    {
                        if (!_item._stopWatch.IsRunning)
                            _item._stopWatch.Start();
                        started = DateTime.Now;
                        if (_item._timer is null)
                            _item._timer = new Timer(UpdateElapsedTime, null, 1000, 1000);
                    }
                    _item.Dispatcher.Invoke(() =>
                    {
                        try { _item.Started = started; }
                        finally
                        {
                            if (_item.AsyncOpStatus != AsyncOpStatusCode.CancellationPending)
                                _item.AsyncOpStatus = AsyncOpStatusCode.Running;
                        }
                    }, DispatcherPriority.Background);
                    CancellationToken.ThrowIfCancellationRequested();
                }

                private void UpdateElapsedTime(object state)
                {
                    _item.Dispatcher.Invoke(() => _item.Duration = _item._stopWatch.Elapsed, DispatcherPriority.Background, CancellationToken);
                }

                /// <summary>
                /// This is invoked from the <see cref="Task.ContinueWith(Action{Task})">task continuation</see> of the background operation within
                /// the <see cref="Add(AsyncOpManagerViewModel{TState, TTask, TItem, TListener}, TItem)"/> method.
                /// </summary>
                /// <param name="task">The completed <typeparamref name="TTask"/> that implemented the background operation.</param>
                /// <param name="owner">The <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}"/> that is tracking the background operation.</param>
                protected internal virtual void RaiseTaskCompleted(TTask task, AsyncOpManagerViewModel<TState, TTask, TItem, TListener> owner)
                {
                    Logger.LogDebug("{MethodName}({task}, {owner}) invoked: Concurrency ID = {ConcurrencyId}; Task = {{ Id = {TaskId}; Status = {TaskStatus} }}", nameof(RaiseTaskCompleted), nameof(TTask),
                        nameof(AsyncOpManagerViewModel<TState, TTask, TItem, TListener>), ConcurrencyId, task.Id, task.Status);
                    DateTime stopped;
                    Timer timer;
                    lock (_item._stopWatch)
                    {
                        if (_item._stopWatch.IsRunning)
                            _item._stopWatch.Stop();
                        stopped = DateTime.Now;
                        timer = _item._timer;
                        _item._timer = null;
                    }
                    if (timer is not null)
                        timer.Dispose();
                    _item.Dispatcher.Invoke(() =>
                    {
                        try { _item.UpdateOpStatus(task.Status); }
                        finally
                        {
                            try
                            {
                                _item.Stopped = stopped;
                                _item.Duration = _item._stopWatch.Elapsed;
                            }
                            finally
                            {
                                try { _item.AsyncOpStatusPropertyChanged -= owner.Item_AsyncOpStatusPropertyChanged; }
                                finally
                                {
                                    try { _item.RaiseCompleted(task); }
                                    finally { owner.RaiseOperationCompleted(_item, task); }
                                }
                            }
                        }
                    });
                }
            }
        }
    }
}
