using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    /// <summary>
    /// Base class for view models which indicate the status of background operations implemented through a <see cref="Func{TState, AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel.StatusListener, TResult}"/>.
    /// <para>Extends <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel" />.</para>
    /// </summary>
    /// <typeparam name="TState">The type of the state object associated with the background <see cref="Task{TResult}"/>.</typeparam>
    /// <typeparam name="TResult">The type of the result value produced by the background operation.</typeparam>
    /// <typeparam name="TItem">The type of item that inherits from <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel" />,
    /// containing the status and results of the background operation.</typeparam>
    /// <typeparam name="TListener">The type of listener that inherits from <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel.StatusListener" />
    /// and is used within the background <see cref="Task{TResult}"/> to update the associated <typeparamref name="TItem">Item</typeparamref>.</typeparam>
    /// <seealso cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel" />
    public abstract class AsyncFuncOpViewModelBase<TState, TResult, TListener, TItem> : AsyncOpResultManagerViewModel<TState, TItem, TListener, TResult>.AsyncOpViewModel
        where TListener : AsyncOpManagerViewModel<TState, Task<TResult>, TItem, TListener>.AsyncOpViewModel.StatusListener
        where TItem : AsyncFuncOpViewModelBase<TState, TResult, TListener, TItem>
    {
        /// <summary>
        /// Occurs when the <see cref="Result"/> property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler ResultPropertyChanged;

        private static readonly DependencyPropertyKey ResultPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Result), typeof(TResult), typeof(AsyncFuncOpViewModelBase<TState, TResult, TListener, TItem>),
                new PropertyMetadata(default, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as AsyncFuncOpViewModelBase<TState, TResult, TListener, TItem>)?.ResultPropertyChanged?.Invoke(d, e)));

        public static readonly DependencyProperty ResultProperty = ResultPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the result value produced by the background operation.
        /// </summary>
        /// <value>The result value returned by the background <see cref="Task{TResult}"/>.</value>
        public TResult Result
        {
            get => (TResult)GetValue(ResultProperty);
            private set => SetValue(ResultPropertyKey, value);
        }

        protected AsyncFuncOpViewModelBase([DisallowNull] FuncItemBuilder builder, [AllowNull] Action<TListener> onListenerCreated) : base(builder, onListenerCreated) { }

        protected abstract class FuncItemBuilder : AsyncOpManagerViewModel<TState, Task<TResult>, TItem, TListener>.ItemBuilder
        {
            private readonly Func<TListener, Task<TResult>> _createTask;
            protected FuncItemBuilder(TState initialState, Func<TListener, Task<TResult>> createTask) : base(initialState)
            {
                _createTask = createTask;
            }
            protected internal override Task<TResult> GetTask(TState state, TListener listener, AsyncOpManagerViewModel<TState, Task<TResult>, TItem, TListener>.AsyncOpViewModel instance) =>
                _createTask(listener);
        }
    }
}
