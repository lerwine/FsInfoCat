using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class CrawlConfigItemVM : DbEntityItemVM<CrawlConfigListItem>
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
        #region VolumeDisplayName Property Members

        private static readonly DependencyPropertyKey VolumeDisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeDisplayName), typeof(string), typeof(CrawlConfigItemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="VolumeDisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeDisplayNameProperty = VolumeDisplayNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string VolumeDisplayName { get => GetValue(VolumeDisplayNameProperty) as string; private set => SetValue(VolumeDisplayNamePropertyKey, value); }

        #endregion
        #region VolumeName Property Members

        private static readonly DependencyPropertyKey VolumeNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeName), typeof(string), typeof(CrawlConfigItemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="VolumeName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeNameProperty = VolumeNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string VolumeName { get => GetValue(VolumeNameProperty) as string; private set => SetValue(VolumeNamePropertyKey, value); }

        #endregion
        #region VolumeIdentifier Property Members

        private static readonly DependencyPropertyKey VolumeIdentifierPropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeIdentifier), typeof(VolumeIdentifier), typeof(CrawlConfigItemVM),
                new PropertyMetadata(VolumeIdentifier.Empty));

        /// <summary>
        /// Identifies the <see cref="VolumeIdentifier"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeIdentifierProperty = VolumeIdentifierPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public VolumeIdentifier VolumeIdentifier { get => (VolumeIdentifier)GetValue(VolumeIdentifierProperty); private set => SetValue(VolumeIdentifierPropertyKey, value); }

        #endregion
        #region BgOps Property Members

        private static readonly DependencyPropertyKey BgOpsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BgOps), typeof(AsyncOps.AsyncBgModalVM), typeof(CrawlConfigItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="BgOps"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BgOpsProperty = BgOpsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public AsyncOps.AsyncBgModalVM BgOps => (AsyncOps.AsyncBgModalVM)GetValue(BgOpsProperty);

        #endregion
        #region FileSystemDisplayName Property Members

        private static readonly DependencyPropertyKey FileSystemDisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileSystemDisplayName), typeof(string), typeof(CrawlConfigItemVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as CrawlConfigItemVM)?.OnFileSystemDisplayNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Identifies the <see cref="FileSystemDisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileSystemDisplayNameProperty = FileSystemDisplayNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public string FileSystemDisplayName { get => GetValue(FileSystemDisplayNameProperty) as string; private set => SetValue(FileSystemDisplayNamePropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="FileSystemDisplayName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FileSystemDisplayName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FileSystemDisplayName"/> property.</param>
        private void OnFileSystemDisplayNamePropertyChanged(string oldValue, string newValue)
        {
            FileSystemDetailText = string.IsNullOrWhiteSpace(newValue) ? FileSystemSymbolicName :
                (string.IsNullOrWhiteSpace(FileSystemSymbolicName) ? newValue :
                $"{oldValue.AsWsNormalizedOrEmpty()} ({FileSystemDisplayName.AsWsNormalizedOrEmpty()})");
        }

        #endregion
        #region FileSystemSymbolicName Property Members

        private static readonly DependencyPropertyKey FileSystemSymbolicNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileSystemSymbolicName), typeof(string), typeof(CrawlConfigItemVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as CrawlConfigItemVM)?.OnFileSystemSymbolicNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Identifies the <see cref="FileSystemSymbolicName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileSystemSymbolicNameProperty = FileSystemSymbolicNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public string FileSystemSymbolicName { get => GetValue(FileSystemSymbolicNameProperty) as string; private set => SetValue(FileSystemSymbolicNamePropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="FileSystemSymbolicName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FileSystemSymbolicName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FileSystemSymbolicName"/> property.</param>
        private void OnFileSystemSymbolicNamePropertyChanged(string oldValue, string newValue)
        {
            FileSystemDetailText = string.IsNullOrWhiteSpace(newValue) ? FileSystemDisplayName :
                (string.IsNullOrWhiteSpace(FileSystemDisplayName) ? newValue :
                $"{oldValue.AsWsNormalizedOrEmpty()} ({FileSystemDisplayName.AsWsNormalizedOrEmpty()})");
        }

        #endregion
        #region FileSystemDetailText Property Members

        private static readonly DependencyPropertyKey FileSystemDetailTextPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileSystemDetailText), typeof(string), typeof(CrawlConfigItemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="FileSystemDetailText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileSystemDetailTextProperty = FileSystemDetailTextPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string FileSystemDetailText { get => GetValue(FileSystemDetailTextProperty) as string; private set => SetValue(FileSystemDetailTextPropertyKey, value); }

        #endregion

        internal CrawlConfigItemVM([DisallowNull] CrawlConfigListItem model)
            : base(model)
        {
            SetValue(BgOpsPropertyKey, new AsyncOps.AsyncBgModalVM());
            SetValue(StartCrawlNowCommandPropertyKey, new Commands.RelayCommand(parameter => StartCrawlNow?.Invoke(this, EventArgs.Empty)));
            SetValue(OpenRootFolderCommandPropertyKey, new Commands.RelayCommand(parameter => ShowLogs?.Invoke(this, EventArgs.Empty)));
            SetValue(OpenRootFolderCommandPropertyKey, new Commands.RelayCommand(parameter => OpenRootFolder?.Invoke(this, EventArgs.Empty)));
            FullName = SubdirectoryItemVM.FromAncestorNames(model.AncestorNames);
            DisplayName = model.DisplayName;
            VolumeDisplayName = model.VolumeDisplayName;
            VolumeName = model.VolumeName;
            VolumeIdentifier = model.VolumeIdentifier;
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
            FileSystemDisplayName = model.FileSystemDisplayName;
            FileSystemSymbolicName = model.FileSystemSymbolicName;
        }

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(CrawlConfigListItem.DisplayName):
                    Dispatcher.CheckInvoke(() => DisplayName = Model?.DisplayName);
                    break;
                case nameof(CrawlConfigListItem.MaxRecursionDepth):
                    Dispatcher.CheckInvoke(() => MaxRecursionDepth = Model?.MaxRecursionDepth ?? DbConstants.DbColDefaultValue_MaxRecursionDepth);
                    break;
                case nameof(CrawlConfigListItem.MaxTotalItems):
                    Dispatcher.CheckInvoke(() => MaxTotalItems = Model?.MaxTotalItems);
                    break;
                case nameof(CrawlConfigListItem.TTL):
                    Dispatcher.CheckInvoke(() =>
                    {
                        long? seconds = Model?.TTL;
                        TTL = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null;
                    });
                    break;
                case nameof(CrawlConfigListItem.RescheduleInterval):
                    Dispatcher.CheckInvoke(() =>
                    {
                        long? seconds = Model?.RescheduleInterval;
                        RescheduleInterval = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null;
                    });
                    break;
                case nameof(CrawlConfigListItem.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Model?.Notes);
                    break;
                case nameof(CrawlConfigListItem.StatusValue):
                    Dispatcher.CheckInvoke(() => StatusValue = Model?.StatusValue ?? CrawlStatus.NotRunning);
                    break;
                case nameof(CrawlConfigListItem.LastCrawlStart):
                    Dispatcher.CheckInvoke(() => LastCrawlStart = Model?.LastCrawlStart);
                    break;
                case nameof(CrawlConfigListItem.LastCrawlEnd):
                    Dispatcher.CheckInvoke(() => LastCrawlEnd = Model?.LastCrawlEnd);
                    break;
                case nameof(CrawlConfigListItem.NextScheduledStart):
                    Dispatcher.CheckInvoke(() => NextScheduledStart = Model?.NextScheduledStart);
                    break;
                case nameof(CrawlConfigListItem.RescheduleFromJobEnd):
                    Dispatcher.CheckInvoke(() => RescheduleFromJobEnd = Model?.RescheduleFromJobEnd ?? false);
                    break;
                case nameof(CrawlConfigListItem.RescheduleAfterFail):
                    Dispatcher.CheckInvoke(() => RescheduleAfterFail = Model?.RescheduleAfterFail ?? false);
                    break;
                case nameof(CrawlConfigListItem.AncestorNames):
                    FullName = SubdirectoryItemVM.FromAncestorNames(Model?.AncestorNames);
                    return;
                case nameof(CrawlConfigListItem.VolumeDisplayName):
                    VolumeDisplayName = Model?.VolumeDisplayName ?? "";
                    break;
                case nameof(CrawlConfigListItem.VolumeName):
                    VolumeName = Model?.VolumeName ?? "";
                    break;
                case nameof(CrawlConfigListItem.VolumeIdentifier):
                    VolumeIdentifier = Model?.VolumeIdentifier ?? VolumeIdentifier.Empty;
                    break;
                case nameof(CrawlConfigListItem.FileSystemDisplayName):
                    FileSystemDisplayName = Model?.FileSystemDisplayName ?? "";
                    break;
                case nameof(CrawlConfigListItem.FileSystemSymbolicName):
                    FileSystemSymbolicName = Model?.FileSystemSymbolicName ?? "";
                    break;
            }
        }

        protected override DbSet<CrawlConfigListItem> GetDbSet(LocalDbContext dbContext) => dbContext.CrawlConfigListing;
    }
}
