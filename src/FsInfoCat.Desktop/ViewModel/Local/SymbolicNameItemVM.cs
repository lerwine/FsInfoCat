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
    public class SymbolicNamePageVM : DbEntityListingPageVM<SymbolicName, SymbolicNameItemVM>
    {
    }
    public class SymbolicNameItemVM : DbEntityItemVM<SymbolicName>
    {
        #region Name Property Members

        private static readonly DependencyPropertyKey NamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Name), typeof(string), typeof(SymbolicNameItemVM), new PropertyMetadata(""));

        public static readonly DependencyProperty NameProperty = NamePropertyKey.DependencyProperty;

        public string Name
        {
            get => GetValue(NameProperty) as string;
            private set => SetValue(NamePropertyKey, value ?? "");
        }

        #endregion
        #region Notes Property Members

        private static readonly DependencyPropertyKey NotesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Notes), typeof(string), typeof(SymbolicNameItemVM), new PropertyMetadata(""));

        public static readonly DependencyProperty NotesProperty = NotesPropertyKey.DependencyProperty;

        public string Notes
        {
            get => GetValue(NotesProperty) as string;
            private set => SetValue(NotesPropertyKey, value ?? "");
        }

        #endregion

        private static readonly DependencyPropertyKey IsInactivePropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsInactive), typeof(bool), typeof(SymbolicNameItemVM),
                new PropertyMetadata(false));

        public static readonly DependencyProperty IsInactiveProperty = IsInactivePropertyKey.DependencyProperty;

        public bool IsInactive
        {
            get => (bool)GetValue(IsInactiveProperty);
            private set => SetValue(IsInactivePropertyKey, value);
        }

        private static readonly DependencyPropertyKey PriorityPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Priority), typeof(int), typeof(SymbolicNameItemVM),
                new PropertyMetadata(0));

        public static readonly DependencyProperty PriorityProperty = PriorityPropertyKey.DependencyProperty;

        public int Priority
        {
            get => (int)GetValue(PriorityProperty);
            private set => SetValue(PriorityPropertyKey, value);
        }

        private static readonly DependencyPropertyKey FileSystemNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileSystemName), typeof(string), typeof(SymbolicNameItemVM), new PropertyMetadata(""));

        public static readonly DependencyProperty FileSystemNameProperty = FileSystemNamePropertyKey.DependencyProperty;

        public string FileSystemName
        {
            get => GetValue(FileSystemNameProperty) as string;
            private set => SetValue(FileSystemNamePropertyKey, value ?? "");
        }

        public SymbolicNameItemVM([DisallowNull] SymbolicName model) : base(model)
        {
            Name = model.Name;
            IsInactive = model.IsInactive;
            Notes = model.Notes;
            Priority = model.Priority;
            FileSystemName = model.FileSystem?.DisplayName;
        }

        protected override void OnModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(SymbolicName.Name):
                    Name = Model.Name;
                    break;
                case nameof(SymbolicName.IsInactive):
                    IsInactive = Model.IsInactive;
                    break;
                case nameof(SymbolicName.Notes):
                    Notes = Model.Notes;
                    break;
                case nameof(SymbolicName.Priority):
                    Priority = Model.Priority;
                    break;
                case nameof(SymbolicName.FileSystem):
                    FileSystemName = Model.FileSystem?.DisplayName;
                    break;
            }
        }
    }
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
                AsyncFuncOpViewModel<FileSystemLookupOptions, FileSystem[]> op = OpAggregate.FromAsync("Loading file systems", "Connecting to the database",
                    new FileSystemLookupOptions(SelectedFileSystem, id, true), FileSystemsLoader, LoadFileSystemsAsync);
                op.GetTask().ContinueWith(OnFileSystemsLoaded, id);
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
                AsyncFuncOpViewModel<FileSystemLookupOptions, FileSystem[]> op = OpAggregate.FromAsync("Loading file systems", "Connecting to the database",
                    new FileSystemLookupOptions(SelectedFileSystem, id, false), FileSystemsLoader, LoadFileSystemsAsync);
                op.GetTask().ContinueWith(OnFileSystemsLoaded, id);
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
                AsyncFuncOpViewModel<FileSystemLookupOptions, FileSystem[]> op = OpAggregate.FromAsync("Loading file systems", "Connecting to the database",
                    new FileSystemLookupOptions(SelectedFileSystem, id, null), FileSystemsLoader, LoadFileSystemsAsync);
                op.GetTask().ContinueWith(OnFileSystemsLoaded, id);
            }
        }

        private static readonly DependencyPropertyKey FileSystemsLoaderPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileSystemsLoader), typeof(AsyncOpResultManagerViewModel<FileSystemLookupOptions, FileSystem[]>), typeof(EditSymbolicNameVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty FileSystemsLoaderProperty = FileSystemsLoaderPropertyKey.DependencyProperty;

        public AsyncOpResultManagerViewModel<FileSystemLookupOptions, FileSystem[]> FileSystemsLoader => (AsyncOpResultManagerViewModel<FileSystemLookupOptions, FileSystem[]>)GetValue(FileSystemsLoaderProperty);

        public EditSymbolicNameVM()
        {
            SetValue(FileSystemOptionsPropertyKey, new ReadOnlyObservableCollection<FileSystemItemVM>(_fileSystemOptions));
            SetValue(FileSystemsLoaderPropertyKey, new AsyncOpResultManagerViewModel<FileSystemLookupOptions, FileSystem[]>());
        }

        protected override void Initialize(SymbolicName model, EntityState state)
        {
            base.Initialize(model, state);
            Name = model.Name;
            IsInactive = model.IsInactive;
            Notes = model.Notes;
            Priority = model.Priority;
            Guid id = model.FileSystemId;
            AsyncFuncOpViewModel<FileSystemLookupOptions, FileSystem[]> op = OpAggregate.FromAsync("Loading file systems", "Connecting to the database",
                new FileSystemLookupOptions(null, id, ShowAllFileSystems ? null : ShowActiveFileSystemsOnly), FileSystemsLoader, LoadFileSystemsAsync);
            op.GetTask().ContinueWith(OnFileSystemsLoaded, id);
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

        protected override DbSet<SymbolicName> GetDbSet(LocalDbContext dbContext) => dbContext.SymbolicNames;

        protected override SymbolicName InitializeNewModel() => new SymbolicName()
        {
            Id = Guid.NewGuid(),
            CreatedOn = DateTime.Now
        };

        protected override void UpdateModelForSave(SymbolicName model, bool isNew)
        {
            model.Name = Name.AsWsNormalizedOrEmpty();
            model.IsInactive = IsInactive;
            model.Notes = Notes.EmptyIfNullOrWhiteSpace();
            model.Priority = Priority;
        }

        protected override void OnSavingModel(EntityEntry<SymbolicName> entityEntry, LocalDbContext dbContext, IStatusListener<ModelViewModel> statusListener)
        {
            string name = entityEntry.Entity.Name;
            SymbolicName existing = (from sn in dbContext.SymbolicNames where sn.Name == name select sn).FirstOrDefaultAsync(statusListener.CancellationToken).Result;
            if (existing is not null && (entityEntry.State == EntityState.Added || existing.Id != entityEntry.Entity.Id))
                throw new AsyncOperationFailureException("Name already used", "That name is already being used.");
            base.OnSavingModel(entityEntry, dbContext, statusListener);
        }

        private static async Task<FileSystem[]> LoadFileSystemsAsync(FileSystemLookupOptions state, IStatusListener<FileSystemLookupOptions> statusListener)
        {
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>();
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

        public record FileSystemLookupOptions(FileSystemItemVM SelectedItem, Guid FileSystemId, bool? ShowActiveOnly);
    }
}
