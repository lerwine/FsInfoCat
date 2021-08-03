using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class CrawlConfigurationsPageVM : DependencyObject
    {
        private readonly ILogger<CrawlConfigurationsPageVM> _logger;
        private readonly List<CrawlConfigurationVM> _allCrawlConfigurations = new();

        #region CrawlConfigurations Property Members

        private static readonly DependencyPropertyKey InnerCrawlConfigurationsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(InnerCrawlConfigurations), typeof(ObservableCollection<CrawlConfigurationVM>), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(null));

        private static readonly DependencyPropertyKey CrawlConfigurationsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CrawlConfigurations), typeof(ReadOnlyObservableCollection<CrawlConfigurationVM>), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(null));

        protected static readonly DependencyProperty InnerCrawlConfigurationsProperty = InnerCrawlConfigurationsPropertyKey.DependencyProperty;

        public static readonly DependencyProperty CrawlConfigurationsProperty = CrawlConfigurationsPropertyKey.DependencyProperty;

        protected ObservableCollection<CrawlConfigurationVM> InnerCrawlConfigurations
        {
            get => (ObservableCollection<CrawlConfigurationVM>)GetValue(InnerCrawlConfigurationsProperty);
            private set => SetValue(InnerCrawlConfigurationsPropertyKey, value);
        }

        public ReadOnlyObservableCollection<CrawlConfigurationVM> CrawlConfigurations
        {
            get
            {
                ReadOnlyObservableCollection<CrawlConfigurationVM> value = (ReadOnlyObservableCollection<CrawlConfigurationVM>)GetValue(CrawlConfigurationsProperty);

                if (value == null)
                {
                    value = new ReadOnlyObservableCollection<CrawlConfigurationVM>(InnerCrawlConfigurations);
                    SetValue(CrawlConfigurationsPropertyKey, value);
                }

                return value;
            }
            private set => SetValue(CrawlConfigurationsPropertyKey, value);
        }

        #endregion

        private static readonly DependencyPropertyKey PathPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Path), typeof(string), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(""));

        public static readonly DependencyProperty PathProperty = PathPropertyKey.DependencyProperty;

        public string Path
        {
            get => GetValue(PathProperty) as string;
            private set => SetValue(PathPropertyKey, value);
        }

        private static readonly DependencyPropertyKey DisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(DisplayName), typeof(string), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(""));

        public static readonly DependencyProperty DisplayNameProperty = DisplayNamePropertyKey.DependencyProperty;

        public string DisplayName
        {
            get => GetValue(DisplayNameProperty) as string;
            private set => SetValue(DisplayNamePropertyKey, value);
        }

        private static readonly DependencyPropertyKey MaxRecursionDepthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MaxRecursionDepth), typeof(ushort), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(DbConstants.DbColDefaultValue_MaxRecursionDepth));

        public static readonly DependencyProperty MaxRecursionDepthProperty = MaxRecursionDepthPropertyKey.DependencyProperty;

        public ushort MaxRecursionDepth
        {
            get => (ushort)GetValue(MaxRecursionDepthProperty);
            private set => SetValue(MaxRecursionDepthPropertyKey, value);
        }

        private static readonly DependencyPropertyKey MaxTotalItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MaxTotalItems), typeof(ulong?), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty MaxTotalItemsProperty = MaxTotalItemsPropertyKey.DependencyProperty;

        public ulong? MaxTotalItems
        {
            get => (ulong?)GetValue(MaxTotalItemsProperty);
            private set => SetValue(MaxTotalItemsPropertyKey, value);
        }

        private static readonly DependencyPropertyKey TTLPropertyKey = DependencyProperty.RegisterReadOnly(nameof(TTL), typeof(TimeSpan?), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TTLProperty = TTLPropertyKey.DependencyProperty;

        public TimeSpan? TTL
        {
            get => (TimeSpan?)GetValue(TTLProperty);
            private set => SetValue(TTLPropertyKey, value);
        }

        private static readonly DependencyPropertyKey StatusValuePropertyKey = DependencyProperty.RegisterReadOnly(nameof(StatusValue), typeof(CrawlStatus), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(CrawlStatus.NotRunning));

        public static readonly DependencyProperty StatusValueProperty = StatusValuePropertyKey.DependencyProperty;

        public CrawlStatus StatusValue
        {
            get => (CrawlStatus)GetValue(StatusValueProperty);
            private set => SetValue(StatusValuePropertyKey, value);
        }

        private static readonly DependencyPropertyKey LastCrawlStartPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastCrawlStart), typeof(DateTime?), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastCrawlStartProperty = LastCrawlStartPropertyKey.DependencyProperty;

        public DateTime? LastCrawlStart
        {
            get => (DateTime?)GetValue(LastCrawlStartProperty);
            private set => SetValue(LastCrawlStartPropertyKey, value);
        }

        private static readonly DependencyPropertyKey LastCrawlEndPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastCrawlEnd), typeof(DateTime?), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastCrawlEndProperty = LastCrawlEndPropertyKey.DependencyProperty;

        public DateTime? LastCrawlEnd
        {
            get => (DateTime?)GetValue(LastCrawlEndProperty);
            private set => SetValue(LastCrawlEndPropertyKey, value);
        }

        private static readonly DependencyPropertyKey NextScheduledStartPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NextScheduledStart), typeof(DateTime?), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty NextScheduledStartProperty = NextScheduledStartPropertyKey.DependencyProperty;

        public DateTime? NextScheduledStart
        {
            get => (DateTime?)GetValue(NextScheduledStartProperty);
            private set => SetValue(NextScheduledStartPropertyKey, value);
        }

        private static readonly DependencyPropertyKey NotesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Notes), typeof(string), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(""));

        public static readonly DependencyProperty NotesProperty = NotesPropertyKey.DependencyProperty;

        public string Notes
        {
            get => GetValue(NotesProperty) as string;
            private set => SetValue(NotesPropertyKey, value);
        }

        private static readonly DependencyPropertyKey CreatedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CreatedOn), typeof(DateTime), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(default));

        public static readonly DependencyProperty CreatedOnProperty = CreatedOnPropertyKey.DependencyProperty;

        public DateTime CreatedOn
        {
            get => (DateTime)GetValue(CreatedOnProperty);
            private set => SetValue(CreatedOnPropertyKey, value);
        }

        private static readonly DependencyPropertyKey ModifiedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ModifiedOn), typeof(DateTime), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(default));

        public static readonly DependencyProperty ModifiedOnProperty = ModifiedOnPropertyKey.DependencyProperty;

        public DateTime ModifiedOn
        {
            get => (DateTime)GetValue(ModifiedOnProperty);
            private set => SetValue(ModifiedOnPropertyKey, value);
        }

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        public DateTime? LastSynchronizedOn
        {
            get => (DateTime?)GetValue(LastSynchronizedOnProperty);
            private set => SetValue(LastSynchronizedOnPropertyKey, value);
        }

        public static readonly DependencyProperty ShowActiveCrawlConfigurationsOnlyProperty = DependencyProperty.Register(nameof(ShowActiveCrawlConfigurationsOnly), typeof(bool), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as CrawlConfigurationsPageVM).OnShowActiveCrawlConfigurationsOnlyPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool ShowActiveCrawlConfigurationsOnly
        {
            get => (bool)GetValue(ShowActiveCrawlConfigurationsOnlyProperty);
            set => SetValue(ShowActiveCrawlConfigurationsOnlyProperty, value);
        }

        protected virtual void OnShowActiveCrawlConfigurationsOnlyPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                ShowInactiveCrawlConfigurationsOnly = false;
                ShowAllCrawlConfigurations = false;
            }
            else if (!(ShowInactiveCrawlConfigurationsOnly || ShowAllCrawlConfigurations))
                ShowInactiveCrawlConfigurationsOnly = true;
        }

        public static readonly DependencyProperty ShowInactiveCrawlConfigurationsOnlyProperty = DependencyProperty.Register(nameof(ShowInactiveCrawlConfigurationsOnly), typeof(bool), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as CrawlConfigurationsPageVM).OnShowInactiveCrawlConfigurationsOnlyPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool ShowInactiveCrawlConfigurationsOnly
        {
            get => (bool)GetValue(ShowInactiveCrawlConfigurationsOnlyProperty);
            set => SetValue(ShowInactiveCrawlConfigurationsOnlyProperty, value);
        }

        protected virtual void OnShowInactiveCrawlConfigurationsOnlyPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                ShowActiveCrawlConfigurationsOnly = false;
                ShowAllCrawlConfigurations = false;
            }
            else if (!(ShowActiveCrawlConfigurationsOnly || ShowAllCrawlConfigurations))
                ShowActiveCrawlConfigurationsOnly = true;
        }

        public static readonly DependencyProperty ShowAllCrawlConfigurationsProperty = DependencyProperty.Register(nameof(ShowAllCrawlConfigurations), typeof(bool), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as CrawlConfigurationsPageVM).OnShowAllCrawlConfigurationsPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool ShowAllCrawlConfigurations
        {
            get => (bool)GetValue(ShowAllCrawlConfigurationsProperty);
            set => SetValue(ShowAllCrawlConfigurationsProperty, value);
        }

        protected virtual void OnShowAllCrawlConfigurationsPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                ShowActiveCrawlConfigurationsOnly = false;
                ShowInactiveCrawlConfigurationsOnly = false;
            }
            else if (!(ShowActiveCrawlConfigurationsOnly || ShowInactiveCrawlConfigurationsOnly))
                ShowActiveCrawlConfigurationsOnly = true;
        }

        private static readonly DependencyPropertyKey NewClickCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NewClickCommand), typeof(Commands.RelayCommand), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(null));

        public static readonly DependencyProperty NewClickCommandProperty = NewClickCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand NewClickCommand => (Commands.RelayCommand)GetValue(NewClickCommandProperty);

        private void OnNewClickExecute(object parameter)
        {
            DirectoryInfo crawlRoot = FolderBrowserVM.Prompt("Crawl Root Folder", "Select root folder for new crawl configuration.");
            if (crawlRoot is not null)
            {
                CrawlConfiguration item = EditCrawlConfigVM.Edit(crawlRoot);
                if (item is not null)
                {
                    if (ShowAllCrawlConfigurations)
                        EditCrawlConfigVM.UpsertItem(item, CrawlConfigurations, _allCrawlConfigurations, true, true);
                    else if (ShowActiveCrawlConfigurationsOnly)
                        EditCrawlConfigVM.UpsertItem(item, CrawlConfigurations, _allCrawlConfigurations, true, false);
                    else
                        EditCrawlConfigVM.UpsertItem(item, CrawlConfigurations, _allCrawlConfigurations, false, true);
                }
            }
        }

        private static readonly DependencyPropertyKey DeleteClickCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(DeleteClickCommand),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(null));

        public static readonly DependencyProperty DeleteClickCommandProperty = DeleteClickCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand DeleteClickCommand => (Commands.RelayCommand)GetValue(DeleteClickCommandProperty);

        private void OnDeleteClickExecute(object parameter)
        {
            // TODO: Implement OnDeleteClickExecute Logic
        }

        private static readonly DependencyPropertyKey EditClickCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EditClickCommand),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(null));

        public static readonly DependencyProperty EditClickCommandProperty = EditClickCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand EditClickCommand => (Commands.RelayCommand)GetValue(EditClickCommandProperty);

        private void OnEditClickExecute(object parameter)
        {
            // TODO: Implement OnEditClickExecute Logic
        }

        private static readonly DependencyPropertyKey OpenRootCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(OpenRootCommand),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(null));

        public static readonly DependencyProperty OpenRootCommandProperty = OpenRootCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand OpenRootCommand => (Commands.RelayCommand)GetValue(OpenRootCommandProperty);

        private void OnOpenRootExecute(object parameter)
        {
            // TODO: Implement OnOpenRootExecute Logic
        }

        private static readonly DependencyPropertyKey ViewLogsClickCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewLogsClickCommand),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(null));

        public static readonly DependencyProperty ViewLogsClickCommandProperty = ViewLogsClickCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewLogsClickCommand => (Commands.RelayCommand)GetValue(ViewLogsClickCommandProperty);

        private void OnViewLogsClickExecute(object parameter)
        {
            // TODO: Implement OnViewLogsClickExecute Logic
        }

        private static readonly DependencyPropertyKey StartNowCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(StartNowCommand),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(null));

        public static readonly DependencyProperty StartNowCommandProperty = StartNowCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand StartNowCommand => (Commands.RelayCommand)GetValue(StartNowCommandProperty);

        private void OnStartNowExecute(object parameter)
        {
            // TODO: Implement OnStartNowExecute Logic
        }

        public CrawlConfigurationsPageVM()
        {
            InnerCrawlConfigurations = new();
            CrawlConfigurations = new(InnerCrawlConfigurations);
            SetValue(NewClickCommandPropertyKey, new Commands.RelayCommand(OnNewClickExecute));
            SetValue(EditClickCommandPropertyKey, new Commands.RelayCommand(OnEditClickExecute));
            SetValue(DeleteClickCommandPropertyKey, new Commands.RelayCommand(OnDeleteClickExecute));
            SetValue(OpenRootCommandPropertyKey, new Commands.RelayCommand(OnOpenRootExecute));
            SetValue(ViewLogsClickCommandPropertyKey, new Commands.RelayCommand(OnViewLogsClickExecute));
            SetValue(StartNowCommandPropertyKey, new Commands.RelayCommand(OnStartNowExecute));
            _logger = App.GetLogger(this);
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            _ = LoadInitialDataAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    _logger.LogError(task.Exception, "Error executing LoadInitialDataAsync");
                    Dispatcher.Invoke(() => OnInitialDataLoadError(task.Exception));
                }
                else if (task.IsCanceled)
                    _logger.LogWarning("LoadInitialDataAsync canceled.");
                else
                    Dispatcher.Invoke(() => OnInitialDataLoaded(task.Result));
            });
        }

        private static async Task<List<(string FullName, Guid SubdirectoryId, CrawlConfiguration Source)>> LoadInitialDataAsync()
        {
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>();
            List<CrawlConfiguration> configurations = await dbContext.CrawlConfigurations.Include(c => c.Root).ToListAsync();
            return await Subdirectory.LoadFullNamesAsync(configurations, c => c.Root, dbContext);
        }

        private void OnInitialDataLoaded(IEnumerable<(string FullName, Guid SubdirectoryId, CrawlConfiguration Source)> items)
        {
            _allCrawlConfigurations.Clear();
            InnerCrawlConfigurations.Clear();
            _allCrawlConfigurations.AddRange(items.Select(i => new CrawlConfigurationVM(i.Source, i.FullName, i.SubdirectoryId)));
            foreach (CrawlConfigurationVM item in ShowAllCrawlConfigurations ? _allCrawlConfigurations :
                    (ShowInactiveCrawlConfigurationsOnly ? _allCrawlConfigurations.Where(i => i.StatusValue == CrawlStatus.Disabled) : _allCrawlConfigurations.Where(i => i.StatusValue != CrawlStatus.Disabled)))
                InnerCrawlConfigurations.Add(item);
        }

        private void OnInitialDataLoadError(AggregateException exception)
        {
            _allCrawlConfigurations.Clear();
            InnerCrawlConfigurations.Clear();
            MainWindow mainWindow = Services.ServiceProvider.GetService<MainWindow>();
            if (mainWindow is not null)
                MessageBox.Show(mainWindow, string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message, "Data Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
