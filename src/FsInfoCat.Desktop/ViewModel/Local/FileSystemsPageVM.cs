using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class FileSystemsPageVM : DbEntityListingPageVM<FileSystem, FileSystemItemVM>
    {
    }
    public class FileSystemItemVM : DbEntityItemVM<FileSystem>
    {
        #region DefaultDriveType

        private static readonly DependencyPropertyKey DefaultDriveTypePropertyKey = DependencyProperty.RegisterReadOnly(nameof(DefaultDriveType), typeof(DriveType?), typeof(FileSystemItemVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty DefaultDriveTypeProperty = DefaultDriveTypePropertyKey.DependencyProperty;

        public DriveType? DefaultDriveType
        {
            get => (DriveType?)GetValue(DefaultDriveTypeProperty);
            private set => SetValue(DefaultDriveTypePropertyKey, value);
        }

        #endregion
        #region DisplayName Property Members

        private static readonly DependencyPropertyKey DisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(DisplayName), typeof(string), typeof(FileSystemItemVM), new PropertyMetadata(""));

        public static readonly DependencyProperty DisplayNameProperty = DisplayNamePropertyKey.DependencyProperty;

        public string DisplayName
        {
            get => GetValue(DisplayNameProperty) as string;
            private set => SetValue(DisplayNamePropertyKey, value);
        }

        #endregion
        #region IsInactive

        private static readonly DependencyPropertyKey IsInactivePropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsInactive), typeof(bool), typeof(FileSystemItemVM),
                new PropertyMetadata(false));

        public static readonly DependencyProperty IsInactiveProperty = IsInactivePropertyKey.DependencyProperty;

        public bool IsInactive
        {
            get => (bool)GetValue(IsInactiveProperty);
            private set => SetValue(IsInactivePropertyKey, value);
        }

        #endregion
        #region MaxNameLength

        private static readonly DependencyPropertyKey MaxNameLengthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MaxNameLength), typeof(uint), typeof(FileSystemItemVM),
                new PropertyMetadata(DbConstants.DbColDefaultValue_MaxNameLength));

        public static readonly DependencyProperty MaxNameLengthProperty = MaxNameLengthPropertyKey.DependencyProperty;

        public uint MaxNameLength
        {
            get => (uint)GetValue(MaxNameLengthProperty);
            private set => SetValue(MaxNameLengthPropertyKey, value);
        }

        #endregion
        #region Notes Property Members

        /// <summary>
        /// Identifies the <see cref="Notes"/> dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey NotesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Notes), typeof(string), typeof(FileSystemItemVM), new PropertyMetadata(""));

        public static readonly DependencyProperty NotesProperty = NotesPropertyKey.DependencyProperty;

        public string Notes
        {
            get => GetValue(NotesProperty) as string;
            private set => SetValue(NotesPropertyKey, value);
        }

        #endregion
        #region ReadOnly

        private static readonly DependencyPropertyKey ReadOnlyPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ReadOnly), typeof(bool), typeof(FileSystemItemVM),
                new PropertyMetadata(false));

        public static readonly DependencyProperty ReadOnlyProperty = ReadOnlyPropertyKey.DependencyProperty;

        /// <summary>
        /// Called when the value of the <see cref="PropertyChangedCallback">PropertyChanged</see> on <see cref="$property$Property"/> is raised.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="$property$"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="$property$"/> property.</param>
        public bool ReadOnly
        {
            get => (bool)GetValue(ReadOnlyProperty);
            private set => SetValue(ReadOnlyPropertyKey, value);
        }

        #endregion
        internal FileSystemItemVM([DisallowNull] FileSystem model)
            : base(model)
        {
            DisplayName = model.DisplayName;
            Notes = model.Notes;
            // TODO: Initialize properties
        }

        /// <summary>
        /// Called when the value of the <see cref="Notes"/> dependency property changes.
        /// </summary>
        /// <param name="propertyName">The previous value of the <see cref="Notes"/>.</param>
        protected override void OnModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FileSystem.DisplayName):
                    Dispatcher.Invoke(() => DisplayName = Model.DisplayName);
                    break;
                case nameof(FileSystem.Notes):
                    Dispatcher.Invoke(() => Notes = Model.Notes);
                    break;
                    // TODO: Check for remainder of properties
            }
        }
    }
    public class EditFileSystemVM : EditDbEntityVM<FileSystem>
    {
        #region DriveTypeOptions Property Members

        private static readonly DependencyPropertyKey DriveTypeOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(DriveTypeOptions),
            typeof(ReadOnlyObservableCollection<DriveType>), typeof(EditFileSystemVM), new PropertyMetadata(null));

        public static readonly DependencyProperty DriveTypeOptionsProperty = DriveTypeOptionsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<DriveType> DriveTypeOptions => (ReadOnlyObservableCollection<DriveType>)GetValue(DriveTypeOptionsProperty);

        #endregion

        public static readonly DependencyProperty SelectedDriveTypeProperty = DependencyProperty.Register(nameof(SelectedDriveType), typeof(DriveType), typeof(EditFileSystemVM),
                new PropertyMetadata(DriveType.Unknown, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditFileSystemVM).OnSelectedDriveTypePropertyChanged((DriveType)e.OldValue, (DriveType)e.NewValue)));

        public DriveType SelectedDriveType
        {
            get => (DriveType)GetValue(SelectedDriveTypeProperty);
            set => SetValue(SelectedDriveTypeProperty, value);
        }

        protected virtual void OnSelectedDriveTypePropertyChanged(DriveType oldValue, DriveType newValue)
        {
            // TODO: Implement OnSelectedDriveTypePropertyChanged Logic
        }

        public static readonly DependencyProperty HasDefaultDriveTypeProperty = DependencyProperty.Register(nameof(HasDefaultDriveType), typeof(bool), typeof(EditFileSystemVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditFileSystemVM).OnHasDefaultDriveTypePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool HasDefaultDriveType
        {
            get => (bool)GetValue(HasDefaultDriveTypeProperty);
            set => SetValue(HasDefaultDriveTypeProperty, value);
        }

        protected virtual void OnHasDefaultDriveTypePropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnHasDefaultDriveTypePropertyChanged Logic
        }

        public static readonly DependencyProperty IsInactiveProperty = DependencyProperty.Register(nameof(IsInactive), typeof(bool), typeof(EditFileSystemVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditFileSystemVM).OnIsInactivePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool IsInactive
        {
            get => (bool)GetValue(IsInactiveProperty);
            set => SetValue(IsInactiveProperty, value);
        }

        protected virtual void OnIsInactivePropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnIsInactivePropertyChanged Logic
        }

        public static readonly DependencyProperty MaxNameLengthProperty = DependencyProperty.Register(nameof(MaxNameLength), typeof(long), typeof(EditFileSystemVM),
                new PropertyMetadata((long)DbConstants.DbColDefaultValue_MaxNameLength, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditFileSystemVM).OnMaxNameLengthPropertyChanged((long)e.OldValue, (long)e.NewValue)));

        public long MaxNameLength
        {
            get => (long)GetValue(MaxNameLengthProperty);
            set => SetValue(MaxNameLengthProperty, value);
        }

        protected virtual void OnMaxNameLengthPropertyChanged(long oldValue, long newValue)
        {
            // TODO: Implement OnMaxNameLengthPropertyChanged Logic
        }

        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register(nameof(ReadOnly), typeof(bool), typeof(EditFileSystemVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditFileSystemVM).OnReadOnlyPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool ReadOnly
        {
            get => (bool)GetValue(ReadOnlyProperty);
            set => SetValue(ReadOnlyProperty, value);
        }

        protected virtual void OnReadOnlyPropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnReadOnlyPropertyChanged Logic
        }

        private static readonly DependencyPropertyKey VolumesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Volumes), typeof(ObservableCollection<VolumeItemVM>), typeof(EditFileSystemVM),
                new PropertyMetadata(new ObservableCollection<VolumeItemVM>()));

        public static readonly DependencyProperty VolumesProperty = VolumesPropertyKey.DependencyProperty;

        public ObservableCollection<VolumeItemVM> Volumes
        {
            get => (ObservableCollection<VolumeItemVM>)this.GetValue(VolumesProperty);
            private set => this.SetValue(VolumesPropertyKey, value);
        }

        private static readonly DependencyPropertyKey SymbolicNamesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SymbolicNames), typeof(ObservableCollection<SymbolicNameItemVM>), typeof(EditFileSystemVM),
                new PropertyMetadata(new ObservableCollection<SymbolicNameItemVM>()));

        public static readonly DependencyProperty SymbolicNamesProperty = SymbolicNamesPropertyKey.DependencyProperty;

        public ObservableCollection<SymbolicNameItemVM> SymbolicNames
        {
            get => (ObservableCollection<SymbolicNameItemVM>)this.GetValue(SymbolicNamesProperty);
            private set => this.SetValue(SymbolicNamesPropertyKey, value);
        }

        #region Background Operation Properties

        //private AsyncOps.AsyncFuncOpViewModel<ModelViewModel, FileSystem> SaveChangesAsync(FileSystem fileSystem) =>
        //    OpAggregate.FromAsync("Saving Changes", "Connecting to database", new(fileSystem, this), SaveChangesOpMgr, SaveChangesAsync);

        #region SaveChangesOpMgr Property Members

        private static async Task<FileSystem> SaveChangesAsync(ModelViewModel state,
            AsyncOps.AsyncFuncOpViewModel<ModelViewModel, FileSystem>.StatusListenerImpl statusListener)
        {
            EditFileSystemVM vm = state.ViewModel ?? throw new ArgumentException($"{nameof(state.ViewModel)} cannot be null.", nameof(state));
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>();
            EntityEntry<FileSystem> entry;
            if (state.Entity is null)
            {
                FileSystem model = vm.Dispatcher.Invoke(() => new FileSystem()
                {
                    Id = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    DisplayName = vm.DisplayName,
                    Notes = vm.Notes
                    // TODO: Initialize model
                });
                model.ModifiedOn = model.CreatedOn;
                entry = dbContext.FileSystems.Add(model);
            }
            else
                entry = dbContext.Entry(state.Entity);
            vm.Dispatcher.Invoke(() =>
            {
                FileSystem model = entry.Entity;
                if (entry.State != EntityState.Added)
                {
                    model.ModifiedOn = DateTime.Now;
                    model.DisplayName = vm.DisplayName;
                    model.Notes = vm.Notes;
                    // TODO: Update model
                }
                // TODO: Update model
            });
            try
            {
                await dbContext.SaveChangesAsync(true, statusListener.CancellationToken);
                if (entry.State != EntityState.Unchanged)
                    throw new InvalidOperationException("Failed to save changes to the database.");
            }
            catch
            {
                if (state.Entity is not null)
                    await entry.ReloadAsync(statusListener.CancellationToken);
                throw;
            }
            return entry.Entity;
        }

        #endregion

        #endregion
        
        #region Other Property Members

        #region DisplayName Property Members

        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(nameof(DisplayName), typeof(string), typeof(EditFileSystemVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditFileSystemVM).OnDisplayNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string DisplayName
        {
            get => GetValue(DisplayNameProperty) as string;
            set => SetValue(DisplayNameProperty, value);
        }

        protected virtual void OnDisplayNamePropertyChanged(string oldValue, string newValue)
        {
            ChangeTracker.SetChangeState(nameof(DisplayName), (newValue = newValue.AsWsNormalizedOrEmpty()) == Model?.DisplayName);
            if (newValue.Length == 0)
                Validation.SetErrorMessage(nameof(DisplayName), FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired);
            else if (newValue.Length > DbConstants.DbColMaxLen_LongName)
                Validation.SetErrorMessage(nameof(DisplayName), FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength);
            else
                Validation.ClearErrorMessages(nameof(DisplayName));
        }

        #endregion

        #region Notes Property Members

        public static readonly DependencyProperty NotesProperty = DependencyProperty.Register(nameof(Notes), typeof(string), typeof(EditFileSystemVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditFileSystemVM).OnNotesPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string Notes
        {
            get => GetValue(NotesProperty) as string;
            set => SetValue(NotesProperty, value);
        }

        protected virtual void OnNotesPropertyChanged(string oldValue, string newValue)
        {
            ChangeTracker.SetChangeState(nameof(Notes), Model?.Notes != newValue);
        }

        #endregion

        #endregion

        public EditFileSystemVM()
        {
            SetValue(DriveTypeOptionsPropertyKey, new ReadOnlyObservableCollection<DriveType>(new ObservableCollection<DriveType>(Enum.GetValues<DriveType>())));
        }

        protected override FileSystem InitializeNewModel() => new FileSystem()
        {
            Id = Guid.NewGuid(),
            CreatedOn = DateTime.Now
        };

        protected override void Initialize(FileSystem model, EntityState state)
        {
            DisplayName = model.DisplayName;
            Notes = model.Notes;
            SelectedDriveType = model.DefaultDriveType ?? DriveType.Unknown;
            HasDefaultDriveType = model.DefaultDriveType.HasValue;
            Volumes = model.Volumes;
            IsInactive = model.IsInactive;
            MaxNameLength = model.MaxNameLength;
            ReadOnly = model.ReadOnly;
            SymbolicNames = model.SymbolicNames;
            base.Initialize(model, state);
        }

        protected override DbSet<FileSystem> GetDbSet(LocalDbContext dbContext) => dbContext.FileSystems;

        protected override void UpdateModelForSave(FileSystem model, bool isNew)
        {
            model.DisplayName = DisplayName;
            model.Notes = Notes;
            model.DefaultDriveType = DefaultDriveType;
            throw new NotImplementedException();
        }
    }
}
