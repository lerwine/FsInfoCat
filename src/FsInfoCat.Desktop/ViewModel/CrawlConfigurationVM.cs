using FsInfoCat.Local;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel
{
    public class CrawlConfigurationVM : DependencyObject
    {
        private static readonly DependencyPropertyKey DisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(DisplayName), typeof(string), typeof(CrawlConfigurationVM), new PropertyMetadata(""));

        public static readonly DependencyProperty DisplayNameProperty = DisplayNamePropertyKey.DependencyProperty;

        public string DisplayName
        {
            get => GetValue(DisplayNameProperty) as string;
            private set => SetValue(DisplayNamePropertyKey, value);
        }

        private static readonly DependencyPropertyKey StatusValuePropertyKey = DependencyProperty.RegisterReadOnly(nameof(StatusValue), typeof(CrawlStatus), typeof(CrawlConfigurationVM),
                new PropertyMetadata(CrawlStatus.NotRunning, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as CrawlConfigurationVM).OnStatusValuePropertyChanged((CrawlStatus)e.OldValue, (CrawlStatus)e.NewValue)));

        public static readonly DependencyProperty StatusValueProperty = StatusValuePropertyKey.DependencyProperty;

        public CrawlStatus StatusValue
        {
            get => (CrawlStatus)GetValue(StatusValueProperty);
            private set => SetValue(StatusValuePropertyKey, value);
        }

        protected virtual void OnStatusValuePropertyChanged(CrawlStatus oldValue, CrawlStatus newValue) => StartCrawlNowCommand.IsEnabled = StatusValue != CrawlStatus.Disabled;

        private static readonly DependencyPropertyKey MaxRecursionDepthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MaxRecursionDepth), typeof(ushort), typeof(CrawlConfigurationVM),
                new PropertyMetadata(DbConstants.DbColDefaultValue_MaxRecursionDepth));

        public static readonly DependencyProperty MaxRecursionDepthProperty = MaxRecursionDepthPropertyKey.DependencyProperty;

        public ushort MaxRecursionDepth
        {
            get => (ushort)GetValue(MaxRecursionDepthProperty);
            private set => SetValue(MaxRecursionDepthPropertyKey, value);
        }

        private static readonly DependencyPropertyKey MaxTotalItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MaxTotalItems), typeof(ulong?), typeof(CrawlConfigurationVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty MaxTotalItemsProperty = MaxTotalItemsPropertyKey.DependencyProperty;

        public ulong? MaxTotalItems
        {
            get => (ulong?)GetValue(MaxTotalItemsProperty);
            private set => SetValue(MaxTotalItemsPropertyKey, value);
        }

        private static readonly DependencyPropertyKey TTLPropertyKey = DependencyProperty.RegisterReadOnly(nameof(TTL), typeof(TimeSpan?), typeof(CrawlConfigurationVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TTLProperty = TTLPropertyKey.DependencyProperty;

        public TimeSpan? TTL
        {
            get => (TimeSpan?)GetValue(TTLProperty);
            private set => SetValue(TTLPropertyKey, value);
        }

        private static readonly DependencyPropertyKey LastCrawlStartPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastCrawlStart), typeof(DateTime?), typeof(CrawlConfigurationVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastCrawlStartProperty = LastCrawlStartPropertyKey.DependencyProperty;

        public DateTime? LastCrawlStart
        {
            get => (DateTime?)GetValue(LastCrawlStartProperty);
            private set => SetValue(LastCrawlStartPropertyKey, value);
        }

        private static readonly DependencyPropertyKey LastCrawlEndPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastCrawlEnd), typeof(DateTime?), typeof(CrawlConfigurationVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastCrawlEndProperty = LastCrawlEndPropertyKey.DependencyProperty;

        public DateTime? LastCrawlEnd
        {
            get => (DateTime?)GetValue(LastCrawlEndProperty);
            private set => SetValue(LastCrawlEndPropertyKey, value);
        }

        private static readonly DependencyPropertyKey NextScheduledStartPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NextScheduledStart), typeof(DateTime?), typeof(CrawlConfigurationVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty NextScheduledStartProperty = NextScheduledStartPropertyKey.DependencyProperty;

        public DateTime? NextScheduledStart
        {
            get => (DateTime?)GetValue(NextScheduledStartProperty);
            private set => SetValue(NextScheduledStartPropertyKey, value);
        }

        private static readonly DependencyPropertyKey RescheduleIntervalPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RescheduleInterval), typeof(TimeSpan?), typeof(CrawlConfigurationVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty RescheduleIntervalProperty = RescheduleIntervalPropertyKey.DependencyProperty;

        public TimeSpan? RescheduleInterval
        {
            get => (TimeSpan?)GetValue(RescheduleIntervalProperty);
            private set => SetValue(RescheduleIntervalPropertyKey, value);
        }

        private static readonly DependencyPropertyKey RescheduleFromJobEndPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RescheduleFromJobEnd), typeof(bool), typeof(CrawlConfigurationVM),
                new PropertyMetadata(false));

        public static readonly DependencyProperty RescheduleFromJobEndProperty = RescheduleFromJobEndPropertyKey.DependencyProperty;

        public bool RescheduleFromJobEnd
        {
            get => (bool)GetValue(RescheduleFromJobEndProperty);
            private set => SetValue(RescheduleFromJobEndPropertyKey, value);
        }

        private static readonly DependencyPropertyKey RescheduleAfterFailPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RescheduleAfterFail), typeof(bool), typeof(CrawlConfigurationVM),
                new PropertyMetadata(false));

        public static readonly DependencyProperty RescheduleAfterFailProperty = RescheduleAfterFailPropertyKey.DependencyProperty;

        public bool RescheduleAfterFail
        {
            get => (bool)GetValue(RescheduleAfterFailProperty);
            private set => SetValue(RescheduleAfterFailPropertyKey, value);
        }

        private static readonly DependencyPropertyKey FullNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(FullName), typeof(string), typeof(CrawlConfigurationVM), new PropertyMetadata(""));

        public static readonly DependencyProperty FullNameProperty = FullNamePropertyKey.DependencyProperty;

        public string FullName
        {
            get => GetValue(FullNameProperty) as string;
            private set => SetValue(FullNamePropertyKey, value);
        }

        private static readonly DependencyPropertyKey NotesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Notes), typeof(string), typeof(CrawlConfigurationVM), new PropertyMetadata(""));

        public static readonly DependencyProperty NotesProperty = NotesPropertyKey.DependencyProperty;

        public string Notes
        {
            get => GetValue(NotesProperty) as string;
            private set => SetValue(NotesPropertyKey, value);
        }

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?), typeof(CrawlConfigurationVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        public DateTime? LastSynchronizedOn
        {
            get => (DateTime?)GetValue(LastSynchronizedOnProperty);
            private set => SetValue(LastSynchronizedOnPropertyKey, value);
        }

        private static readonly DependencyPropertyKey CreatedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CreatedOn), typeof(DateTime), typeof(CrawlConfigurationVM),
                new PropertyMetadata(default));

        public static readonly DependencyProperty CreatedOnProperty = CreatedOnPropertyKey.DependencyProperty;

        public DateTime CreatedOn
        {
            get => (DateTime)GetValue(CreatedOnProperty);
            private set => SetValue(CreatedOnPropertyKey, value);
        }

        private static readonly DependencyPropertyKey ModifiedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ModifiedOn), typeof(DateTime), typeof(CrawlConfigurationVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ModifiedOnProperty = ModifiedOnPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey ModelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Model), typeof(CrawlConfiguration), typeof(CrawlConfigurationVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ModelProperty = ModelPropertyKey.DependencyProperty;

        public CrawlConfiguration Model
        {
            get => (CrawlConfiguration)GetValue(ModelProperty);
            private set => SetValue(ModelPropertyKey, value);
        }

        public DateTime ModifiedOn
        {
            get => (DateTime)GetValue(ModifiedOnProperty);
            private set => SetValue(ModifiedOnPropertyKey, value);
        }

        public event EventHandler StartCrawlNow;

        private static readonly DependencyPropertyKey StartCrawlNowCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(StartCrawlNowCommand),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationVM), new PropertyMetadata(null));

        public static readonly DependencyProperty StartCrawlNowCommandProperty = StartCrawlNowCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand StartCrawlNowCommand => (Commands.RelayCommand)GetValue(StartCrawlNowCommandProperty);

        public event EventHandler Edit;

        private static readonly DependencyPropertyKey EditCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EditCommand),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationVM), new PropertyMetadata(null));

        public static readonly DependencyProperty EditCommandProperty = EditCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand EditCommand => (Commands.RelayCommand)GetValue(EditCommandProperty);

        public event EventHandler Delete;

        private static readonly DependencyPropertyKey DeleteCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(DeleteCommand),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationVM), new PropertyMetadata(null));

        public static readonly DependencyProperty DeleteCommandProperty = DeleteCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand DeleteCommand => (Commands.RelayCommand)GetValue(DeleteCommandProperty);

        public event EventHandler OpenRootFolder;

        private static readonly DependencyPropertyKey OpenRootFolderCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(OpenRootFolderCommand),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationVM), new PropertyMetadata(null));

        public static readonly DependencyProperty OpenRootFolderCommandProperty = OpenRootFolderCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand OpenRootFolderCommand => (Commands.RelayCommand)GetValue(OpenRootFolderCommandProperty);

        public event EventHandler ShowLogs;

        private static readonly DependencyPropertyKey ShowLogsCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ShowLogsCommand),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationVM), new PropertyMetadata(null));

        public static readonly DependencyProperty ShowLogsCommandProperty = ShowLogsCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand ShowLogsCommand => (Commands.RelayCommand)GetValue(ShowLogsCommandProperty);

        internal CrawlConfigurationVM([DisallowNull] CrawlConfiguration model, string fullName, Guid rootId)
        {
            SetValue(StartCrawlNowCommandPropertyKey, new Commands.RelayCommand(parameter => StartCrawlNow?.Invoke(this, EventArgs.Empty)));
            SetValue(EditCommandPropertyKey, new Commands.RelayCommand(parameter => Edit?.Invoke(this, EventArgs.Empty)));
            SetValue(DeleteCommandPropertyKey, new Commands.RelayCommand(parameter => Delete?.Invoke(this, EventArgs.Empty)));
            SetValue(OpenRootFolderCommandPropertyKey, new Commands.RelayCommand(parameter => ShowLogs?.Invoke(this, EventArgs.Empty)));
            SetValue(OpenRootFolderCommandPropertyKey, new Commands.RelayCommand(parameter => OpenRootFolder?.Invoke(this, EventArgs.Empty)));
            FullName = fullName;
            Subdirectory root = (Model = model ?? throw new ArgumentNullException(nameof(model))).Root;
            model.PropertyChanged += Model_PropertyChanged;
            if (root is not null && root.Id != rootId)
                _ = Subdirectory.LookupFullNameAsync(root).ContinueWith(task =>
                  {
                      if (task.IsFaulted)
                          Dispatcher.Invoke(() => OnLookupFullNameError(task.Exception));
                      else if (!task.IsCanceled)
                          Dispatcher.Invoke(() => OnLookupFullNameComplete(task.Result ?? ""));
                  });
            DisplayName = model.DisplayName;
            MaxRecursionDepth = model.MaxRecursionDepth;
            MaxTotalItems = model.MaxTotalItems;
            long? seconds = model.TTL;
            TTL = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null;
            seconds = model.RescheduleInterval;
            RescheduleInterval = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null;
            Notes = model.Notes;
            StatusValue = model.StatusValue;
            LastCrawlStart = model.LastCrawlStart;
            LastCrawlEnd = model.LastCrawlEnd;
            NextScheduledStart = model.NextScheduledStart;
            RescheduleFromJobEnd = model.RescheduleFromJobEnd;
            RescheduleAfterFail = model.RescheduleAfterFail;
            LastSynchronizedOn = model.LastSynchronizedOn;
            CreatedOn = model.CreatedOn;
            ModifiedOn = model.ModifiedOn;
        }

        private void OnLookupFullNameComplete(string result)
        {
            if (FullName.Equals(result))
                return;
            FullName = result;
        }

        private void OnLookupFullNameError(AggregateException exception)
        {
            MessageBox.Show(Application.Current.MainWindow, string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message,
                "Full Name Lookup Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CrawlConfiguration.DisplayName):
                    Dispatcher.Invoke(() => DisplayName = Model.DisplayName);
                    break;
                case nameof(CrawlConfiguration.MaxRecursionDepth):
                    Dispatcher.Invoke(() => MaxRecursionDepth = Model.MaxRecursionDepth);
                    break;
                case nameof(CrawlConfiguration.MaxTotalItems):
                    Dispatcher.Invoke(() => MaxTotalItems = Model.MaxTotalItems);
                    break;
                case nameof(CrawlConfiguration.TTL):
                    Dispatcher.Invoke(() =>
                    {
                        long? seconds = Model.TTL;
                        TTL = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null;
                    });
                    break;
                case nameof(CrawlConfiguration.RescheduleInterval):
                    Dispatcher.Invoke(() =>
                    {
                        long? seconds = Model.RescheduleInterval;
                        RescheduleInterval = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null;
                    });
                    break;
                case nameof(CrawlConfiguration.Notes):
                    Dispatcher.Invoke(() => Notes = Model.Notes);
                    break;
                case nameof(CrawlConfiguration.StatusValue):
                    Dispatcher.Invoke(() => StatusValue = Model.StatusValue);
                    break;
                case nameof(CrawlConfiguration.LastCrawlStart):
                    Dispatcher.Invoke(() => LastCrawlStart = Model.LastCrawlStart);
                    break;
                case nameof(CrawlConfiguration.LastCrawlEnd):
                    Dispatcher.Invoke(() => LastCrawlEnd = Model.LastCrawlEnd);
                    break;
                case nameof(CrawlConfiguration.NextScheduledStart):
                    Dispatcher.Invoke(() => NextScheduledStart = Model.NextScheduledStart);
                    break;
                case nameof(CrawlConfiguration.RescheduleFromJobEnd):
                    Dispatcher.Invoke(() => RescheduleFromJobEnd = Model.RescheduleFromJobEnd);
                    break;
                case nameof(CrawlConfiguration.RescheduleAfterFail):
                    Dispatcher.Invoke(() => RescheduleAfterFail = Model.RescheduleAfterFail);
                    break;
                case nameof(CrawlConfiguration.LastSynchronizedOn):
                    Dispatcher.Invoke(() => LastSynchronizedOn = Model.LastSynchronizedOn);
                    break;
                case nameof(CrawlConfiguration.CreatedOn):
                    Dispatcher.Invoke(() => CreatedOn = Model.CreatedOn);
                    break;
                case nameof(CrawlConfiguration.ModifiedOn):
                    Dispatcher.Invoke(() => ModifiedOn = Model.ModifiedOn);
                    break;
                case nameof(CrawlConfiguration.Root):
                    Subdirectory root = Model.Root;
                    if (root is null)
                        OnLookupFullNameComplete("");
                    else
                        _ = Subdirectory.LookupFullNameAsync(root).ContinueWith(task =>
                          {
                              if (task.IsFaulted)
                                  Dispatcher.Invoke(() => OnLookupFullNameError(task.Exception));
                              else if (!task.IsCanceled)
                                  Dispatcher.Invoke(() => OnLookupFullNameComplete(task.Result ?? ""));
                          });
                    return;
            }
        }

        internal static CrawlConfigurationVM UpsertItem(CrawlConfiguration item, ReadOnlyObservableCollection<CrawlConfigurationVM> crawlConfigurations, List<CrawlConfigurationVM> allCrawlConfigurations, bool showActive, bool showInactive)
        {
            // TODO: Add or update view model item
            throw new NotImplementedException();
        }
    }
}
