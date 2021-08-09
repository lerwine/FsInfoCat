using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    /// <summary>
    /// Tracks <c><see cref="System.Func{T1, T2, TResult}"/>&lt;<typeparamref name="TState"/>, <see cref="System.Threading.CancellationToken"/>, <typeparamref name="TResult"/>&gt;</c> delegates asynchronously executed as background operations.
    /// <para>Extends <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}" />.</para>
    /// </summary>
    /// <typeparam name="TState">The type of the state object associated with the background <see cref="Task"/>.</typeparam>
    /// <typeparam name="TItem">The type of item that inherits from <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel"/>,
    /// containing the status and results of the background operation.</typeparam>
    /// <typeparam name="TListener">The type of listener that inherits from <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel.StatusListener" />
    /// and is used within the background <see cref="Task{TResult}"/> to update the associated <typeparamref name="TItem">Item</typeparamref>.</typeparam>
    /// <typeparam name="TResult">The type of the result value of the <c><see cref="System.Func{T1, T2, TResult}"/>&lt;<typeparamref name="TState"/>, <see cref="System.Threading.CancellationToken"/>, <typeparamref name="TResult"/>&gt;</c> delegate.</typeparam>
    /// <seealso cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}" />
    public class AsyncOpResultManagerViewModel<TState, TItem, TListener, TResult> : AsyncOpManagerViewModel<TState, Task<TResult>, TItem, TListener>
        where TItem : AsyncOpResultManagerViewModel<TState, TItem, TListener, TResult>.AsyncOpViewModel
        where TListener : AsyncOpResultManagerViewModel<TState, TItem, TListener, TResult>.AsyncOpViewModel.StatusListener
    {
        /// <summary>
        /// Occurs when the value of the <see cref="LastResult"/> property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler LastResultPropertyChanged;

        private static readonly DependencyPropertyKey LastResultPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastResult), typeof(TResult), typeof(AsyncOpResultManagerViewModel<TState, TItem, TListener, TResult>),
                new PropertyMetadata(default, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as AsyncOpResultManagerViewModel<TState, TItem, TListener, TResult>)?.LastResultPropertyChanged?.Invoke(d, e)));

        public static readonly DependencyProperty LastResultProperty = LastResultPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the last result value returned from a background operation.
        /// </summary>
        /// <value>The last result value returned from the background operation of a tracked <see cref="TItem"/>.</value>
        public TResult LastResult
        {
            get => (TResult)GetValue(LastResultProperty);
            private set => SetValue(LastResultPropertyKey, value);

        }
    }

    /// <summary>
    /// Tracks <c><see cref="System.Func{T, TResult}"/>&lt;<see cref="System.Threading.CancellationToken"/>, <typeparamref name="TResult"/>&gt;</c> delegates asynchronously executed as background operations.
    /// <para>Extends <see cref="AsyncOpResultManagerViewModel{object, TItem, TListener, TResult}" />.</para>
    /// </summary>
    /// <typeparam name="TItem">The type of item that inherits from <see cref="AsyncOpManagerViewModel{object, Task{TResult}, TItem, TListener}.AsyncOpViewModel"/>,
    /// containing the status and results of the background operation.</typeparam>
    /// <typeparam name="TListener">The type of listener that inherits from <see cref="AsyncOpManagerViewModel{object, Task{TResult}, TItem, TListener}.AsyncOpViewModel.StatusListener" />
    /// and is used within the background <see cref="Task{TResult}"/> to update the associated <typeparamref name="TItem">Item</typeparamref>.</typeparam>
    /// <typeparam name="TResult">The type of the result value of the <c><see cref="System.Func{T, TResult}"/>&lt;<see cref="System.Threading.CancellationToken"/>, <typeparamref name="TResult"/>&gt;</c> delegate.</typeparam>
    /// <seealso cref="AsyncOpResultManagerViewModel{object, TItem, TListener, TResult}" />
    public class AsyncOpResultManagerViewModel<TItem, TListener, TResult> : AsyncOpResultManagerViewModel<object, TItem, TListener, TResult>
        where TItem : AsyncOpResultManagerViewModel<TItem, TListener, TResult>.AsyncOpViewModel
        where TListener : AsyncOpResultManagerViewModel<TItem, TListener, TResult>.AsyncOpViewModel.StatusListener
    {
    }
}
