using FsInfoCat.Desktop.ViewModel.AsyncOps;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    /// <summary>
    /// View model for <see cref="View.Local.EditSymbolicNameWindow"/>
    /// </summary>
    public class EditSymbolicNameVM : EditDbEntityVM<SymbolicName>
    {
        #region Name Property Members

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(nameof(Name), typeof(string), typeof(EditSymbolicNameVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditSymbolicNameVM).OnNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string Name
        {
            get => GetValue(NameProperty) as string;
            set => SetValue(NameProperty, value);
        }

        protected virtual void OnNamePropertyChanged(string oldValue, string newValue)
        {

            if ((newValue = newValue.AsWsNormalizedOrEmpty()).Length == 0)
                Validation.SetErrorMessage(nameof(Name), "Name is required.");
            else if (newValue.Length > DbConstants.DbColMaxLen_SimpleName)
                Validation.SetErrorMessage(nameof(Name), $"Name cannot be more than {DbConstants.DbColMaxLen_SimpleName} characters.");
            else
                Validation.ClearErrorMessages(nameof(Name));
            ChangeTracker.SetChangeState(nameof(Name), newValue != Model?.Name);
        }

        #endregion
        #region Priority Property Members

        public static readonly DependencyProperty PriorityProperty = DependencyProperty.Register(nameof(Priority), typeof(int), typeof(EditSymbolicNameVM),
                new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditSymbolicNameVM).OnPriorityPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        public int Priority
        {
            get => (int)GetValue(PriorityProperty);
            set => SetValue(PriorityProperty, value);
        }

        protected virtual void OnPriorityPropertyChanged(int oldValue, int newValue)
        {
            ChangeTracker.SetChangeState(nameof(Priority), newValue != Model?.Priority);
        }

        #endregion
        #region FileSystemOptions Property Members

        private readonly ObservableCollection<FileSystemItemVM> _backingFileSystemOptions = new();

        private static readonly DependencyPropertyKey FileSystemOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileSystemOptions), typeof(ReadOnlyObservableCollection<FileSystemItemVM>), typeof(EditSymbolicNameVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty FileSystemOptionsProperty = FileSystemOptionsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<FileSystemItemVM> FileSystemOptions => (ReadOnlyObservableCollection<FileSystemItemVM>)GetValue(FileSystemOptionsProperty);

        #endregion
        #region PickFromActiveFileSystems Property Members

        private int _ignoreFileSystemOptionsChange = 0;

        private static readonly DependencyPropertyKey PickFromActiveFileSystemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PickFromActiveFileSystems), typeof(ThreeStateViewModel), typeof(EditSymbolicNameVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="PickFromActiveFileSystems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PickFromActiveFileSystemsProperty = PickFromActiveFileSystemsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ThreeStateViewModel PickFromActiveFileSystems => (ThreeStateViewModel)GetValue(PickFromActiveFileSystemsProperty);

        private void PickFromActiveFileSystems_ValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_ignoreFileSystemOptionsChange > 0)
                return;
            bool? newvalue = e.NewValue as bool?;
            FileSystemItemVM current = SelectedFileSystem;
            if (newvalue.HasValue && e.OldValue is null)
            {
                bool isActive = newvalue.Value;
                foreach (FileSystemItemVM item in _backingFileSystemOptions.Where(item => item.IsInactive != isActive).ToArray())
                    _backingFileSystemOptions.Remove(item);
                if (current?.IsInactive == isActive)
                    SelectedFileSystem = null;
                return;
            }
            IWindowsAsyncJobFactoryService service = Services.ServiceProvider.GetRequiredService<IWindowsAsyncJobFactoryService>();
            service.RunAsync("Loading data", "Getting file system options", PickFromActiveFileSystems.Value, ReloadFileSystemsAsync)
                .ContinueWith(task => Dispatcher.Invoke(() =>
            {
                _backingFileSystemOptions.Clear();
                foreach (FileSystemListItem entity in task.Result)
                    _backingFileSystemOptions.Add(new(entity));
                if (current is null)
                    return;
                if (newvalue.HasValue && current?.IsInactive == newvalue.Value)
                    SelectedFileSystem = null;
                else
                {
                    Guid id = current.Model.Id;
                    SelectedFileSystem = _backingFileSystemOptions.FirstOrDefault(v => v.Model.Id == id);
                }
            }), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        #endregion
        #region SelectedFileSystem Property Members

        public static readonly DependencyProperty SelectedFileSystemProperty = DependencyProperty.Register(nameof(SelectedFileSystem), typeof(FileSystemItemVM), typeof(EditSymbolicNameVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditSymbolicNameVM).OnSelectedFileSystemPropertyChanged((FileSystemItemVM)e.OldValue, (FileSystemItemVM)e.NewValue)));

        public FileSystemItemVM SelectedFileSystem
        {
            get => (FileSystemItemVM)GetValue(SelectedFileSystemProperty);
            set => SetValue(SelectedFileSystemProperty, value);
        }

        protected virtual void OnSelectedFileSystemPropertyChanged(FileSystemItemVM oldValue, FileSystemItemVM newValue)
        {
            ChangeTracker.SetChangeState(nameof(SelectedFileSystem), newValue?.Model.Id != Model?.FileSystemId);
        }

        #endregion
        #region IsInactive Property Members

        public static readonly DependencyProperty IsInactiveProperty = DependencyProperty.Register(nameof(IsInactive), typeof(bool), typeof(EditSymbolicNameVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditSymbolicNameVM).OnIsInactivePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool IsInactive
        {
            get => (bool)GetValue(IsInactiveProperty);
            set => SetValue(IsInactiveProperty, value);
        }

        protected virtual void OnIsInactivePropertyChanged(bool oldValue, bool newValue)
        {
            ChangeTracker.SetChangeState(nameof(IsInactive), newValue != Model?.IsInactive);
        }

        #endregion
        #region Notes Property Members

        public static readonly DependencyProperty NotesProperty = DependencyProperty.Register(nameof(Notes), typeof(string), typeof(EditSymbolicNameVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditSymbolicNameVM).OnNotesPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string Notes
        {
            get => GetValue(NotesProperty) as string;
            set => SetValue(NotesProperty, value);
        }

        protected virtual void OnNotesPropertyChanged(string oldValue, string newValue)
        {
            ChangeTracker.SetChangeState(nameof(Notes), newValue.EmptyIfNullOrWhiteSpace() != Model?.Notes);
        }

        #endregion

        public EditSymbolicNameVM()
        {
            ThreeStateViewModel fileSystemDisplayOptions = new(true);
            SetValue(FileSystemOptionsPropertyKey, new ReadOnlyObservableCollection<FileSystemItemVM>(_backingFileSystemOptions));
            SetValue(PickFromActiveFileSystemsPropertyKey, fileSystemDisplayOptions);
            fileSystemDisplayOptions.ValuePropertyChanged += PickFromActiveFileSystems_ValuePropertyChanged;
        }

        protected override DbSet<SymbolicName> GetDbSet([DisallowNull] LocalDbContext dbContext) => dbContext.SymbolicNames;

        protected override Task<bool> OnSaveChangesAsync([DisallowNull] EntityEntry<SymbolicName> entry, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener, bool force = false)
        {
            string name = entry.Entity.Name;
            SymbolicName existing = (from sn in dbContext.SymbolicNames where sn.Name == name select sn).FirstOrDefaultAsync(statusListener.CancellationToken).Result;
            // TODO: Determine whether it's better to throw AsyncOperationFailureException or to Dispatcher.Invoke and return boolean
            if (existing is not null && (entry.State == EntityState.Added || existing.Id != entry.Entity.Id))
                throw new AsyncOperationFailureException("Name already used", "That name is already being used.");
            return base.OnSaveChangesAsync(entry, dbContext, statusListener, force);
        }

        private static async Task<ICollection<FileSystemListItem>> ReloadFileSystemsAsync(bool? selectActive, IWindowsStatusListener statusListener)
        {
            using IServiceScope scope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            if (selectActive.HasValue)
            {
                if (selectActive.Value)
                    return await (from f in dbContext.FileSystemListing where f.IsInactive == false select f).ToArrayAsync(statusListener.CancellationToken);
                return await (from f in dbContext.FileSystemListing where f.IsInactive == true select f).ToArrayAsync(statusListener.CancellationToken);
            }
            return await dbContext.FileSystemListing.ToArrayAsync(statusListener.CancellationToken);
        }

        protected override void OnModelPropertyChanged(SymbolicName oldValue, SymbolicName newValue)
        {
            FileSystemListItem previouslySelected = (oldValue is null) ? null : Dispatcher.CheckInvoke(() =>
            {
                Guid id = oldValue.FileSystemId;
                return _backingFileSystemOptions.FirstOrDefault(o => o.Model?.Id == id)?.Model;
            });
            if (newValue is null)
            {
                SetSelectedFileSystemAsync(null, previouslySelected);
                Notes = Name = "";
                IsInactive = false;
                Priority = 0;
                return;
            }
            SetSelectedFileSystemAsync(newValue.FileSystemId, previouslySelected);
            Name = newValue.Name;
            IsInactive = newValue.IsInactive;
            Notes = newValue.Notes;
            Priority = newValue.Priority;
        }

        private void SetSelectedFileSystemAsync(Guid? fileSystemId, FileSystemListItem previouslySelected)
        {
            Task<ICollection<FileSystemListItem>> result;
            if (fileSystemId.HasValue)
            {
                Guid id = fileSystemId.Value;
                if (Dispatcher.CheckInvoke(() =>
                {
                    if (SelectedFileSystem?.Model?.Id == id)
                        return true;
                    FileSystemItemVM current = _backingFileSystemOptions.FirstOrDefault(o => o.Model?.Id == id);
                    if (current is null)
                        return false;
                    SelectedFileSystem = current;
                    return true;
                }))
                    return;

                bool? displayOptions;
                Interlocked.Increment(ref _ignoreFileSystemOptionsChange);
                try
                {
                    displayOptions = Dispatcher.CheckInvoke(() =>
                    {
                        if (previouslySelected is not null)
                        {
                            if (previouslySelected.IsInactive)
                            {
                                if (PickFromActiveFileSystems.IsTrue)
                                    PickFromActiveFileSystems.IsNull = true;
                            }
                            else if (PickFromActiveFileSystems.IsFalse)
                                PickFromActiveFileSystems.IsTrue = true;
                        }
                        return PickFromActiveFileSystems.Value;
                    });
                }
                finally { Interlocked.Decrement(ref _ignoreFileSystemOptionsChange); }
                IWindowsAsyncJobFactoryService service = Services.ServiceProvider.GetRequiredService<IWindowsAsyncJobFactoryService>();
                result = service.RunAsync("Loading data", "Getting file system options", displayOptions, ReloadFileSystemsAsync);
            }
            else
            {
                if (Dispatcher.CheckInvoke(() =>
                {
                    SelectedFileSystem = null;
                    return _backingFileSystemOptions.Count > 0;
                }))
                    return;
                IWindowsAsyncJobFactoryService service = Services.ServiceProvider.GetRequiredService<IWindowsAsyncJobFactoryService>();
                result = service.RunAsync("Loading data", "Getting file system options", Dispatcher.CheckInvoke(() => PickFromActiveFileSystems.Value),
                    ReloadFileSystemsAsync);
            }

            result.ContinueWith(task => Dispatcher.Invoke(() =>
            {
                _backingFileSystemOptions.Clear();
                foreach (FileSystemListItem entity in task.Result)
                    _backingFileSystemOptions.Add(new(entity));
                if (fileSystemId.HasValue)
                {
                    Guid id = fileSystemId.Value;
                    FileSystemItemVM vm = _backingFileSystemOptions.FirstOrDefault(v => v.Model.Id == id);
                    if (vm is null)
                    {
                        if (previouslySelected is null)
                            return null;
                        vm = new(previouslySelected);
                        _backingFileSystemOptions.Add(vm);
                    }
                    SelectedFileSystem = vm;
                    return vm;
                }
                return null;
            }), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        protected override bool OnBeforeSave()
        {
            SymbolicName model = Model;
            if (model is null)
                return false;
            model.Name = Name.AsWsNormalizedOrEmpty();
            model.IsInactive = IsInactive;
            model.Notes = Notes.EmptyIfNullOrWhiteSpace();
            model.Priority = Priority;
            model.FileSystemId = SelectedFileSystem.Model.Id;
            return true;
        }
    }
}
