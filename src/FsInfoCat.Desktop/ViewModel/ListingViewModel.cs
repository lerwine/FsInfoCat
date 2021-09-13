using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    public abstract class ListingViewModel<TEntity, TItemViewModel, TFilterOptions, TEditResult> : FilteredItemsViewModel
        where TEntity : DbEntity
        where TItemViewModel : DbEntityRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
        where TEditResult : class, IEntityEditResult<TEntity>
    {
        #region PageTitle Property Members

        protected static readonly DependencyPropertyKey PageTitlePropertyKey = DependencyPropertyBuilder<ListingViewModel<TEntity, TItemViewModel, TFilterOptions, TEditResult>, string>
            .Register(nameof(PageTitle))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="PageTitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PageTitleProperty = PageTitlePropertyKey.DependencyProperty;

        public string PageTitle { get => GetValue(PageTitleProperty) as string; protected set => SetValue(PageTitlePropertyKey, value); }

        #endregion
        #region Items Property Members

        private readonly ObservableCollection<TItemViewModel> _backingItems = new();

        private static readonly DependencyPropertyKey ItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Items), typeof(ReadOnlyObservableCollection<TItemViewModel>),
            typeof(ListingViewModel<TEntity, TItemViewModel, TFilterOptions, TEditResult>),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Items"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the items to be displayed in the page listing.
        /// </summary>
        /// <value>The items to be displayed in the page listing.</value>
        public ReadOnlyObservableCollection<TItemViewModel> Items => (ReadOnlyObservableCollection<TItemViewModel>)GetValue(ItemsProperty);

        #endregion

        protected ListingViewModel()
        {
            SetValue(ItemsPropertyKey, new ReadOnlyObservableCollection<TItemViewModel>(_backingItems));
        }

        protected abstract bool EntityMatchesCurrentFilter([DisallowNull] TEntity entity);

        protected abstract IQueryable<TEntity> GetQueryableListing(TFilterOptions options, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener);

        protected abstract TItemViewModel CreateItemViewModel([DisallowNull] TEntity entity);

        protected abstract Task<PageFunction<TEditResult>> GetEditPageAsync(TItemViewModel item, [DisallowNull] IWindowsStatusListener statusListener);

        protected abstract bool ConfirmItemDelete([DisallowNull] TItemViewModel item, object parameter);

        //protected abstract Task<TEditArgs> LoadItemAsync([DisallowNull] TEntity item, [DisallowNull] IWindowsStatusListener statusListener);

        protected abstract void OnReloadTaskCompleted(TFilterOptions options);

        protected abstract void OnReloadTaskFaulted([DisallowNull] Exception exception, TFilterOptions options);

        protected abstract void OnReloadTaskCanceled(TFilterOptions options);

        protected abstract void OnEditTaskFaulted([DisallowNull] Exception exception, TItemViewModel item);

        protected abstract Task<EntityEntry> DeleteEntityFromDbContextAsync([DisallowNull] TEntity entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener);

        protected abstract void OnDeleteTaskFaulted([DisallowNull] Exception exception, [DisallowNull] TItemViewModel item);

        private async Task<bool> DeleteItemAsync((TItemViewModel Item, TEntity Entity) targets, [DisallowNull] IWindowsStatusListener statusListener)
        {
            using IServiceScope scope = Services.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            EntityEntry entry = await DeleteEntityFromDbContextAsync(targets.Entity, dbContext, statusListener);
            return entry.State == EntityState.Detached;
        }

        protected virtual void OnItemDeleteCommand([DisallowNull] TItemViewModel item, object parameter)
        {
            if (ConfirmItemDelete(item, parameter))
            {
                IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
                jobFactory.StartNew("Deleting data", "Opening database", (item, item.Entity), DeleteItemAsync).Task.ContinueWith(task => Dispatcher.Invoke(() =>
                {
                    if (task.IsCanceled)
                        return;
                    if (task.IsFaulted)
                        OnDeleteTaskFaulted(task.Exception, item);
                    else if (task.Result)
                        _backingItems.Remove(item);
                }));
            }
        }

        protected virtual IAsyncJob ReloadAsync(TFilterOptions options)
        {
            IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
            IAsyncJob job = jobFactory.StartNew("Loading data", "Opening database", options, LoadItemsAsync);
            job.Task.ContinueWith(task => Dispatcher.Invoke(() =>
            {
                if (task.IsCanceled)
                    OnReloadTaskCanceled(options);
                else if (task.IsFaulted)
                {
                    if (task.Exception.InnerExceptions.Count == 1)
                        OnReloadTaskFaulted(task.Exception.InnerException, options);
                    else
                        OnReloadTaskFaulted(task.Exception, options);
                }
                else
                    OnReloadTaskCompleted(options);
            }, DispatcherPriority.Background));
            return job;
        }

        private async Task LoadItemsAsync(TFilterOptions options, [DisallowNull] IWindowsStatusListener statusListener)
        {
            using IServiceScope scope = Services.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            IQueryable<TEntity> items = GetQueryableListing(options, dbContext, statusListener);
            _ = await Dispatcher.InvokeAsync(ClearItems, DispatcherPriority.Background, statusListener.CancellationToken);
            await items.ForEachAsync(async item => await AddItemAsync(item, statusListener), statusListener.CancellationToken);
        }

        protected virtual TItemViewModel[] ClearItems()
        {
            VerifyAccess();
            TItemViewModel[] removedItems = _backingItems.ToArray();
            _backingItems.Clear();
            foreach (TItemViewModel item in removedItems)
            {
                item.EditCommand -= Item_EditCommand;
                item.DeleteCommand -= Item_DeleteCommand;
            }
            return removedItems;
        }

        private DispatcherOperation AddItemAsync([DisallowNull] TEntity entity, [DisallowNull] IWindowsStatusListener statusListener) =>
            Dispatcher.InvokeAsync(() => AddItem(CreateItemViewModel(entity)), DispatcherPriority.Background, statusListener.CancellationToken);

        protected virtual void AddItem(TItemViewModel item)
        {
            VerifyAccess();
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (!_backingItems.Any(i => ReferenceEquals(i, item)))
            {
                _backingItems.Add(item);
                item.EditCommand += Item_EditCommand;
                item.DeleteCommand += Item_DeleteCommand;
            }
        }

        protected virtual bool RemoveItem(TItemViewModel item)
        {
            VerifyAccess();
            return item is not null && _backingItems.Remove(item);
        }

        private void OnGetEditPageComplete(Task<PageFunction<TEditResult>> task, TItemViewModel item) => Dispatcher.Invoke(() =>
        {
            if (task.IsCanceled)
                return;
            if (task.IsFaulted)
                OnEditTaskFaulted(task.Exception, item);
            else
            {
                PageFunction<TEditResult> page = task.Result;
                if (page is null)
                    return;
                page.Return += Page_Return;
                Services.ServiceProvider.GetRequiredService<IApplicationNavigation>().Navigate(page);
            }
        });

        protected sealed override void OnAddNewItemCommand(object parameter)
        {
            IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
            jobFactory.StartNew("Loading database record", "Opening database", (TItemViewModel)null, GetEditPageAsync).Task.ContinueWith(task => OnGetEditPageComplete(task, null));
        }

        private void Item_EditCommand(object sender, Commands.CommandEventArgs e)
        {
            if (sender is TItemViewModel item)
            {
                IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
                jobFactory.StartNew("Loading database record", "Opening database", item, GetEditPageAsync).Task.ContinueWith(task => OnGetEditPageComplete(task, item));
            }
        }

        private void Page_Return(object sender, ReturnEventArgs<TEditResult> e)
        {
            switch (e.Result.State)
            {
                case EntityEditResultState.Added:
                    if (EntityMatchesCurrentFilter(e.Result.ItemEntity))
                        _backingItems.Add(CreateItemViewModel(e.Result.ItemEntity));
                    break;
                case EntityEditResultState.Modified:
                    if (!EntityMatchesCurrentFilter(e.Result.ItemEntity))
                        _backingItems.Remove(Items.FirstOrDefault(i => ReferenceEquals(i.Entity, e.Result.ItemEntity)));
                    break;
                case EntityEditResultState.Deleted:
                    _backingItems.Remove(Items.FirstOrDefault(i => ReferenceEquals(i.Entity, e.Result.ItemEntity)));
                    break;
            }
        }

        private void Item_DeleteCommand(object sender, Commands.CommandEventArgs e)
        {
            if (sender is TItemViewModel item)
                OnItemDeleteCommand(item, e.Parameter);
        }
    }
}
