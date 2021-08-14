using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class CrawlConfigurationsPageVM : DependencyObject
    {
        private readonly ILogger<CrawlConfigurationsPageVM> _logger;
        private readonly List<CrawlConfigurationVM> _allCrawlConfigurations = new();
        private Task _lastDataLoadTask;

        #region Crawl Configuration Items Members

        #region CrawlConfigurations Property Members

        private static readonly DependencyPropertyKey InnerCrawlConfigurationsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(InnerCrawlConfigurations),
            typeof(ObservableCollection<CrawlConfigurationVM>), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(null));

        private static readonly DependencyPropertyKey CrawlConfigurationsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CrawlConfigurations),
            typeof(ReadOnlyObservableCollection<CrawlConfigurationVM>), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(null));

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
        #region SelectedCrawlConfig Property Members

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
                oldValue.StartCrawlNow -= SelectedCrawlConfig_StartCrawlNow;
                oldValue.Edit -= SelectedCrawlConfig_Edit;
                oldValue.Delete -= SelectedCrawlConfig_Delete;
                //oldValue.OpenRootFolder -= SelectedCrawlConfig_OpenRootFolder;
                oldValue.ShowLogs -= SelectedCrawlConfig_ShowLogs;
            }
            if (newValue is null)
                SelectedCrawlConfigRootPath = "";
            else
            {
                newValue.StartCrawlNow += SelectedCrawlConfig_StartCrawlNow;
                newValue.Edit += SelectedCrawlConfig_Edit;
                newValue.Delete += SelectedCrawlConfig_Delete;
                //newValue.OpenRootFolder += SelectedCrawlConfig_OpenRootFolder;
                newValue.ShowLogs += SelectedCrawlConfig_ShowLogs;
            }
        }

        private void SelectedCrawlConfig_StartCrawlNow(object sender, EventArgs e)
        {
            // TODO: Implement SelecteItem_StartCrawlNow
            throw new NotImplementedException();
        }

        private void SelectedCrawlConfig_Edit(object sender, EventArgs e)
        {
            CrawlConfiguration model = SelectedCrawlConfig?.Model;
            if (model is not null)
                EditCrawlConfigVM.Edit(model);
        }

        private void SelectedCrawlConfig_Delete(object sender, EventArgs e)
        {
            // TODO: Implement SelecteItem_Delete
            throw new NotImplementedException();
        }

        private void SelectedCrawlConfig_OpenRootFolder(object sender, EventArgs e)
        {
            // TODO: Implement SelecteItem_OpenRootFolder
            throw new NotImplementedException();
        }

        private void SelectedCrawlConfig_ShowLogs(object sender, EventArgs e)
        {
            // TODO: Implement SelecteItem_ShowLogs
            throw new NotImplementedException();
        }

        #endregion
        #region SelectedCrawlConfigRootPath Property Members

        private static readonly DependencyPropertyKey SelectedCrawlConfigRootPathPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SelectedCrawlConfigRootPath), typeof(string), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(""));

        public static readonly DependencyProperty SelectedCrawlConfigRootPathProperty = SelectedCrawlConfigRootPathPropertyKey.DependencyProperty;

        public string SelectedCrawlConfigRootPath
        {
            get => GetValue(SelectedCrawlConfigRootPathProperty) as string;
            private set => SetValue(SelectedCrawlConfigRootPathPropertyKey, value);
        }

        #endregion

        #endregion
        #region Listing Options Members

        #region ShowActiveCrawlConfigurationsOnly Property Members

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

        #endregion

        #region ShowInactiveCrawlConfigurationsOnly Property Members

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

        #endregion

        #region ShowAllCrawlConfigurations Property Members

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

        #endregion

        #endregion
        #region Background Operations Members

        private static readonly DependencyPropertyKey OpAggregatePropertyKey = DependencyProperty.RegisterReadOnly(nameof(OpAggregate), typeof(AsyncOps.AsyncOpAggregate), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(null));

        public static readonly DependencyProperty OpAggregateProperty = OpAggregatePropertyKey.DependencyProperty;

        public AsyncOps.AsyncOpAggregate OpAggregate => (AsyncOps.AsyncOpAggregate)GetValue(OpAggregateProperty);

        #region CrawlConfigsLoader Property Members

        private static readonly DependencyPropertyKey CrawlConfigsLoaderPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CrawlConfigsLoader),
            typeof(AsyncOps.AsyncOpResultManagerViewModel<bool?, IList<Subdirectory.CrawlConfigWithFullRootPath<CrawlConfiguration>>>), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty CrawlConfigsLoaderProperty = CrawlConfigsLoaderPropertyKey.DependencyProperty;

        public AsyncOps.AsyncOpResultManagerViewModel<bool?, IList<Subdirectory.CrawlConfigWithFullRootPath<CrawlConfiguration>>> CrawlConfigsLoader =>
            (AsyncOps.AsyncOpResultManagerViewModel<bool?, IList<Subdirectory.CrawlConfigWithFullRootPath<CrawlConfiguration>>>)GetValue(CrawlConfigsLoaderProperty);
        
        #endregion

        #endregion
        #region NewClickCommand Property Members

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

        #endregion

        public Task GetLastDataLoadTask() => _lastDataLoadTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrawlConfigurationsPageVM"/> class.
        /// </summary>
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
            AsyncOps.AsyncOpAggregate opAggregate = new();
            SetValue(OpAggregatePropertyKey, opAggregate);
            AsyncOps.AsyncOpResultManagerViewModel<bool?, IList<Subdirectory.CrawlConfigWithFullRootPath<CrawlConfiguration>>> crawlConfigsLoader = new();
            SetValue(CrawlConfigsLoaderPropertyKey, crawlConfigsLoader);
            AsyncOps.AsyncFuncOpViewModel<bool?, IList<Subdirectory.CrawlConfigWithFullRootPath<CrawlConfiguration>>> asyncOp =
                opAggregate.FromAsync("Loading crawl configurations", "Connecting to database", null, crawlConfigsLoader, LoadCrawlConfigsAsync);
            asyncOp.GetTask().ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                    Dispatcher.Invoke(() => OnInitialDataLoaded(task.Result));
            });
        }

        private void OnInitialDataLoaded(IList<Subdirectory.CrawlConfigWithFullRootPath<CrawlConfiguration>> items)
        {
            _logger.LogInformation("{Count} crawl configurations loaded from database", items.Count);
            _allCrawlConfigurations.Clear();
            InnerCrawlConfigurations.Clear();
            _allCrawlConfigurations.AddRange(items.Select(i => new CrawlConfigurationVM(i.Source, i.FullName, i.SubdirectoryId)));
            foreach (CrawlConfigurationVM item in ShowAllCrawlConfigurations ? _allCrawlConfigurations :
                    (ShowInactiveCrawlConfigurationsOnly ? _allCrawlConfigurations.Where(i => i.StatusValue == CrawlStatus.Disabled) : _allCrawlConfigurations.Where(i => i.StatusValue != CrawlStatus.Disabled)))
                InnerCrawlConfigurations.Add(item);
            SelectedCrawlConfig = InnerCrawlConfigurations.FirstOrDefault();
        }

        private async Task<IList<Subdirectory.CrawlConfigWithFullRootPath<CrawlConfiguration>>> LoadCrawlConfigsAsync(bool? isActive,
            AsyncOps.AsyncFuncOpViewModel<bool?, IList<Subdirectory.CrawlConfigWithFullRootPath<CrawlConfiguration>>>.StatusListenerImpl statusListener)
        {
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>();
            List<CrawlConfiguration> configurations;
            DispatcherOperation dispatcherOperation = statusListener.BeginSetMessage("Reading from database...");
            try
            {
                if (isActive.HasValue)
                {
                    if (isActive.Value)
                        configurations = await (from cfg in dbContext.CrawlConfigurations.Include(c => c.Root) where cfg.StatusValue != CrawlStatus.Disabled select cfg).ToListAsync();
                    else
                        configurations = await (from cfg in dbContext.CrawlConfigurations.Include(c => c.Root) where cfg.StatusValue == CrawlStatus.Disabled select cfg).ToListAsync();
                }
                else
                    configurations = await dbContext.CrawlConfigurations.Include(c => c.Root).ToListAsync();
            }
            finally { await dispatcherOperation; }
            List<Subdirectory.CrawlConfigWithFullRootPath<CrawlConfiguration>> result;
            dispatcherOperation = statusListener.BeginSetMessage($"Calculating full paths of {configurations.Count} configuration items");
            try
            {
                result = await Subdirectory.BuildFullNamesAsync(configurations, c => c.Root, dbContext, statusListener.CancellationToken);
            }
            finally { await dispatcherOperation; }
            return result;
        }
    }
}
