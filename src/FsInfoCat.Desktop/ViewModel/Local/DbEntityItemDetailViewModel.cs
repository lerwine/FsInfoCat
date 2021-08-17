using FsInfoCat.Desktop.ViewModel.AsyncOps;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public abstract class DbEntityItemDetailViewModel<TDbEntity, TItemVM> : DependencyObject
        where TDbEntity : LocalDbEntity, new()
        where TItemVM : DbEntityItemVM<TDbEntity>
    {
        #region BgOps Property Members

        private static readonly DependencyPropertyKey BgOpsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BgOps), typeof(AsyncOps.AsyncBgModalVM), typeof(DbEntityItemDetailViewModel<TDbEntity, TItemVM>),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="BgOps"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BgOpsProperty = BgOpsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the asynchronous operation view model.
        /// </summary>
        /// <value>The view model to be bound to the <see cref="View.AsyncBgModalControl"/>.</value>
        public AsyncOps.AsyncBgModalVM BgOps => (AsyncOps.AsyncBgModalVM)GetValue(BgOpsProperty);

        #endregion
        #region RelatedItemLoader Property Members

        private static readonly DependencyPropertyKey RelatedItemLoaderPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RelatedItemLoader), typeof(AsyncOps.AsyncOpResultManagerViewModel<Guid, int>), typeof(DbEntityItemDetailViewModel<TDbEntity, TItemVM>),
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
        #region CurrentItem Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="CurrentItem"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler CurrentItemPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="CurrentItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CurrentItemProperty = DependencyProperty.Register(nameof(CurrentItem), typeof(TItemVM), typeof(DbEntityItemDetailViewModel<TDbEntity, TItemVM>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DbEntityItemDetailViewModel<TDbEntity, TItemVM>)?.OnCurrentItemPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public TItemVM CurrentItem { get => (TItemVM)GetValue(CurrentItemProperty); set => SetValue(CurrentItemProperty, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="CurrentItemProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="CurrentItemProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnCurrentItemPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnCurrentItemPropertyChanged((TItemVM)args.OldValue, (TItemVM)args.NewValue); }
            finally { CurrentItemPropertyChanged?.Invoke(this, args); }
        }

        /// <summary>
        /// Called when the value of the <see cref="CurrentItem"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CurrentItem"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CurrentItem"/> property.</param>
        protected abstract void OnCurrentItemPropertyChanged(TItemVM oldValue, TItemVM newValue);

        #endregion

        public DbEntityItemDetailViewModel()
        {
            SetValue(BgOpsPropertyKey, new AsyncOps.AsyncBgModalVM());
            SetValue(RelatedItemLoaderPropertyKey, new AsyncOps.AsyncOpResultManagerViewModel<Guid, int>());
        }
    }
    public class FileSystemItemDetailViewModel : DbEntityItemDetailViewModel<FileSystem, FileSystemItemVM>
    {
        #region NewSymbolicName Property Members

        /// <summary>
        /// Occurs when the <see cref="NewSymbolicName">NewSymbolicName Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> AddNewSymbolicName;

        private static readonly DependencyPropertyKey NewSymbolicNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(NewSymbolicName),
            typeof(Commands.RelayCommand), typeof(FileSystemItemDetailViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="NewSymbolicName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NewSymbolicNameProperty = NewSymbolicNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand NewSymbolicName => (Commands.RelayCommand)GetValue(NewSymbolicNameProperty);

        /// <summary>
        /// Called when the NewSymbolicName event is raised by <see cref="NewSymbolicName" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="NewSymbolicName" />.</param>
        private void RaiseAddNewSymbolicName(object parameter)
        {
            try { OnAddNewSymbolicName(parameter); }
            finally { AddNewSymbolicName?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="NewSymbolicName">NewSymbolicName Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="NewSymbolicName" />.</param>
        protected virtual void OnAddNewSymbolicName(object parameter)
        {
            // TODO: Implement OnAddNewSymbolicName Logic
        }

        #endregion
        #region ViewOptions Property Members

        private static readonly DependencyPropertyKey ViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptions), typeof(ThreeStateViewModel), typeof(FileSystemItemDetailViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionsProperty = ViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ThreeStateViewModel ViewOptions => (ThreeStateViewModel)GetValue(ViewOptionsProperty);

        #endregion
        #region SymbolicNames Property Members

        private readonly ObservableCollection<SymbolicNameItemVM> _backingSymbolicNames = new();

        private static readonly DependencyPropertyKey SymbolicNamesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SymbolicNames), typeof(ReadOnlyObservableCollection<SymbolicNameItemVM>), typeof(FileSystemItemDetailViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SymbolicNames"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SymbolicNamesProperty = SymbolicNamesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the symbolic names.
        /// </summary>
        /// <value>The symbolic names for the <see cref="DbEntityItemDetailViewModel{FileSystem, FileSystemItemVM}.CurrentItem"/>.</value>
        public ReadOnlyObservableCollection<SymbolicNameItemVM> SymbolicNames => (ReadOnlyObservableCollection<SymbolicNameItemVM>)GetValue(SymbolicNamesProperty);

        #endregion

        protected override void OnCurrentItemPropertyChanged(FileSystemItemVM oldValue, FileSystemItemVM newValue)
        {
            _backingSymbolicNames.Clear();
            StartSymbolicNamesLoad(newValue?.Model.Id, ViewOptions.Value);
        }

        private void StartSymbolicNamesLoad(Guid? fileSystemId, bool? showActive)
        {
            _backingSymbolicNames.Clear();
            if (fileSystemId.HasValue)
                _ = BgOps.FromAsync("", "", fileSystemId.Value, RelatedItemLoader, new Func<Guid, IStatusListener<Guid>, Task<int>>((id, listener) =>
                Task.Run(async () => await LoadSymbolicNamesAsync(id, showActive, listener))));
        }

        private async Task<int> LoadSymbolicNamesAsync(Guid fileSystemId, bool? showActive, IStatusListener statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            SymbolicName[] items;
            if (showActive.HasValue)
            {
                if (showActive.Value)
                    items = await (from s in dbContext.SymbolicNames where !s.IsInactive && s.FileSystemId == fileSystemId select s).ToArrayAsync(statusListener.CancellationToken);
                else
                    items = await (from s in dbContext.SymbolicNames where s.IsInactive && s.FileSystemId == fileSystemId select s).ToArrayAsync(statusListener.CancellationToken);
            }
            else
                items = await (from s in dbContext.SymbolicNames where s.FileSystemId == fileSystemId select s).ToArrayAsync(statusListener.CancellationToken);

            return await Dispatcher.InvokeAsync(() =>
            {
                _backingSymbolicNames.Clear();
                if (CurrentItem?.Model.Id == fileSystemId)
                {
                    CurrentItem.SymbolicNameCount = items.Length;
                    foreach (SymbolicName item in items)
                        _backingSymbolicNames.Add(new SymbolicNameItemVM(item));
                    return items.Length;
                }
                return 0;
            }, DispatcherPriority.Background, statusListener.CancellationToken);
        }

        public FileSystemItemDetailViewModel()
        {
            SetValue(SymbolicNamesPropertyKey, new ReadOnlyObservableCollection<SymbolicNameItemVM>(_backingSymbolicNames));
            ThreeStateViewModel viewOptions = new(true);
            SetValue(ViewOptionsPropertyKey, viewOptions);
            viewOptions.ValuePropertyChanged += (s, e) => StartSymbolicNamesLoad(CurrentItem?.Model.Id, e.NewValue as bool?);
            SetValue(NewSymbolicNamePropertyKey, new Commands.RelayCommand(RaiseAddNewSymbolicName));
        }
    }
}
