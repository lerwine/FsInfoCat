using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{

    /// <summary>
    /// Base class for view models that track background operations, providing bindable status information properties.
    /// <para>Extends <see cref="DependencyObject" />.</para>
    /// </summary>
    /// <typeparam name="TState">The type of the state object associated with the background <see cref="Task"/>.</typeparam>
    /// <typeparam name="TTask">The type of <see cref="Task"/> executed as the background operation.</typeparam>
    /// <typeparam name="TItem">The type of item that inherits from <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel" />,
    /// containing the status and results of the background operation.</typeparam>
    /// <typeparam name="TListener">The type of listener that inherits from <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.StatusListener" />
    /// and is used within the background <typeparamref name="TTask">Task</typeparamref> to update the associated <typeparamref name="TItem">Item</typeparamref>.</typeparam>
    /// <seealso cref="DependencyObject" />
    public partial class AsyncOpManagerViewModel<TState, TTask, TItem, TListener> : DependencyObject, IAsyncOpManagerViewModel
        where TTask : Task
        where TItem : AsyncOpManagerViewModel<TState, TTask, TItem, TListener>.AsyncOpViewModel
        where TListener : AsyncOpManagerViewModel<TState, TTask, TItem, TListener>.AsyncOpViewModel.StatusListener
    {
        protected readonly ILogger<AsyncOpManagerViewModel<TState, TTask, TItem, TListener>> Logger;
        private readonly ObservableCollection<TItem> _pendingOperations = new();
        private readonly ObservableCollection<TItem> _activeOperations = new();
        private readonly ObservableCollection<TItem> _completedOperations = new();
        private readonly ObservableCollection<TItem> _failedOperations = new();
        private readonly ObservableCollection<TItem> _canceledOperations = new();
        private readonly ObservableCollection<TItem> _successsfulOperations = new();
        private readonly ObservableCollection<TItem> _allOperations = new();

        #region Events

        /// <summary>
        /// Occurs when the the <see cref="IsBusy"/> property has changed.
        /// </summary>
        /// <remarks>This event is raised on the UI thread.</remarks>
        public event DependencyPropertyChangedEventHandler IsBusyPropertyChanged;

        /// <summary>
        /// Occurs when a background operation ran to completion without fault or cancellation.
        /// </summary>
        /// <remarks>This event is raised on the UI thread.</remarks>
        public event EventHandler<OpItemEventArgs<TState, TTask, TItem, TListener>> OperationRanToCompletion;

        /// <summary>
        /// Occurs when a background operation has been canceled.
        /// </summary>
        /// <remarks>This event is raised on the UI thread.</remarks>
        public event EventHandler<OpItemEventArgs<TState, TTask, TItem, TListener>> OperationCanceled;

        /// <summary>
        /// Occurs when a background operation was completed due to an unhandled exception.
        /// </summary>
        /// <remarks>This event is raised on the UI thread.</remarks>
        public event EventHandler<OpItemFailedEventArgs<TState, TTask, TItem, TListener>> OperationFailed;

        #endregion
        #region Dependency Properties

        #region IsBusy Property Members

        private static readonly DependencyPropertyKey IsBusyPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsBusy), typeof(bool), typeof(AsyncOpManagerViewModel<TState, TTask, TItem, TListener>),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as AsyncOpManagerViewModel<TState, TTask, TItem, TListener>)?.RaiseIsBusyPropertyChanged(e)));

        public static readonly DependencyProperty IsBusyProperty = IsBusyPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets a value indicating whether this instance contains one or more active background operations.
        /// </summary>
        /// <value><see langword="true"/> if <see cref="ActiveOperations"/> is not empty; otherwise, <see langword="false"/> to indicate there are no active background operations.</value>
        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            private set => SetValue(IsBusyPropertyKey, value);
        }

        private void RaiseIsBusyPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            Logger.LogInformation("{PropertyName} changed from {OldValue} to {NewValue}", args.Property.Name, args.OldValue, args.NewValue);
            IsBusyPropertyChanged?.Invoke(this, args);
        }

        #endregion

        #region PendingOperations Property Members

        private static readonly DependencyPropertyKey PendingOperationsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PendingOperations), typeof(ReadOnlyObservableCollection<TItem>), typeof(AsyncOpManagerViewModel<TState, TTask, TItem, TListener>),
                new PropertyMetadata(null));

        public static readonly DependencyProperty PendingOperationsProperty = PendingOperationsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the pending operation items.
        /// </summary>
        /// <value>The items that represent pending operations.</value>
        public ReadOnlyObservableCollection<TItem> PendingOperations
        {
            get => (ReadOnlyObservableCollection<TItem>)GetValue(PendingOperationsProperty);
            private set => SetValue(PendingOperationsPropertyKey, value);
        }

        #endregion

        #region ActiveOperations Property Members

        private static readonly DependencyPropertyKey ActiveOperationsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ActiveOperations), typeof(ReadOnlyObservableCollection<TItem>), typeof(AsyncOpManagerViewModel<TState, TTask, TItem, TListener>),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ActiveOperationsProperty = ActiveOperationsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the active operation items.
        /// </summary>
        /// <value>The items that represent active operations.</value>
        public ReadOnlyObservableCollection<TItem> ActiveOperations
        {
            get => (ReadOnlyObservableCollection<TItem>)GetValue(ActiveOperationsProperty);
            private set => SetValue(ActiveOperationsPropertyKey, value);
        }

        #endregion

        #region CompletedOperations Property Members

        private static readonly DependencyPropertyKey CompletedOperationsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CompletedOperations), typeof(ReadOnlyObservableCollection<TItem>), typeof(AsyncOpManagerViewModel<TState, TTask, TItem, TListener>),
                new PropertyMetadata(null));

        public static readonly DependencyProperty CompletedOperationsProperty = CompletedOperationsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the completed operation items.
        /// </summary>
        /// <value>The items that represent completed operations.</value>
        public ReadOnlyObservableCollection<TItem> CompletedOperations
        {
            get => (ReadOnlyObservableCollection<TItem>)GetValue(CompletedOperationsProperty);
            private set => SetValue(CompletedOperationsPropertyKey, value);
        }

        #endregion

        #region FailedOperations Property Members

        private static readonly DependencyPropertyKey FailedOperationsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FailedOperations), typeof(ReadOnlyObservableCollection<TItem>), typeof(AsyncOpManagerViewModel<TState, TTask, TItem, TListener>),
                new PropertyMetadata(null));

        public static readonly DependencyProperty FailedOperationsProperty = FailedOperationsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the failed operation items.
        /// </summary>
        /// <value>The items that represent operations that were completed due to an unhandled exception.</value>
        public ReadOnlyObservableCollection<TItem> FailedOperations
        {
            get => (ReadOnlyObservableCollection<TItem>)GetValue(FailedOperationsProperty);
            private set => SetValue(FailedOperationsPropertyKey, value);
        }

        #endregion

        #region CanceledOperations Property Members

        private static readonly DependencyPropertyKey CanceledOperationsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CanceledOperations), typeof(ReadOnlyObservableCollection<TItem>), typeof(AsyncOpManagerViewModel<TState, TTask, TItem, TListener>),
                new PropertyMetadata(null));

        public static readonly DependencyProperty CanceledOperationsProperty = CanceledOperationsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the canceled operation items.
        /// </summary>
        /// <value>The items that represent operations that were completed due to cancellation.</value>
        public ReadOnlyObservableCollection<TItem> CanceledOperations
        {
            get => (ReadOnlyObservableCollection<TItem>)GetValue(CanceledOperationsProperty);
            private set => SetValue(CanceledOperationsPropertyKey, value);
        }

        #endregion

        #region SuccessfulOperations Property Members

        private static readonly DependencyPropertyKey SuccessfulOperationsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SuccessfulOperations), typeof(ReadOnlyObservableCollection<TItem>), typeof(AsyncOpManagerViewModel<TState, TTask, TItem, TListener>),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SuccessfulOperationsProperty = SuccessfulOperationsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the successful operation items.
        /// </summary>
        /// <value>The items that represent operations that ran to completion without fault or cancellation.</value>
        public ReadOnlyObservableCollection<TItem> SuccessfulOperations
        {
            get => (ReadOnlyObservableCollection<TItem>)GetValue(SuccessfulOperationsProperty);
            private set => SetValue(SuccessfulOperationsPropertyKey, value);
        }

        #endregion

        #region AllOperations Property Members

        private static readonly DependencyPropertyKey AllOperationsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AllOperations), typeof(ReadOnlyObservableCollection<TItem>), typeof(AsyncOpManagerViewModel<TState, TTask, TItem, TListener>),
                new PropertyMetadata(null));

        public static readonly DependencyProperty AllOperationsProperty = AllOperationsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets all operation items.
        /// </summary>
        /// <value>The items that represent all operations tracked by this instance.</value>
        public ReadOnlyObservableCollection<TItem> AllOperations
        {
            get => (ReadOnlyObservableCollection<TItem>)GetValue(AllOperationsProperty);
            private set => SetValue(AllOperationsPropertyKey, value);
        }

        #endregion

        #endregion

        public AsyncOpManagerViewModel()
        {
            Logger = App.GetLogger(this);
            PendingOperations = new(_pendingOperations);
            ActiveOperations = new(_activeOperations);
            CompletedOperations = new(_completedOperations);
            FailedOperations = new(_failedOperations);
            CanceledOperations = new(_canceledOperations);
            SuccessfulOperations = new(_successsfulOperations);
            AllOperations = new(_allOperations);
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            _activeOperations.CollectionChanged += ActiveOperations_CollectionChanged;
        }

        public void CancelAll(bool throwOnFirstException)
        {
            if (CheckAccess())
            {
                TItem[] items = _pendingOperations.Concat(_activeOperations).ToArray();
                Logger.LogInformation("Cancelling {OperationCount} operation(s): throwOnFirstException = {throwOnFirstException}", items.Length, throwOnFirstException);
                foreach (TItem item in items)
                    item.Cancel(throwOnFirstException);
            }
            else
            {
                Logger.LogInformation("Queueing {MethodName} invocation on UI thread: throwOnFirstException = {throwOnFirstException}", nameof(CancelAll), throwOnFirstException);
                Dispatcher.Invoke(() => CancelAll(throwOnFirstException));
            }
        }

        public void CancelAll()
        {
            if (CheckAccess())
            {
                TItem[] items = _pendingOperations.Concat(_activeOperations).ToArray();
                Logger.LogInformation("Cancelling {OperationCount} operation(s)", items.Length);
                foreach (TItem item in items)
                    item.Cancel();
            }
            else
            {
                Logger.LogInformation("Queueing {MethodName} invocation on UI thread", nameof(CancelAll));
                Dispatcher.Invoke(() => CancelAll());
            }
        }

        /// <summary>
        /// Starts a pending background operation.
        /// </summary>
        /// <param name="item">The <typeparamref name="TItem"/> representing the background operation to start.</param>
        /// <param name="scheduler">The optional scheduler to use. The default is the <see cref="TaskScheduler.Default">scheduler provided by the .NET framework</see>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The specified <paramref name="item"/> is not in the <see cref="PendingOperations"/> collection.</exception>
        public void StartPendingOperation(TItem item, TaskScheduler scheduler = null)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            VerifyAccess();
            if (!_pendingOperations.Contains(item))
                throw new ArgumentOutOfRangeException(nameof(item));
            item.GetTask().Start(scheduler ?? TaskScheduler.Default);
        }

        private void ActiveOperations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => IsBusy = _activeOperations.Count > 0;

        private void RaiseOperationCompleted(TItem item, Task task)
        {
            Dispatcher.Invoke(() =>
            {
                switch (task.Status)
                {
                    case TaskStatus.Faulted:
                        RaiseOperationFailed(item, task.Exception);
                        break;
                    case TaskStatus.Canceled:
                        RaiseOperationCanceled(item);
                        break;
                    default:
                        RaiseOperationRanToCompletion(item);
                        break;
                }
            });
        }

        private void Item_AsyncOpStatusPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is TItem item && _allOperations.Contains(item)))
                return;
            switch ((AsyncOpStatusCode)e.NewValue)
            {
                case AsyncOpStatusCode.NotStarted:
                    if (_pendingOperations.Contains(item))
                        return;
                    try
                    {
                        if (_activeOperations.Contains(item))
                            _activeOperations.Remove(item);
                        else
                            try
                            {
                                if (_completedOperations.Contains(item))
                                    _completedOperations.Remove(item);
                            }
                            finally
                            {
                                if (_successsfulOperations.Contains(item))
                                    _successsfulOperations.Remove(item);
                                else if (_failedOperations.Contains(item))
                                    _failedOperations.Remove(item);
                                else if (_canceledOperations.Contains(item))
                                    _canceledOperations.Remove(item);
                            }
                    }
                    finally { _pendingOperations.Add(item); }
                    break;
                case AsyncOpStatusCode.RanToCompletion:
                    if (_successsfulOperations.Contains(item))
                        return;
                    try
                    {
                        if (!_completedOperations.Contains(item))
                            _completedOperations.Add(item);
                    }
                    finally
                    {
                        try
                        {
                            if (_pendingOperations.Contains(item))
                                _pendingOperations.Remove(item);
                            else if (_activeOperations.Contains(item))
                                _activeOperations.Remove(item);
                            else if (_failedOperations.Contains(item))
                                _failedOperations.Remove(item);
                            else if (_canceledOperations.Contains(item))
                                _canceledOperations.Remove(item);
                        }
                        finally { _successsfulOperations.Add(item); }
                    }

                    break;
                case AsyncOpStatusCode.Faulted:
                    if (_failedOperations.Contains(item))
                        return;
                    try
                    {
                        if (!_completedOperations.Contains(item))
                            _completedOperations.Add(item);
                    }
                    finally
                    {
                        try
                        {
                            if (_pendingOperations.Contains(item))
                                _pendingOperations.Remove(item);
                            else if (_activeOperations.Contains(item))
                                _activeOperations.Remove(item);
                            else if (_successsfulOperations.Contains(item))
                                _successsfulOperations.Remove(item);
                            else if (_canceledOperations.Contains(item))
                                _canceledOperations.Remove(item);
                        }
                        finally { _failedOperations.Add(item); }
                    }
                    break;
                case AsyncOpStatusCode.Canceled:
                    if (_canceledOperations.Contains(item))
                        return;
                    try
                    {
                        if (!_completedOperations.Contains(item))
                            _completedOperations.Add(item);
                    }
                    finally
                    {
                        try
                        {
                            if (_pendingOperations.Contains(item))
                                _pendingOperations.Remove(item);
                            else if (_activeOperations.Contains(item))
                                _activeOperations.Remove(item);
                            else if (_successsfulOperations.Contains(item))
                                _successsfulOperations.Remove(item);
                            else if (_failedOperations.Contains(item))
                                _failedOperations.Remove(item);
                            if (!_completedOperations.Contains(item))
                                _completedOperations.Add(item);
                        }
                        finally { _canceledOperations.Add(item); }
                    }
                    break;
                default:
                    if (_activeOperations.Contains(item))
                        return;
                    try { _activeOperations.Add(item); }
                    finally
                    {
                        if (_pendingOperations.Contains(item))
                            _pendingOperations.Remove(item);
                        else
                            try
                            {
                                if (_completedOperations.Contains(item))
                                    _completedOperations.Remove(item);
                            }
                            finally
                            {
                                if (_successsfulOperations.Contains(item))
                                    _successsfulOperations.Remove(item);
                                else if (_failedOperations.Contains(item))
                                    _failedOperations.Remove(item);
                                else if (_canceledOperations.Contains(item))
                                    _canceledOperations.Remove(item);
                            }
                    }
                    break;
            }
        }

        private void RaiseOperationFailed([DisallowNull] TItem item, [DisallowNull] AggregateException exception)
        {
            OnOperationFailed(new OpItemFailedEventArgs<TState, TTask, TItem, TListener>(item, exception));
        }

        /// <summary>
        /// Called when a background operation was completed due to an unhandled exception.
        /// </summary>
        /// <param name="args">The event argument that contains the failed <typeparamref name="TItem">item</typeparamref> and <see cref="Exception"/>.</param>
        protected virtual void OnOperationFailed([DisallowNull] OpItemFailedEventArgs<TState, TTask, TItem, TListener> args)
        {
            VerifyAccess();
            OperationFailed?.Invoke(this, args);
        }

        private void RaiseOperationCanceled([DisallowNull] TItem item)
        {
            OnOperationCanceled(new OpItemEventArgs<TState, TTask, TItem, TListener>(item));
        }

        /// <summary>
        /// Called when a background operation was completed due to a cancellation.
        /// </summary>
        /// <param name="args">The event argument that contains the canceled <typeparamref name="TItem">item</typeparamref>.</param>
        protected virtual void OnOperationCanceled([DisallowNull] OpItemEventArgs<TState, TTask, TItem, TListener> args)
        {
            VerifyAccess();
            OperationCanceled?.Invoke(this, args);
        }

        private void RaiseOperationRanToCompletion([DisallowNull] TItem item)
        {
            OnOperationRanToCompletion(new OpItemEventArgs<TState, TTask, TItem, TListener>(item));
        }

        /// <summary>
        /// Called when a background operation ran to completion without fault or cancellation.
        /// </summary>
        /// <param name="args">The event argument that contains the successfuly completed <typeparamref name="TItem">item</typeparamref>.</param>
        protected virtual void OnOperationRanToCompletion([DisallowNull] OpItemEventArgs<TState, TTask, TItem, TListener> args)
        {
            VerifyAccess();
            OperationRanToCompletion?.Invoke(this, args);
        }

        /// <summary>
        /// Removes the operation item, canceling it if it is not completed.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns><see langword="true"/> the item was removed; otherwise, <see langword="false"/>.</returns>
        public bool RemoveOperation(TItem item)
        {
            if (!CheckAccess())
                return Dispatcher.Invoke(() => RemoveOperation(item));
            if (item is null || !_allOperations.Contains(item))
                return false;
            if (_completedOperations.Contains(item))
            {
                _completedOperations.Remove(item);
                if (_successsfulOperations.Contains(item))
                    _successsfulOperations.Remove(item);
                else if (_failedOperations.Contains(item))
                    _failedOperations.Remove(item);
                else if (_canceledOperations.Contains(item))
                    _canceledOperations.Remove(item);
            }
            else
            {
                item.Cancel(true);
                if (_pendingOperations.Contains(item))
                    _pendingOperations.Remove(item);
                else
                {
                    item.GetTask().ContinueWith(t =>
                    {
                        if (_completedOperations.Contains(item))
                            _completedOperations.Remove(item);
                        if (_successsfulOperations.Contains(item))
                            _successsfulOperations.Remove(item);
                        else if (_failedOperations.Contains(item))
                            _failedOperations.Remove(item);
                        else if (_canceledOperations.Contains(item))
                            _canceledOperations.Remove(item);
                        _allOperations.Remove(item);
                    });
                    return true;
                }
            }
            _allOperations.Remove(item);
            return true;
        }
    }

    /// <summary>
    /// Base class for view models that track background operations, providing bindable status information properties.
    /// <para>Extends <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}" /></para>
    /// </summary>
    /// <typeparam name="TTask">The type of <see cref="Task"/> executed as the background operation.</typeparam>
    /// <typeparam name="TItem">The type of item that inherits from <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel" />,
    /// containing the status and results of the background operation.</typeparam>
    /// <typeparam name="TListener">The type of listener that inherits from <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.StatusListener" />
    /// and is used within the background <typeparamref name="TTask">Task</typeparamref> to update the associated <typeparamref name="TItem">Item</typeparamref>.</typeparam>
    /// <seealso cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}" />
    public class AsyncOpManagerViewModel<TTask, TItem, TListener> : AsyncOpManagerViewModel<object, TTask, TItem, TListener>
        where TTask : Task
        where TItem : AsyncOpManagerViewModel<TTask, TItem, TListener>.AsyncOpViewModel
        where TListener : AsyncOpManagerViewModel<TTask, TItem, TListener>.AsyncOpViewModel.StatusListener
    {
    }
}
