using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.Local
{
    public abstract class ListingViewModel<TEntity, TItem, TOptions> : DependencyObject
        where TEntity : LocalDbEntity
        where TItem : DbEntityRowViewModel<TEntity>, ILocalCrudEntityRowViewModel<TEntity>
    {
        #region ShowFilterOptions Command Property Members

        private static readonly DependencyPropertyKey ShowFilterOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ShowFilterOptions),
            typeof(Commands.RelayCommand), typeof(ListingViewModel<TEntity, TItem, TOptions>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ShowFilterOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowFilterOptionsProperty = ShowFilterOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ShowFilterOptions => (Commands.RelayCommand)GetValue(ShowFilterOptionsProperty);

        protected virtual void OnShowFilterOptionsCommand(object parameter) => ViewOptionsVisible = true;

        #endregion
        #region SaveFilterOptions Command Property Members

        private static readonly DependencyPropertyKey SaveFilterOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SaveFilterOptions),
            typeof(Commands.RelayCommand), typeof(ListingViewModel<TEntity, TItem, TOptions>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SaveFilterOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SaveFilterOptionsProperty = SaveFilterOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand SaveFilterOptions => (Commands.RelayCommand)GetValue(SaveFilterOptionsProperty);

        private void RaiseSaveFilterOptionsCommand(object parameter)
        {
            ViewOptionsVisible = false;
            OnApplyFilterOptionsCommand(parameter);
        }

        #endregion
        #region CancelFilterOptions Command Property Members

        private static readonly DependencyPropertyKey CancelFilterOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CancelFilterOptions),
            typeof(Commands.RelayCommand), typeof(ListingViewModel<TEntity, TItem, TOptions>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="CancelFilterOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CancelFilterOptionsProperty = CancelFilterOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand CancelFilterOptions => (Commands.RelayCommand)GetValue(CancelFilterOptionsProperty);

        protected virtual void OnCancelFilterOptionsCommand(object parameter) => ViewOptionsVisible = false;

        #endregion
        #region ViewOptionsVisible Property Members

        private static readonly DependencyPropertyKey ViewOptionsVisiblePropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptionsVisible), typeof(bool),
            typeof(ListingViewModel<TEntity, TItem, TOptions>), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="ViewOptionsVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionsVisibleProperty = ViewOptionsVisiblePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool ViewOptionsVisible { get => (bool)GetValue(ViewOptionsVisibleProperty); private set => SetValue(ViewOptionsVisiblePropertyKey, value); }

        #endregion
        #region AddNewItem Command Property Members

        private static readonly DependencyPropertyKey AddNewItemPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AddNewItem), typeof(Commands.RelayCommand),
            typeof(ListingViewModel<TEntity, TItem, TOptions>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="AddNewItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AddNewItemProperty = AddNewItemPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand AddNewItem => (Commands.RelayCommand)GetValue(AddNewItemProperty);

        #endregion
        #region Refresh Command Property Members

        private static readonly DependencyPropertyKey RefreshPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Refresh), typeof(Commands.RelayCommand),
            typeof(ListingViewModel<TEntity, TItem, TOptions>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Refresh"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RefreshProperty = RefreshPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand Refresh => (Commands.RelayCommand)GetValue(RefreshProperty);

        #endregion
        #region Items Property Members

        private readonly ObservableCollection<TItem> _backingItems = new();

        private static readonly DependencyPropertyKey ItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Items), typeof(ReadOnlyObservableCollection<TItem>),
            typeof(ListingViewModel<TEntity, TItem, TOptions>),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Items"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the items to be displayed in the page listing.
        /// </summary>
        /// <value>The items to be displayed in the page listing.</value>
        public ReadOnlyObservableCollection<TItem> Items => (ReadOnlyObservableCollection<TItem>)GetValue(ItemsProperty);

        #endregion

        protected ListingViewModel()
        {
            SetValue(ItemsPropertyKey, new ReadOnlyObservableCollection<TItem>(_backingItems));
            SetValue(AddNewItemPropertyKey, new Commands.RelayCommand(OnAddNewItemCommand));
            SetValue(RefreshPropertyKey, new Commands.RelayCommand(OnRefreshCommand));
            SetValue(ShowFilterOptionsPropertyKey, new Commands.RelayCommand(OnShowFilterOptionsCommand));
            SetValue(SaveFilterOptionsPropertyKey, new Commands.RelayCommand(RaiseSaveFilterOptionsCommand));
            SetValue(CancelFilterOptionsPropertyKey, new Commands.RelayCommand(OnCancelFilterOptionsCommand));
        }

        protected abstract IQueryable<TEntity> GetQueryableListing(TOptions options, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener);

        protected abstract TItem CreateItemViewModel([DisallowNull] TEntity entity);

        protected abstract void OnApplyFilterOptionsCommand(object parameter);

        protected abstract void OnRefreshCommand(object parameter);

        protected abstract void OnItemEditCommand([DisallowNull] TItem item, object parameter);

        protected abstract bool ConfirmItemDelete([DisallowNull] TItem item, object parameter);

        protected abstract Task<int> DeleteEntityFromDbContextAsync([DisallowNull] TEntity entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener);

        protected abstract void OnAddNewItemCommand(object parameter);

        protected virtual void OnItemDeleteCommand([DisallowNull] TItem item, object parameter)
        {
            if (ConfirmItemDelete(item, parameter))
                DeleteItemAsync(item);
        }

        protected IAsyncJob DeleteItemAsync([DisallowNull] TItem item)
        {
            IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
            return jobFactory.StartNew("Deleting data", "Opening database", (item, item.Entity), DeleteItemAsync);
        }

        protected IAsyncJob ReloadAsync(TOptions options)
        {
            IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
            return jobFactory.StartNew("Loading data", "Opening database", options, LoadItemsAsync);
        }

        private async Task LoadItemsAsync(TOptions options, [DisallowNull] IWindowsStatusListener statusListener)
        {
            using IServiceScope scope = Services.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            IQueryable<TEntity> items = GetQueryableListing(options, dbContext, statusListener);
            await Dispatcher.InvokeAsync(ClearItems, DispatcherPriority.Background, statusListener.CancellationToken);
            await items.ForEachAsync(async item => await AddItemAsync(item, statusListener), statusListener.CancellationToken);
        }

        private async Task DeleteItemAsync((TItem Item, TEntity Entity) targets, [DisallowNull] IWindowsStatusListener statusListener)
        {
            using IServiceScope scope = Services.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            using DbContextEventReceiver eventReceiver = new(dbContext);
            await DeleteEntityFromDbContextAsync(targets.Entity, dbContext, statusListener);
            if (eventReceiver.SavedChangesOccurred && !eventReceiver.SaveChangesFailedOcurred)
                await Dispatcher.InvokeAsync(() =>
                {
                    targets.Item.EditCommand -= Item_EditCommand;
                    targets.Item.DeleteCommand -= Item_DeleteCommand;
                    _backingItems.Remove(targets.Item);
                    OnItemDeleted(targets.Item);
                }, DispatcherPriority.Background, statusListener.CancellationToken);
        }

        protected virtual void OnItemDeleted([DisallowNull] TItem item) { }

        protected virtual TItem[] ClearItems()
        {
            CheckAccess();
            TItem[] removedItems = _backingItems.ToArray();
            _backingItems.Clear();
            foreach (TItem item in removedItems)
            {
                item.EditCommand -= Item_EditCommand;
                item.DeleteCommand -= Item_DeleteCommand;
            }
            return removedItems;
        }

        private DispatcherOperation AddItemAsync([DisallowNull] TEntity entity, [DisallowNull] IWindowsStatusListener statusListener) =>
            Dispatcher.InvokeAsync(() => AddItem(CreateItemViewModel(entity)), DispatcherPriority.Background, statusListener.CancellationToken);

        protected virtual void AddItem(TItem item)
        {
            CheckAccess();
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (!_backingItems.Any(i => ReferenceEquals(i, item)))
            {
                _backingItems.Add(item);
                item.EditCommand += Item_EditCommand;
                item.DeleteCommand += Item_DeleteCommand;
            }
        }

        private void Item_EditCommand(object sender, Commands.CommandEventArgs e)
        {
            if (sender is TItem item)
                OnItemEditCommand(item, e.Parameter);
        }

        private void Item_DeleteCommand(object sender, Commands.CommandEventArgs e)
        {
            if (sender is TItem item)
                OnItemDeleteCommand(item, e.Parameter);
        }
    }
}
