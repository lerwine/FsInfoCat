using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    /// <summary>
    /// View Model for <see cref="View.Local.CrawlConfigRowDetail"/> and  <see cref="DbEntityListingPageVM{TDbEntity, TItemVM}.Items"/>
    /// in the <see cref="CrawlConfigurationsPageVM"/> view model.
    /// </summary>
    public sealed class CrawlConfigItemVM : DbEntityItemVM<CrawlConfigListItem>
    {
        #region Command Members

        #region StartCrawl Property Members

        /// <summary>
        /// Occurs when the <see cref="StartCrawl">StartCrawl Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> StartCrawlInvoked;

        private static readonly DependencyPropertyKey StartCrawlPropertyKey = DependencyProperty.RegisterReadOnly(nameof(StartCrawl),
            typeof(Commands.RelayCommand), typeof(CrawlConfigItemVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="StartCrawl"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StartCrawlProperty = StartCrawlPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand StartCrawl => (Commands.RelayCommand)GetValue(StartCrawlProperty);

        /// <summary>
        /// Called when the StartCrawl event is raised by <see cref="StartCrawl" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="StartCrawl" />.</param>
        private void RaiseStartCrawlInvoked(object parameter)
        {
            try { OnStartCrawlInvoked(parameter); }
            finally { StartCrawlInvoked?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="StartCrawl">StartCrawl Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="StartCrawl" />.</param>
        private void OnStartCrawlInvoked(object parameter)
        {
            // TODO: Implement OnStartCrawlInvoked Logic
        }

        #endregion
        #region StopCrawl Property Members

        /// <summary>
        /// Occurs when the <see cref="StopCrawl">StopCrawl Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> StopCrawlInvoked;

        private static readonly DependencyPropertyKey StopCrawlPropertyKey = DependencyProperty.RegisterReadOnly(nameof(StopCrawl),
            typeof(Commands.RelayCommand), typeof(CrawlConfigItemVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="StopCrawl"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StopCrawlProperty = StopCrawlPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand StopCrawl => (Commands.RelayCommand)GetValue(StopCrawlProperty);

        /// <summary>
        /// Called when the StopCrawl event is raised by <see cref="StopCrawl" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="StopCrawl" />.</param>
        private void RaiseStopCrawlInvoked(object parameter)
        {
            try { OnStopCrawlInvoked(parameter); }
            finally { StopCrawlInvoked?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="StopCrawl">StopCrawl Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="StopCrawl" />.</param>
        private void OnStopCrawlInvoked(object parameter)
        {
            // TODO: Implement OnStopCrawlInvoked Logic
        }

        #endregion
        #region OpenRootSubdirectory Property Members

        /// <summary>
        /// Occurs when the <see cref="OpenRootSubdirectory">OpenRootSubdirectory Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> OpenRootSubdirectoryInvoked;

        private static readonly DependencyPropertyKey OpenRootSubdirectoryPropertyKey = DependencyProperty.RegisterReadOnly(nameof(OpenRootSubdirectory),
            typeof(Commands.RelayCommand), typeof(CrawlConfigItemVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="OpenRootSubdirectory"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OpenRootSubdirectoryProperty = OpenRootSubdirectoryPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand OpenRootSubdirectory => (Commands.RelayCommand)GetValue(OpenRootSubdirectoryProperty);

        /// <summary>
        /// Called when the OpenRootSubdirectory event is raised by <see cref="OpenRootSubdirectory" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="OpenRootSubdirectory" />.</param>
        private void RaiseOpenRootSubdirectoryInvoked(object parameter)
        {
            try { OnOpenRootSubdirectoryInvoked(parameter); }
            finally { OpenRootSubdirectoryInvoked?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="OpenRootSubdirectory">OpenRootSubdirectory Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="OpenRootSubdirectory" />.</param>
        private void OnOpenRootSubdirectoryInvoked(object parameter)
        {
            // TODO: Implement OnOpenRootSubdirectoryInvoked Logic
        }

        #endregion
        #region ShowCrawlActivityRecords Property Members

        /// <summary>
        /// Occurs when the <see cref="ShowCrawlActivityRecords">ShowCrawlActivityRecords Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ShowCrawlActivityRecordsInvoked;

        private static readonly DependencyPropertyKey ShowCrawlActivityRecordsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ShowCrawlActivityRecords),
            typeof(Commands.RelayCommand), typeof(CrawlConfigItemVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ShowCrawlActivityRecords"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowCrawlActivityRecordsProperty = ShowCrawlActivityRecordsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ShowCrawlActivityRecords => (Commands.RelayCommand)GetValue(ShowCrawlActivityRecordsProperty);

        /// <summary>
        /// Called when the ShowCrawlActivityRecords event is raised by <see cref="ShowCrawlActivityRecords" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowCrawlActivityRecords" />.</param>
        private void RaiseShowCrawlActivityRecordsInvoked(object parameter)
        {
            try { OnShowCrawlActivityRecordsInvoked(parameter); }
            finally { ShowCrawlActivityRecordsInvoked?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="ShowCrawlActivityRecords">ShowCrawlActivityRecords Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowCrawlActivityRecords" />.</param>
        private void OnShowCrawlActivityRecordsInvoked(object parameter)
        {
            using IServiceScope scope = Services.ServiceProvider.CreateScope();
            IApplicationNavigation applicationNavigation = scope.ServiceProvider.GetRequiredService<IApplicationNavigation>();
            applicationNavigation.Navigate(new Uri(MainVM.Page_Uri_Local_CrawlConfigurations), Model);
        }

        #endregion

        #endregion
        #region DisplayName Property Members

        private static readonly DependencyPropertyKey DisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(DisplayName), typeof(string),
            typeof(CrawlConfigItemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="DisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = DisplayNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public string DisplayName { get => GetValue(DisplayNameProperty) as string; private set => SetValue(DisplayNamePropertyKey, value); }

        #endregion
        #region LastCrawlEnd Property Members

        private static readonly DependencyPropertyKey LastCrawlEndPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastCrawlEnd), typeof(DateTime?),
            typeof(CrawlConfigItemVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="LastCrawlEnd"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastCrawlEndProperty = LastCrawlEndPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DateTime? LastCrawlEnd { get => (DateTime?)GetValue(LastCrawlEndProperty); private set => SetValue(LastCrawlEndPropertyKey, value); }

        #endregion
        #region LastCrawlStart Property Members

        private static readonly DependencyPropertyKey LastCrawlStartPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastCrawlStart), typeof(DateTime?),
            typeof(CrawlConfigItemVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="LastCrawlStart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastCrawlStartProperty = LastCrawlStartPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DateTime? LastCrawlStart { get => (DateTime?)GetValue(LastCrawlStartProperty); private set => SetValue(LastCrawlStartPropertyKey, value); }

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

        private static readonly DependencyPropertyKey NextScheduledStartPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NextScheduledStart),
            typeof(DateTime?), typeof(CrawlConfigItemVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="NextScheduledStart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NextScheduledStartProperty = NextScheduledStartPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DateTime? NextScheduledStart { get => (DateTime?)GetValue(NextScheduledStartProperty); private set => SetValue(NextScheduledStartPropertyKey, value); }

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
        #region RootPath Property Members

        private static readonly DependencyPropertyKey RootPathPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RootPath), typeof(string), typeof(CrawlConfigItemVM), new PropertyMetadata(""));

        public static readonly DependencyProperty RootPathProperty = RootPathPropertyKey.DependencyProperty;

        public string RootPath
        {
            get => GetValue(RootPathProperty) as string;
            private set => SetValue(RootPathPropertyKey, value);
        }

        #endregion
        #region StatusValue Property Members

        private static readonly DependencyPropertyKey StatusValuePropertyKey = DependencyProperty.RegisterReadOnly(nameof(StatusValue), typeof(CrawlStatus), typeof(CrawlConfigItemVM),
                new PropertyMetadata(CrawlStatus.NotRunning, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as CrawlConfigItemVM).OnStatusValuePropertyChanged((CrawlStatus)e.NewValue)));

        public static readonly DependencyProperty StatusValueProperty = StatusValuePropertyKey.DependencyProperty;

        public CrawlStatus StatusValue
        {
            get => (CrawlStatus)GetValue(StatusValueProperty);
            private set => SetValue(StatusValuePropertyKey, value);
        }

        private void OnStatusValuePropertyChanged(CrawlStatus newValue)
        {
            switch (newValue)
            {
                case CrawlStatus.Disabled:
                    StartCrawl.IsEnabled = StopCrawl.IsEnabled = false;
                    break;
                case CrawlStatus.InProgress:
                    StartCrawl.IsEnabled = false;
                    StopCrawl.IsEnabled = true;
                    break;
                default:
                    StopCrawl.IsEnabled = false;
                    StartCrawl.IsEnabled = true;
                    break;
            }
        }

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
            SetValue(StartCrawlPropertyKey, new Commands.RelayCommand(RaiseStartCrawlInvoked));
            SetValue(StopCrawlPropertyKey, new Commands.RelayCommand(RaiseStopCrawlInvoked));
            SetValue(OpenRootSubdirectoryPropertyKey, new Commands.RelayCommand(RaiseOpenRootSubdirectoryInvoked));
            SetValue(ShowCrawlActivityRecordsPropertyKey, new Commands.RelayCommand(RaiseShowCrawlActivityRecordsInvoked));
            RootPath = SubdirectoryItemVM.FromAncestorNames(model.AncestorNames);
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
                    RootPath = SubdirectoryItemVM.FromAncestorNames(Model?.AncestorNames);
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
