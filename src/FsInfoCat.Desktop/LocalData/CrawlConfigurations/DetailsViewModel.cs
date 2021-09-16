using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.LocalData.CrawlConfigurations
{
    public class DetailsViewModel : CrawlConfigurationDetailsViewModel<CrawlConfiguration, SubdirectoryListItemWithAncestorNames, SubdirectoryListItemViewModel, CrawlJobLogListItem, CrawlJobListItemViewModel>
    {
        #region AddNewCrawlJobLog Command Property Members

        private static readonly DependencyPropertyKey AddNewCrawlJobLogPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AddNewCrawlJobLog), typeof(Commands.RelayCommand),
            typeof(DetailsViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="AddNewCrawlJobLog"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AddNewCrawlJobLogProperty = AddNewCrawlJobLogPropertyKey.DependencyProperty;

        public Commands.RelayCommand AddNewCrawlJobLog => (Commands.RelayCommand)GetValue(AddNewCrawlJobLogProperty);

        #endregion
        #region UpstreamId Property Members

        private static readonly DependencyPropertyKey UpstreamIdPropertyKey = DependencyPropertyBuilder<DetailsViewModel, Guid?>
            .Register(nameof(UpstreamId))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="UpstreamId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UpstreamIdProperty = UpstreamIdPropertyKey.DependencyProperty;

        public Guid? UpstreamId { get => (Guid?)GetValue(UpstreamIdProperty); private set => SetValue(UpstreamIdPropertyKey, value); }

        #endregion
        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyPropertyBuilder<DetailsViewModel, DateTime?>
            .Register(nameof(LastSynchronizedOn))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="LastSynchronizedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        public DateTime? LastSynchronizedOn { get => (DateTime?)GetValue(LastSynchronizedOnProperty); private set => SetValue(LastSynchronizedOnPropertyKey, value); }

        #endregion
        #region ListItem Property Members

        private static readonly DependencyPropertyKey ListItemPropertyKey = DependencyPropertyBuilder<DetailsViewModel, CrawlConfigListItem>
            .Register(nameof(ListItem))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ListItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListItemProperty = ListItemPropertyKey.DependencyProperty;

        public CrawlConfigListItem ListItem { get => (CrawlConfigListItem)GetValue(ListItemProperty); private set => SetValue(ListItemPropertyKey, value); }

        #endregion

        public DetailsViewModel([DisallowNull] CrawlConfiguration entity, [DisallowNull] CrawlConfigListItem itemEntity) : base(entity, itemEntity)
        {
            SetValue(AddNewCrawlJobLogPropertyKey, new Commands.RelayCommand(OnAddNewCrawlJobLogCommand));
            ListItem = itemEntity;
            UpstreamId = entity.UpstreamId;
            LastSynchronizedOn = entity.LastSynchronizedOn;
        }

        private bool ConfirmCrawlJobLogDelete([DisallowNull] CrawlJobListItemViewModel item, object parameter) => MessageBox.Show(Application.Current.MainWindow, "This action cannot be undone!\n\nAre you sure you want to delete this crawl completion log entry?", "Delete Completion Log Entry", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;

        protected override CrawlJobListItemViewModel CreateCrawlJobLogViewModel([DisallowNull] CrawlJobLogListItem entity) => new(entity);

        private Task<int> DeleteCrawlJobLogFromDbContextAsync([DisallowNull] CrawlJobLogListItem entity, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            // TODO: Implement DeleteCrawlJobLogFromDbContextAsync(CrawlJobLogListItem, LocalDbContext, IWindowsStatusListener)
            throw new NotImplementedException();
        }

        protected override IQueryable<CrawlJobLogListItem> GetQueryableCrawlJobLogListing([DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            Guid id = Entity.Id;
            statusListener.SetMessage("Reading from database");
            return dbContext.CrawlJobListing.Where(j => j.ConfigurationId == id);
        }

        private void OnAddNewCrawlJobLogCommand(object parameter)
        {
            IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
            jobFactory.StartNew("Loading database record", "Opening database", (CrawlJobListItemViewModel)null, GetEditPageAsync).Task.ContinueWith(task => OnGetEditPageComplete(task, null));
        }

        private void OnCrawlJobLogEditCommand([DisallowNull] CrawlJobListItemViewModel item, object parameter)
        {
            IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
            jobFactory.StartNew("Loading database record", "Opening database", item, GetEditPageAsync).Task.ContinueWith(task => OnGetEditPageComplete(task, item));
        }

        private async Task<PageFunction<ItemFunctionResultEventArgs>> GetEditPageAsync(CrawlJobListItemViewModel item, [DisallowNull] IWindowsStatusListener statusListener)
        {
            CrawlJobLog crawlJobLog;
            if (item is null)
                crawlJobLog = new() { Configuration = Entity };
            else
            {
                using IServiceScope scope = Services.CreateScope();
                using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
                Guid id = item.Entity.Id;
                crawlJobLog = await dbContext.CrawlJobLogs.FirstOrDefaultAsync(j => j.Id == id, statusListener.CancellationToken);
                if (crawlJobLog is null)
                {
                    await Dispatcher.ShowMessageBoxAsync("Item not found in database. Click OK to refresh listing.", "Security Exception", MessageBoxButton.OK, MessageBoxImage.Error,
                        statusListener.CancellationToken);
                    ReloadAsync();
                    return null;
                }
            }
            return await Dispatcher.InvokeAsync(() => new CrawlLogs.EditPage(new(crawlJobLog, item?.Entity)), DispatcherPriority.Normal, statusListener.CancellationToken);
        }

        private void OnGetEditPageComplete(Task<PageFunction<ItemFunctionResultEventArgs>> task, CrawlJobListItemViewModel item) => Dispatcher.Invoke(() =>
        {
            if (task.IsCanceled)
                return;
            if (task.IsFaulted)
            {
                Exception exception = (task.Exception.InnerExceptions.Count > 1) ? task.Exception : task.Exception.InnerExceptions[0];
                _ = MessageBox.Show(Application.Current.MainWindow,
                    ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                        (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                        .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                        "There was an unexpected error while loading items from the databse.\n\nSee logs for further information",
                    "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                PageFunction<ItemFunctionResultEventArgs> page = task.Result;
                if (page is null)
                    return;
                page.Return += Page_Return;
                Services.ServiceProvider.GetRequiredService<IApplicationNavigation>().Navigate(page);
            }
        });

        private void Page_Return(object sender, ReturnEventArgs<ItemFunctionResultEventArgs> e)
        {
            switch (e.Result.State)
            {
                case EntityEditResultState.Added:
                    if (e.Result.Entity is CrawlJobLogListItem addedItem && addedItem.ConfigurationId == Entity.Id)
                        AddCrawlJobLogItem(new CrawlJobListItemViewModel(addedItem));
                    break;
                case EntityEditResultState.Modified:
                    if (e.Result.State is CrawlJobLogListItem modifiedItem && modifiedItem.ConfigurationId != Entity.Id)
                        RemoveCrawlJobLogItem(Logs.FirstOrDefault(i => ReferenceEquals(i.Entity, modifiedItem)));
                    break;
                case EntityEditResultState.Deleted:
                    if (e.Result.State is CrawlJobLogListItem deletedItem && deletedItem.ConfigurationId != Entity.Id)
                        RemoveCrawlJobLogItem(Logs.FirstOrDefault(i => ReferenceEquals(i.Entity, deletedItem)));
                    break;
            }
        }

        protected override void OnReloadTaskFaulted(Exception exception)
        {
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override SubdirectoryListItemViewModel CreateSubdirectoryViewModel(SubdirectoryListItemWithAncestorNames subdirectory) => new(subdirectory);

        protected async override Task<SubdirectoryListItemWithAncestorNames> LoadSubdirectoryAsync(Guid id, LocalDbContext dbContext, IWindowsStatusListener statusListener)
        {
            return await dbContext.SubdirectoryListingWithAncestorNames.FirstOrDefaultAsync(s => s.Id == id);
        }
        private Task<int> DeleteCrawlJobLogFromDbContextAsync([DisallowNull] CrawlJobListItemViewModel entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            throw new NotImplementedException();
        }

        protected override bool AddCrawlJobLogItem(CrawlJobListItemViewModel item)
        {
            if (base.AddCrawlJobLogItem(item))
            {
                item.EditCommand += Item_EditCommand;
                item.DeleteCommand += Item_DeleteCommand;
                return true;
            }
            return false;
        }

        protected override bool RemoveCrawlJobLogItem(CrawlJobListItemViewModel item)
        {
            if (base.RemoveCrawlJobLogItem(item))
            {
                item.EditCommand -= Item_EditCommand;
                item.DeleteCommand -= Item_DeleteCommand;
                return true;
            }
            return false;
        }

        protected override CrawlJobListItemViewModel[] ClearItems()
        {
            CrawlJobListItemViewModel[] removedItems = base.ClearItems();
            foreach (CrawlJobListItemViewModel item in removedItems)
            {
                item.EditCommand -= Item_EditCommand;
                item.DeleteCommand -= Item_DeleteCommand;
            }
            return removedItems;
        }

        private async Task DeleteItemAsync((CrawlJobListItemViewModel Item, CrawlJobLogListItem Entity) targets, [DisallowNull] IWindowsStatusListener statusListener)
        {
            using IServiceScope scope = Services.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            using DbContextEventReceiver eventReceiver = new(dbContext);
            _ = await DeleteCrawlJobLogFromDbContextAsync(targets.Entity, dbContext, statusListener);
            if (eventReceiver.SavedChangesOccurred && !eventReceiver.SaveChangesFailedOcurred)
                await Dispatcher.InvokeAsync(() => RemoveCrawlJobLogItem(targets.Item), DispatcherPriority.Background, statusListener.CancellationToken);
        }

        protected IAsyncJob DeleteCrawlJobLogAsync([DisallowNull] CrawlJobListItemViewModel item)
        {
            IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
            return jobFactory.StartNew("Deleting data", "Opening database", (item, item.Entity), DeleteItemAsync);
        }

        private void Item_EditCommand(object sender, Commands.CommandEventArgs e)
        {
            if (sender is CrawlJobListItemViewModel item)
                OnCrawlJobLogDeleteCommand(item, e.Parameter);
        }

        protected virtual void OnCrawlJobLogDeleteCommand([DisallowNull] CrawlJobListItemViewModel item, object parameter)
        {
            if (ConfirmCrawlJobLogDelete(item, parameter))
                _ = DeleteCrawlJobLogAsync(item);
        }

        private void Item_DeleteCommand(object sender, Commands.CommandEventArgs e)
        {
            if (sender is CrawlJobListItemViewModel item)
                OnCrawlJobLogDeleteCommand(item, e.Parameter);
        }
    }
}
