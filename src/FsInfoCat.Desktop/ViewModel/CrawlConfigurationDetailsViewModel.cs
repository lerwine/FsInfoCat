using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
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
        #region RefreshCrawlJobLogs Command Property Members

        private static readonly DependencyPropertyKey RefreshCrawlJobLogsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RefreshCrawlJobLogs),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationDetailsViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="RefreshCrawlJobLogs"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RefreshCrawlJobLogsProperty = RefreshCrawlJobLogsPropertyKey.DependencyProperty;

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

        public ReadOnlyObservableCollection<TCrawlJobLogItem> Logs => (ReadOnlyObservableCollection<TCrawlJobLogItem>)GetValue(LogsProperty);

        #endregion

        public CrawlConfigurationDetailsViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            SetValue(RefreshCrawlJobLogsPropertyKey, new Commands.RelayCommand(o => ReloadAsync()));
            SetValue(LogsPropertyKey, new ReadOnlyObservableCollection<TCrawlJobLogItem>(_backingLogs));
        }

        private async Task LoadItemsAsync([DisallowNull] IWindowsStatusListener statusListener)
        {
            using IServiceScope scope = Services.CreateScope();
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
            IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
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

        //protected abstract void OnAddNewCrawlJobLogCommand(object parameter);

        //protected abstract bool ConfirmCrawlJobLogDelete([DisallowNull] TCrawlJobLogItem item, object parameter);

        protected abstract IQueryable<TCrawlJobLogEntity> GetQueryableCrawlJobLogListing([DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener);

        protected abstract TCrawlJobLogItem CreateCrawlJobLogViewModel([DisallowNull] TCrawlJobLogEntity entity);
    }
}
