using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public class AsyncOpAggregate : DependencyObject
    {
        private Collection<IAsyncOpViewModel> _operations = new();
        private readonly ILogger<AsyncOpAggregate> _logger;

        /// <summary>
        /// Occurs when the <see cref="AsyncOpStatus"/> has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler AsyncOpStatusPropertyChanged;

        /// <summary>
        /// Occurs when the <see cref="StatusMessage"/> has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler StatusMessagePropertyChanged;

        /// <summary>
        /// Occurs when the <see cref="MessageLevel"/> has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler MessageLevelPropertyChanged;

        /// <summary>
        /// Occurs when the <see cref="Duration"/> has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler DurationPropertyChanged;

        /// <summary>
        /// Occurs n the current <see cref="System.Windows.Threading.Dispatcher"/> thread when the <see cref="CancelOperationCommand"/> is invoked.
        /// </summary>
        public event EventHandler CancelOperation;

        /// <summary>
        /// Occurs n the current <see cref="System.Windows.Threading.Dispatcher"/> thread after the <see cref="Task"/> is completed with <see cref="Task.Status"/> of <see cref="TaskStatus.Faulted"/>
        /// or the final <see cref="IAsyncOpViewModel.MessageLevel"/> is <see cref="StatusMessageLevel.Error"/>.
        /// </summary>
        public event EventHandler Faulted;

        /// <summary>
        /// Occurs on the current <see cref="System.Windows.Threading.Dispatcher"/> thread after the <see cref="Task"/> is completed with <see cref="Task.Status"/> of <see cref="TaskStatus.Canceled"/>.
        /// </summary>
        public event EventHandler Canceled;

        /// <summary>
        /// Occurs n the current <see cref="System.Windows.Threading.Dispatcher"/> thread after the <see cref="Task"/> has run to completion without fault or cancellation.
        /// </summary>
        public event EventHandler RanToCompletion;

        #region CancelOperation Property Members

        private static readonly DependencyPropertyKey CancelOperationCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CancelOperationCommand),
            typeof(Commands.RelayCommand), typeof(AsyncOpAggregate), new PropertyMetadata(null));

        public static readonly DependencyProperty CancelOperationCommandProperty = CancelOperationCommandPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the cancel operation command.
        /// </summary>
        /// <value>The bindable <see cref="System.Windows.Input.ICommand"/> that cancels the current background operation.</value>
        public Commands.RelayCommand CancelOperationCommand => (Commands.RelayCommand)GetValue(CancelOperationCommandProperty);

        private void InvokeCancelOperationCommand(object parameter)
        {
            using IDisposable loggerScope = _logger.BeginScope("{EventName} event raised; parameter = {parameter}", nameof(CancelOperation), parameter);
            try { OnCancelOperationExecute(parameter); }
            finally { CancelOperation?.Invoke(this, EventArgs.Empty); }
        }

        private void OnCancelOperationExecute(object parameter)
        {
            if (_operations.Count > 0)
            {
                _logger.LogInformation("Cancelling {OperationCount} operations", _operations.Count);
                foreach (IAsyncOpViewModel item in _operations.ToArray())
                    item.Cancel(true);
            }
            else
            {
                _logger.LogDebug("Resetting background operation status");
                AsyncOpStatus = AsyncOpStatusCode.NotStarted;
            }
        }

        #endregion
        #region AsyncOpStatus Property Members

        private static readonly DependencyPropertyKey AsyncOpStatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AsyncOpStatus), typeof(AsyncOpStatusCode), typeof(AsyncOpAggregate),
                new PropertyMetadata(AsyncOpStatusCode.NotStarted, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AsyncOpAggregate)?.RaiseAsyncOpStatusPropertyChanged(e)));

        public static readonly DependencyProperty AsyncOpStatusProperty = AsyncOpStatusPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the status of the current asynchronous operation.
        /// </summary>
        /// <value>The <see cref="AsyncOpStatusCode"/> value that indicates the status of the current background operation.</value>
        public AsyncOpStatusCode AsyncOpStatus
        {
            get => (AsyncOpStatusCode)GetValue(AsyncOpStatusProperty);
            private set => SetValue(AsyncOpStatusPropertyKey, value);
        }

        private void RaiseAsyncOpStatusPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            _logger.LogInformation("{PropertyName} changed from {OldValue} to {NewValue}", args.Property.Name, args.OldValue, args.NewValue);
            AsyncOpStatusPropertyChanged?.Invoke(this, args);
        }

        #endregion
        #region Title Property Members

        private static readonly DependencyPropertyKey TitlePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Title), typeof(string), typeof(AsyncOpAggregate), new PropertyMetadata(""));

        public static readonly DependencyProperty TitleProperty = TitlePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the title of the current asynchronous operation.
        /// </summary>
        /// <value>The bindable value intended be shown within the title border of the <see cref="View.AsyncBgModalControl"/>.</value>
        public string Title
        {
            get => GetValue(TitleProperty) as string;
            private set => SetValue(TitlePropertyKey, value);
        }

        #endregion
        #region StatusMessage Property Members

        private static readonly DependencyPropertyKey StatusMessagePropertyKey = DependencyProperty.RegisterReadOnly(nameof(StatusMessage), typeof(string), typeof(AsyncOpAggregate),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AsyncOpAggregate)?.RaiseStatusMessagePropertyChanged(e)));

        public static readonly DependencyProperty StatusMessageProperty = StatusMessagePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the message that describes activity of the current asynchronous operation.
        /// </summary>
        /// <value>The bindable value intended be shown within the content border of the <see cref="View.AsyncBgModalControl"/>.</value>
        public string StatusMessage
        {
            get => GetValue(StatusMessageProperty) as string;
            private set => SetValue(StatusMessagePropertyKey, value);
        }

        private void RaiseStatusMessagePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            _logger.LogInformation("{PropertyName} changed from {OldValue} to {NewValue}", args.Property.Name, args.OldValue, args.NewValue);
            StatusMessagePropertyChanged?.Invoke(this, args);
        }

        #endregion
        #region MessageLevel

        private static readonly DependencyPropertyKey MessageLevelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MessageLevel), typeof(StatusMessageLevel), typeof(AsyncOpAggregate),
                new PropertyMetadata(StatusMessageLevel.Information, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AsyncOpAggregate)?.RaiseMessageLevelPropertyChanged(e)));

        public static readonly DependencyProperty MessageLevelProperty = MessageLevelPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the severity of the current <see cref="StatusMessage"/> of the current asynchronous operation.
        /// </summary>
        /// <value>The bindable value intended be shown within the title border of the <see cref="View.AsyncBgModalControl"/>.</value>
        public StatusMessageLevel MessageLevel
        {
            get => (StatusMessageLevel)GetValue(MessageLevelProperty);
            private set => SetValue(MessageLevelPropertyKey, value);
        }

        private void RaiseMessageLevelPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            _logger.LogInformation("{PropertyName} changed from {OldValue} to {NewValue}", args.Property.Name, args.OldValue, args.NewValue);
            MessageLevelPropertyChanged?.Invoke(this, args);
        }

        #endregion
        #region Duration

        private static readonly DependencyPropertyKey DurationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Duration), typeof(TimeSpan), typeof(AsyncOpAggregate),
                new PropertyMetadata(TimeSpan.Zero, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AsyncOpAggregate)?.RaiseDurationPropertyChanged(e)));

        public static readonly DependencyProperty DurationProperty = DurationPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the amount of time that the backround operation was running.
        /// </summary>
        /// <value>The amount of time that the backround operation was running.</value>
        public TimeSpan Duration
        {
            get => (TimeSpan)GetValue(DurationProperty);
            private set => SetValue(DurationPropertyKey, value);
        }

        private void RaiseDurationPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            _logger.LogInformation("{PropertyName} changed from {OldValue} to {NewValue}", args.Property.Name, args.OldValue, args.NewValue);
            DurationPropertyChanged?.Invoke(this, args);
        }

        #endregion

        public AsyncOpAggregate()
        {
            _logger = App.GetLogger(this);
            _logger.LogDebug("Instantiating new {TypeName}", nameof(AsyncOpAggregate));
            SetValue(CancelOperationCommandPropertyKey, new Commands.RelayCommand(OnCancelOperationExecute));
        }

        /// <summary>
        /// Starts a new background operation from an asynchronous method.
        /// </summary>
        /// <typeparam name="TState">The type of the state value that is passed to the background operation.</typeparam>
        /// <typeparam name="TResult">The type of value produced by the background operation.</typeparam>
        /// <param name="title">The title of the background operation. This is intended be shown within the title border of the <see cref="View.AsyncBgModalControl"/>.</param>
        /// <param name="initialMessage">The initial message to display in the <see cref="View.AsyncBgModalControl"/>.</param>
        /// <param name="initialState">The value that is passed to the background operation.</param>
        /// <param name="operationManager">The background operation manager.</param>
        /// <param name="func">The function that returns a <see cref="Task{TResult}"/> presumably by invoking an asynchronous method.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}"/> object that contains status information of the background task.</returns>
        public AsyncFuncOpViewModel<TState, TResult> FromAsync<TState, TResult>(string title, string initialMessage, TState initialState,
            [DisallowNull] AsyncOpResultManagerViewModel<TState, TResult> operationManager,
            [DisallowNull] Func<TState, IStatusListener<TState>, Task<TResult>> func)
        {
            VerifyAccess();
            Guid concurrencyId = Guid.NewGuid();
            IDisposable loggerScope = _logger.BeginScope("Starting new background operation: Title = {Title}; Initial State = {InitialState}; Concurrency ID = {ConcurrencyId}", title, initialState, concurrencyId);
            Title = title;
            StatusMessage = initialMessage;
            AsyncFuncOpViewModel<TState, TResult> op = AsyncFuncOpViewModel<TState, TResult>.FromAsync(initialState, initialMessage, operationManager, func, concurrencyId);
            OnOperationCreated(op).ContinueWith(task =>
            {
                if (task.IsCanceled)
                    _logger.LogWarning("Background operation canceled: Title = {Title}; Initial State = {InitialState}; Concurrency ID = {ConcurrencyId}; Task ID = {TaskId}", title, initialState, concurrencyId, task.Id);
                else if (task.IsFaulted)
                    _logger.LogError(task.Exception, "Background operation faulted: Message = {Message}; Operation Title = {Title}; Initial State = {InitialState}; Concurrency ID = {ConcurrencyId}; Task ID = {TaskId}",
                        task.Exception.Message, title, initialState, concurrencyId, task.Id);
                else
                    _logger.LogDebug("Background operation ran to completion: Title = {Title}; Initial State = {InitialState}; Concurrency ID = {ConcurrencyId}; Task ID = {TaskId}", title, initialState, concurrencyId, task.Id);
                using (loggerScope)
                    OnOperationCompleted(operationManager, task, op);
            });
            return op;
        }

        /// <summary>
        /// Starts a new background operation from an asynchronous method.
        /// </summary>
        /// <typeparam name="TResult">The type of value produced by the background operation.</typeparam>
        /// <param name="title">The title of the background operation. This is intended be shown within the title border of the <see cref="View.AsyncBgModalControl"/>.</param>
        /// <param name="initialMessage">The initial message to display in the <see cref="View.AsyncBgModalControl"/>.</param>
        /// <param name="operationManager">The background operation manager.</param>
        /// <param name="func">The function that returns a <see cref="Task{TResult}"/> presumably by invoking an asynchronous method.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object that contains status information of the background task.</returns>
        public AsyncFuncOpViewModel<TResult> FromAsync<TResult>(string title, string initialMessage, [DisallowNull] AsyncOpResultManagerViewModel<TResult> operationManager,
            [DisallowNull] Func<IStatusListener, Task<TResult>> func)
        {
            VerifyAccess();
            Guid concurrencyId = Guid.NewGuid();
            IDisposable loggerScope = _logger.BeginScope("Starting new background operation: Title = {Title}; Concurrency ID = {ConcurrencyId}", title, concurrencyId);
            Title = title;
            StatusMessage = initialMessage;
            AsyncFuncOpViewModel<TResult> op = AsyncFuncOpViewModel<TResult>.FromAsync(initialMessage, operationManager, func, concurrencyId);
            OnOperationCreated(op).ContinueWith(task =>
            {
                if (task.IsCanceled)
                    _logger.LogWarning("Background operation canceled: Title = {Title}; Initial State = {InitialState}; Concurrency ID = {ConcurrencyId}; Task ID = {TaskId}", title, concurrencyId, task.Id);
                else if (task.IsFaulted)
                    _logger.LogError(task.Exception, "Background operation faulted: Message = {Message}; Operation Title = {Title}; Initial State = {InitialState}; Concurrency ID = {ConcurrencyId}; Task ID = {TaskId}",
                        task.Exception.Message, title, concurrencyId, task.Id);
                else
                    _logger.LogDebug("Background operation ran to completion: Title = {Title}; Initial State = {InitialState}; Concurrency ID = {ConcurrencyId}; Task ID = {TaskId}", title, concurrencyId, task.Id);
                using (loggerScope)
                    OnOperationCompleted(operationManager, task, op);
            });
            return op;
        }

        private Task OnOperationCreated(IAsyncOpViewModel op)
        {
            _operations.Add(op);
            op.AsyncOpStatusPropertyChanged += Op_AsyncOpStatusPropertyChanged;
            op.StatusMessagePropertyChanged += Op_StatusMessagePropertyChanged;
            op.MessageLevelPropertyChanged += Op_MessageLevelPropertyChanged;
            op.DurationPropertyChanged += Op_DurationPropertyChanged;
            StatusMessage = op.StatusMessage;
            MessageLevel = op.MessageLevel;
            AsyncOpStatus = op.AsyncOpStatus;
            return op.GetTask();
        }

        private void OnOperationCompleted(IAsyncOpManagerViewModel operationManager, Task task, IAsyncOpViewModel op)
        {
            op.AsyncOpStatusPropertyChanged -= Op_AsyncOpStatusPropertyChanged;
            op.StatusMessagePropertyChanged -= Op_StatusMessagePropertyChanged;
            op.MessageLevelPropertyChanged -= Op_MessageLevelPropertyChanged;
            op.DurationPropertyChanged -= Op_DurationPropertyChanged;
            Dispatcher.Invoke(() =>
            {
                try { _operations.Remove(op); }
                finally
                {
                    try
                    {
                        if (task.IsCanceled)
                            Canceled?.Invoke(this, EventArgs.Empty);
                        else if (task.IsFaulted || op.MessageLevel == StatusMessageLevel.Error)
                        {
                            MessageBox.Show(Application.Current.MainWindow, op.StatusMessage, Title, MessageBoxButton.OK, MessageBoxImage.Error);
                            Faulted?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            if (op.MessageLevel == StatusMessageLevel.Warning)
                                MessageBox.Show(Application.Current.MainWindow, op.StatusMessage, Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                            RanToCompletion?.Invoke(this, EventArgs.Empty);
                        }
                    }
                    finally
                    {
                        try { AsyncOpStatus = AsyncOpStatusCode.NotStarted; }
                        finally { operationManager.RemoveOperation(op); }
                    }
                }
            });
        }

        private void Op_DurationPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _logger.LogDebug("{MethodName} invoked: sender = {sender}; EventArgs = {{ Property = {{ Name = {Name}; OwnwerType = {OwnerType} }}; OldValue = {OldValue}; NewValue = {NewValue} }}",
                nameof(Op_DurationPropertyChanged), sender, e.Property.Name, e.Property.OwnerType, e.OldValue, e.NewValue);
            if (e.NewValue is TimeSpan timeSpan)
                Duration = timeSpan;
        }

        private void Op_MessageLevelPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _logger.LogDebug("{MethodName} invoked: sender = {sender}; EventArgs = {{ Property = {{ Name = {Name}; OwnwerType = {OwnerType} }}; OldValue = {OldValue}; NewValue = {NewValue} }}",
                nameof(Op_MessageLevelPropertyChanged), sender, e.Property.Name, e.Property.OwnerType, e.OldValue, e.NewValue);
            if (e.NewValue is StatusMessageLevel messageLevel)
                MessageLevel = messageLevel;
        }

        private void Op_StatusMessagePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _logger.LogDebug("{MethodName} invoked: sender = {sender}; EventArgs = {{ Property = {{ Name = {Name}; OwnwerType = {OwnerType} }}; OldValue = {OldValue}; NewValue = {NewValue} }}",
                nameof(Op_StatusMessagePropertyChanged), sender, e.Property.Name, e.Property.OwnerType, e.OldValue, e.NewValue);
            if (e.NewValue is string statusMessage)
                StatusMessage = statusMessage;
        }

        private void Op_AsyncOpStatusPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _logger.LogDebug("{MethodName} invoked: sender = {sender}; EventArgs = {{ Property = {{ Name = {Name}; OwnwerType = {OwnerType} }}; OldValue = {OldValue}; NewValue = {NewValue} }}",
                nameof(Op_AsyncOpStatusPropertyChanged), sender, e.Property.Name, e.Property.OwnerType, e.OldValue, e.NewValue);
            if (e.NewValue is AsyncOpStatusCode statusCode)
                AsyncOpStatus = statusCode;
        }
    }
}
