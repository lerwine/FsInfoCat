using FsInfoCat.AsyncOps;
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
    public partial class BackgroundJobVM : DependencyObject
    {
        private readonly ILogger<BackgroundJobVM> _logger;
        private readonly ProgressObserver _observer;
        private readonly IDisposable _subscription;

        #region OperationId Property Members

        private static readonly DependencyPropertyKey OperationIdPropertyKey = DependencyPropertyBuilder<BackgroundJobVM, Guid>
            .Register(nameof(OperationId))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="OperationId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OperationIdProperty = OperationIdPropertyKey.DependencyProperty;

        public Guid OperationId => (Guid)GetValue(OperationIdProperty);

        #endregion
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
        #region Timestamp Property Members

        private static readonly DependencyPropertyKey TimestampPropertyKey = DependencyPropertyBuilder<BackgroundJobVM, TimeSpan?>
            .Register(nameof(Timestamp))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Timestamp"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TimestampProperty = TimestampPropertyKey.DependencyProperty;

        public TimeSpan? Timestamp { get => (TimeSpan?)GetValue(TimestampProperty); private set => SetValue(TimestampPropertyKey, value); }

        #endregion
        #region PercentComplete Property Members

        private static readonly DependencyPropertyKey PercentCompletePropertyKey = DependencyPropertyBuilder<BackgroundJobVM, byte?>
            .Register(nameof(PercentComplete))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="PercentComplete"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PercentCompleteProperty = PercentCompletePropertyKey.DependencyProperty;

        public byte? PercentComplete { get => (byte?)GetValue(PercentCompleteProperty); private set => SetValue(PercentCompletePropertyKey, value); }

        #endregion
        #region Items Property Members

        private readonly ObservableCollection<BackgroundJobVM> _backingItems = new();

        private static readonly DependencyPropertyKey ItemsPropertyKey = DependencyPropertyBuilder<BackgroundJobVM, ReadOnlyObservableCollection<BackgroundJobVM>>
            .Register(nameof(Items))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Items"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<BackgroundJobVM> Items => (ReadOnlyObservableCollection<BackgroundJobVM>)GetValue(ItemsProperty);

        #endregion

        public BackgroundJobVM([DisallowNull] ICancellableOperation progressInfo, MessageCode? code, [DisallowNull] IObservable<IBackgroundProgressEvent> observable, [DisallowNull] Action<BackgroundJobVM> onCompleted)
        {
            _logger = App.GetLogger(this);
            SetValue(OperationIdPropertyKey, progressInfo.OperationId);
            SetValue(ItemsPropertyKey, new ReadOnlyObservableCollection<BackgroundJobVM>(_backingItems));
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

        private static bool TryFindParentVM(Guid parentId, [DisallowNull] ObservableCollection<BackgroundJobVM> backingItems, out BackgroundJobVM vm)
        {
            vm = backingItems.FirstOrDefault(i => i.OperationId == parentId);
            if (vm is not null)
                return true;
            foreach (BackgroundJobVM item in backingItems)
            {
                if (TryFindParentVM(parentId, item._backingItems, out vm))
                    return true;
            }
            return false;
        }

        internal static void OnOperationStarted([DisallowNull] Dispatcher dispatcher, [DisallowNull] ObservableCollection<BackgroundJobVM> backingItems, [DisallowNull] ICancellableOperation progressInfo, MessageCode? code,
            [DisallowNull] IObservable<IBackgroundProgressEvent> observable) => dispatcher.Invoke(() =>
            {
                Guid? parentId = progressInfo.ParentId;
                if (parentId.HasValue && TryFindParentVM(parentId.Value, backingItems, out BackgroundJobVM parentVM))
                    backingItems = parentVM._backingItems;
                backingItems.Add(new BackgroundJobVM(progressInfo, code, observable, item => dispatcher.Invoke(() => backingItems.Remove(item))));
            });

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
                MessageLevel = messageCode?.ToStatusMessageLevel(StatusMessageLevel.Information) ?? StatusMessageLevel.Information;
            else
                MessageLevel = (error is null) ? StatusMessageLevel.Information : (error is WarningException) ? StatusMessageLevel.Warning : StatusMessageLevel.Error;
        }
    }
}
