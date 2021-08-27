using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    /// <summary>
    /// View Model for <see cref="View.Local.FileSystemDetailUserControl"/> and items in the <see cref=""/>.
    /// </summary>
    public class FileSystemItemDetailViewModel : DbEntityItemDetailViewModel<FileSystemListItem, FileSystemItemVM>
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
        #region ShowViewOptions Command Property Members

        private static readonly DependencyPropertyKey ShowViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ShowViewOptions),
            typeof(Commands.RelayCommand), typeof(FileSystemItemDetailViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ShowViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowViewOptionsProperty = ShowViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ShowViewOptions => (Commands.RelayCommand)GetValue(ShowViewOptionsProperty);

        private void OnShowViewOptions(object parameter)
        {
            // TODO: Implement OnShowViewOptions Logic
        }

        #endregion
        #region IsEditingViewOptions Property Members

        private static readonly DependencyPropertyKey IsEditingViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsEditingViewOptions), typeof(bool), typeof(FileSystemItemDetailViewModel),
                new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsEditingViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsEditingViewOptionsProperty = IsEditingViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool IsEditingViewOptions { get => (bool)GetValue(IsEditingViewOptionsProperty); private set => SetValue(IsEditingViewOptionsPropertyKey, value); }

        #endregion
        #region ViewOptionsOkClick Command Property Members

        private static readonly DependencyPropertyKey ViewOptionsOkClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptionsOkClick),
            typeof(Commands.RelayCommand), typeof(FileSystemItemDetailViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewOptionsOkClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionsOkClickProperty = ViewOptionsOkClickPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ViewOptionsOkClick => (Commands.RelayCommand)GetValue(ViewOptionsOkClickProperty);

        private void OnViewOptionsOkClick(object parameter)
        {
            // TODO: Implement OnViewOptionsOkClick Logic
        }

        #endregion
        #region ViewOptionCancelClick Command Property Members

        private static readonly DependencyPropertyKey ViewOptionCancelClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptionCancelClick),
            typeof(Commands.RelayCommand), typeof(FileSystemItemDetailViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewOptionCancelClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionCancelClickProperty = ViewOptionCancelClickPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ViewOptionCancelClick => (Commands.RelayCommand)GetValue(ViewOptionCancelClickProperty);

        private void OnViewOptionCancelClick(object parameter)
        {
            // TODO: Implement OnViewOptionCancelClick Logic
        }

        #endregion
        #region EditingViewOptions Property Members

        private static readonly DependencyPropertyKey EditingViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EditingViewOptions), typeof(ThreeStateViewModel), typeof(FileSystemItemDetailViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="EditingViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditingViewOptionsProperty = EditingViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ThreeStateViewModel EditingViewOptions => (ThreeStateViewModel)GetValue(EditingViewOptionsProperty);

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

        private readonly ObservableCollection<SymbolicNameWithFileSystemItemVM> _backingSymbolicNames = new();

        private static readonly DependencyPropertyKey SymbolicNamesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SymbolicNames), typeof(ReadOnlyObservableCollection<SymbolicNameWithFileSystemItemVM>), typeof(FileSystemItemDetailViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SymbolicNames"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SymbolicNamesProperty = SymbolicNamesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the symbolic names.
        /// </summary>
        /// <value>The symbolic names for the <see cref="DbEntityItemDetailViewModel{FileSystem, FileSystemItemVM}.CurrentItem"/>.</value>
        public ReadOnlyObservableCollection<SymbolicNameWithFileSystemItemVM> SymbolicNames => (ReadOnlyObservableCollection<SymbolicNameWithFileSystemItemVM>)GetValue(SymbolicNamesProperty);

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
            {
                IWindowsAsyncJobFactoryService service = Services.ServiceProvider.GetRequiredService<IWindowsAsyncJobFactoryService>();
                service.RunAsync("", "", (fileSystemId.Value, showActive), LoadSymbolicNamesAsync);
            }
        }

        private async Task<int> LoadSymbolicNamesAsync((Guid fileSystemId, bool? showActive) state, IWindowsStatusListener statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            SymbolicNameListItem[] items;
            if (state.showActive.HasValue)
            {
                if (state.showActive.Value)
                    items = await (from s in dbContext.SymbolicNameListing where !s.IsInactive && s.FileSystemId == state.fileSystemId select s).ToArrayAsync(statusListener.CancellationToken);
                else
                    items = await (from s in dbContext.SymbolicNameListing where s.IsInactive && s.FileSystemId == state.fileSystemId select s).ToArrayAsync(statusListener.CancellationToken);
            }
            else
                items = await (from s in dbContext.SymbolicNameListing where s.FileSystemId == state.fileSystemId select s).ToArrayAsync(statusListener.CancellationToken);

            return await Dispatcher.InvokeAsync(() =>
            {
                _backingSymbolicNames.Clear();
                if (CurrentItem?.Model.Id == state.fileSystemId)
                {
                    CurrentItem.SymbolicNameCount = items.Length;
                    foreach (SymbolicNameListItem item in items)
                        _backingSymbolicNames.Add(new SymbolicNameWithFileSystemItemVM(item));
                    return items.Length;
                }
                return 0;
            }, DispatcherPriority.Background, statusListener.CancellationToken);
        }

        public FileSystemItemDetailViewModel()
        {
            SetValue(SymbolicNamesPropertyKey, new ReadOnlyObservableCollection<SymbolicNameWithFileSystemItemVM>(_backingSymbolicNames));
            ThreeStateViewModel viewOptions = new(true);
            SetValue(ViewOptionsPropertyKey, viewOptions);
            SetValue(EditingViewOptionsPropertyKey, new ThreeStateViewModel());
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            SetValue(NewSymbolicNamePropertyKey, new Commands.RelayCommand(RaiseAddNewSymbolicName));
            SetValue(ShowViewOptionsPropertyKey, new Commands.RelayCommand(() => IsEditingViewOptions = true));
            SetValue(ViewOptionsOkClickPropertyKey, new Commands.RelayCommand(() =>
            {
                IsEditingViewOptions = false;
                ViewOptions.Value = EditingViewOptions.Value;
            }));
            SetValue(ViewOptionCancelClickPropertyKey, new Commands.RelayCommand(() =>
            {
                EditingViewOptions.Value = ViewOptions.Value;
                IsEditingViewOptions = false;
            }));
            viewOptions.ValuePropertyChanged += (s, e) => StartSymbolicNamesLoad(CurrentItem?.Model.Id, e.NewValue as bool?);
        }
    }
}
