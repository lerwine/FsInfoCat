using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class CrawlConfigItemVM : DbEntityItemVM<CrawlConfiguration>
    {
        public event EventHandler StartCrawlNow;
        public event EventHandler OpenRootFolder;
        public event EventHandler ShowLogs;

        #region Command Members

        #region StartCrawlNow Command Members

        private static readonly DependencyPropertyKey StartCrawlNowCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(StartCrawlNowCommand),
            typeof(Commands.RelayCommand), typeof(CrawlConfigItemVM), new PropertyMetadata(null));

        public static readonly DependencyProperty StartCrawlNowCommandProperty = StartCrawlNowCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand StartCrawlNowCommand => (Commands.RelayCommand)GetValue(StartCrawlNowCommandProperty);

        #endregion
        #region OpenRootFolder Command Members

        private static readonly DependencyPropertyKey OpenRootFolderCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(OpenRootFolderCommand),
            typeof(Commands.RelayCommand), typeof(CrawlConfigItemVM), new PropertyMetadata(null));

        public static readonly DependencyProperty OpenRootFolderCommandProperty = OpenRootFolderCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand OpenRootFolderCommand => (Commands.RelayCommand)GetValue(OpenRootFolderCommandProperty);

        #endregion
        #region ShowLogs Command Members

        private static readonly DependencyPropertyKey ShowLogsCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ShowLogsCommand),
            typeof(Commands.RelayCommand), typeof(CrawlConfigItemVM), new PropertyMetadata(null));

        public static readonly DependencyProperty ShowLogsCommandProperty = ShowLogsCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand ShowLogsCommand => (Commands.RelayCommand)GetValue(ShowLogsCommandProperty);

        #endregion

        #endregion
        #region DisplayName Property Members

        private static readonly DependencyPropertyKey DisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(DisplayName), typeof(string), typeof(CrawlConfigItemVM), new PropertyMetadata(""));

        public static readonly DependencyProperty DisplayNameProperty = DisplayNamePropertyKey.DependencyProperty;

        public string DisplayName
        {
            get => GetValue(DisplayNameProperty) as string;
            private set => SetValue(DisplayNamePropertyKey, value);
        }

        #endregion
        #region LastCrawlEnd Property Members

        private static readonly DependencyPropertyKey LastCrawlEndPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastCrawlEnd), typeof(DateTime?), typeof(CrawlConfigItemVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastCrawlEndProperty = LastCrawlEndPropertyKey.DependencyProperty;

        public DateTime? LastCrawlEnd
        {
            get => (DateTime?)GetValue(LastCrawlEndProperty);
            private set => SetValue(LastCrawlEndPropertyKey, value);
        }

        #endregion
        #region LastCrawlStart Property Members

        private static readonly DependencyPropertyKey LastCrawlStartPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastCrawlStart), typeof(DateTime?), typeof(CrawlConfigItemVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastCrawlStartProperty = LastCrawlStartPropertyKey.DependencyProperty;

        public DateTime? LastCrawlStart
        {
            get => (DateTime?)GetValue(LastCrawlStartProperty);
            private set => SetValue(LastCrawlStartPropertyKey, value);
        }

        #endregion
        #region MaxRecursionDepth Property Members

        private static readonly DependencyPropertyKey MaxRecursionDepthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MaxRecursionDepth), typeof(ushort), typeof(CrawlConfigItemVM),
                new PropertyMetadata(DbConstants.DbColDefaultValue_MaxRecursionDepth));

        public static readonly DependencyProperty MaxRecursionDepthProperty = MaxRecursionDepthPropertyKey.DependencyProperty;

        public ushort MaxRecursionDepth
        {
            get => (ushort)GetValue(MaxRecursionDepthProperty);
            private set => SetValue(MaxRecursionDepthPropertyKey, value);
        }

        #endregion
        #region MaxTotalItems Property Members

        private static readonly DependencyPropertyKey MaxTotalItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MaxTotalItems), typeof(ulong?), typeof(CrawlConfigItemVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty MaxTotalItemsProperty = MaxTotalItemsPropertyKey.DependencyProperty;

        public ulong? MaxTotalItems
        {
            get => (ulong?)GetValue(MaxTotalItemsProperty);
            private set => SetValue(MaxTotalItemsPropertyKey, value);
        }

        #endregion
        #region NextScheduledStart Property Members

        private static readonly DependencyPropertyKey NextScheduledStartPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NextScheduledStart), typeof(DateTime?), typeof(CrawlConfigItemVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty NextScheduledStartProperty = NextScheduledStartPropertyKey.DependencyProperty;

        public DateTime? NextScheduledStart
        {
            get => (DateTime?)GetValue(NextScheduledStartProperty);
            private set => SetValue(NextScheduledStartPropertyKey, value);
        }

        #endregion
        #region Notes Property Members

        private static readonly DependencyPropertyKey NotesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Notes), typeof(string), typeof(CrawlConfigItemVM), new PropertyMetadata(""));

        public static readonly DependencyProperty NotesProperty = NotesPropertyKey.DependencyProperty;

        public string Notes
        {
            get => GetValue(NotesProperty) as string;
            private set => SetValue(NotesPropertyKey, value);
        }

        #endregion
        #region RescheduleAfterFail Property Members

        private static readonly DependencyPropertyKey RescheduleAfterFailPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RescheduleAfterFail), typeof(bool), typeof(CrawlConfigItemVM),
                new PropertyMetadata(false));

        public static readonly DependencyProperty RescheduleAfterFailProperty = RescheduleAfterFailPropertyKey.DependencyProperty;

        public bool RescheduleAfterFail
        {
            get => (bool)GetValue(RescheduleAfterFailProperty);
            private set => SetValue(RescheduleAfterFailPropertyKey, value);
        }

        #endregion
        #region RescheduleFromJobEnd Property Members

        private static readonly DependencyPropertyKey RescheduleFromJobEndPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RescheduleFromJobEnd), typeof(bool), typeof(CrawlConfigItemVM),
                new PropertyMetadata(false));

        public static readonly DependencyProperty RescheduleFromJobEndProperty = RescheduleFromJobEndPropertyKey.DependencyProperty;

        public bool RescheduleFromJobEnd
        {
            get => (bool)GetValue(RescheduleFromJobEndProperty);
            private set => SetValue(RescheduleFromJobEndPropertyKey, value);
        }

        #endregion
        #region RescheduleInterval Property Members

        private static readonly DependencyPropertyKey RescheduleIntervalPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RescheduleInterval), typeof(TimeSpan?), typeof(CrawlConfigItemVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty RescheduleIntervalProperty = RescheduleIntervalPropertyKey.DependencyProperty;

        public TimeSpan? RescheduleInterval
        {
            get => (TimeSpan?)GetValue(RescheduleIntervalProperty);
            private set => SetValue(RescheduleIntervalPropertyKey, value);
        }

        #endregion
        #region FullName Property Members

        private static readonly DependencyPropertyKey FullNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(FullName), typeof(string), typeof(CrawlConfigItemVM), new PropertyMetadata(""));

        public static readonly DependencyProperty FullNameProperty = FullNamePropertyKey.DependencyProperty;

        public string FullName
        {
            get => GetValue(FullNameProperty) as string;
            private set => SetValue(FullNamePropertyKey, value);
        }

        #endregion
        #region StatusValue Property Members

        private static readonly DependencyPropertyKey StatusValuePropertyKey = DependencyProperty.RegisterReadOnly(nameof(StatusValue), typeof(CrawlStatus), typeof(CrawlConfigItemVM),
                new PropertyMetadata(CrawlStatus.NotRunning, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as CrawlConfigItemVM).OnStatusValuePropertyChanged((CrawlStatus)e.OldValue, (CrawlStatus)e.NewValue)));

        public static readonly DependencyProperty StatusValueProperty = StatusValuePropertyKey.DependencyProperty;

        public CrawlStatus StatusValue
        {
            get => (CrawlStatus)GetValue(StatusValueProperty);
            private set => SetValue(StatusValuePropertyKey, value);
        }

        protected virtual void OnStatusValuePropertyChanged(CrawlStatus oldValue, CrawlStatus newValue) => StartCrawlNowCommand.IsEnabled = StatusValue != CrawlStatus.Disabled;

        #endregion
        #region TTL Property Members

        private static readonly DependencyPropertyKey TTLPropertyKey = DependencyProperty.RegisterReadOnly(nameof(TTL), typeof(TimeSpan?), typeof(CrawlConfigItemVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TTLProperty = TTLPropertyKey.DependencyProperty;

        public TimeSpan? TTL
        {
            get => (TimeSpan?)GetValue(TTLProperty);
            private set => SetValue(TTLPropertyKey, value);
        }

        #endregion

        internal CrawlConfigItemVM([DisallowNull] CrawlConfiguration model, string fullName, Guid rootId)
            : base(model)
        {
            SetValue(StartCrawlNowCommandPropertyKey, new Commands.RelayCommand(parameter => StartCrawlNow?.Invoke(this, EventArgs.Empty)));
            SetValue(OpenRootFolderCommandPropertyKey, new Commands.RelayCommand(parameter => ShowLogs?.Invoke(this, EventArgs.Empty)));
            SetValue(OpenRootFolderCommandPropertyKey, new Commands.RelayCommand(parameter => OpenRootFolder?.Invoke(this, EventArgs.Empty)));
            FullName = fullName;
            Subdirectory root = Model.Root;
            if (root is not null && root.Id != rootId)
                // DEFERRED: Replace with background job view model
                _ = Subdirectory.LookupFullNameAsync(root, CancellationToken.None).ContinueWith(task =>
                  {
                      if (task.IsFaulted)
                          Dispatcher.Invoke(() => OnLookupFullNameError(task.Exception));
                      else if (!task.IsCanceled)
                          Dispatcher.Invoke(() => OnLookupFullNameComplete(task.Result ?? ""));
                  }, CancellationToken.None);
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
        }

        private void OnLookupFullNameComplete(string result)
        {
            if (FullName.Equals(result))
                return;
            FullName = result;
        }

        private static void OnLookupFullNameError(AggregateException exception)
        {
            MessageBox.Show(Application.Current.MainWindow, string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message,
                "Full Name Lookup Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        internal static CrawlConfigItemVM UpsertItem(CrawlConfiguration item, ReadOnlyObservableCollection<CrawlConfigItemVM> crawlConfigurations, List<CrawlConfigItemVM> allCrawlConfigurations, bool showActive, bool showInactive)
        {
            // TODO: Add or update view model item
            throw new NotImplementedException();
        }

        protected override void OnModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(CrawlConfiguration.DisplayName):
                    Dispatcher.CheckInvoke(() => DisplayName = Model?.DisplayName);
                    break;
                case nameof(CrawlConfiguration.MaxRecursionDepth):
                    Dispatcher.CheckInvoke(() => MaxRecursionDepth = Model?.MaxRecursionDepth ?? DbConstants.DbColDefaultValue_MaxRecursionDepth);
                    break;
                case nameof(CrawlConfiguration.MaxTotalItems):
                    Dispatcher.CheckInvoke(() => MaxTotalItems = Model?.MaxTotalItems);
                    break;
                case nameof(CrawlConfiguration.TTL):
                    Dispatcher.CheckInvoke(() =>
                    {
                        long? seconds = Model?.TTL;
                        TTL = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null;
                    });
                    break;
                case nameof(CrawlConfiguration.RescheduleInterval):
                    Dispatcher.CheckInvoke(() =>
                    {
                        long? seconds = Model?.RescheduleInterval;
                        RescheduleInterval = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null;
                    });
                    break;
                case nameof(CrawlConfiguration.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Model?.Notes);
                    break;
                case nameof(CrawlConfiguration.StatusValue):
                    Dispatcher.CheckInvoke(() => StatusValue = Model?.StatusValue ?? CrawlStatus.NotRunning);
                    break;
                case nameof(CrawlConfiguration.LastCrawlStart):
                    Dispatcher.CheckInvoke(() => LastCrawlStart = Model?.LastCrawlStart);
                    break;
                case nameof(CrawlConfiguration.LastCrawlEnd):
                    Dispatcher.CheckInvoke(() => LastCrawlEnd = Model?.LastCrawlEnd);
                    break;
                case nameof(CrawlConfiguration.NextScheduledStart):
                    Dispatcher.CheckInvoke(() => NextScheduledStart = Model?.NextScheduledStart);
                    break;
                case nameof(CrawlConfiguration.RescheduleFromJobEnd):
                    Dispatcher.CheckInvoke(() => RescheduleFromJobEnd = Model?.RescheduleFromJobEnd ?? false);
                    break;
                case nameof(CrawlConfiguration.RescheduleAfterFail):
                    Dispatcher.CheckInvoke(() => RescheduleAfterFail = Model?.RescheduleAfterFail ?? false);
                    break;
                case nameof(CrawlConfiguration.Root):
                    Subdirectory root = Model?.Root;
                    if (root is null)
                        OnLookupFullNameComplete("");
                    else
                        // TODO: Replace with async view model job
                        _ = Subdirectory.LookupFullNameAsync(root, CancellationToken.None).ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                                Dispatcher.CheckInvoke(() => OnLookupFullNameError(task.Exception));
                            else if (!task.IsCanceled)
                                Dispatcher.CheckInvoke(() => OnLookupFullNameComplete(task.Result ?? ""));
                        });
                    return;
            }
        }

        protected override DbSet<CrawlConfiguration> GetDbSet(LocalDbContext dbContext) => dbContext.CrawlConfigurations;
    }
}
