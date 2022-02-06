using FsInfoCat.Activities;
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
    public abstract class ListingViewModel<TEntity, TItemViewModel, TFilterOptions> : FilteredItemsViewModel
        where TEntity : DbEntity
        where TItemViewModel : DbEntityRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
    {
        #region PageTitle Property Members

        protected static readonly DependencyPropertyKey PageTitlePropertyKey = DependencyPropertyBuilder<ListingViewModel<TEntity, TItemViewModel, TFilterOptions>, string>
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
            typeof(ListingViewModel<TEntity, TItemViewModel, TFilterOptions>),
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

        protected abstract IQueryable<TEntity> GetQueryableListing(TFilterOptions options, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IActivityProgress progress);

        protected abstract TItemViewModel CreateItemViewModel([DisallowNull] TEntity entity);

        protected abstract Task<PageFunction<ItemFunctionResultEventArgs>> GetEditPageAsync(TItemViewModel item, [DisallowNull] IActivityProgress progress);

        protected abstract Task<PageFunction<ItemFunctionResultEventArgs>> GetDetailPageAsync([DisallowNull] TItemViewModel item, [DisallowNull] IActivityProgress progress);

        protected abstract bool ConfirmItemDelete([DisallowNull] TItemViewModel item, object parameter);

        protected abstract void OnReloadTaskCompleted(TFilterOptions options);

        protected abstract void OnReloadTaskFaulted([DisallowNull] Exception exception, TFilterOptions options);

        protected abstract void OnReloadTaskCanceled(TFilterOptions options);

        protected abstract void OnEditTaskFaulted([DisallowNull] Exception exception, TItemViewModel item);

        protected abstract Task<EntityEntry> DeleteEntityFromDbContextAsync([DisallowNull] TEntity entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IActivityProgress progress);

        private async Task<bool> DeleteItemAsync((TItemViewModel Item, TEntity Entity) targets, [DisallowNull] IActivityProgress progress)
        {
            using IServiceScope scope = Hosting.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            EntityEntry entry = await DeleteEntityFromDbContextAsync(targets.Entity, dbContext, progress);
            return entry.State == EntityState.Detached;
        }

        protected abstract void OnDeleteTaskFaulted([DisallowNull] Exception exception, [DisallowNull] TItemViewModel item);

        protected virtual void OnItemDeleteCommand([DisallowNull] TItemViewModel item, object parameter)
        {
            if (ConfirmItemDelete(item, parameter))
            {
                // TODO: Implement ListingViewModel{TEntity, TItemViewModel, TFilterOptions}.OnItemDeleteCommand
                throw new NotImplementedException();
                //IWindowsAsyncJobFactoryService jobFactory = Hosting.GetRequiredService<IWindowsAsyncJobFactoryService>();
                //jobFactory.StartNew("Deleting data", "Opening database", (item, item.Entity), DeleteItemAsync).Task.ContinueWith(task => Dispatcher.Invoke(() =>
                //{
                //    if (task.IsCanceled)
                //        return;
                //    if (task.IsFaulted)
                //        OnDeleteTaskFaulted(task.Exception, item);
                //    else if (task.Result && _backingItems.Remove(item))
                //    {
                //        item.OpenCommand -= Item_OpenCommand;
                //        item.EditCommand -= Item_EditCommand;
                //        item.DeleteCommand -= Item_DeleteCommand;
                //    }
                //}));
            }
        }

        protected virtual IAsyncAction<IActivityEvent> RefreshAsync(TFilterOptions options)
        {
            Hosting.GetAsyncActivityService().InvokeAsync(activityDescription: "", initialStatusMessage: "", progress => LoadItemsAsync(options, progress));
            // TODO: Implement ListingViewModel{TEntity, TItemViewModel, TFilterOptions}.RefreshAsync
            throw new NotImplementedException();
            //IWindowsAsyncJobFactoryService jobFactory = Hosting.GetRequiredService<IWindowsAsyncJobFactoryService>();
            //IAsyncJob job = jobFactory.StartNew("Loading data", "Opening database", options, LoadItemsAsync);
            //job.Task.ContinueWith(task => Dispatcher.Invoke(() =>
            //{
            //    if (task.IsCanceled)
            //        OnReloadTaskCanceled(options);
            //    else if (task.IsFaulted)
            //    {
            //        if (task.Exception.InnerExceptions.Count == 1)
            //            OnReloadTaskFaulted(task.Exception.InnerException, options);
            //        else
            //            OnReloadTaskFaulted(task.Exception, options);
            //    }
            //    else
            //        OnReloadTaskCompleted(options);
            //}, DispatcherPriority.Background));
            //return job;
        }

        private async Task LoadItemsAsync(TFilterOptions options, [DisallowNull] IActivityProgress progress)
        {
            using IServiceScope scope = Hosting.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            IQueryable<TEntity> items = GetQueryableListing(options, dbContext, progress);
            _ = await Dispatcher.InvokeAsync(ClearItems, DispatcherPriority.Background, progress.Token);
            await items.ForEachAsync(async item => await AddItemAsync(item, progress), progress.Token);
        }

        protected virtual TItemViewModel[] ClearItems()
        {
            VerifyAccess();
            TItemViewModel[] removedItems = _backingItems.ToArray();
            _backingItems.Clear();
            foreach (TItemViewModel item in removedItems)
            {
                item.OpenCommand -= Item_OpenCommand;
                item.EditCommand -= Item_EditCommand;
                item.DeleteCommand -= Item_DeleteCommand;
            }
            return removedItems;
        }

        private DispatcherOperation AddItemAsync([DisallowNull] TEntity entity, [DisallowNull] IActivityProgress progress) =>
            Dispatcher.InvokeAsync(() => AddItem(CreateItemViewModel(entity)), DispatcherPriority.Background, progress.Token);

        protected virtual void AddItem(TItemViewModel item)
        {
            VerifyAccess();
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (!_backingItems.Any(i => ReferenceEquals(i, item)))
            {
                _backingItems.Add(item);
                item.OpenCommand += Item_OpenCommand;
                item.EditCommand += Item_EditCommand;
                item.DeleteCommand += Item_DeleteCommand;
            }
        }

        private void Item_OpenCommand(object sender, Commands.CommandEventArgs e)
        {
            if (sender is TItemViewModel item)
            {
                // TODO: Implement ListingViewModel{TEntity, TItemViewModel, TFilterOptions}.Item_OpenCommand
                throw new NotImplementedException();
                //IWindowsAsyncJobFactoryService jobFactory = Hosting.GetRequiredService<IWindowsAsyncJobFactoryService>();
                //jobFactory.StartNew("Loading database record", "Opening database", item, GetDetailPageAsync).Task.ContinueWith(task => OnGetEditPageComplete(task, item));
            }
        }

        protected virtual bool RemoveItem(TItemViewModel item)
        {
            VerifyAccess();
            if (item is not null && _backingItems.Remove(item))
            {
                item.OpenCommand -= Item_OpenCommand;
                item.EditCommand -= Item_EditCommand;
                item.DeleteCommand -= Item_DeleteCommand;
                return true;
            }
            return false;
        }

        private void OnGetEditPageComplete(Task<PageFunction<ItemFunctionResultEventArgs>> task, TItemViewModel item) => Dispatcher.Invoke(() =>
        {
            if (task.IsCanceled)
                return;
            if (task.IsFaulted)
                OnEditTaskFaulted(task.Exception, item);
            else
            {
                PageFunction<ItemFunctionResultEventArgs> page = task.Result;
                if (page is null)
                    return;
                page.Return += Page_Return;
                Hosting.ServiceProvider.GetRequiredService<IApplicationNavigation>().Navigate(page);
            }
        });

        protected sealed override void OnAddNewItemCommand(object parameter)
        {
            // TODO: Implement ListingViewModel{TEntity, TItemViewModel, TFilterOptions}.OnAddNewItemCommand
            throw new NotImplementedException();
            //IWindowsAsyncJobFactoryService jobFactory = Hosting.GetRequiredService<IWindowsAsyncJobFactoryService>();
            //jobFactory.StartNew("Loading database record", "Opening database", (TItemViewModel)null, GetEditPageAsync).Task.ContinueWith(task => OnGetEditPageComplete(task, null));
        }

        private void Item_EditCommand(object sender, Commands.CommandEventArgs e)
        {
            if (sender is TItemViewModel item)
            {
                // TODO: Implement ListingViewModel{TEntity, TItemViewModel, TFilterOptions}.Item_EditCommand
                throw new NotImplementedException();
                //IWindowsAsyncJobFactoryService jobFactory = Hosting.GetRequiredService<IWindowsAsyncJobFactoryService>();
                //jobFactory.StartNew("Loading database record", "Opening database", item, GetEditPageAsync).Task.ContinueWith(task => OnGetEditPageComplete(task, item));
            }
        }

        private void Page_Return(object sender, ReturnEventArgs<ItemFunctionResultEventArgs> e)
        {
            TItemViewModel item;
            switch (e.Result.FunctionResult)
            {
                case ItemFunctionResult.Inserted:
                    // BUG: This does not work. e.Result.State will be null
                    if (e.Result.Entity is TEntity addedEntity && EntityMatchesCurrentFilter(addedEntity))
                    {
                        item = CreateItemViewModel(addedEntity);
                        _backingItems.Add(item);
                        item.OpenCommand += Item_OpenCommand;
                        item.EditCommand += Item_EditCommand;
                        item.DeleteCommand += Item_DeleteCommand;
                    }
                    break;
                case ItemFunctionResult.ChangesSaved:
                    if (e.Result.State is TEntity modifiedEntity && !EntityMatchesCurrentFilter(modifiedEntity))
                    {
                        item = Items.FirstOrDefault(i => ReferenceEquals(i.Entity, modifiedEntity));
                        if (_backingItems.Remove(item))
                        {
                            item.OpenCommand -= Item_OpenCommand;
                            item.EditCommand -= Item_EditCommand;
                            item.DeleteCommand -= Item_DeleteCommand;
                        }
                    }
                    break;
                case ItemFunctionResult.Deleted:
                    if (e.Result.State is TEntity deletedEntity)
                    {
                        item = Items.FirstOrDefault(i => ReferenceEquals(i.Entity, deletedEntity));
                        if (_backingItems.Remove(item))
                        {
                            item.OpenCommand -= Item_OpenCommand;
                            item.EditCommand -= Item_EditCommand;
                            item.DeleteCommand -= Item_DeleteCommand;
                        }
                    }
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
