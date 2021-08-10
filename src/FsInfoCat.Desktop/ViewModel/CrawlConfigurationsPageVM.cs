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

        public static readonly DependencyProperty SelectedCrawlConfigProperty = DependencyProperty.Register(nameof(SelectedCrawlConfig), typeof(CrawlConfigurationVM), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as CrawlConfigurationsPageVM).OnSelectedCrawlConfigPropertyChanged((CrawlConfigurationVM)e.OldValue, (CrawlConfigurationVM)e.NewValue)));

        public CrawlConfigurationVM SelectedCrawlConfig
        {
            get => (CrawlConfigurationVM)GetValue(SelectedCrawlConfigProperty);
            set => SetValue(SelectedCrawlConfigProperty, value);
        }

        protected virtual void OnSelectedCrawlConfigPropertyChanged(CrawlConfigurationVM oldValue, CrawlConfigurationVM newValue)
        {
            if (oldValue is not null)
            {
                oldValue.StartCrawlNow -= SelecteItem_StartCrawlNow;
                oldValue.Edit -= SelecteItem_Edit;
                oldValue.Delete -= SelecteItem_Delete;
                oldValue.OpenRootFolder -= SelecteItem_OpenRootFolder;
                oldValue.ShowLogs -= SelecteItem_ShowLogs;
            }
            if (newValue is not null)
            {
                newValue.StartCrawlNow += SelecteItem_StartCrawlNow;
                newValue.Edit += SelecteItem_Edit;
                newValue.Delete += SelecteItem_Delete;
                newValue.OpenRootFolder += SelecteItem_OpenRootFolder;
                newValue.ShowLogs += SelecteItem_ShowLogs;
            }
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
                CrawlConfigurationVM selectedCrawlConfig = SelectedCrawlConfig;
                InnerCrawlConfigurations.Clear();
                foreach (CrawlConfigurationVM vm in _allCrawlConfigurations.Where(i => i.StatusValue != CrawlStatus.Disabled))
                    InnerCrawlConfigurations.Add(vm);
                SelectedCrawlConfig = (selectedCrawlConfig is null || selectedCrawlConfig.StatusValue == CrawlStatus.Disabled) ? InnerCrawlConfigurations.FirstOrDefault() : selectedCrawlConfig;
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
                CrawlConfigurationVM selectedCrawlConfig = SelectedCrawlConfig;
                InnerCrawlConfigurations.Clear();
                foreach (CrawlConfigurationVM vm in _allCrawlConfigurations.Where(i => i.StatusValue == CrawlStatus.Disabled))
                    InnerCrawlConfigurations.Add(vm);
                SelectedCrawlConfig = (selectedCrawlConfig is null || selectedCrawlConfig.StatusValue != CrawlStatus.Disabled) ? InnerCrawlConfigurations.FirstOrDefault() : selectedCrawlConfig;
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
                CrawlConfigurationVM selectedCrawlConfig = SelectedCrawlConfig;
                InnerCrawlConfigurations.Clear();
                foreach (CrawlConfigurationVM vm in _allCrawlConfigurations)
                    InnerCrawlConfigurations.Add(vm);
                SelectedCrawlConfig = selectedCrawlConfig ?? InnerCrawlConfigurations.FirstOrDefault();
            }
            else if (!(ShowActiveCrawlConfigurationsOnly || ShowInactiveCrawlConfigurationsOnly))
                ShowActiveCrawlConfigurationsOnly = true;
        }

        private static readonly DependencyPropertyKey NewClickCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NewClickCommand), typeof(Commands.RelayCommand), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(null));

        public static readonly DependencyProperty NewClickCommandProperty = NewClickCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand NewClickCommand => (Commands.RelayCommand)GetValue(NewClickCommandProperty);

        private void OnNewClickExecute(object parameter)
        {
            using System.Windows.Forms.FolderBrowserDialog dialog = new()
            {
                Description = FsInfoCat.Properties.Resources.Description_SelectCrawlRootFolder,
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
            };
            if (dialog.ShowDialog(new WindowOwner()) != System.Windows.Forms.DialogResult.OK)
                return;
            //DirectoryInfo crawlRoot = FolderBrowserVM.Prompt(FsInfoCat.Properties.Resources.DisplayName_CrawlRootFolder, FsInfoCat.Properties.Resources.Description_SelectCrawlRootFolder);
            //if (crawlRoot is null || !EditCrawlConfigVM.Edit(crawlRoot, out CrawlConfiguration model, out bool isNew))
            //    return;
            if (!EditCrawlConfigVM.Edit(dialog.SelectedPath, out CrawlConfiguration model, out bool isNew))
                return;
            CrawlConfigurationVM item;
            if (isNew || (item = _allCrawlConfigurations.FirstOrDefault(i => ReferenceEquals(i.Model, model))) is null)
            {
                if (ShowAllCrawlConfigurations)
                    item = CrawlConfigurationVM.UpsertItem(model, CrawlConfigurations, _allCrawlConfigurations, true, true);
                else if (ShowActiveCrawlConfigurationsOnly)
                    item = CrawlConfigurationVM.UpsertItem(model, CrawlConfigurations, _allCrawlConfigurations, true, false);
                else
                    item = CrawlConfigurationVM.UpsertItem(model, CrawlConfigurations, _allCrawlConfigurations, false, true);
            }
            if (item is not null)
                SelectedCrawlConfig = item;
        }

        private void SelecteItem_StartCrawlNow(object sender, EventArgs e)
        {
            // TODO: Implement SelecteItem_StartCrawlNow
            throw new NotImplementedException();
        }

        private void SelecteItem_Edit(object sender, EventArgs e)
        {
            CrawlConfiguration model = SelectedCrawlConfig?.Model;
            if (model is not null)
                EditCrawlConfigVM.Edit(model);
        }

        private void SelecteItem_Delete(object sender, EventArgs e)
        {
            // TODO: Implement SelecteItem_Delete
            throw new NotImplementedException();
        }

        private void SelecteItem_OpenRootFolder(object sender, EventArgs e)
        {
            // TODO: Implement SelecteItem_OpenRootFolder
            throw new NotImplementedException();
        }

        private void SelecteItem_ShowLogs(object sender, EventArgs e)
        {
            // TODO: Implement SelecteItem_ShowLogs
            throw new NotImplementedException();
        }

        public CrawlConfigurationsPageVM()
        {
            InnerCrawlConfigurations = new();
            CrawlConfigurations = new(InnerCrawlConfigurations);
            SetValue(NewClickCommandPropertyKey, new Commands.RelayCommand(OnNewClickExecute));
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
            SelectedCrawlConfig = InnerCrawlConfigurations.FirstOrDefault();
        }

        private void OnInitialDataLoadError(AggregateException exception)
        {
            _allCrawlConfigurations.Clear();
            InnerCrawlConfigurations.Clear();
            MainWindow mainWindow = Services.ServiceProvider.GetService<MainWindow>();
            if (mainWindow is not null)
                MessageBox.Show(mainWindow, string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message, FsInfoCat.Properties.Resources.DisplayName_DataLoadError, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    public class WindowOwner : System.Windows.Forms.IWin32Window
    {
        private IntPtr _handle;
        public WindowOwner() : this(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle) { }
        public WindowOwner(IntPtr handle) { _handle = handle; }
        public IntPtr Handle { get { return _handle; } }
    }
}
