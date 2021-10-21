using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class CrawlConfigurationViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem> :
        CrawlConfigurationRowViewModel<TEntity>, IItemFunctionViewModel<TEntity>
        where TEntity : DbEntity, ICrawlConfiguration, ICrawlConfigurationRow
        where TSubdirectoryEntity : DbEntity, ISubdirectoryListItemWithAncestorNames
        where TSubdirectoryItem : SubdirectoryListItemWithAncestorNamesViewModel<TSubdirectoryEntity>
        where TCrawlJobLogEntity : DbEntity, ICrawlJobListItem
        where TCrawlJobLogItem : CrawlJobListItemViewModel<TCrawlJobLogEntity>
    {
        #region RefreshCrawlJobLogs Command Property Members

        private static readonly DependencyPropertyKey RefreshCrawlJobLogsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RefreshCrawlJobLogs),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="RefreshCrawlJobLogs"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RefreshCrawlJobLogsProperty = RefreshCrawlJobLogsPropertyKey.DependencyProperty;

        public Commands.RelayCommand RefreshCrawlJobLogs => (Commands.RelayCommand)GetValue(RefreshCrawlJobLogsProperty);

        #endregion
        #region Delete Property Members

        /// <summary>
        /// Occurs when the <see cref="Delete">Delete Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> DeleteCommand;

        private static readonly DependencyPropertyKey DeletePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Delete),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Delete"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DeleteProperty = DeletePropertyKey.DependencyProperty;

        public Commands.RelayCommand Delete => (Commands.RelayCommand)GetValue(DeleteProperty);

        /// <summary>
        /// Called when the Delete event is raised by <see cref="Delete" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Delete" />.</param>
        protected virtual void RaiseDeleteCommand(object parameter) => DeleteCommand?.Invoke(this, new(parameter));

        #endregion
        #region Root Property Members

        /// <summary>
        /// Identifies the <see cref="Root"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RootProperty = DependencyPropertyBuilder<CrawlConfigurationViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>, TSubdirectoryItem>
            .Register(nameof(Root))
            .DefaultValue(null)
            .AsReadWrite();

        public TSubdirectoryItem Root { get => (TSubdirectoryItem)GetValue(RootProperty); set => SetValue(RootProperty, value); }

        #endregion
        #region Logs Property Members

        private readonly ObservableCollection<TCrawlJobLogItem> _backingLogs = new();

        private static readonly DependencyPropertyKey LogsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Logs), typeof(ReadOnlyObservableCollection<TCrawlJobLogItem>),
            typeof(CrawlConfigurationViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Logs"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LogsProperty = LogsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<TCrawlJobLogItem> Logs => (ReadOnlyObservableCollection<TCrawlJobLogItem>)GetValue(LogsProperty);

        #endregion
        #region Completed Event Members

        public event EventHandler<ItemFunctionResultEventArgs> Completed;

        internal object InvocationState { get; }

        object IItemFunctionViewModel.InvocationState => InvocationState;

        protected virtual void OnItemFunctionResult(ItemFunctionResultEventArgs args) => Completed?.Invoke(this, args);

        protected void RaiseItemInsertedResult([DisallowNull] DbEntity entity) => OnItemFunctionResult(new(ItemFunctionResult.Inserted, entity, InvocationState));

        protected void RaiseItemUpdatedResult() => OnItemFunctionResult(new(ItemFunctionResult.ChangesSaved, Entity, InvocationState));

        protected void RaiseItemDeletedResult() => OnItemFunctionResult(new(ItemFunctionResult.Deleted, Entity, InvocationState));

        protected void RaiseItemUnmodifiedResult() => OnItemFunctionResult(new(ItemFunctionResult.Unmodified, Entity, InvocationState));

        #endregion

        public CrawlConfigurationViewModel([DisallowNull] TEntity entity, object state = null) : base(entity)
        {
            InvocationState = state;
            SetValue(RefreshCrawlJobLogsPropertyKey, new Commands.RelayCommand(o => ReloadAsync()));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
            SetValue(LogsPropertyKey, new ReadOnlyObservableCollection<TCrawlJobLogItem>(_backingLogs));
        }

        protected async Task LoadSubdirectoryAsync(Guid id, IWindowsStatusListener statusListener)
        {
            using IServiceScope scope = Hosting.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            TSubdirectoryEntity subdirectory = await LoadSubdirectoryAsync(id, dbContext, statusListener);
            Root = (subdirectory is null) ? null : CreateSubdirectoryViewModel(subdirectory);
        }

        private async Task LoadItemsAsync([DisallowNull] IWindowsStatusListener statusListener)
        {
            using IServiceScope scope = Hosting.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            ISubdirectory root = Entity.Root;
            if (root is not null)
            {
                TSubdirectoryEntity subdirectory = await LoadSubdirectoryAsync(root.Id, dbContext, statusListener);
                if (subdirectory is not null)
                    Root = CreateSubdirectoryViewModel(subdirectory);
            }
            IQueryable<TCrawlJobLogEntity> items = GetQueryableCrawlJobLogListing(dbContext, statusListener);
            _ = await Dispatcher.InvokeAsync(ClearItems, DispatcherPriority.Background, statusListener.CancellationToken);
            await items.ForEachAsync(async item => await AddCrawlJobLogItemAsync(item, statusListener), statusListener.CancellationToken);
        }

        protected abstract TSubdirectoryItem CreateSubdirectoryViewModel(TSubdirectoryEntity subdirectory);

        protected abstract Task<TSubdirectoryEntity> LoadSubdirectoryAsync(Guid id, LocalDbContext dbContext, IWindowsStatusListener statusListener);

        private DispatcherOperation AddCrawlJobLogItemAsync([DisallowNull] TCrawlJobLogEntity entity, [DisallowNull] IWindowsStatusListener statusListener) =>
            Dispatcher.InvokeAsync(() => AddCrawlJobLogItem(CreateCrawlJobLogViewModel(entity)), DispatcherPriority.Background, statusListener.CancellationToken);

        protected virtual bool AddCrawlJobLogItem(TCrawlJobLogItem item)
        {
            VerifyAccess();
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (_backingLogs.Any(i => ReferenceEquals(i, item)))
                return false;
            _backingLogs.Add(item);
            return true;
        }

        protected virtual bool RemoveCrawlJobLogItem(TCrawlJobLogItem item)
        {
            VerifyAccess();
            return item is not null && _backingLogs.Remove(item);
        }

        protected virtual IAsyncJob ReloadAsync()
        {
            IWindowsAsyncJobFactoryService jobFactory = Hosting.GetRequiredService<IWindowsAsyncJobFactoryService>();
            IAsyncJob job = jobFactory.StartNew("Loading data", "Opening database", LoadItemsAsync);
            job.Task.ContinueWith(task => Dispatcher.Invoke(() =>
            {
                if (task.Exception.InnerExceptions.Count == 1)
                    OnReloadTaskFaulted(task.Exception.InnerException);
                else
                    OnReloadTaskFaulted(task.Exception);
            }, DispatcherPriority.Background), TaskContinuationOptions.OnlyOnFaulted);
            return job;
        }

        protected abstract void OnReloadTaskFaulted(Exception exception);

        protected virtual TCrawlJobLogItem[] ClearItems()
        {
            VerifyAccess();
            TCrawlJobLogItem[] removedItems = _backingLogs.ToArray();
            _backingLogs.Clear();
            return removedItems;
        }


        protected void SetRootSubdirectory(ISubdirectory root)
        {
            if (root is null or TSubdirectoryItem)
                Dispatcher.CheckInvoke(() => Root = root as TSubdirectoryItem);
            else
                SetRootSubdirectory(root.Id);
        }

        protected void SetRootSubdirectory(Guid id)
        {
            Guid? g = Root?.Entity.Id;
            if (g.HasValue && g.Value == id)
                return;
            IWindowsAsyncJobFactoryService jobFactory = Hosting.GetRequiredService<IWindowsAsyncJobFactoryService>();
            jobFactory.StartNew("Loading data", "Opening database", id, LoadSubdirectoryAsync).Task.ContinueWith(task => Dispatcher.Invoke(() =>
                MessageBox.Show(Application.Current.MainWindow, "Unexpected error while reading from the database. See error logs for more information.",
                    "Database Error", MessageBoxButton.OK, MessageBoxImage.Error), DispatcherPriority.Background), TaskContinuationOptions.OnlyOnFaulted);
        }

        protected abstract IQueryable<TCrawlJobLogEntity> GetQueryableCrawlJobLogListing([DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener);

        protected abstract TCrawlJobLogItem CreateCrawlJobLogViewModel([DisallowNull] TCrawlJobLogEntity entity);
    }
}
