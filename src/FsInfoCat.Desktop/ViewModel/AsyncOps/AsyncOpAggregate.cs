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
        public event DependencyPropertyChangedEventHandler AsyncOpStatusPropertyChanged;
        public event DependencyPropertyChangedEventHandler StatusMessagePropertyChanged;
        public event DependencyPropertyChangedEventHandler MessageLevelPropertyChanged;
        public event DependencyPropertyChangedEventHandler DurationPropertyChanged;
        public event EventHandler CancelOperation;

        #region CancelOperation Property Members

        private static readonly DependencyPropertyKey CancelOperationCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CancelOperationCommand),
            typeof(Commands.RelayCommand), typeof(AsyncOpAggregate), new PropertyMetadata(null));

        public static readonly DependencyProperty CancelOperationCommandProperty = CancelOperationCommandPropertyKey.DependencyProperty;

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

        public AsyncFuncOpViewModel<TState, TResult> FromAsync<TState, TResult>(string title, string initialMessage, TState initialState,
            [DisallowNull] AsyncOpResultManagerViewModel<TState, AsyncFuncOpViewModel<TState, TResult>, AsyncFuncOpViewModel<TState, TResult>.StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<TState, AsyncFuncOpViewModel<TState, TResult>.StatusListenerImpl, Task<TResult>> func)
        {
            VerifyAccess();
            Guid concurrencyId = Guid.NewGuid();
            IDisposable loggerScope = _logger.BeginScope("Starting new background operation: Title = {Title}; Initial State = {InitialState}; Concurrency ID = {ConcurrencyId}", title, initialState, concurrencyId);
            Title = title;
            StatusMessage = initialMessage;
            AsyncFuncOpViewModel<TState, TResult> op = AsyncFuncOpViewModel<TState, TResult>.FromAsync(initialState, initialMessage, operationManager, func, concurrencyId);
            _operations.Add(op);
            op.AsyncOpStatusPropertyChanged += Op_AsyncOpStatusPropertyChanged;
            op.StatusMessagePropertyChanged += Op_StatusMessagePropertyChanged;
            op.MessageLevelPropertyChanged += Op_MessageLevelPropertyChanged;
            op.DurationPropertyChanged += Op_DurationPropertyChanged;
            StatusMessage = op.StatusMessage;
            MessageLevel = op.MessageLevel;
            AsyncOpStatus = op.AsyncOpStatus;
            op.GetTask().ContinueWith(task =>
            {
                if (task.IsCanceled)
                    _logger.LogWarning("Background operation canceled: Title = {Title}; Initial State = {InitialState}; Concurrency ID = {ConcurrencyId}; Task ID = {TaskId}", title, initialState, concurrencyId, task.Id);
                else if (task.IsFaulted)
                    _logger.LogError(task.Exception, "Background operation faulted: Message = {Message}; Operation Title = {Title}; Initial State = {InitialState}; Concurrency ID = {ConcurrencyId}; Task ID = {TaskId}",
                        task.Exception.Message, title, initialState, concurrencyId, task.Id);
                else
                    _logger.LogDebug("Background operation ran to completion: Title = {Title}; Initial State = {InitialState}; Concurrency ID = {ConcurrencyId}; Task ID = {TaskId}", title, initialState, concurrencyId, task.Id);
                using (loggerScope)
                {
                    op.AsyncOpStatusPropertyChanged -= Op_AsyncOpStatusPropertyChanged;
                    op.StatusMessagePropertyChanged -= Op_StatusMessagePropertyChanged;
                    op.MessageLevelPropertyChanged -= Op_MessageLevelPropertyChanged;
                    op.DurationPropertyChanged -= Op_DurationPropertyChanged;
                    Dispatcher.Invoke(() =>
                    {
                        _operations.Remove(op);
                        StatusMessage = op.StatusMessage;
                        MessageLevel = op.MessageLevel;
                        AsyncOpStatus = op.AsyncOpStatus;
                        operationManager.RemoveOperation(op);
                    });
                }
            });
            return op;
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
