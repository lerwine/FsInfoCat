using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public abstract class DbEntityListingPageVM<TDbEntity, TItemVM> : DependencyObject, INotifyNavigationContentChanged
        where TDbEntity : LocalDbEntity, new()
        where TItemVM : DbEntityItemVM<TDbEntity>
    {
        protected readonly ILogger<DbEntityListingPageVM<TDbEntity, TItemVM>> Logger;

        #region Items Property Members

        private readonly ObservableCollection<TItemVM> _backingItems = new();

        private static readonly DependencyPropertyKey ItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Items), typeof(ReadOnlyObservableCollection<TItemVM>), typeof(DbEntityListingPageVM<TDbEntity, TItemVM>),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Items"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets The items to be displayed.
        /// </summary>
        /// <value>The items to be displayed in the listing page.</value>
        public ReadOnlyObservableCollection<TItemVM> Items => (ReadOnlyObservableCollection<TItemVM>)GetValue(ItemsProperty);

        #endregion
        #region SelectedItem Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedItem"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler SelectedItemPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="SelectedItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(TItemVM), typeof(DbEntityListingPageVM<TDbEntity, TItemVM>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DbEntityListingPageVM<TDbEntity, TItemVM>)?.OnSelectedItemPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public TItemVM SelectedItem { get => (TItemVM)GetValue(SelectedItemProperty); set => SetValue(SelectedItemProperty, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="SelectedItemProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="SelectedItemProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnSelectedItemPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnSelectedItemPropertyChanged((TItemVM)args.OldValue, (TItemVM)args.NewValue); }
            finally { SelectedItemPropertyChanged?.Invoke(this, args); }
        }

        /// <summary>
        /// Called when the value of the <see cref="SelectedItem"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SelectedItem"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SelectedItem"/> property.</param>
        protected virtual void OnSelectedItemPropertyChanged(TItemVM oldValue, TItemVM newValue)
        {
            // TODO: Implement OnSelectedItemPropertyChanged Logic
        }

        #endregion
        #region NewItemClick Command Members

        /// <summary>
        /// Occurs when the <see cref="NewItemClickCommand">NewItemClick Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> NewItemClick;

        private static readonly DependencyPropertyKey NewItemClickCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NewItemClickCommand),
            typeof(Commands.RelayCommand), typeof(DbEntityListingPageVM<TDbEntity, TItemVM>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref=""/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NewItemClickCommandProperty = NewItemClickCommandPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand NewItemClickCommand => (Commands.RelayCommand)GetValue(NewItemClickCommandProperty);

        /// <summary>
        /// Called when the <see cref="NewItemClickCommand">NewItemClick Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="NewItemClickCommand" />.</param>
        protected virtual void OnNewItemClick(object parameter)
        {
            // TODO: Implement OnNewItemClick Logic
        }

        #endregion
        #region BgOps Property Members

        private static readonly DependencyPropertyKey BgOpsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BgOps), typeof(AsyncOps.AsyncOpAggregate), typeof(DbEntityListingPageVM<TDbEntity, TItemVM>),
                new PropertyMetadata());

        /// <summary>
        /// Identifies the <see cref="BgOps"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BgOpsProperty = BgOpsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public AsyncOps.AsyncOpAggregate BgOps => (AsyncOps.AsyncOpAggregate)GetValue(BgOpsProperty);

        #endregion
        #region EntityDbOp Property Members

        private static readonly DependencyPropertyKey EntityDbOpPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EntityDbOp), typeof(AsyncOps.AsyncOpResultManagerViewModel<TDbEntity, bool?>), typeof(DbEntityListingPageVM<TDbEntity, TItemVM>),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="EntityDbOp"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EntityDbOpProperty = EntityDbOpPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public AsyncOps.AsyncOpResultManagerViewModel<TDbEntity, bool?> EntityDbOp => (AsyncOps.AsyncOpResultManagerViewModel<TDbEntity, bool?>)GetValue(EntityDbOpProperty);

        #endregion
        #region ListingLoader Property Members

        private static readonly DependencyPropertyKey ListingLoaderPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ListingLoader), typeof(AsyncOps.AsyncOpResultManagerViewModel<int>), typeof(DbEntityListingPageVM<TDbEntity, TItemVM>),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ListingLoader"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListingLoaderProperty = ListingLoaderPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public AsyncOps.AsyncOpResultManagerViewModel<int> ListingLoader { get => (AsyncOps.AsyncOpResultManagerViewModel<int>)GetValue(ListingLoaderProperty); private set => SetValue(ListingLoaderPropertyKey, value); }

        #endregion
        #region RelatedItemLoader Property Members

        private static readonly DependencyPropertyKey RelatedItemLoaderPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RelatedItemLoader), typeof(AsyncOps.AsyncOpResultManagerViewModel<Guid, int>), typeof(DbEntityListingPageVM<TDbEntity, TItemVM>),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="RelatedItemLoader"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RelatedItemLoaderProperty = RelatedItemLoaderPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public AsyncOps.AsyncOpResultManagerViewModel<Guid, int> RelatedItemLoader { get => (AsyncOps.AsyncOpResultManagerViewModel<Guid, int>)GetValue(RelatedItemLoaderProperty); private set => SetValue(RelatedItemLoaderPropertyKey, value); }

        #endregion
        public DbEntityListingPageVM()
        {
            SetValue(ItemsPropertyKey, new ReadOnlyObservableCollection<TItemVM>(_backingItems));
            SetValue(NewItemClickCommandPropertyKey, new Commands.RelayCommand(OnNewItemClick));
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            Logger = App.GetLogger(this);
            SetValue(BgOpsPropertyKey, new AsyncOps.AsyncOpAggregate());
            SetValue(EntityDbOpPropertyKey, new AsyncOps.AsyncOpResultManagerViewModel<TDbEntity, bool?>());
            SetValue(ListingLoaderPropertyKey, new AsyncOps.AsyncOpResultManagerViewModel<int>());
            SetValue(RelatedItemLoaderPropertyKey, new AsyncOps.AsyncOpResultManagerViewModel<Guid, int>());
        }

        internal Task<int> LoadItemsAsync()
        {
            ListingLoader.CancelAll();
            AsyncOps.AsyncFuncOpViewModel<int> bgOp = BgOps.FromAsync("Loading items", "Connecting to database...", ListingLoader, GetItemsLoaderFactory());
            return bgOp.GetTask();
        }

        protected abstract Func<AsyncOps.IStatusListener, Task<int>> GetItemsLoaderFactory();

        protected abstract TItemVM CreateItem(TDbEntity entity);

        protected abstract TDbEntity InitializeNewEntity();

        protected abstract DbSet<TDbEntity> GetDbSet(LocalDbContext dbContext);

        protected async Task<int> OnEntitiesLoaded([DisallowNull] IEnumerable<TDbEntity> entities, AsyncOps.IStatusListener statusListener)
        {
            if (entities is null)
                throw new ArgumentNullException(nameof(entities));
            (TItemVM[] oldItems, TItemVM[] newItems) = await Dispatcher.InvokeAsync(() =>
            {
                TItemVM[] old = _backingItems.ToArray();
                _backingItems.Clear();
                return (old, entities.Select(e => CreateItem(e)).ToArray());
            }, DispatcherPriority.Background, statusListener.CancellationToken);
            foreach (TItemVM item in oldItems)
            {
                item.Edit -= Item_Edit;
                item.Delete -= Item_Delete;
            }
            foreach (TItemVM item in newItems)
            {
                item.Edit += Item_Edit;
                item.Delete += Item_Delete;
            }
            return await Dispatcher.InvokeAsync(() =>
            {
                _backingItems.Clear();
                foreach (TItemVM item in newItems)
                    _backingItems.Add(item);
                return _backingItems.Count;
            }, DispatcherPriority.Background, statusListener.CancellationToken);
        }

        protected async Task<int> OnEntitiesLoaded<TResultItem>([DisallowNull] IEnumerable<TResultItem> results, AsyncOps.IStatusListener statusListener, Func<TResultItem, TItemVM> createItem)
        {
            if (results is null)
                throw new ArgumentNullException(nameof(results));
            if (createItem is null)
                throw new ArgumentNullException(nameof(createItem));
            (TItemVM[] oldItems, TItemVM[] newItems) = await Dispatcher.InvokeAsync(() =>
            {
                TItemVM[] old = _backingItems.ToArray();
                _backingItems.Clear();
                return (old, results.Select(e => createItem(e)).ToArray());
            }, DispatcherPriority.Background, statusListener.CancellationToken);
            foreach (TItemVM item in oldItems)
            {
                item.Edit -= Item_Edit;
                item.Delete -= Item_Delete;
            }
            foreach (TItemVM item in newItems)
            {
                item.Edit += Item_Edit;
                item.Delete += Item_Delete;
            }
            return await Dispatcher.InvokeAsync(() =>
            {
                _backingItems.Clear();
                foreach (TItemVM item in newItems)
                    _backingItems.Add(item);
                return _backingItems.Count;
            }, DispatcherPriority.Background, statusListener.CancellationToken);
        }

        protected internal Task<(TItemVM, bool?)> SaveChangesAsync([DisallowNull] TDbEntity entity)
        {
            TItemVM item = _backingItems.FirstOrDefault(i => ReferenceEquals(i.Model, entity));
            string title;
            if (item is null)
            {
                item = CreateItem(entity);
                title = GetSaveNewProgressTitle(item);
            }
            else
                title = GetSaveExistingProgressTitle(item);
            AsyncOps.AsyncFuncOpViewModel<TDbEntity, bool?> asyncFuncOpViewModel = BgOps.FromAsync(title, "Connecting to database...", entity, EntityDbOp, OnSaveChangesAsync);
            return asyncFuncOpViewModel.GetTask().ContinueWith(task =>
            {
                if (!task.Result.HasValue)
                    return (item, (bool?)null);
                if (!task.Result.Value)
                    return (item, false);
                Dispatcher.Invoke(() =>
                {
                    if (!_backingItems.Contains(item))
                        _backingItems.Add(item);
                });
                return (item, true);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private async Task<bool?> OnSaveChangesAsync([DisallowNull] TDbEntity entity, AsyncOps.IStatusListener<TDbEntity> statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            EntityEntry<TDbEntity> entry = dbContext.Entry(entity);
            bool isNew;
            switch (entry.State)
            {
                case EntityState.Detached:
                    entry = GetDbSet(dbContext).Add(entity);
                    isNew = true;
                    break;
                case EntityState.Deleted:
                    statusListener.SetMessage("Item has been deleted from the database.", StatusMessageLevel.Error);
                    return null;
                case EntityState.Added:
                    isNew = true;
                    break;
                default:
                    isNew = false;
                    break;
            }

            DispatcherOperation dispatcherOperation = statusListener.BeginSetMessage(isNew ? "Inserting record into database..." : "Updating database record...");
            await dbContext.SaveChangesAsync(statusListener.CancellationToken);
            await dispatcherOperation;
            await Dispatcher.InvokeAsync(() =>
            {
                statusListener.SetMessage(isNew ? "Record inserted into database." : "Database record updated.");
                if (isNew || !_backingItems.Any(i => ReferenceEquals(i.Model, entity)))
                    _backingItems.Add(CreateItem(entity));
            }, DispatcherPriority.Background, statusListener.CancellationToken);
            return isNew;
        }

        protected internal Task<TItemVM> DeleteItemAsync([DisallowNull] TDbEntity entity)
        {
            VerifyAccess();
            TItemVM item = _backingItems.FirstOrDefault(i => ReferenceEquals(i.Model, entity));
            if (item is null)
                return Task.FromResult<TItemVM>(null);
            string title = GetDeleteProgressTitle(item);
            AsyncOps.AsyncFuncOpViewModel<TDbEntity, bool?> asyncFuncOpViewModel = BgOps.FromAsync(title, "Connecting to database...", entity, EntityDbOp, OnDeleteItemAsync);
            return asyncFuncOpViewModel.GetTask().ContinueWith(task =>
            {
                if (!(task.Result ?? false))
                    return null;
                Dispatcher.Invoke(() => _backingItems.Remove(item));
                return item;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private async Task<bool?> OnDeleteItemAsync([DisallowNull] TDbEntity entity, AsyncOps.IStatusListener<TDbEntity> statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            DispatcherOperation dispatcherOperation;
            EntityEntry<TDbEntity> entry = dbContext.Entry(entity);
            switch (entry.State)
            {
                case EntityState.Added:
                case EntityState.Detached:
                    statusListener.SetMessage("Item does not exist in the database.", StatusMessageLevel.Error);
                    return null;
                case EntityState.Deleted:
                    statusListener.SetMessage("Item was already deleted.", StatusMessageLevel.Warning);
                    return false;
                default:
                    dispatcherOperation = statusListener.BeginSetMessage("Deleting from database...");
                    GetDbSet(dbContext).Remove(entity);
                    break;
            }
            await dbContext.SaveChangesAsync(statusListener.CancellationToken);
            await dispatcherOperation;
            statusListener.SetMessage("Item deleted.");
            return true;
        }

        private void Item_Delete(object sender, Commands.CommandEventArgs e)
        {
            if (sender is TItemVM item && PromptItemDeleting(item, e.Parameter))
                DeleteItemAsync(item.Model);
        }

        private void Item_Edit(object sender, Commands.CommandEventArgs e)
        {
            if (sender is TItemVM item && ShowModalItemEditWindow(item, e.Parameter))
                SaveChangesAsync(item.Model);
        }

        protected abstract string GetSaveNewProgressTitle(TItemVM item);

        protected abstract string GetSaveExistingProgressTitle(TItemVM item);

        protected abstract string GetDeleteProgressTitle(TItemVM item);

        /// <summary>
        /// Displays a modal item edit window
        /// </summary>
        /// <param name="item">The item to edit.</param>
        /// <param name="parameter">The parameter passed to the edit command.</param>
        /// <returns><see langword="true"/> if there are changes to be saved to the database; otherwise, <see langword="false"/>.</returns>
        /// <remarks>If an item is to deleted from the database from the edit window, then the edit view model should call <see cref="DeleteItemAsync(TDbEntity)"/>
        /// and then return <see langword="false"/>.</remarks>
        protected abstract bool ShowModalItemEditWindow(TItemVM item, object parameter);

        /// <summary>
        /// Displays a modal dialog to confirm that they want to delete the item.
        /// </summary>
        /// <param name="item">The item to be deleted.</param>
        /// <param name="parameter">The parameter passed to the delete command.</param>
        /// <returns><see langword="true"/> if the item should be deleted from the database; otherwise, <see langword="false"/>.</returns>
        protected abstract bool PromptItemDeleting(TItemVM item, object parameter);

        protected virtual void OnNavigatedTo(MainVM mainVM)
        {
            LoadItemsAsync();
            // TODO: Do initial load from DB
        }

        protected virtual void OnNavigatedFrom(MainVM mainVM)
        {
            EntityDbOp.CancelAll();
        }

        void INotifyNavigationContentChanged.OnNavigatedTo(MainVM mainVM) => OnNavigatedTo(mainVM);

        void INotifyNavigationContentChanged.OnNavigatedFrom(MainVM mainVM) => OnNavigatedFrom(mainVM);
    }
}
