using FsInfoCat.Desktop.ViewModel.AsyncOps;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
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
    public class CrawlConfigurationsPageVM : DbEntityListingPageVM<CrawlConfigListItem, CrawlConfigItemVM>
    {
        #region ShowLogs Command Property Members

        private static readonly DependencyPropertyKey ShowLogsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ShowLogs),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ShowLogs"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowLogsProperty = ShowLogsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ShowLogs => (Commands.RelayCommand)GetValue(ShowLogsProperty);

        private void OnShowLogs(object parameter)
        {
            // TODO: Implement OnShowLogs Logic
        }

        #endregion
        #region ShowViewOptions Command Property Members

        private static readonly DependencyPropertyKey ShowViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ShowViewOptions), typeof(Commands.RelayCommand), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ShowViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowViewOptionsProperty = ShowViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ShowViewOptions => (Commands.RelayCommand)GetValue(ShowViewOptionsProperty);

        #endregion
        #region IsEditingViewOptions Property Members

        private static readonly DependencyPropertyKey IsEditingViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsEditingViewOptions), typeof(bool), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsEditingViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsEditingViewOptionsProperty = IsEditingViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool IsEditingViewOptions { get => (bool)GetValue(IsEditingViewOptionsProperty); private set => SetValue(IsEditingViewOptionsPropertyKey, value); }

        #endregion
        #region ViewOptionsOkClick Command Property Members

        private static readonly DependencyPropertyKey ViewOptionsOkClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptionsOkClick),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewOptionsOkClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionsOkClickProperty = ViewOptionsOkClickPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ViewOptionsOkClick => (Commands.RelayCommand)GetValue(ViewOptionsOkClickProperty);

        #endregion
        #region ViewOptionCancelClick Command Property Members

        private static readonly DependencyPropertyKey ViewOptionCancelClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptionCancelClick),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewOptionCancelClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionCancelClickProperty = ViewOptionCancelClickPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ViewOptionCancelClick => (Commands.RelayCommand)GetValue(ViewOptionCancelClickProperty);

        #endregion
        #region EditingViewOptions Property Members

        private static readonly DependencyPropertyKey EditingViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EditingViewOptions), typeof(ThreeStateViewModel), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="EditingViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditingViewOptionsProperty = EditingViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ThreeStateViewModel EditingViewOptions => (ThreeStateViewModel)GetValue(EditingViewOptionsProperty);

        #endregion
        #region ViewOptions Property Members

        private static readonly DependencyPropertyKey ViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptions), typeof(ThreeStateViewModel), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionsProperty = ViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ThreeStateViewModel ViewOptions => (ThreeStateViewModel)GetValue(ViewOptionsProperty);

        #endregion
        
        public CrawlConfigurationsPageVM()
        {
            ThreeStateViewModel viewOptions = new(true);
            SetValue(ViewOptionsPropertyKey, viewOptions);
            SetValue(EditingViewOptionsPropertyKey, new ThreeStateViewModel(viewOptions.Value));
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            SetValue(ShowViewOptionsPropertyKey, new Commands.RelayCommand(() => IsEditingViewOptions = true));
            SetValue(ShowLogsPropertyKey, new Commands.RelayCommand(OnShowLogs));
            SetValue(ViewOptionsOkClickPropertyKey, new Commands.RelayCommand(() =>
            {
                IsEditingViewOptions = false;
                ViewOptions.Value = EditingViewOptions.Value;
            }));
            SetValue(ViewOptionCancelClickPropertyKey, new Commands.RelayCommand(() =>
            {
                EditingViewOptions.Value = ViewOptions.Value;
                IsEditingViewOptions = false;
            }));
            viewOptions.ValuePropertyChanged += (s, e) =>
            {
                EditingViewOptions.Value = ViewOptions.Value;
                LoadItemsAsync();
            };
        }

        protected override Func<IWindowsStatusListener, Task<int>> GetItemsLoaderFactory()
        {
            bool? viewOptions = ViewOptions.Value;
            return listener => Task.Run(async () => await LoadItemsAsync(viewOptions, listener));
        }

        private async Task<int> LoadItemsAsync(bool? showActive, IWindowsStatusListener statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            IQueryable<CrawlConfigListItem> items;
            if (showActive.HasValue)
            {
                if (showActive.Value)
                    items = from v in dbContext.CrawlConfigListing where v.StatusValue !=  CrawlStatus.Disabled select v;
                else
                    items = from v in dbContext.CrawlConfigListing where v.StatusValue == CrawlStatus.Disabled select v;
            }
            else
                items = from v in dbContext.CrawlConfigListing select v;
            return await OnEntitiesLoaded(items, statusListener, r => new CrawlConfigItemVM(r));
        }

        protected override DbSet<CrawlConfigListItem> GetDbSet(LocalDbContext dbContext) => dbContext.CrawlConfigListing;

        protected async Task<CrawlConfiguration> AddNewItemAsync(object parameter)
        {
            (string, Subdirectory)? result = await Dispatcher.Invoke(() => EditCrawlConfigVM.BrowseForFolderAsync(null, BgOps));
            while (result?.Item2.CrawlConfiguration is not null)
            {
                Dispatcher.Invoke(() => MessageBox.Show(Application.Current.MainWindow, "Not Available", "That subdirectory already has a crawl configuration.",
                    MessageBoxButton.OK, MessageBoxImage.Error));
                result = await Dispatcher.Invoke(() => EditCrawlConfigVM.BrowseForFolderAsync(null, BgOps));
            }
            string path = result?.Item1;
            return string.IsNullOrEmpty(path) ? null : await Dispatcher.InvokeAsync(() => ShowEditNewDialog(path), DispatcherPriority.Background);
        }

        protected override void OnAddNewItem(object parameter) => AddNewItemAsync(parameter).Wait();

        private CrawlConfiguration ShowEditNewDialog(string rootPath)
        {
            CrawlConfiguration entity = new();
            bool? isInactive = ViewOptions.Value;
            if (isInactive.HasValue)
                entity.StatusValue = isInactive.Value ? CrawlStatus.Disabled : CrawlStatus.NotRunning;
            EditCrawlConfigVM viewModel = new();
            AttachedProperties.SetFullName(viewModel, rootPath);
            if (viewModel.ShowDialog(new View.Local.EditCrawlConfigWindow(), entity, true) ?? false)
                return entity;
            return null;
        }

        protected override bool ShowModalItemEditWindow(CrawlConfigItemVM item, object parameter, out string saveProgressTitle)
        {
            saveProgressTitle = "Saving crawl configuration";
            return BgOps.FromAsync("Loading Details", "Connecting to database", item.Model.Id, LoadItemAsync).ContinueWith(task => Dispatcher.Invoke(() =>
            {
                CrawlConfiguration entity = task.Result;
                if (entity is null)
                    return false;
                EditCrawlConfigVM viewModel = new();
                AttachedProperties.SetFullName(viewModel, item.FullName);
                return viewModel.ShowDialog(new View.Local.EditCrawlConfigWindow(), entity, false) ?? false;
            }), TaskContinuationOptions.OnlyOnRanToCompletion).Result;
        }

        private async Task<CrawlConfiguration> LoadItemAsync(Guid id, IWindowsStatusListener arg)
        {
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            return await dbContext.CrawlConfigurations.FindAsync(id);
        }

        protected override bool PromptItemDeleting(CrawlConfigItemVM item, object parameter, out string deleteProgressTitle)
        {
            throw new NotImplementedException();
        }
    }
}
