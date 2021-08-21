using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    /// <summary>
    /// View model used by <see cref="View.AsyncBgModalControl"/> to display background operation status in a modal context.
    /// </summary>
    /// <seealso cref="DependencyObject" />
    public class AsyncBgModalVM : DependencyObject, IAsyncWindowsBackgroundOperationManager
    {
        private readonly object _syncRoot = new();
        private ListenerImpl _currentListener;
        private readonly ILogger<AsyncBgModalVM> _logger;
        private Border _outerMessageBorder;

        #region Events

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
        /// Occurs n the current <see cref="Dispatcher"/> thread when the <see cref="CancelOperation"/> is invoked.
        /// </summary>
        public event EventHandler OperationCancelRequested;

        /// <summary>
        /// Occurs n the current <see cref="Dispatcher"/> thread after the <see cref="Task"/> is completed with <see cref="Task.Status"/> of <see cref="TaskStatus.Faulted"/>
        /// or the final <see cref="IAsyncOpViewModel.MessageLevel"/> is <see cref="StatusMessageLevel.Error"/>.
        /// </summary>
        public event EventHandler OperationFaulted;

        /// <summary>
        /// Occurs on the current <see cref="Dispatcher"/> thread after the <see cref="Task"/> is completed with <see cref="Task.Status"/> of <see cref="TaskStatus.Canceled"/>.
        /// </summary>
        public event EventHandler OperationCanceled;

        /// <summary>
        /// Occurs n the current <see cref="Dispatcher"/> thread after the <see cref="Task"/> has run to completion without fault or cancellation.
        /// </summary>
        public event EventHandler OperationRanToCompletion;

        #endregion
        #region CancelOperation Command Members

        private static readonly DependencyPropertyKey CancelOperationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CancelOperation),
            typeof(Commands.RelayCommand), typeof(AsyncBgModalVM), new PropertyMetadata(null));

        public static readonly DependencyProperty CancelOperationProperty = CancelOperationPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the cancel operation command.
        /// </summary>
        /// <value>The bindable <see cref="System.Windows.Input.ICommand"/> that cancels the current background operation.</value>
        public Commands.RelayCommand CancelOperation => (Commands.RelayCommand)GetValue(CancelOperationProperty);

        private void RaiseOperationCancelRequested(object parameter)
        {
            using IDisposable loggerScope = _logger.BeginScope("{EventName} event raised; parameter = {parameter}", nameof(OperationCancelRequested), parameter);
            try { OnOperationCancelRequested(parameter); }
            finally { OperationCancelRequested?.Invoke(this, EventArgs.Empty); }
        }

        private void OnOperationCancelRequested(object parameter)
        {
            ListenerImpl item;
            lock (_syncRoot)
            {
                item = _currentListener;
                _currentListener = null;
            }
            if (item is null)
            {
                _logger.LogDebug("Resetting background operation status");
                AsyncOpStatus = AsyncOpStatusCode.NotStarted;
            }
            else
            {
                _logger.LogInformation("Cancelling background operation: ConcurrencyId = {ConcurrencyId}", item.ConcurrencyId);
                item.Cancel();
            }
        }

        #endregion
        #region NotifyOuterMessageBorderLoaded Command Property Members

        private static readonly DependencyPropertyKey NotifyOuterMessageBorderLoadedPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NotifyOuterMessageBorderLoaded),
            typeof(Commands.RelayCommand), typeof(AsyncBgModalVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="NotifyOuterMessageBorderLoaded"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotifyOuterMessageBorderLoadedProperty = NotifyOuterMessageBorderLoadedPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand NotifyOuterMessageBorderLoaded => (Commands.RelayCommand)GetValue(NotifyOuterMessageBorderLoadedProperty);

        private void OnNotifyOuterMessageBorderLoaded(object parameter)
        {
            if (parameter is Border outerMessageBorder)
            {
                _outerMessageBorder = outerMessageBorder;
                RecalculateOuterMessageBorderSize(outerMessageBorder);
            }
        }

        #endregion
        #region NotifyOuterMessageBorderUnloaded Command Property Members

        private static readonly DependencyPropertyKey NotifyOuterMessageBorderUnloadedPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NotifyOuterMessageBorderUnloaded),
            typeof(Commands.RelayCommand), typeof(AsyncBgModalVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="NotifyOuterMessageBorderUnloaded"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotifyOuterMessageBorderUnloadedProperty = NotifyOuterMessageBorderUnloadedPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand NotifyOuterMessageBorderUnloaded => (Commands.RelayCommand)GetValue(NotifyOuterMessageBorderUnloadedProperty);

        private void OnNotifyOuterMessageBorderUnloaded(object parameter)
        {
            if (parameter is Border outerMessageBorder)
                _outerMessageBorder = null;
        }

        #endregion
        #region NotifyViewSizeChanged Command Property Members

        private static readonly DependencyPropertyKey NotifyViewSizeChangedPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NotifyViewSizeChanged),
            typeof(Commands.RelayCommand), typeof(AsyncBgModalVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="NotifyViewSizeChanged"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotifyViewSizeChangedProperty = NotifyViewSizeChangedPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand NotifyViewSizeChanged => (Commands.RelayCommand)GetValue(NotifyViewSizeChangedProperty);

        private void OnNotifyViewSizeChanged(object parameter) => RecalculateOuterMessageBorderSize(_outerMessageBorder);

        #endregion
        #region AsyncOpStatus Property Members

        private static readonly DependencyPropertyKey AsyncOpStatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AsyncOpStatus), typeof(AsyncOpStatusCode), typeof(AsyncBgModalVM),
                new PropertyMetadata(AsyncOpStatusCode.NotStarted, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AsyncBgModalVM)?.RaiseAsyncOpStatusPropertyChanged(e)));

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
            switch ((args.NewValue as AsyncOpStatusCode?) ?? AsyncOpStatusCode.NotStarted)
            {
                case AsyncOpStatusCode.CancellationPending:
                case AsyncOpStatusCode.Running:
                    ControlVisibility = Visibility.Visible;
                    break;
                default:
                    ControlVisibility = Visibility.Collapsed;
                    break;
            }
        }

        #endregion
        #region Title Property Members

        private static readonly DependencyPropertyKey TitlePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Title), typeof(string), typeof(AsyncBgModalVM), new PropertyMetadata(""));

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

        private static readonly DependencyPropertyKey StatusMessagePropertyKey = DependencyProperty.RegisterReadOnly(nameof(StatusMessage), typeof(string), typeof(AsyncBgModalVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AsyncBgModalVM)?.RaiseStatusMessagePropertyChanged(e)));

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
        #region MessageLevel Property Members

        private static readonly DependencyPropertyKey MessageLevelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MessageLevel), typeof(StatusMessageLevel), typeof(AsyncBgModalVM),
                new PropertyMetadata(StatusMessageLevel.Information, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AsyncBgModalVM)?.RaiseMessageLevelPropertyChanged(e)));

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
        #region Duration Property Members

        private static readonly DependencyPropertyKey DurationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Duration), typeof(TimeSpan), typeof(AsyncBgModalVM),
                new PropertyMetadata(TimeSpan.Zero, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AsyncBgModalVM)?.RaiseDurationPropertyChanged(e)));

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
        #region PopupBorderWidth Property Members

        private static readonly DependencyPropertyKey PopupBorderWidthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PopupBorderWidth), typeof(double), typeof(AsyncBgModalVM),
                new PropertyMetadata(200.0));

        /// <summary>
        /// Identifies the <see cref="PopupBorderWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PopupBorderWidthProperty = PopupBorderWidthPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the width of the inner popup border width.
        /// </summary>
        /// <value>The width of the inner popup border as calculated by the <see cref="RecalculatedPopupBorderSize(Border)"/> method.</value>
        public double PopupBorderWidth { get => (double)GetValue(PopupBorderWidthProperty); private set => SetValue(PopupBorderWidthPropertyKey, value); }

        #endregion
        #region PopupBorderHeight Property Members

        private static readonly DependencyPropertyKey PopupBorderHeightPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PopupBorderHeight), typeof(double), typeof(AsyncBgModalVM),
                new PropertyMetadata(100.0));

        /// <summary>
        /// Identifies the <see cref="PopupBorderHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PopupBorderHeightProperty = PopupBorderHeightPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the height of the inner popup border width.
        /// </summary>
        /// <value>The width of the inner popup border as calculated by the <see cref="RecalculatedPopupBorderSize(Border)"/> method.</value>
        public double PopupBorderHeight { get => (double)GetValue(PopupBorderHeightProperty); private set => SetValue(PopupBorderHeightPropertyKey, value); }

        #endregion
        #region ControlVisibility Property Members

        private static readonly DependencyPropertyKey ControlVisibilityPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ControlVisibility), typeof(Visibility), typeof(AsyncBgModalVM),
                new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Identifies the <see cref="ControlVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ControlVisibilityProperty = ControlVisibilityPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public Visibility ControlVisibility { get => (Visibility)GetValue(ControlVisibilityProperty); private set => SetValue(ControlVisibilityPropertyKey, value); }

        #endregion

        public AsyncBgModalVM()
        {
            _logger = App.GetLogger(this);
            _logger.LogDebug("Instantiating new {TypeName}", nameof(AsyncBgModalVM));
            SetValue(CancelOperationPropertyKey, new Commands.RelayCommand(OnOperationCancelRequested));
            SetValue(NotifyOuterMessageBorderLoadedPropertyKey, new Commands.RelayCommand(OnNotifyOuterMessageBorderLoaded));
            SetValue(NotifyOuterMessageBorderUnloadedPropertyKey, new Commands.RelayCommand(OnNotifyOuterMessageBorderUnloaded));
            SetValue(NotifyViewSizeChangedPropertyKey, new Commands.RelayCommand(OnNotifyViewSizeChanged));
        }

        /// <summary>
        /// Recalculates the <see cref="PopupBorderWidth"/> and <see cref="PopupBorderHeight"/> values.
        /// </summary>
        /// <param name="outerBorderControl">The <see cref="Border"/> control of <see cref="View.AsyncBgModalControl"/>
        /// whose <see cref="FrameworkElement.Width"/> and <see cref="FrameworkElement.Height"/> are bound to the <see cref="PopupBorderWidth"/>
        /// and <see cref="PopupBorderHeight"/> properties, respectively.</param>
        /// <remarks>This method is invoked from the <see cref="FrameworkElement.Loaded"/> </remarks>
        public void RecalculateOuterMessageBorderSize(Border outerMessageBorder)
        {
            FrameworkElement parent = outerMessageBorder?.Parent as FrameworkElement;
            double maxH, maxW;
            if (parent is null)
                return;

            Thickness margin = outerMessageBorder.Margin;
            if ((maxH = parent.ActualHeight - margin.Top - margin.Bottom) <= outerMessageBorder.MinHeight)
                maxH = outerMessageBorder.MinHeight;
            if ((maxW = parent.ActualWidth - margin.Left - margin.Right) <= outerMessageBorder.MinHeight)
                maxW = outerMessageBorder.MinWidth;

            if (maxH <= 0.0 || maxW <= 0.0)
                return;

            double w = parent.ActualWidth * 0.75;
            double h = parent.ActualHeight * 0.75;
            Size desired = outerMessageBorder.DesiredSize;
            if (w < desired.Width)
                w = desired.Width;
            if (h < desired.Height)
                h = desired.Height;

            PopupBorderWidth = (w > maxW) ? maxW : w;
            PopupBorderHeight = (h > maxH) ? maxH : h;
        }

        /// <summary>
        /// Starts a new background operation from an asynchronous method.
        /// </summary>
        /// <typeparam name="TState">The type of the state value that is passed to the background operation.</typeparam>
        /// <typeparam name="TResult">The type of value produced by the background operation.</typeparam>
        /// <param name="title">The title of the background operation. This is intended be shown within the title border of the <see cref="View.AsyncBgModalControl"/>.</param>
        /// <param name="initialMessage">The initial message to display in the <see cref="View.AsyncBgModalControl"/>.</param>
        /// <param name="state">The value that is passed to the background operation.</param>
        /// <param name="func">The function that returns a <see cref="Task{TResult}"/> presumably by invoking an asynchronous method.</param>
        /// <returns>A <see cref="Task{TResult}"/> object that contains status information of the background task.</returns>
        public Task<TResult> FromAsync<TState, TResult>(string title, string initialMessage, TState state,
            [DisallowNull] Func<TState, IWindowsStatusListener, Task<TResult>> func)
        {
            ListenerImpl item = CreateNewListener(title, initialMessage, Guid.NewGuid());
            IDisposable loggerScope = _logger.BeginScope("Starting new background operation: Title = {Title}; State = {State}; Concurrency ID = {ConcurrencyId}",
                title, state, item.ConcurrencyId);
            lock (_syncRoot)
            {
                if (_currentListener is not null)
                    _currentListener.Cancel();
                return (_currentListener = item).FromAsync(loggerScope, state, func);
            }
        }

        Task<TResult> IAsyncBackgroundOperationManager.FromAsync<TState, TResult>(string title, string initialMessage, TState state,
            [DisallowNull] Func<TState, IStatusListener, Task<TResult>> func) => FromAsync(title, initialMessage, state, func);

        /// <summary>
        /// Starts a new background operation from an asynchronous method.
        /// </summary>
        /// <typeparam name="TResult">The type of value produced by the background operation.</typeparam>
        /// <param name="title">The title of the background operation. This is intended be shown within the title border of the <see cref="View.AsyncBgModalControl"/>.</param>
        /// <param name="initialMessage">The initial message to display in the <see cref="View.AsyncBgModalControl"/>.</param>
        /// <param name="func">The function that returns a <see cref="Task{TResult}"/> presumably by invoking an asynchronous method.</param>
        /// <returns>A <see cref="Task{TResult}"/> object that contains status information of the background task.</returns>
        public Task<TResult> FromAsync<TResult>(string title, string initialMessage, [DisallowNull] Func<IWindowsStatusListener, Task<TResult>> func)
        {
            Guid concurrencyId = Guid.NewGuid();
            ListenerImpl item = CreateNewListener(title, initialMessage, Guid.NewGuid());
            IDisposable loggerScope = _logger.BeginScope("Starting new background operation: Title = {Title}; Concurrency ID = {ConcurrencyId}",
                title, item.ConcurrencyId);
            lock (_syncRoot)
            {
                if (_currentListener is not null)
                    _currentListener.Cancel();
                return (_currentListener = item).FromAsync(loggerScope, func);
            }
        }

        Task<TResult> IAsyncBackgroundOperationManager.FromAsync<TResult>(string title, string initialMessage, [DisallowNull] Func<IStatusListener, Task<TResult>> func) =>
            FromAsync(title, initialMessage, func);

        public Task FromAsync<TState>(string title, string initialMessage, TState state, [DisallowNull] Func<TState, IWindowsStatusListener, Task> func)
        {
            Guid concurrencyId = Guid.NewGuid();
            ListenerImpl item = CreateNewListener(title, initialMessage, Guid.NewGuid());
            IDisposable loggerScope = _logger.BeginScope("Starting new background operation: Title = {Title}; State = {State}; Concurrency ID = {ConcurrencyId}",
                title, state, item.ConcurrencyId);
            lock (_syncRoot)
            {
                if (_currentListener is not null)
                    _currentListener.Cancel();
                return (_currentListener = item).FromAsync(loggerScope, state, func);
            }
        }

        Task IAsyncBackgroundOperationManager.FromAsync<TState>(string title, string initialMessage, TState state, [DisallowNull] Func<TState, IStatusListener, Task> func) =>
            FromAsync(title, initialMessage, state, func);

        public Task FromAsync(string title, string initialMessage, [DisallowNull] Func<IWindowsStatusListener, Task> func)
        {
            Guid concurrencyId = Guid.NewGuid();
            ListenerImpl item = CreateNewListener(title, initialMessage, Guid.NewGuid());
            IDisposable loggerScope = _logger.BeginScope("Starting new background operation: Title = {Title}; Concurrency ID = {ConcurrencyId}",
                title, item.ConcurrencyId);
            lock (_syncRoot)
            {
                if (_currentListener is not null)
                    _currentListener.Cancel();
                return (_currentListener = item).FromAsync(loggerScope, func);
            }
        }

        Task IAsyncBackgroundOperationManager.FromAsync(string title, string initialMessage, [DisallowNull] Func<IStatusListener, Task> func) =>
            FromAsync(title, initialMessage, func);

        private ListenerImpl CreateNewListener(string title, string initialMessage, Guid concurrencyId)
        {
            VerifyAccess();
            Title = title;
            StatusMessage = initialMessage;
            return new(this, concurrencyId);
        }

        private void RaiseTaskCompleted(ListenerImpl item, [DisallowNull] IDisposable loggerScope, Task task)
        {
            bool notSuperceded;
            lock (_syncRoot)
            {
                notSuperceded = ReferenceEquals(_currentListener, item);
                if (notSuperceded)
                    _currentListener = null;
            }
            Dispatcher.Invoke(() =>
            {
                using (loggerScope)
                {
                    try
                    {
                        try
                        {
                            if (task.IsCanceled)
                            {
                                _logger.LogWarning("Background operation canceled: ConcurrencyId = {ConcurrencyId}", item.ConcurrencyId);
                                OperationCanceled?.Invoke(this, EventArgs.Empty);
                            }
                            else if (task.IsFaulted || MessageLevel == StatusMessageLevel.Error)
                            {
                                if (task.IsFaulted)
                                    _logger.LogError(task.Exception, "Background operation faulted: ConcurrencyId = {ConcurrencyId}", item.ConcurrencyId);
                                else
                                    _logger.LogError("Background operation ended with error: ConcurrencyId = {ConcurrencyId}", item.ConcurrencyId);
                                if (notSuperceded)
                                    MessageBox.Show(Application.Current.MainWindow, StatusMessage, Title, MessageBoxButton.OK, MessageBoxImage.Error);
                                OperationFaulted?.Invoke(this, EventArgs.Empty);
                            }
                            else
                            {
                                _logger.LogInformation("Background operation ran to completion: ConcurrencyId = {ConcurrencyId}", item.ConcurrencyId);
                                if (notSuperceded && MessageLevel == StatusMessageLevel.Warning)
                                    MessageBox.Show(Application.Current.MainWindow, StatusMessage, Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                                OperationRanToCompletion?.Invoke(this, EventArgs.Empty);
                            }
                        }
                        finally { AsyncOpStatus = AsyncOpStatusCode.NotStarted; }
                    }
                    finally { item.Dispose(); }
                }
            });
        }

        public void CancelAll() => RaiseOperationCancelRequested(null);

        class ListenerImpl : IWindowsStatusListener, IDisposable
        {
            private readonly CancellationTokenSource _tokenSource = new();
            private readonly AsyncBgModalVM _owner;
            private bool _isDisposed;

            internal ListenerImpl(AsyncBgModalVM owner, Guid concurrencyId)
            {
                _owner = owner ?? throw new ArgumentNullException();
                CancellationToken = _tokenSource.Token;
                ConcurrencyId = concurrencyId;
            }

            public CancellationToken CancellationToken { get; }

            public Guid ConcurrencyId { get; }

            public ILogger Logger => _owner._logger;

            public DispatcherOperation BeginSetMessage([AllowNull] string message, StatusMessageLevel level, DispatcherPriority
                priority = DispatcherPriority.Background) => _owner.Dispatcher.InvokeAsync(() =>
                {
                    _owner.StatusMessage = message;
                    _owner.MessageLevel = level;
                }, priority, CancellationToken);

            public DispatcherOperation BeginSetMessage([AllowNull] string message, DispatcherPriority priority = DispatcherPriority.Background) =>
                _owner.Dispatcher.InvokeAsync(() => _owner.StatusMessage = message, priority, CancellationToken);


            public void SetMessage([AllowNull] string message, StatusMessageLevel level, TimeSpan timeout, DispatcherPriority priority = DispatcherPriority.Background) =>
                _owner.Dispatcher.Invoke(() =>
                {
                    _owner.StatusMessage = message;
                    _owner.MessageLevel = level;
                }, priority, CancellationToken, timeout);

            public void SetMessage([AllowNull] string message, StatusMessageLevel level, DispatcherPriority priority = DispatcherPriority.Background) =>
                _owner.Dispatcher.Invoke(() =>
                {
                    _owner.StatusMessage = message;
                    _owner.MessageLevel = level;
                }, priority, CancellationToken);

            public void SetMessage([AllowNull] string message, TimeSpan timeout, DispatcherPriority priority = DispatcherPriority.Background) =>
                _owner.Dispatcher.Invoke(() => _owner.StatusMessage = message, priority, CancellationToken, timeout);

            public void SetMessage([AllowNull] string message, DispatcherPriority priority = DispatcherPriority.Background) =>
                _owner.Dispatcher.Invoke(() => _owner.StatusMessage = message, priority, CancellationToken);

            internal Task<TResult> FromAsync<TState, TResult>([DisallowNull] IDisposable loggerScope, TState state, [DisallowNull] Func<TState, IWindowsStatusListener, Task<TResult>> func)
            {
                Task<TResult> task = Task.Run(async () =>
                {
                    await _owner.Dispatcher.InvokeAsync(() => _owner.AsyncOpStatus = AsyncOpStatusCode.Running, DispatcherPriority.Normal, CancellationToken);
                    return await func(state, this);
                }, CancellationToken);
                task.ContinueWith(task => _owner.RaiseTaskCompleted(this, loggerScope, task));
                return task;
            }

            internal Task<TResult> FromAsync<TResult>([DisallowNull] IDisposable loggerScope, [DisallowNull] Func<IWindowsStatusListener, Task<TResult>> func)
            {
                Task<TResult> task = Task.Run(async () =>
                {
                    await _owner.Dispatcher.InvokeAsync(() => _owner.AsyncOpStatus = AsyncOpStatusCode.Running, DispatcherPriority.Normal, CancellationToken);
                    return await func(this);
                }, CancellationToken);
                task.ContinueWith(task => _owner.RaiseTaskCompleted(this, loggerScope, task));
                return task;
            }

            internal Task FromAsync<TState>([DisallowNull] IDisposable loggerScope, TState state, [DisallowNull] Func<TState, IWindowsStatusListener, Task> func)
            {
                Task task = Task.Run(async () =>
                {
                    await _owner.Dispatcher.InvokeAsync(() => _owner.AsyncOpStatus = AsyncOpStatusCode.Running, DispatcherPriority.Normal, CancellationToken);
                    await func(state, this);
                }, CancellationToken);
                task.ContinueWith(task => _owner.RaiseTaskCompleted(this, loggerScope, task));
                return task;
            }

            internal Task FromAsync([DisallowNull] IDisposable loggerScope, [DisallowNull] Func<IWindowsStatusListener, Task> func)
            {
                Task task = Task.Run(async () =>
                {
                    Logger.LogInformation("Background operation started: ConcurrencyId = {ConcurrencyId}", ConcurrencyId);
                    await _owner.Dispatcher.InvokeAsync(() => _owner.AsyncOpStatus = AsyncOpStatusCode.Running, DispatcherPriority.Normal, CancellationToken);
                    await func(this);
                }, CancellationToken);
                task.ContinueWith(task => _owner.RaiseTaskCompleted(this, loggerScope, task));
                return task;
            }

            internal void Cancel()
            {
                if (!_tokenSource.IsCancellationRequested)
                    _tokenSource.Cancel(true);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!_isDisposed)
                {
                    if (disposing)
                        _tokenSource.Dispose();
                    _isDisposed = true;
                }
            }

            public void Dispose()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }

            void IStatusListener.SetMessage([AllowNull] string message, StatusMessageLevel level, TimeSpan timeout) => SetMessage(message, level, timeout);
            void IStatusListener.SetMessage([AllowNull] string message, StatusMessageLevel level) => SetMessage(message, level);
            void IStatusListener.SetMessage([AllowNull] string message, TimeSpan timeout) => SetMessage(message, timeout);
            Task IStatusListener.BeginSetMessage([AllowNull] string message, StatusMessageLevel level) => BeginSetMessage(message, level).Task;
            void IStatusListener.SetMessage([AllowNull] string message) => SetMessage(message);
            Task IStatusListener.BeginSetMessage([AllowNull] string message) => BeginSetMessage(message).Task;
            DispatcherOperation IWindowsStatusListener.BeginSetMessage([AllowNull] string message, StatusMessageLevel level) => BeginSetMessage(message, level);
            DispatcherOperation IWindowsStatusListener.BeginSetMessage([AllowNull] string message) => BeginSetMessage(message);
        }
    }
}
