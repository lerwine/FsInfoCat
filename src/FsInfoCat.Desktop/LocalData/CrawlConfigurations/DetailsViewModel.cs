using FsInfoCat.Activities;
using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
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
        private IAsyncAction<IActivityEvent> _currentAction;

        #region StartCrawl Command Property Members

        /// <summary>
        /// Occurs when the <see cref="StartCrawl"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> StartCrawlCommand;

        private static readonly DependencyPropertyKey StartCrawlPropertyKey = DependencyPropertyBuilder<DetailsViewModel, Commands.RelayCommand>
            .Register(nameof(StartCrawl))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="StartCrawl"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StartCrawlProperty = StartCrawlPropertyKey.DependencyProperty;

        public Commands.RelayCommand StartCrawl => (Commands.RelayCommand)GetValue(StartCrawlProperty);

        /// <summary>
        /// Called when the StartCrawl event is raised by <see cref="StartCrawl" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="StartCrawl" />.</param>
        protected void RaiseStartCrawlCommand(object parameter) // => StartCrawlCommand?.Invoke(this, new(parameter));
        {
            try { OnStartCrawlCommand(parameter); }
            finally { StartCrawlCommand?.Invoke(this, new(parameter)); }
        }

        private async Task CrawlAsync(IActivityProgress progress)
        {
            using IServiceScope scope = Hosting.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Guid id = Entity.RootId;
            Subdirectory subdirectory = await dbContext.Subdirectories.FirstOrDefaultAsync(s => s.Id == id);

        }

        /// <summary>
        /// Called when the <see cref="StartCrawl">StartCrawl Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="StartCrawl" />.</param>
        protected virtual void OnStartCrawlCommand(object parameter)
        {
            IAsyncActivityService backgroundService = Hosting.GetAsyncActivityService();
            StatusValue = Model.CrawlStatus.InProgress;
            IAsyncAction<IActivityEvent> currentAction = backgroundService.InvokeAsync("", "", CrawlAsync);
            _currentAction = currentAction;
            currentAction.Task.ContinueWith(task => Dispatcher.Invoke(() =>
            {
                if (ReferenceEquals(_currentAction, currentAction))
                {
                    _currentAction = null;
                    if (task.IsCanceled)
                        StatusValue = Model.CrawlStatus.Canceled;
                    else if (task.IsFaulted)
                        StatusValue = Model.CrawlStatus.Failed;
                    else if (StatusValue == Model.CrawlStatus.InProgress)
                        StatusValue = Model.CrawlStatus.Completed;
                }
            }));
        }

        #endregion
        #region StopCrawl Command Property Members

        /// <summary>
        /// Occurs when the <see cref="StopCrawl"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> StopCrawlCommand;

        private static readonly DependencyPropertyKey StopCrawlPropertyKey = DependencyPropertyBuilder<DetailsViewModel, Commands.RelayCommand>
            .Register(nameof(StopCrawl))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="StopCrawl"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StopCrawlProperty = StopCrawlPropertyKey.DependencyProperty;

        public Commands.RelayCommand StopCrawl => (Commands.RelayCommand)GetValue(StopCrawlProperty);

        /// <summary>
        /// Called when the StopCrawl event is raised by <see cref="StopCrawl" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="StopCrawl" />.</param>
        protected void RaiseStopCrawlCommand(object parameter) // => StopCrawlCommand?.Invoke(this, new(parameter));
        {
            try { OnStopCrawlCommand(parameter); }
            finally { StopCrawlCommand?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="StopCrawl">StopCrawl Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="StopCrawl" />.</param>
        protected virtual void OnStopCrawlCommand(object parameter)
        {
            // TODO: Implement OnStopCrawlCommand Logic
        }

        #endregion
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

        private static readonly DependencyPropertyKey ListItemPropertyKey = DependencyPropertyBuilder<DetailsViewModel, CrawlConfigListItemBase>
            .Register(nameof(ListItem))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ListItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListItemProperty = ListItemPropertyKey.DependencyProperty;

        public CrawlConfigListItemBase ListItem { get => (CrawlConfigListItemBase)GetValue(ListItemProperty); private set => SetValue(ListItemPropertyKey, value); }

        #endregion

        public DetailsViewModel([DisallowNull] CrawlConfiguration entity, [DisallowNull] CrawlConfigListItemBase itemEntity) : base(entity, itemEntity)
        {
            SetValue(AddNewCrawlJobLogPropertyKey, new Commands.RelayCommand(OnAddNewCrawlJobLogCommand));
            SetValue(StartCrawlPropertyKey, new Commands.RelayCommand(RaiseStartCrawlCommand));
            SetValue(StopCrawlPropertyKey, new Commands.RelayCommand(RaiseStopCrawlCommand));
            ListItem = itemEntity;
            UpstreamId = entity.UpstreamId;
            LastSynchronizedOn = entity.LastSynchronizedOn;
            OnStatusValueChanged(entity.StatusValue);
        }

        private void OnStatusValueChanged(Model.CrawlStatus statusValue)
        {
            switch (statusValue)
            {
                case Model.CrawlStatus.Disabled:
                    StartCrawl.IsEnabled = StopCrawl.IsEnabled = false;
                    break;
                case Model.CrawlStatus.InProgress:
                    StartCrawl.IsEnabled = false;
                    StopCrawl.IsEnabled = true;
                    break;
                default:
                    StartCrawl.IsEnabled = true;
                    StopCrawl.IsEnabled = false;
                    break;
            }
        }

        protected override void OnStatusValuePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            base.OnStatusValuePropertyChanged(args);
            OnStatusValueChanged((Model.CrawlStatus)args.NewValue);
        }

        private bool ConfirmCrawlJobLogDelete([DisallowNull] CrawlJobListItemViewModel item, object parameter) => MessageBox.Show(Application.Current.MainWindow, "This action cannot be undone!\n\nAre you sure you want to delete this crawl completion log entry?", "Delete Completion Log Entry", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;

        protected override CrawlJobListItemViewModel CreateCrawlJobLogViewModel([DisallowNull] CrawlJobLogListItem entity) => new(entity);

        private Task<int> DeleteCrawlJobLogFromDbContextAsync([DisallowNull] CrawlJobLogListItem entity, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IActivityProgress progress)
        {
            // TODO: Implement DeleteCrawlJobLogFromDbContextAsync(CrawlJobLogListItem, LocalDbContext, IWindowsStatusListener
            Dispatcher.ShowMessageBoxAsync("You  have invoked a command which has not yet been implemented.", "Not Implemented", MessageBoxButton.OK, MessageBoxImage.Hand, progress.Token);
            throw new NotImplementedException($"{nameof(DeleteCrawlJobLogFromDbContextAsync)} not implemented");
        }

        protected override IQueryable<CrawlJobLogListItem> GetQueryableCrawlJobLogListing([DisallowNull] LocalDbContext dbContext, [DisallowNull] IActivityProgress progress)
        {
            Guid id = Entity.Id;
            progress.Report("Reading from database");
            return dbContext.CrawlJobListing.Where(j => j.ConfigurationId == id);
        }

        private void OnAddNewCrawlJobLogCommand(object parameter)
        {
            // TODO: Implement OnAddNewCrawlJobLogCommand
            throw new NotImplementedException();
            //IWindowsAsyncJobFactoryService jobFactory = Hosting.GetRequiredService<IWindowsAsyncJobFactoryService>();
            //jobFactory.StartNew("Loading database record", "Opening database", (CrawlJobListItemViewModel)null, GetEditPageAsync).Task.ContinueWith(task => OnGetEditPageComplete(task, null));
        }

        private void OnCrawlJobLogEditCommand([DisallowNull] CrawlJobListItemViewModel item, object parameter)
        {
            // TODO: Implement OnCrawlJobLogEditCommand
            throw new NotImplementedException();
            //IWindowsAsyncJobFactoryService jobFactory = Hosting.GetRequiredService<IWindowsAsyncJobFactoryService>();
            //jobFactory.StartNew("Loading database record", "Opening database", item, GetEditPageAsync).Task.ContinueWith(task => OnGetEditPageComplete(task, item));
        }

        private async Task<PageFunction<ItemFunctionResultEventArgs>> GetEditPageAsync(CrawlJobListItemViewModel item, [DisallowNull] IActivityProgress progress)
        {
            CrawlJobLog crawlJobLog;
            if (item is null)
                crawlJobLog = new() { Configuration = Entity };
            else
            {
                using IServiceScope scope = Hosting.CreateScope();
                using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
                Guid id = item.Entity.Id;
                crawlJobLog = await dbContext.CrawlJobLogs.FirstOrDefaultAsync(j => j.Id == id, progress.Token);
                if (crawlJobLog is null)
                {
                    await Dispatcher.ShowMessageBoxAsync("Item not found in database. Click OK to refresh listing.", "Security Exception", MessageBoxButton.OK, MessageBoxImage.Error,
                        progress.Token);
                    ReloadAsync();
                    return null;
                }
            }
            return await Dispatcher.InvokeAsync(() => new CrawlLogs.EditPage(new(crawlJobLog, item?.Entity)), DispatcherPriority.Normal, progress.Token);
        }

        private void OnGetEditPageComplete(Task<PageFunction<ItemFunctionResultEventArgs>> task, CrawlJobListItemViewModel item) => Dispatcher.Invoke(() =>
        {
            if (task.IsCanceled)
                return;
            if (task.IsFaulted)
            {
                Exception exception = (task.Exception.InnerExceptions.Count > 1) ? task.Exception : task.Exception.InnerExceptions[0];
                _ = MessageBox.Show(Application.Current.MainWindow,
                    ((exception is ActivityException aExc) ? aExc.ToString().NullIfWhiteSpace() :
                        (exception as AggregateException)?.InnerExceptions.OfType<ActivityException>().Select(e => e.ToString())
                        .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                        "There was an unexpected error while loading items from the database.\n\nSee logs for further information",
                    "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                PageFunction<ItemFunctionResultEventArgs> page = task.Result;
                if (page is null)
                    return;
                page.Return += Page_Return;
                Hosting.ServiceProvider.GetRequiredService<IApplicationNavigation>().Navigate(page);
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
                ((exception is ActivityException aExc) ? aExc.ToString().NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<ActivityException>().Select(e => e.ToString())
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the database.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override SubdirectoryListItemViewModel CreateSubdirectoryViewModel(SubdirectoryListItemWithAncestorNames subdirectory) => new(subdirectory);

        protected async override Task<SubdirectoryListItemWithAncestorNames> LoadSubdirectoryAsync(Guid id, LocalDbContext dbContext, IActivityProgress progress)
        {
            return await dbContext.SubdirectoryListingWithAncestorNames.FirstOrDefaultAsync(s => s.Id == id);
        }
        private Task<int> DeleteCrawlJobLogFromDbContextAsync([DisallowNull] CrawlJobListItemViewModel entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IActivityProgress progress)
        {
            // TODO: Implement DeleteCrawlJobLogFromDbContextAsync
            Dispatcher.ShowMessageBoxAsync("You  have invoked a command which has not yet been implemented.", "Not Implemented", MessageBoxButton.OK, MessageBoxImage.Hand, progress.Token);
            throw new NotImplementedException($"{nameof(DeleteCrawlJobLogFromDbContextAsync)} not implemented");
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

        private async Task DeleteItemAsync((CrawlJobListItemViewModel Item, CrawlJobLogListItem Entity) targets, [DisallowNull] IActivityProgress progress)
        {
            using IServiceScope scope = Hosting.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            using DbContextEventReceiver eventReceiver = new(dbContext);
            _ = await DeleteCrawlJobLogFromDbContextAsync(targets.Entity, dbContext, progress);
            if (eventReceiver.SavedChangesOccurred && !eventReceiver.SaveChangesFailedOcurred)
                await Dispatcher.InvokeAsync(() => RemoveCrawlJobLogItem(targets.Item), DispatcherPriority.Background, progress.Token);
        }

        protected IAsyncAction<IActivityEvent> DeleteCrawlJobLogAsync([DisallowNull] CrawlJobListItemViewModel item)
        {
            // TODO: Implement DeleteCrawlJobLogAsync
            throw new NotImplementedException();
            //IWindowsAsyncJobFactoryService jobFactory = Hosting.GetRequiredService<IWindowsAsyncJobFactoryService>();
            //return jobFactory.StartNew("Deleting data", "Opening database", (item, item.Entity), DeleteItemAsync);
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
