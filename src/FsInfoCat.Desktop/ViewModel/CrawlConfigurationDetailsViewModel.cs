using FsInfoCat.Local;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class CrawlConfigurationDetailsViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem> : CrawlConfigurationRowViewModel<TEntity>
        where TEntity : DbEntity, ICrawlConfiguration, ICrawlConfigurationRow
        where TSubdirectoryEntity : DbEntity, ISubdirectoryListItemWithAncestorNames
        where TSubdirectoryItem : SubdirectoryListItemWithAncestorNamesViewModel<TSubdirectoryEntity>
        where TCrawlJobLogEntity : DbEntity, ICrawlJobListItem
        where TCrawlJobLogItem : CrawlJobListItemViewModel<TCrawlJobLogEntity>
    {
        #region AddNewCrawlJobLog Command Property Members

        private static readonly DependencyPropertyKey AddNewCrawlJobLogPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AddNewCrawlJobLog), typeof(Commands.RelayCommand),
            typeof(CrawlConfigurationDetailsViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="AddNewCrawlJobLog"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AddNewCrawlJobLogProperty = AddNewCrawlJobLogPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand AddNewCrawlJobLog => (Commands.RelayCommand)GetValue(AddNewCrawlJobLogProperty);

        #endregion
        #region RefreshCrawlJobLogs Command Property Members

        private static readonly DependencyPropertyKey RefreshCrawlJobLogsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RefreshCrawlJobLogs),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationDetailsViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="RefreshCrawlJobLogs"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RefreshCrawlJobLogsProperty = RefreshCrawlJobLogsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand RefreshCrawlJobLogs => (Commands.RelayCommand)GetValue(RefreshCrawlJobLogsProperty);

        #endregion
        #region Root Property Members

        /// <summary>
        /// Identifies the <see cref="Root"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RootProperty = DependencyProperty.Register(nameof(Root), typeof(TSubdirectoryItem),
            typeof(CrawlConfigurationDetailsViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as CrawlConfigurationDetailsViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>)?
                    .OnRootPropertyChanged((TSubdirectoryItem)e.OldValue, (TSubdirectoryItem)e.NewValue)));

        /// <summary>
        /// Gets or sets the starting subdirectory for the configured subdirectory crawl.
        /// </summary>
        /// <value>The starting subdirectory for the configured subdirectory crawl.</value>
        public TSubdirectoryItem Root { get => (TSubdirectoryItem)GetValue(RootProperty); set => SetValue(RootProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Root"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Root"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Root"/> property.</param>
        protected virtual void OnRootPropertyChanged(TSubdirectoryItem oldValue, TSubdirectoryItem newValue) { }

        #endregion
        #region Logs Property Members

        private readonly ObservableCollection<TCrawlJobLogItem> _backingLogs = new();

        private static readonly DependencyPropertyKey LogsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Logs), typeof(ReadOnlyObservableCollection<TCrawlJobLogItem>),
            typeof(CrawlConfigurationDetailsViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Logs"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LogsProperty = LogsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ReadOnlyObservableCollection<TCrawlJobLogItem> Logs => (ReadOnlyObservableCollection<TCrawlJobLogItem>)GetValue(LogsProperty);

        #endregion

        public CrawlConfigurationDetailsViewModel([DisallowNull] TEntity entity, [AllowNull] TSubdirectoryItem root) : base(entity)
        {
            SetValue(LogsPropertyKey, new ReadOnlyObservableCollection<TCrawlJobLogItem>(_backingLogs));
            SetValue(AddNewCrawlJobLogPropertyKey, new Commands.RelayCommand(OnAddNewCrawlJobLogCommand));
            SetValue(RefreshCrawlJobLogsPropertyKey, new Commands.RelayCommand(OnRefreshCrawlJobLogsCommand));
            Root = root;
        }

        protected abstract void OnRefreshCrawlJobLogsCommand(object parameter);

        protected abstract void OnAddNewCrawlJobLogCommand(object parameter);

        protected abstract void OnCrawlJobLogEditCommand([DisallowNull] TCrawlJobLogItem item, object parameter);

        protected abstract bool ConfirmCrawlJobLogDelete([DisallowNull] TCrawlJobLogItem item, object parameter);

        protected abstract IQueryable<TCrawlJobLogEntity> GetQueryableCrawlJobLogListing([DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener);

        protected abstract TCrawlJobLogItem CreateCrawlJobLogViewModel([DisallowNull] TCrawlJobLogEntity entity);

        protected abstract Task<int> DeleteCrawlJobLogFromDbContextAsync([DisallowNull] TCrawlJobLogEntity entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener);

        protected virtual void OnCrawlJobLogDeleted([DisallowNull] TCrawlJobLogItem item) { }

        private async Task DeleteItemAsync((TCrawlJobLogItem Item, TCrawlJobLogEntity Entity) targets, [DisallowNull] IWindowsStatusListener statusListener)
        {
            using IServiceScope scope = Services.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            using DbContextEventReceiver eventReceiver = new(dbContext);
            await DeleteCrawlJobLogFromDbContextAsync(targets.Entity, dbContext, statusListener);
            if (eventReceiver.SavedChangesOccurred && !eventReceiver.SaveChangesFailedOcurred)
                await Dispatcher.InvokeAsync(() =>
                {
                    targets.Item.EditCommand -= Item_EditCommand;
                    targets.Item.DeleteCommand -= Item_DeleteCommand;
                    _backingLogs.Remove(targets.Item);
                    OnCrawlJobLogDeleted(targets.Item);
                }, DispatcherPriority.Background, statusListener.CancellationToken);
        }

        protected IAsyncJob DeleteCrawlJobLogAsync([DisallowNull] TCrawlJobLogItem item)
        {
            IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
            return jobFactory.StartNew("Deleting data", "Opening database", (item, item.Entity), DeleteItemAsync);
        }

        private void Item_EditCommand(object sender, Commands.CommandEventArgs e)
        {
            if (sender is TCrawlJobLogItem item)
                OnCrawlJobLogEditCommand(item, e.Parameter);
        }

        protected virtual void OnCrawlJobLogDeleteCommand([DisallowNull] TCrawlJobLogItem item, object parameter)
        {
            if (ConfirmCrawlJobLogDelete(item, parameter))
                DeleteCrawlJobLogAsync(item);
        }

        private void Item_DeleteCommand(object sender, Commands.CommandEventArgs e)
        {
            if (sender is TCrawlJobLogItem item)
                OnCrawlJobLogDeleteCommand(item, e.Parameter);
        }
    }
}