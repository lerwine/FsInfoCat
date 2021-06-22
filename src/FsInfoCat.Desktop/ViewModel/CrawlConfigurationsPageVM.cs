using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class CrawlConfigurationsPageVM : DependencyObject
    {
        private readonly ILogger<CrawlConfigurationsPageVM> _logger;
        private readonly List<CrawlConfigurationVM> _allCrawlConfigurations = new();

        #region CrawlConfigurations Property Members

        private static readonly DependencyPropertyKey InnerCrawlConfigurationsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(InnerCrawlConfigurations), typeof(ObservableCollection<CrawlConfigurationVM>), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(new ObservableCollection<CrawlConfigurationVM>()));

        private static readonly DependencyPropertyKey CrawlConfigurationsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(CrawlConfigurations), typeof(ReadOnlyObservableCollection<CrawlConfigurationVM>), typeof(CrawlConfigurationsPageVM),
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
            DependencyProperty.Register(nameof(ShowActiveCrawlConfigurationsOnly), typeof(bool), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as CrawlConfigurationsPageVM).OnShowActiveCrawlConfigurationsOnlyPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

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
            DependencyProperty.Register(nameof(ShowInactiveCrawlConfigurationsOnly), typeof(bool), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as CrawlConfigurationsPageVM).OnShowInactiveCrawlConfigurationsOnlyPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

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
            DependencyProperty.Register(nameof(ShowAllCrawlConfigurations), typeof(bool), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as CrawlConfigurationsPageVM).OnShowAllCrawlConfigurationsPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

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

        private static readonly DependencyPropertyKey NewCrawlConfigCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NewCrawlConfigCommand),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(null, null, (DependencyObject d, object baseValue) =>
                (baseValue is Commands.RelayCommand rc) ? rc : new Commands.RelayCommand(((CrawlConfigurationsPageVM)d).InvokeNewCrawlConfigCommand)));

        public static readonly DependencyProperty NewCrawlConfigCommandProperty = NewCrawlConfigCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand NewCrawlConfigCommand => (Commands.RelayCommand)GetValue(NewCrawlConfigCommandProperty);

        private void InvokeNewCrawlConfigCommand(object parameter)
        {
            try { OnNewCrawlConfigExecute(parameter); }
            finally { NewCrawlConfig?.Invoke(this, EventArgs.Empty); }
        }

        private void OnNewCrawlConfigExecute(object parameter)
        {
            // TODO: Implement OnNewCrawlConfigExecute Logic
        }

        public CrawlConfigurationsPageVM()
        {
            _logger = Services.ServiceProvider.GetRequiredService<ILogger<CrawlConfigurationsPageVM>>();
            LoadInitialDataAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                    Dispatcher.Invoke(() => OnInitialDataLoadError(task.Exception));
                else if (!task.IsCanceled)
                    Dispatcher.Invoke(() => OnInitialDataLoaded(task.Result));
            });
        }

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
