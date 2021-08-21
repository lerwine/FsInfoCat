using FsInfoCat.Desktop.ViewModel.AsyncOps;
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

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class EditSymbolicNameVM : EditDbEntityVM<SymbolicName>
    {
        private readonly ObservableCollection<FileSystemItemVM> _fileSystemOptions = new();

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

        #region FileSystemOptions Property Members

        private static readonly DependencyPropertyKey FileSystemOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileSystemOptions), typeof(ReadOnlyObservableCollection<FileSystemItemVM>), typeof(EditSymbolicNameVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty FileSystemOptionsProperty = FileSystemOptionsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<FileSystemItemVM> FileSystemOptions => (ReadOnlyObservableCollection<FileSystemItemVM>)GetValue(FileSystemOptionsProperty);

        #endregion

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

        public static readonly DependencyProperty ShowActiveFileSystemsOnlyProperty = DependencyProperty.Register(nameof(ShowActiveFileSystemsOnly), typeof(bool), typeof(EditSymbolicNameVM),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditSymbolicNameVM).OnShowActiveFileSystemsOnlyPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool ShowActiveFileSystemsOnly
        {
            get => (bool)GetValue(ShowActiveFileSystemsOnlyProperty);
            set => SetValue(ShowActiveFileSystemsOnlyProperty, value);
        }

        protected virtual void OnShowActiveFileSystemsOnlyPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                ShowInactiveFileSystemsOnly = ShowAllFileSystems = false;
                Guid id = SelectedFileSystem?.Model?.Id ?? Guid.Empty;
                Task<FileSystem[]> task = OpAggregate.FromAsync("Loading file systems", "Connecting to the database",
                    new FileSystemLookupOptions(SelectedFileSystem, id, true), LoadFileSystemsAsync);
                task.ContinueWith(OnFileSystemsLoaded, id);
            }
        }

        public static readonly DependencyProperty ShowInactiveFileSystemsOnlyProperty = DependencyProperty.Register(nameof(ShowInactiveFileSystemsOnly), typeof(bool), typeof(EditSymbolicNameVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditSymbolicNameVM).OnShowInactiveFileSystemsOnlyPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool ShowInactiveFileSystemsOnly
        {
            get => (bool)GetValue(ShowInactiveFileSystemsOnlyProperty);
            set => SetValue(ShowInactiveFileSystemsOnlyProperty, value);
        }

        protected virtual void OnShowInactiveFileSystemsOnlyPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                ShowActiveFileSystemsOnly = ShowAllFileSystems = false;
                Guid id = SelectedFileSystem?.Model?.Id ?? Guid.Empty;
                Task<FileSystem[]> task = OpAggregate.FromAsync("Loading file systems", "Connecting to the database",
                    new FileSystemLookupOptions(SelectedFileSystem, id, false), LoadFileSystemsAsync);
                task.ContinueWith(OnFileSystemsLoaded, id);
            }
        }

        public static readonly DependencyProperty ShowAllFileSystemsProperty = DependencyProperty.Register(nameof(ShowAllFileSystems), typeof(bool), typeof(EditSymbolicNameVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditSymbolicNameVM).OnShowAllFileSystemsPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool ShowAllFileSystems
        {
            get => (bool)GetValue(ShowAllFileSystemsProperty);
            set => SetValue(ShowAllFileSystemsProperty, value);
        }

        protected virtual void OnShowAllFileSystemsPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                ShowActiveFileSystemsOnly = ShowInactiveFileSystemsOnly = false;
                Guid id = SelectedFileSystem?.Model?.Id ?? Guid.Empty;
                Task<FileSystem[]> task = OpAggregate.FromAsync("Loading file systems", "Connecting to the database",
                    new FileSystemLookupOptions(SelectedFileSystem, id, null), LoadFileSystemsAsync);
                task.ContinueWith(OnFileSystemsLoaded, id);
            }
        }

        public EditSymbolicNameVM()
        {
            SetValue(FileSystemOptionsPropertyKey, new ReadOnlyObservableCollection<FileSystemItemVM>(_fileSystemOptions));
        }

        private void OnFileSystemsLoaded(Task<FileSystem[]> task, object state)
        {
            if (task.IsCompletedSuccessfully)
                Dispatcher.Invoke(() =>
                {
                    foreach (FileSystem fs in task.Result)
                        _fileSystemOptions.Add(new(fs));
                    Guid id = (Guid)state;
                    SelectedFileSystem = _fileSystemOptions.FirstOrDefault(f => f.Model.Id == id);
                });
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

        private static async Task<FileSystem[]> LoadFileSystemsAsync(FileSystemLookupOptions state, IWindowsStatusListener statusListener)
        {
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            FileSystem[] fileSystems;
            if (state.ShowActiveOnly.HasValue)
            {
                if (state.ShowActiveOnly.Value)
                    fileSystems = await (from fs in dbContext.FileSystems where !fs.IsInactive select fs).ToArrayAsync(statusListener.CancellationToken);
                else
                    fileSystems = await (from fs in dbContext.FileSystems where fs.IsInactive select fs).ToArrayAsync(statusListener.CancellationToken);
            }
            else
                fileSystems = await dbContext.FileSystems.ToArrayAsync();
            Guid id = state.FileSystemId;
            if (!fileSystems.Any(f => f.Id == id))
            {
                if (state.SelectedItem?.Model is not null)
                    return fileSystems.Concat(new FileSystem[] { state.SelectedItem.Model }).ToArray();
                FileSystem item = await dbContext.FileSystems.FindAsync(new object[] { id }, statusListener.CancellationToken);
                if (item is not null)
                    return fileSystems.Concat(new FileSystem[] { item }).ToArray();
            }
            return fileSystems;
        }

        protected override void OnModelPropertyChanged(SymbolicName oldValue, SymbolicName newValue)
        {
            if (newValue is null)
            {
                // TODO: Initialize to default values
                return;
            }
            Name = newValue.Name;
            IsInactive = newValue.IsInactive;
            Notes = newValue.Notes;
            Priority = newValue.Priority;
            Guid id = newValue.FileSystemId;
            Task<FileSystem[]> task = OpAggregate.FromAsync("Loading file systems", "Connecting to the database",
                new FileSystemLookupOptions(null, id, ShowAllFileSystems ? null : ShowActiveFileSystemsOnly), LoadFileSystemsAsync);
            task.ContinueWith(OnFileSystemsLoaded, id);
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
            return true;
        }

        public record FileSystemLookupOptions(FileSystemItemVM SelectedItem, Guid FileSystemId, bool? ShowActiveOnly);
    }
}
