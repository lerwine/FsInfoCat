using Microsoft.EntityFrameworkCore;
using FsInfoCat.Local;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace FsInfoCat.Desktop.ViewModel
{
    public class MainVM : DependencyObject
    {
        private readonly ILogger<MainVM> _logger;
        private readonly List<CrawlConfigurationVM> _allCrawlConfigurations = new();

        #region CrawlConfigurations Property Members

        private static readonly DependencyPropertyKey InnerCrawlConfigurationsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(InnerCrawlConfigurations), typeof(ObservableCollection<CrawlConfigurationVM>), typeof(MainVM),
                new PropertyMetadata(new ObservableCollection<CrawlConfigurationVM>()));

        private static readonly DependencyPropertyKey CrawlConfigurationsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(CrawlConfigurations), typeof(ReadOnlyObservableCollection<CrawlConfigurationVM>), typeof(MainVM),
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

        public static readonly DependencyProperty ShowActiveCrawlConfigurationsOnlyProperty =
            DependencyProperty.Register(nameof(ShowActiveCrawlConfigurationsOnly), typeof(bool), typeof(MainVM),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as MainVM).OnShowActiveCrawlConfigurationsOnlyPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

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

        public static readonly DependencyProperty ShowInactiveCrawlConfigurationsOnlyProperty =
            DependencyProperty.Register(nameof(ShowInactiveCrawlConfigurationsOnly), typeof(bool), typeof(MainVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as MainVM).OnShowInactiveCrawlConfigurationsOnlyPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

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

        public static readonly DependencyProperty ShowAllCrawlConfigurationsProperty =
            DependencyProperty.Register(nameof(ShowAllCrawlConfigurations), typeof(bool), typeof(MainVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as MainVM).OnShowAllCrawlConfigurationsPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool ShowAllCrawlConfigurations
        {
            get => (bool)GetValue(ShowAllCrawlConfigurationsProperty);
            set => this.SetValue(ShowAllCrawlConfigurationsProperty, value);
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

        private static readonly DependencyPropertyKey NewCrawlConfigurationCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NewCrawlConfigurationCommand),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null, null, (DependencyObject d, object baseValue) =>
                (baseValue is Commands.RelayCommand rc) ? rc : new Commands.RelayCommand(((MainVM)d).OnNewCrawlConfigurationExecute)));

        public static readonly DependencyProperty NewCrawlConfigurationCommandProperty = NewCrawlConfigurationCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand NewCrawlConfigurationCommand => (Commands.RelayCommand)GetValue(NewCrawlConfigurationCommandProperty);

        private void OnNewCrawlConfigurationExecute(object parameter)
        {
            MainWindow mainWindow = Services.ServiceProvider.GetService<MainWindow>();
            if (mainWindow is null)
                return;
            // TODO: Show directory picker
            // TODO: Show edit window
        }

        public MainVM()
        {
            _logger = Services.ServiceProvider.GetRequiredService<ILogger<MainVM>>();
        }

        internal Task InitializeAsync() => LoadInitialDataAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
                Dispatcher.Invoke(() => OnInitialDataLoadError(task.Exception));
            else if (!task.IsCanceled)
                Dispatcher.Invoke(() => OnInitialDataLoaded(task.Result));
        });

        private static async Task<List<((string, Guid), CrawlConfiguration)>> LoadInitialDataAsync()
        {
            using LocalDbContext dbContext = Services.ServiceProvider.GetService<LocalDbContext>();
            List<CrawlConfiguration> configurations = await dbContext.CrawlConfigurations.ToListAsync();
            return await Subdirectory.LoadFullNamesAsync(configurations, c =>
            {
                Subdirectory root = c.Root;
                if (root is null)
                    c.Root = root = Subdirectory.FindByCrawlConfiguration(c.Id, dbContext);
                return root;
            }, dbContext);
            
        }

        private void OnInitialDataLoaded(IEnumerable<((string, Guid), CrawlConfiguration)> items)
        {
            _allCrawlConfigurations.Clear();
            InnerCrawlConfigurations.Clear();
            _allCrawlConfigurations.AddRange(items.Select(i => new CrawlConfigurationVM(i.Item2, i.Item1.Item1, i.Item1.Item2)));
            foreach (CrawlConfigurationVM item in ShowAllCrawlConfigurations ? _allCrawlConfigurations :
                    (ShowInactiveCrawlConfigurationsOnly ? _allCrawlConfigurations.Where(i => i.IsInactive) : _allCrawlConfigurations.Where(i => !i.IsInactive)))
                InnerCrawlConfigurations.Add(item);
        }

        private void OnInitialDataLoadError(AggregateException exception)
        {
            _allCrawlConfigurations.Clear();
            InnerCrawlConfigurations.Clear();
            MainWindow mainWindow = Services.ServiceProvider.GetService<MainWindow>();
            if (mainWindow is not null)
                MessageBox.Show(mainWindow, string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message,
                    "Data Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
