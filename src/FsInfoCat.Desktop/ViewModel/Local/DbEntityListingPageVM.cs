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
    public abstract class DbEntityListingPageVM<TDbEntity, TItemVM> : DependencyObject, INotifyNavigationContentChanged, IHasAsyncWindowsBackgroundOperationManager
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
        #region NewItemClick Property Members

        /// <summary>
        /// Occurs when the <see cref="NewItemClick">NewItemClick Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> AddNewItem;

        private static readonly DependencyPropertyKey NewItemClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NewItemClick),
            typeof(Commands.RelayCommand), typeof(DbEntityListingPageVM<TDbEntity, TItemVM>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="NewItemClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NewItemClickProperty = NewItemClickPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand NewItemClick => (Commands.RelayCommand)GetValue(NewItemClickProperty);

        /// <summary>
        /// Called when the NewItemClick event is raised by <see cref="NewItemClick" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="NewItemClick" />.</param>
        internal void RaiseAddNewItem(object parameter)
        {
            try { OnAddNewItem(parameter); }
            finally { AddNewItem?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="NewItemClick">NewItemClick Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="NewItemClick" />.</param>
        protected abstract void OnAddNewItem(object parameter);

        #endregion
        #region BgOps Property Members

        private static readonly DependencyPropertyKey BgOpsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BgOps), typeof(AsyncOps.AsyncBgModalVM), typeof(DbEntityListingPageVM<TDbEntity, TItemVM>),
                new PropertyMetadata());

        /// <summary>
        /// Identifies the <see cref="BgOps"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BgOpsProperty = BgOpsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public AsyncOps.AsyncBgModalVM BgOps => (AsyncOps.AsyncBgModalVM)GetValue(BgOpsProperty);

        #endregion

        public DbEntityListingPageVM()
        {
            SetValue(ItemsPropertyKey, new ReadOnlyObservableCollection<TItemVM>(_backingItems));
            SetValue(NewItemClickPropertyKey, new Commands.RelayCommand(RaiseAddNewItem));
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            Logger = App.GetLogger(this);
            SetValue(BgOpsPropertyKey, new AsyncOps.AsyncBgModalVM());
        }

        internal Task<int> LoadItemsAsync()
        {
            return BgOps.FromAsync("Loading items", "Connecting to database...", GetItemsLoaderFactory());
        }

        protected abstract Func<IWindowsStatusListener, Task<int>> GetItemsLoaderFactory();

        protected abstract DbSet<TDbEntity> GetDbSet(LocalDbContext dbContext);

        protected async Task<int> OnEntitiesLoaded([DisallowNull] IEnumerable<TDbEntity> entities, IWindowsStatusListener statusListener, Func<TDbEntity, TItemVM> createItem)
        {
            if (entities is null)
                throw new ArgumentNullException(nameof(entities));
            (TItemVM[] oldItems, TItemVM[] newItems) = await Dispatcher.InvokeAsync(() =>
            {
                TItemVM[] old = _backingItems.ToArray();
                _backingItems.Clear();
                return (old, entities.Select(createItem).ToArray());
            }, DispatcherPriority.Background, statusListener.CancellationToken);
            foreach (TItemVM item in oldItems)
            {
                item.EditRequest -= Item_Edit;
                item.DeleteRequest -= Item_Delete;
            }
            foreach (TItemVM item in newItems)
            {
                item.EditRequest += Item_Edit;
                item.DeleteRequest += Item_Delete;
            }
            return await Dispatcher.InvokeAsync(() =>
            {
                _backingItems.Clear();
                foreach (TItemVM item in newItems)
                    _backingItems.Add(item);
                return _backingItems.Count;
            }, DispatcherPriority.Background, statusListener.CancellationToken);
        }

        protected async Task<int> OnEntitiesLoaded<TResultItem>([DisallowNull] IEnumerable<TResultItem> results, IWindowsStatusListener statusListener, Func<TResultItem, TItemVM> createItem)
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
                item.EditRequest -= Item_Edit;
                item.DeleteRequest -= Item_Delete;
            }
            foreach (TItemVM item in newItems)
            {
                item.EditRequest += Item_Edit;
                item.DeleteRequest += Item_Delete;
            }
            return await Dispatcher.InvokeAsync(() =>
            {
                _backingItems.Clear();
                foreach (TItemVM item in newItems)
                    _backingItems.Add(item);
                return _backingItems.Count;
            }, DispatcherPriority.Background, statusListener.CancellationToken);
        }

        /// <summary>
        /// Removes an item from the <see cref="Items"/> collection without deleting it from the database.
        /// </summary>
        /// <param name="item">The <typeparamref name="TItemVM"/> to remove.</param>
        /// <returns><see langword="true"/> if an item was removed; otherwise, <see langword="false"/>.</returns>
        protected bool RemoveItem(TItemVM item)
        {
            VerifyAccess();
            return item is not null && _backingItems.Remove(item);
        }

        private async Task<bool?> SaveEntityAsync([DisallowNull] TDbEntity entity, IWindowsStatusListener statusListener)
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
            return isNew;
        }

        /// <summary>
        /// This gets called after <see cref="SaveChangesAsync(TItemVM, bool)"/> is successful, which inserts the <typeparamref name="TItemVM"/> into the <see cref="Items"/>
        /// collection if it was a newly inserted record.
        /// </summary>
        /// <param name="item">The <typeparamref name="TItemVM"/> of the <typeparamref name="TDbEntity"/> to was saved.</param>
        /// <param name="isNew"><see langword="true"/> if a new entity was inserted into the database;
        /// Otherwise, <see langword="false"/> if changes to an existing item were saved.</param>
        /// <remarks>Overriding methods can prevent items from being inserted into the <see cref="Items"/> collection by refraining from invoking the base implementation.
        /// <para>Items which no longer match the current listing filter conditions can be removed without deleting it from the database by
        /// using the <see cref="RemoveItem(TItemVM)"/> method.</para></remarks>
        protected virtual void OnChangesSaved(TItemVM item, bool isNew)
        {
            if (!_backingItems.Contains(item))
                _backingItems.Add(item);
        }

        /// <summary>
        /// Asynchronously saves changes to the underlying <typeparamref name="TDbEntity"/>.
        /// </summary>
        /// <param name="item">The <typeparamref name="TItemVM"/> of the <typeparamref name="TDbEntity"/> to be saved.</param>
        /// <param name="saveProgressTitle">The title to display in the modal popup control that is presented while the operation is in progress.</param>
        /// <returns><see langword="true"/> if a new item was inserted into the database, <see langword="false"/> if changes
        /// to an existing item were saved; otherwise, <see langword="null"/> of no changes were saved.</returns>
        /// <remarks><see cref="OnChangesSaved(TItemVM, bool)"/> will be invoked if operation is successful.</remarks>
        protected internal Task<bool?> SaveChangesAsync([DisallowNull] TItemVM item, string saveProgressTitle)
        {
            Task<bool?> task = BgOps.FromAsync(saveProgressTitle, "Connecting to database...", item.Model, SaveEntityAsync);
            task.ContinueWith(task =>
            {
                bool? isNew = task.Result;
                if (isNew.HasValue)
                    Dispatcher.Invoke(() => OnChangesSaved(item, isNew.Value));
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            return task;
        }

        private async Task<bool?> DeleteEntityAsync([DisallowNull] TDbEntity entity, IWindowsStatusListener statusListener)
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
                    return false;
                case EntityState.Deleted:
                    statusListener.SetMessage("Item was already deleted.", StatusMessageLevel.Warning);
                    return null;
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

        /// <summary>
        /// This gets automatically called to delete the <typeparamref name="TItemVM"/> from the <see cref="Items"/> collection after <see cref="DeleteItemAsync(TItemVM)"/> is successful.
        /// </summary>
        /// <param name="item">The <typeparamref name="TItemVM"/> of the <typeparamref name="TDbEntity"/> that was deleted.</param>
        protected virtual void OnItemDeleted(TItemVM item) => _backingItems.Remove(item);

        /// <summary>
        /// Deletes an item from the database.
        /// </summary>
        /// <param name="item">The <typeparamref name="TItemVM"/> of the <typeparamref name="TDbEntity"/> to be deleted.</param>
        /// <param name="deleteProgressTitle">The title to display in the modal popup control that is presented while the operation is in progress.</param>
        /// <returns><see langword="true"/> if an item was deleted from the database, <see langword="false"/> if the item does not exist;
        /// otherwise, <see langword="null"/> the item was already deleted.</returns>
        /// <remarks><see cref="OnChangesSaved(TItemVM, bool)"/> will be invoked if operation is successful.</remarks>
        protected internal virtual Task<bool?> DeleteItemAsync([DisallowNull] TItemVM item, string deleteProgressTitle)
        {
            VerifyAccess();
            Task<bool?> task = BgOps.FromAsync(deleteProgressTitle, "Connecting to database...", item.Model, DeleteEntityAsync);
            task.ContinueWith(t =>
            {
                if (t.Result ?? false)
                    Dispatcher.Invoke(() => OnItemDeleted(item));
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            return task;
        }

        private void Item_Delete(object sender, Commands.CommandEventArgs e)
        {
            if (sender is TItemVM item && PromptItemDeleting(item, e.Parameter, out string deleteProgressTitle))
                DeleteItemAsync(item, deleteProgressTitle);
        }

        private void Item_Edit(object sender, Commands.CommandEventArgs e)
        {
            if (sender is TItemVM item && ShowModalItemEditWindow(item, e.Parameter, out string saveProgressTitle))
                SaveChangesAsync(item, saveProgressTitle);
        }

        //protected abstract string GetSaveNewProgressTitle(TItemVM item);

        //protected abstract string GetSaveExistingProgressTitle(TItemVM item);

        //protected abstract string GetDeleteProgressTitle(TItemVM item);

        /// <summary>
        /// Displays a modal item edit window
        /// </summary>
        /// <param name="item">The item to edit.</param>
        /// <param name="parameter">The parameter passed to the edit command.</param>
        /// <param name="saveProgressTitle">The title to display in the modal popup control that is presented while the operation is in progress.</param>
        /// <returns><see langword="true"/> if there are changes to be saved to the database; otherwise, <see langword="false"/>.</returns>
        /// <remarks>If an item is to deleted from the database from the edit window, then the edit view model should call <see cref="DeleteItemAsync(TDbEntity)"/>
        /// and then return <see langword="false"/>.</remarks>
        protected abstract bool ShowModalItemEditWindow(TItemVM item, object parameter, out string saveProgressTitle);

        /// <summary>
        /// Displays a modal dialog to confirm that they want to delete the item.
        /// </summary>
        /// <param name="item">The item to be deleted.</param>
        /// <param name="parameter">The parameter passed to the delete command.</param>
        /// <param name="deleteProgressTitle">The title to display in the modal popup control that is presented while the operation is in progress.</param>
        /// <returns><see langword="true"/> if the item should be deleted from the database; otherwise, <see langword="false"/>.</returns>
        protected abstract bool PromptItemDeleting(TItemVM item, object parameter, out string deleteProgressTitle);

        protected virtual void OnNavigatedTo(MainVM mainVM)
        {
            LoadItemsAsync();
            // TODO: Do initial load from DB
        }

        protected virtual void OnNavigatedFrom(MainVM mainVM)
        {
            BgOps.CancelAll();
        }

        void INotifyNavigationContentChanged.OnNavigatedTo(MainVM mainVM) => OnNavigatedTo(mainVM);

        void INotifyNavigationContentChanged.OnNavigatedFrom(MainVM mainVM) => OnNavigatedFrom(mainVM);

        IAsyncWindowsBackgroundOperationManager IHasAsyncWindowsBackgroundOperationManager.GetAsyncBackgroundOperationManager()
        {
            if (CheckAccess())
                return BgOps;
            return Dispatcher.Invoke(() => BgOps);
        }

        IAsyncBackgroundOperationManager IHasAsyncBackgroundOperationManager.GetAsyncBackgroundOperationManager()
        {
            if (CheckAccess())
                return BgOps;
            return Dispatcher.Invoke(() => BgOps);
        }
    }

    public abstract class DbEntityListingPageVM<TDbEntity, TItemVM, TSelectionVM> : DbEntityListingPageVM<TDbEntity, TItemVM>
        where TDbEntity : LocalDbEntity, new()
        where TItemVM : DbEntityItemVM<TDbEntity>
        where TSelectionVM : DbEntityItemDetailViewModel<TDbEntity, TItemVM>, new()
    {
        #region ItemSelection Property Members

        private static readonly DependencyPropertyKey ItemSelectionPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ItemSelection), typeof(TSelectionVM),
            typeof(DbEntityListingPageVM<TDbEntity, TItemVM, TSelectionVM>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ItemSelection"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemSelectionProperty = ItemSelectionPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public TSelectionVM ItemSelection => (TSelectionVM)GetValue(ItemSelectionProperty);

        #endregion

        protected DbEntityListingPageVM()
        {
            SetValue(ItemSelectionPropertyKey, new TSelectionVM());
        }

        /// <summary>
        /// Called when the <typeparamref name="TItemVM"/> has changed.
        /// </summary>
        /// <param name="oldValue">The previous <see cref="DbEntityItemDetailViewModel{TDbEntity, TItemVM}.CurrentItem"/> value on the <see cref="ItemSelection"/> property.</param>
        /// <param name="newValue">The new <see cref="DbEntityItemDetailViewModel{TDbEntity, TItemVM}.CurrentItem"/> value on the <see cref="ItemSelection"/> property.</param>
        /// <remarks><see cref="DbEntityItemDetailViewModel{TDbEntity, TItemVM}.CurrentItemPropertyChanged"/> should be used, instead</remarks>
        [Obsolete("Use DbEntityItemDetailViewModel{TDbEntity, TItemVM}.CurrentItemPropertyChanged, instead")]
        protected virtual void OnCurrentItemPropertyChanged(TItemVM oldValue, TItemVM newValue) { }
    }
}
