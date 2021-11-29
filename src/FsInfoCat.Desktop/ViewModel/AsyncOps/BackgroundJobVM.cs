using FsInfoCat.AsyncOps;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class BackgroundJobVM : DependencyObject
    {
        private readonly ILogger<BackgroundJobVM> _logger;
        private readonly ProgressObserver _observer;
        private readonly IDisposable _subscription;

        #region Activity Property Members

        private static readonly DependencyPropertyKey ActivityPropertyKey = DependencyPropertyBuilder<BackgroundJobVM, string>
            .Register(nameof(Activity))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Activity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ActivityProperty = ActivityPropertyKey.DependencyProperty;

        public string Activity { get => GetValue(ActivityProperty) as string; private set => SetValue(ActivityPropertyKey, value); }

        #endregion
        #region StatusDescription Property Members

        private static readonly DependencyPropertyKey StatusDescriptionPropertyKey = DependencyPropertyBuilder<BackgroundJobVM, string>
            .Register(nameof(StatusDescription))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="StatusDescription"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusDescriptionProperty = StatusDescriptionPropertyKey.DependencyProperty;

        public string StatusDescription { get => GetValue(StatusDescriptionProperty) as string; private set => SetValue(StatusDescriptionPropertyKey, value); }

        #endregion
        #region CurrentOperation Property Members

        private static readonly DependencyPropertyKey CurrentOperationPropertyKey = DependencyPropertyBuilder<BackgroundJobVM, string>
            .Register(nameof(CurrentOperation))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="CurrentOperation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CurrentOperationProperty = CurrentOperationPropertyKey.DependencyProperty;

        public string CurrentOperation { get => GetValue(CurrentOperationProperty) as string; private set => SetValue(CurrentOperationPropertyKey, value); }

        #endregion
        #region Title Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="Title"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler TitlePropertyChanged;

        /// <summary>
        /// Identifies the <see cref="Title"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(BackgroundJobVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as BackgroundJobVM).TitlePropertyChanged?.Invoke(d, e)));

        [Obsolete("Use Activity")]
        public string Title { get => GetValue(TitleProperty) as string; set => SetValue(TitleProperty, value); }

        #endregion
        #region Message Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="Message"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler MessagePropertyChanged;

        /// <summary>
        /// Identifies the <see cref="Message"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message), typeof(string), typeof(BackgroundJobVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as BackgroundJobVM).MessagePropertyChanged?.Invoke(d, e)));

        [Obsolete("Use StatusDescription and/or CurrentOperation")]
        public string Message { get => GetValue(MessageProperty) as string; set => SetValue(MessageProperty, value); }

        #endregion
        #region MessageLevel Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="MessageLevel"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler MessageLevelPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="MessageLevel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MessageLevelProperty = DependencyProperty.Register(nameof(MessageLevel), typeof(StatusMessageLevel),
            typeof(BackgroundJobVM), new PropertyMetadata(StatusMessageLevel.Information,
                (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as BackgroundJobVM)?.MessageLevelPropertyChanged?.Invoke(d, e)));

        public StatusMessageLevel MessageLevel { get => (StatusMessageLevel)GetValue(MessageLevelProperty); set => SetValue(MessageLevelProperty, value); }

        #endregion
        #region Cancel Property Members

        [Obsolete("Do not use")]
        public event EventHandler<Commands.CommandEventArgs> CancelInvoked;

        private static readonly DependencyPropertyKey CancelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Cancel),
            typeof(Commands.RelayCommand), typeof(BackgroundJobVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Cancel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CancelProperty = CancelPropertyKey.DependencyProperty;

        public Commands.RelayCommand Cancel => (Commands.RelayCommand)GetValue(CancelProperty);

        #endregion
        #region IsCompleted Property Members

        private static readonly DependencyPropertyKey IsCompletedPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsCompleted), typeof(bool),
            typeof(BackgroundJobVM), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsCompleted"/> dependency property.
        /// </summary>
        [Obsolete("Do not use")]
        public static readonly DependencyProperty IsCompletedProperty = IsCompletedPropertyKey.DependencyProperty;

        [Obsolete("Do not use")]
        public bool IsCompleted { get => (bool)GetValue(IsCompletedProperty); private set => SetValue(IsCompletedPropertyKey, value); }

        #endregion
        #region JobStatus Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="JobStatus"/> dependency property has changed.
        /// </summary>
        [Obsolete("Do not use")]
        public event DependencyPropertyChangedEventHandler JobStatusPropertyChanged;

        private static readonly DependencyPropertyKey JobStatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(JobStatus), typeof(AsyncJobStatus),
            typeof(BackgroundJobVM), new PropertyMetadata(AsyncJobStatus.WaitingToRun, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as BackgroundJobVM)?.JobStatusPropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Identifies the <see cref="JobStatus"/> dependency property.
        /// </summary>
        [Obsolete("Do not use")]
        public static readonly DependencyProperty JobStatusProperty = JobStatusPropertyKey.DependencyProperty;

        [Obsolete("Do not use")]
        public AsyncJobStatus JobStatus { get => (AsyncJobStatus)GetValue(JobStatusProperty); private set => SetValue(JobStatusPropertyKey, value); }

        #endregion
        #region Duration Property Members

        private static readonly DependencyPropertyKey DurationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Duration), typeof(TimeSpan), typeof(BackgroundJobVM),
                new PropertyMetadata(TimeSpan.Zero));

        /// <summary>
        /// Identifies the <see cref="Duration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DurationProperty = DurationPropertyKey.DependencyProperty;

        public TimeSpan Duration { get => (TimeSpan)GetValue(DurationProperty); private set => SetValue(DurationPropertyKey, value); }

        #endregion

        public BackgroundJobVM([DisallowNull] ICancellableOperation progressInfo, MessageCode? code, [DisallowNull] IObservable<IBackgroundProgressEvent> observable, [DisallowNull] Action<BackgroundJobVM> onCompleted)
        {
            _logger = App.GetLogger(this);
            Activity = progressInfo.Activity;
            CurrentOperation = progressInfo.CurrentOperation;
            StatusDescription = progressInfo.StatusDescription;
            _observer = new ProgressObserver(this, onCompleted);
            _subscription = observable.Subscribe(_observer);
            SetValue(CancelPropertyKey, new Commands.RelayCommand(parameter =>
            {
                if (!progressInfo.IsCancellationRequested)
                    progressInfo.Cancel();
            }));
        }

        [Obsolete("Use IBackgroundProgressService and/or Create(IBackgroundProgressInfo, MessageCode?, IObservable<IBackgroundProgressEvent>, Action<BackgroundJobVM>, out ProgressObserver), instead")]
        public BackgroundJobVM()
        {
            _logger = App.GetLogger(this);
            SetValue(CancelPropertyKey, new Commands.RelayCommand(parameter => CancelInvoked?.Invoke(this, new(parameter))));
        }

        internal static BackgroundJobVM Create(ICancellableOperation progressInfo, MessageCode? code, [DisallowNull] IObservable<IBackgroundProgressEvent> observable, [DisallowNull] Action<BackgroundJobVM> onCompleted, out ProgressObserver observer)
        {
            BackgroundJobVM item = new BackgroundJobVM(progressInfo, code, observable, onCompleted);
            observer = item._observer;
            return item;
        }

        private void OnError(Exception error)
        {
            if (error is AggregateException aggregateException && aggregateException.InnerExceptions.Count == 1)
                error = aggregateException.InnerExceptions[0];
            if (error is AsyncOperationException asyncOpException)
            {
                if (asyncOpException.Code.TryGetDescription(out string codeDescription))
                {
                    if (string.IsNullOrWhiteSpace(asyncOpException.CurrentOperation))
                        MessageBox.Show(App.Current.MainWindow, messageBoxText: $@"An unexpected has occurred:
Activity: {asyncOpException.Activity}
Error Type: {asyncOpException.Code.GetDisplayName()} ({codeDescription})
Message: {asyncOpException.Message}", "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    else MessageBox.Show(App.Current.MainWindow, messageBoxText: $@"An unexpected has occurred:
Activity: {asyncOpException.Activity}
Error Type: {asyncOpException.Code.GetDisplayName()} ({codeDescription})
Message: {asyncOpException.Message}
Current Operation: {asyncOpException.CurrentOperation}", "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (string.IsNullOrWhiteSpace(asyncOpException.CurrentOperation))
                    MessageBox.Show(App.Current.MainWindow, messageBoxText: $@"An unexpected has occurred:
Activity: {asyncOpException.Activity}
Error Type: {asyncOpException.Code.GetDisplayName()}
Message: {asyncOpException.Message}", "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else MessageBox.Show(App.Current.MainWindow, messageBoxText: $@"An unexpected has occurred:
Activity: {asyncOpException.Activity}
Error Type: {asyncOpException.Code.GetDisplayName()}
Message: {asyncOpException.Message}
Current Operation: {asyncOpException.CurrentOperation}", "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageLevel = StatusMessageLevel.Error;
            }
            else
            {
                string currentOperation = CurrentOperation;
                if (string.IsNullOrWhiteSpace(currentOperation))
                    MessageBox.Show(App.Current.MainWindow, messageBoxText: $@"An unexpected has occurred:
Activity: {Activity}
Latest Status: {StatusDescription}
Message: {error.Message}", "Unexpected Error", MessageBoxButton.OK, (error is WarningException) ? MessageBoxImage.Warning : MessageBoxImage.Error);
                else MessageBox.Show(App.Current.MainWindow, messageBoxText: $@"An unexpected has occurred:
Activity: {Activity}
Latest Status: {StatusDescription}
Message: {error.Message}
Current Operation: {currentOperation}", "Unexpected Error", MessageBoxButton.OK, (error is WarningException) ? MessageBoxImage.Warning : MessageBoxImage.Error);
            }
        }

        private void OnProgressEvent(IBackgroundProgressEvent progressEvent)
        {
            StatusDescription = progressEvent.StatusDescription;
            CurrentOperation = progressEvent.CurrentOperation;
            MessageCode? messageCode = progressEvent.Code;
            Exception error = (progressEvent is IBackgroundOperationErrorOptEvent errorOptEvent) ? errorOptEvent.Error : null;
            if (messageCode.HasValue)
                MessageLevel = messageCode.Value.GetAmbientValue(StatusMessageLevel.Information);
            else
                MessageLevel = (error is null) ? StatusMessageLevel.Information : (error is WarningException) ? StatusMessageLevel.Warning : StatusMessageLevel.Error;
        }

        [Obsolete("Use IBackgroundProgressService and/or Create(IBackgroundProgressInfo, MessageCode?, IObservable<IBackgroundProgressEvent>, Action<BackgroundJobVM>, out ProgressObserver), instead")]
        public static BackgroundJobVM Create([DisallowNull] string title, [DisallowNull] string initialMessage,
            [DisallowNull] Func<StatusListener, IBackgroundJob, Task> createTask, out IAsyncJob job)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException($"'{nameof(title)}' cannot be null or whitespace.", nameof(title));
            if (string.IsNullOrWhiteSpace(initialMessage))
                throw new ArgumentException($"'{nameof(initialMessage)}' cannot be null or whitespace.", nameof(initialMessage));
            if (createTask is null)
                throw new ArgumentNullException(nameof(createTask));

            BackgroundJobVM viewModel = new();
            // TODO: Implement BackgroundJobVM.Create(string, string, Func{StatusListener, IBackgroundJob, Task}, out IAsyncJob)
            throw new NotImplementedException();
            //job = new Job(title, initialMessage, viewModel, createTask);
            //viewModel.Message = job.Message;
            //viewModel.Title = job.Title;
            //viewModel.MessageLevel = job.MessageLevel;
            //return viewModel;
        }

        [Obsolete("Use IBackgroundProgressService and/or Create(IBackgroundProgressInfo, MessageCode?, IObservable<IBackgroundProgressEvent>, Action<BackgroundJobVM>, out ProgressObserver), instead")]
        public static BackgroundJobVM Create<TResult>([DisallowNull] string title, [DisallowNull] string initialMessage,
            [DisallowNull] Func<StatusListener, IBackgroundJob, Task<TResult>> createTask, out IAsyncJob<TResult> job)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException($"'{nameof(title)}' cannot be null or whitespace.", nameof(title));
            if (string.IsNullOrWhiteSpace(initialMessage))
                throw new ArgumentException($"'{nameof(initialMessage)}' cannot be null or whitespace.", nameof(initialMessage));
            if (createTask is null)
                throw new ArgumentNullException(nameof(createTask));

            BackgroundJobVM viewModel = new() { Title = title, Message = initialMessage };
            // TODO: Implement BackgroundJobVM.Create(string, string, Func{StatusListener, IBackgroundJob, Task{TResult}}, out IAsyncJob{TResult})
            throw new NotImplementedException();
            //job = new Job<TResult>(title, initialMessage, viewModel, createTask);
            //viewModel.Message = job.Message;
            //viewModel.Title = job.Title;
            //viewModel.MessageLevel = job.MessageLevel;
            //return viewModel;
        }
    }
}
