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

        public event EventHandler NewCrawlConfig;

        private static readonly DependencyPropertyKey NewCrawlConfigCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NewCrawlConfigCommand), typeof(Commands.RelayCommand), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(null));

        public static readonly DependencyProperty NewCrawlConfigCommandProperty = NewCrawlConfigCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand NewCrawlConfigCommand => (Commands.RelayCommand)GetValue(NewCrawlConfigCommandProperty);

        private void InvokeNewCrawlConfigCommand(object parameter)
        {
            try { OnNewCrawlConfigExecute(parameter); }
            finally { NewCrawlConfig?.Invoke(this, EventArgs.Empty); }
        }

        private void OnNewCrawlConfigExecute(object parameter)
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

        public CrawlConfigurationsPageVM()
        {
            InnerCrawlConfigurations = new();
            CrawlConfigurations = new(InnerCrawlConfigurations);
            SetValue(NewCrawlConfigCommandPropertyKey, new Commands.RelayCommand(InvokeNewCrawlConfigCommand));
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
