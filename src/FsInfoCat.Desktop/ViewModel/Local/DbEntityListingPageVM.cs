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
            SetValue(NewItemClickCommandPropertyKey, new Commands.RelayCommand(OnNewItemClick));
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
            Task<bool?> task = BgOps.FromAsync(title, "Connecting to database...", entity, OnSaveChangesAsync);
            return task.ContinueWith(task =>
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

        private async Task<bool?> OnSaveChangesAsync([DisallowNull] TDbEntity entity, AsyncOps.IStatusListener statusListener)
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
            Task<bool?> task = BgOps.FromAsync(title, "Connecting to database...", entity, OnDeleteItemAsync);
            return task.ContinueWith(task =>
            {
                if (!(task.Result ?? false))
                    return null;
                Dispatcher.Invoke(() => _backingItems.Remove(item));
                return item;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private async Task<bool?> OnDeleteItemAsync([DisallowNull] TDbEntity entity, AsyncOps.IStatusListener statusListener)
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
            BgOps.RaiseOperationCancelRequested(null);
        }

        void INotifyNavigationContentChanged.OnNavigatedTo(MainVM mainVM) => OnNavigatedTo(mainVM);

        void INotifyNavigationContentChanged.OnNavigatedFrom(MainVM mainVM) => OnNavigatedFrom(mainVM);
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
