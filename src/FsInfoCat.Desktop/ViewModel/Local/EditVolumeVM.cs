using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class EditVolumeVM : EditDbEntityVM<Volume>, IHasSubdirectoryEntity
    {
        #region DisplayName Property Members

        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(nameof(DisplayName), typeof(string), typeof(EditVolumeVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditVolumeVM).OnDisplayNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

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

        public static readonly DependencyProperty NotesProperty = DependencyProperty.Register(nameof(Notes), typeof(string), typeof(EditVolumeVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditVolumeVM).OnNotesPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string Notes
        {
            get => GetValue(NotesProperty) as string;
            set => SetValue(NotesProperty, value);
        }

        protected virtual void OnNotesPropertyChanged(string oldValue, string newValue)
        {
            ChangeTracker.SetChangeState(nameof(Notes), Model?.Notes != newValue.EmptyIfNullOrWhiteSpace());
        }

        #endregion
        #region VolumeIdentifierTypeOptions Property Members

        private static readonly DependencyPropertyKey VolumeIdentifierTypeOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeIdentifierTypeOptions), typeof(ReadOnlyObservableCollection<VolumeIdType>), typeof(EditVolumeVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty VolumeIdentifierTypeOptionsProperty = VolumeIdentifierTypeOptionsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<VolumeIdType> VolumeIdentifierTypeOptions => (ReadOnlyObservableCollection<VolumeIdType>)GetValue(VolumeIdentifierTypeOptionsProperty);

        #endregion
        #region SelectedVolumeIdType Property Members

        public static readonly DependencyProperty SelectedVolumeIdTypeProperty = DependencyProperty.Register(nameof(SelectedVolumeIdType), typeof(VolumeIdType), typeof(EditVolumeVM),
                new PropertyMetadata(VolumeIdType.VSN, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditVolumeVM).OnSelectedVolumeIdTypePropertyChanged((VolumeIdType)e.OldValue, (VolumeIdType)e.NewValue)));

        public VolumeIdType SelectedVolumeIdType
        {
            get => (VolumeIdType)GetValue(SelectedVolumeIdTypeProperty);
            set => SetValue(SelectedVolumeIdTypeProperty, value);
        }

        protected virtual void OnSelectedVolumeIdTypePropertyChanged(VolumeIdType oldValue, VolumeIdType newValue)
        {
            string oldText = VolumeId;
            string newText = VolumeId?.EmptyIfNullOrWhiteSpace();
            if (newText.Length == 0)
                Validation.SetErrorMessage(nameof(VolumeId), FsInfoCat.Properties.Resources.ErrorMessage_IdentifierRequired);
            else
            {
                if (newText.Length > 0 && VolumeIdentifier.TryParse(newText, out VolumeIdentifier volumeIdentifier))
                    switch (newValue)
                    {
                        case VolumeIdType.VSN:
                            if (volumeIdentifier.SerialNumber.HasValue)
                            {
                                Validation.ClearErrorMessages(nameof(VolumeId));
                                VolumeId = VolumeIdentifier.ToVsnString(volumeIdentifier.SerialNumber.Value);
                                return;
                            }
                            break;
                        case VolumeIdType.UUID:
                            if (volumeIdentifier.UUID.HasValue)
                            {
                                Validation.ClearErrorMessages(nameof(VolumeId));
                                VolumeId = volumeIdentifier.UUID.Value.ToString("d");
                                return;
                            }
                            break;
                        case VolumeIdType.UncPath:
                            if (volumeIdentifier.Location.IsUnc)
                            {
                                Validation.ClearErrorMessages(nameof(VolumeId));
                                VolumeId = volumeIdentifier.Location.LocalPath;
                                return;
                            }
                            break;
                        case VolumeIdType.Url:
                            Validation.ClearErrorMessages(nameof(VolumeId));
                            VolumeId = volumeIdentifier.Location.AbsoluteUri;
                            return;
                    }
                if (newText != oldText)
                    VolumeId = newText;
                else
                    switch (newValue)
                    {
                        case VolumeIdType.VSN:
                            Validation.SetErrorMessage(nameof(VolumeId), "Invalid VSN string");
                            break;
                        case VolumeIdType.UUID:
                            Validation.SetErrorMessage(nameof(VolumeId), "Invalid UUID string");
                            break;
                        case VolumeIdType.UncPath:
                            Validation.SetErrorMessage(nameof(VolumeId), "Invalid UNC path");
                            break;
                        case VolumeIdType.Url:
                            Validation.SetErrorMessage(nameof(VolumeId), "Invalid or unsupported URL");
                            return;
                    }
            }
        }

        #endregion
        #region VolumeId Property Members

        public static readonly DependencyProperty VolumeIdProperty = DependencyProperty.Register(nameof(VolumeId), typeof(string), typeof(EditVolumeVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditVolumeVM).OnVolumeIdPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string VolumeId
        {
            get => GetValue(VolumeIdProperty) as string;
            set => SetValue(VolumeIdProperty, value);
        }

        protected virtual void OnVolumeIdPropertyChanged(string oldValue, string newValue)
        {
            if ((newValue = newValue.EmptyIfNullOrWhiteSpace()).Length == 0)
                Validation.SetErrorMessage(nameof(VolumeId), FsInfoCat.Properties.Resources.ErrorMessage_IdentifierRequired);
            else
                switch (SelectedVolumeIdType)
                {
                    case VolumeIdType.VSN:
                        if (VolumeIdentifier.VsnRegex.IsMatch(newValue))
                            Validation.ClearErrorMessages(nameof(VolumeId));
                        else
                            Validation.SetErrorMessage(nameof(VolumeId), "Invalid VSN string");
                        break;
                    case VolumeIdType.UUID:
                        if (VolumeIdentifier.UuidRegex.IsMatch(newValue))
                            Validation.ClearErrorMessages(nameof(VolumeId));
                        else
                            Validation.SetErrorMessage(nameof(VolumeId), "Invalid UUID string");
                        break;
                    case VolumeIdType.UncPath:
                        if (Path.IsPathRooted(newValue) && Path.GetPathRoot(newValue) == Path.GetPathRoot(Path.GetFullPath(newValue)) && Uri.TryCreate(newValue, UriKind.Absolute, out Uri uri) && uri.IsUnc)
                            Validation.ClearErrorMessages(nameof(VolumeId));
                        else
                            Validation.SetErrorMessage(nameof(VolumeId), "Invalid UNC path");
                        break;
                    default:
                        if (Uri.IsWellFormedUriString(newValue, UriKind.Absolute) && VolumeIdentifier.TryParse(newValue, out _))
                            Validation.ClearErrorMessages(nameof(VolumeId));
                        else
                            Validation.SetErrorMessage(nameof(VolumeId), "Invalid or unsupported URL");
                        return;
                }
        }

        #endregion
        #region MaxNameLengthValue Property Members

        private static readonly DependencyPropertyKey MaxNameLengthValuePropertyKey = DependencyProperty.RegisterReadOnly(nameof(MaxNameLengthValue), typeof(uint), typeof(EditVolumeVM),
                new PropertyMetadata(DbConstants.DbColDefaultValue_MaxNameLength, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditVolumeVM).OnMaxNameLengthValuePropertyChanged((uint)e.OldValue, (uint)e.NewValue)));

        public static readonly DependencyProperty MaxNameLengthValueProperty = MaxNameLengthValuePropertyKey.DependencyProperty;

        public uint MaxNameLengthValue
        {
            get => (uint)GetValue(MaxNameLengthValueProperty);
            private set => SetValue(MaxNameLengthValuePropertyKey, value);
        }

        protected virtual void OnMaxNameLengthValuePropertyChanged(uint oldValue, uint newValue)
        {
            // TODO: Implement OnMaxNameLengthValuePropertyChanged Logic
        }

        #endregion
        #region ExplicitMaxNameLength Property Members

        private static readonly DependencyPropertyKey ExplicitMaxNameLengthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ExplicitMaxNameLength), typeof(bool), typeof(EditVolumeVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditVolumeVM).OnExplicitMaxNameLengthPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public static readonly DependencyProperty ExplicitMaxNameLengthProperty = ExplicitMaxNameLengthPropertyKey.DependencyProperty;

        public bool ExplicitMaxNameLength
        {
            get => (bool)GetValue(ExplicitMaxNameLengthProperty);
            private set => SetValue(ExplicitMaxNameLengthPropertyKey, value);
        }

        protected virtual void OnExplicitMaxNameLengthPropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnExplicitMaxNameLengthPropertyChanged Logic
        }

        #endregion
        #region ReadOnly Property Members

        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register(nameof(ReadOnly), typeof(bool), typeof(EditVolumeVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditVolumeVM).OnReadOnlyPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool ReadOnly
        {
            get => (bool)GetValue(ReadOnlyProperty);
            set => SetValue(ReadOnlyProperty, value);
        }

        protected virtual void OnReadOnlyPropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnReadOnlyPropertyChanged Logic
        }

        #endregion
        #region ReadWrite Property Members

        public static readonly DependencyProperty ReadWriteProperty = DependencyProperty.Register(nameof(ReadWrite), typeof(bool), typeof(EditVolumeVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditVolumeVM).OnReadWritePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool ReadWrite
        {
            get => (bool)GetValue(ReadWriteProperty);
            set => SetValue(ReadWriteProperty, value);
        }

        protected virtual void OnReadWritePropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnReadWritePropertyChanged Logic
        }

        #endregion
        #region RwFileSystemDefault Property Members

        public static readonly DependencyProperty RwFileSystemDefaultProperty = DependencyProperty.Register(nameof(RwFileSystemDefault), typeof(bool), typeof(EditVolumeVM),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditVolumeVM).OnRwFileSystemDefaultPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool RwFileSystemDefault
        {
            get => (bool)GetValue(RwFileSystemDefaultProperty);
            set => SetValue(RwFileSystemDefaultProperty, value);
        }

        protected virtual void OnRwFileSystemDefaultPropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnRwFileSystemDefaultPropertyChanged Logic
        }

        #endregion
        #region VolumeStatusOptions Property Members

        private static readonly DependencyPropertyKey VolumeStatusOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeStatusOptions), typeof(ReadOnlyObservableCollection<VolumeStatus>), typeof(EditVolumeVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty VolumeStatusOptionsProperty = VolumeStatusOptionsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<VolumeStatus> VolumeStatusOptions => (ReadOnlyObservableCollection<VolumeStatus>)GetValue(VolumeStatusOptionsProperty);

        #endregion
        #region SelectedVolumeStatus Property Members

        public static readonly DependencyProperty SelectedVolumeStatusProperty = DependencyProperty.Register(nameof(SelectedVolumeStatus), typeof(VolumeStatus), typeof(EditVolumeVM),
                new PropertyMetadata(VolumeStatus.Unknown, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditVolumeVM).OnSelectedVolumeStatusPropertyChanged((VolumeStatus)e.OldValue, (VolumeStatus)e.NewValue)));

        public VolumeStatus SelectedVolumeStatus
        {
            get => (VolumeStatus)GetValue(SelectedVolumeStatusProperty);
            set => SetValue(SelectedVolumeStatusProperty, value);
        }

        protected virtual void OnSelectedVolumeStatusPropertyChanged(VolumeStatus oldValue, VolumeStatus newValue)
        {
            // TODO: Implement OnSelectedVolumeStatusPropertyChanged Logic
        }

        #endregion
        #region DriveTypeOptions Property Members

        private static readonly DependencyPropertyKey DriveTypeOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(DriveTypeOptions),
            typeof(ReadOnlyObservableCollection<DriveType>), typeof(EditVolumeVM), new PropertyMetadata(null));

        public static readonly DependencyProperty DriveTypeOptionsProperty = DriveTypeOptionsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<DriveType> DriveTypeOptions => (ReadOnlyObservableCollection<DriveType>)GetValue(DriveTypeOptionsProperty);

        #endregion
        #region SelectedDriveType Property Members

        public static readonly DependencyProperty SelectedDriveTypeProperty = DependencyProperty.Register(nameof(SelectedDriveType), typeof(DriveType), typeof(EditVolumeVM),
                new PropertyMetadata(DriveType.Unknown, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditVolumeVM).OnSelectedDriveTypePropertyChanged((DriveType)e.OldValue, (DriveType)e.NewValue)));

        public DriveType SelectedDriveType
        {
            get => (DriveType)GetValue(SelectedDriveTypeProperty);
            set => SetValue(SelectedDriveTypeProperty, value);
        }

        protected virtual void OnSelectedDriveTypePropertyChanged(DriveType oldValue, DriveType newValue)
        {
            // TODO: Implement OnSelectedDriveTypePropertyChanged Logic
        }

        #endregion
        #region VolumeName Property Members

        public static readonly DependencyProperty VolumeNameProperty = DependencyProperty.Register(nameof(VolumeName), typeof(string), typeof(EditVolumeVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditVolumeVM).OnVolumeNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string VolumeName
        {
            get => GetValue(VolumeNameProperty) as string;
            set => SetValue(VolumeNameProperty, value);
        }

        protected virtual void OnVolumeNamePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnVolumeNamePropertyChanged Logic
        }

        #endregion
        #region RootDirectory Property Members

        private static readonly DependencyPropertyKey RootDirectoryPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RootDirectory), typeof(Subdirectory), typeof(EditVolumeVM),
            new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditVolumeVM).OnRootDirectoryPropertyChanged((Subdirectory)e.OldValue, (Subdirectory)e.NewValue)));

        public static readonly DependencyProperty RootDirectoryProperty = RootDirectoryPropertyKey.DependencyProperty;

        public Subdirectory RootDirectory
        {
            get => (Subdirectory)GetValue(RootDirectoryProperty);
            private set => SetValue(RootDirectoryPropertyKey, value);
        }

        protected virtual void OnRootDirectoryPropertyChanged(Subdirectory oldValue, Subdirectory newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            ChangeTracker.SetChangeState(nameof(Path), Model?.RootDirectory?.Id != newValue?.Id);
        }

        #endregion
        #region FileSystemOptions Property Members

        private readonly ObservableCollection<FileSystemItemVM> _backingFileSystemOptions = new();

        private static readonly DependencyPropertyKey FileSystemOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileSystemOptions), typeof(ReadOnlyObservableCollection<FileSystemItemVM>), typeof(EditVolumeVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="FileSystemOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileSystemOptionsProperty = FileSystemOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ReadOnlyObservableCollection<FileSystemItemVM> FileSystemOptions => (ReadOnlyObservableCollection<FileSystemItemVM>)GetValue(FileSystemOptionsProperty);

        #endregion
        #region PickFromActiveFileSystems Property Members

        private int _ignoreFileSystemOptionsChange = 0;

        private static readonly DependencyPropertyKey PickFromActiveFileSystemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PickFromActiveFileSystems), typeof(ThreeStateViewModel), typeof(EditVolumeVM),
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
            BgOps.FromAsync("Loading data", "Getting file system options", PickFromActiveFileSystems.Value, ReloadFileSystemsAsync).ContinueWith(task => Dispatcher.Invoke(() =>
            {
                _backingFileSystemOptions.Clear();
                foreach (FileSystem entity in task.Result)
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

        /// <summary>
        /// Identifies the <see cref="SelectedFileSystem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedFileSystemProperty = DependencyProperty.Register(nameof(SelectedFileSystem), typeof(FileSystemItemVM), typeof(EditVolumeVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditVolumeVM)?.OnSelectedFileSystemPropertyChanged((FileSystemItemVM)e.OldValue, (FileSystemItemVM)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public FileSystemItemVM SelectedFileSystem { get => (FileSystemItemVM)GetValue(SelectedFileSystemProperty); set => SetValue(SelectedFileSystemProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="SelectedFileSystem"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SelectedFileSystem"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SelectedFileSystem"/> property.</param>
        private void OnSelectedFileSystemPropertyChanged(FileSystemItemVM oldValue, FileSystemItemVM newValue)
        {
            // TODO: Implement OnSelectedFileSystemPropertyChanged Logic
        }

        #endregion

        public EditVolumeVM()
        {
            ThreeStateViewModel fileSystemDisplayOptions = new(true);
            SetValue(FileSystemOptionsPropertyKey, new ReadOnlyObservableCollection<FileSystemItemVM>(_backingFileSystemOptions));
            SetValue(PickFromActiveFileSystemsPropertyKey, fileSystemDisplayOptions);
            SetValue(VolumeIdentifierTypeOptionsPropertyKey, new ReadOnlyObservableCollection<VolumeIdType>(new(Enum.GetValues<VolumeIdType>())));
            SetValue(VolumeStatusOptionsPropertyKey, new ReadOnlyObservableCollection<VolumeStatus>(new(Enum.GetValues<VolumeStatus>())));
            fileSystemDisplayOptions.ValuePropertyChanged += PickFromActiveFileSystems_ValuePropertyChanged;
        }

        protected override DbSet<Volume> GetDbSet([DisallowNull] LocalDbContext dbContext) => dbContext.Volumes;

        protected override void OnModelPropertyChanged(Volume oldValue, Volume newValue)
        {
            if (newValue is null)
            {
                SetSelectedFileSystemAsync(null);
                VolumeName = Notes = DisplayName = "";
                SelectedVolumeIdType = VolumeIdType.VSN;
                VolumeId = VolumeIdentifier.Empty;
                MaxNameLengthValue = DbConstants.DbColDefaultValue_MaxNameLength;
                ExplicitMaxNameLength =false;
                newValue.Notes.EmptyIfNullOrWhiteSpace();
                RwFileSystemDefault = true;
                SelectedVolumeStatus = VolumeStatus.Unknown;
                SelectedDriveType = DriveType.Unknown;
                newValue.VolumeName.AsWsNormalizedOrEmpty();
                RootDirectory = null;
                return;
            }
            SetSelectedFileSystemAsync(newValue.FileSystem);
            DisplayName = newValue.DisplayName.AsWsNormalizedOrEmpty();
            VolumeIdentifier vid = newValue.Identifier;
            if (vid.SerialNumber.HasValue)
            {
                SelectedVolumeIdType = VolumeIdType.VSN;
                VolumeId = VolumeIdentifier.ToVsnString(vid.SerialNumber.Value);
            }
            else if (vid.UUID.HasValue)
            {
                SelectedVolumeIdType = VolumeIdType.UUID;
                VolumeId = vid.UUID.Value.ToString("d");
            }
            else
            {
                SelectedVolumeIdType = VolumeIdType.UncPath;
                VolumeId = vid.Location.LocalPath;
            }
            MaxNameLengthValue = newValue.MaxNameLength ?? DbConstants.DbColDefaultValue_MaxNameLength;
            ExplicitMaxNameLength = newValue.MaxNameLength.HasValue;
            Notes = newValue.Notes.EmptyIfNullOrWhiteSpace();
            bool? b = newValue.ReadOnly;
            if (b.HasValue)
            {
                if (b.Value)
                    ReadOnly = true;
                else
                    ReadWrite = true;
            }
            else
                RwFileSystemDefault = true;
            SelectedVolumeStatus = newValue.Status;
            SelectedDriveType = newValue.Type;
            VolumeName = newValue.VolumeName.AsWsNormalizedOrEmpty();
            RootDirectory = newValue.RootDirectory;
        }

        private void SetSelectedFileSystemAsync(FileSystem fileSystem)
        {
            Task<ICollection<FileSystem>> result;
            if (fileSystem is null)
            {
                if (Dispatcher.CheckInvoke(() =>
                {
                    SelectedFileSystem = null;
                    return _backingFileSystemOptions.Count > 0;
                }))
                    return;
                result = BgOps.FromAsync("Loading data", "Getting file system options", Dispatcher.CheckInvoke(() => PickFromActiveFileSystems.Value), ReloadFileSystemsAsync);
            }
            else
            {
                Guid id = fileSystem.Id;
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
                        if (fileSystem.IsInactive)
                        {
                            if (PickFromActiveFileSystems.IsTrue)
                                PickFromActiveFileSystems.IsNull = true;
                        }
                        else if (PickFromActiveFileSystems.IsFalse)
                            PickFromActiveFileSystems.IsTrue = true;
                        return PickFromActiveFileSystems.Value;
                    });
                }
                finally { Interlocked.Decrement(ref _ignoreFileSystemOptionsChange); }
                result = BgOps.FromAsync("Loading data", "Getting file system options", displayOptions, ReloadFileSystemsAsync);
            }

            result.ContinueWith(task => Dispatcher.Invoke(() =>
            {
                _backingFileSystemOptions.Clear();
                foreach (FileSystem entity in task.Result)
                    _backingFileSystemOptions.Add(new(entity));
                if (fileSystem is null)
                    return null;
                Guid id = fileSystem.Id;
                FileSystemItemVM vm = _backingFileSystemOptions.FirstOrDefault(v => v.Model.Id == id);
                if (vm is null)
                {
                    vm = new(fileSystem);
                    _backingFileSystemOptions.Add(vm);
                }
                SelectedFileSystem = vm;
                return vm;
            }), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private static async Task<ICollection<FileSystem>> ReloadFileSystemsAsync(bool? selectActive, IWindowsStatusListener statusListener)
        {
            using IServiceScope scope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            if (selectActive.HasValue)
            {
                if (selectActive.Value)
                    return await (from f in dbContext.FileSystems where f.IsInactive == false select f).ToArrayAsync(statusListener.CancellationToken);
                return await (from f in dbContext.FileSystems where f.IsInactive == true select f).ToArrayAsync(statusListener.CancellationToken);
            }
            return await dbContext.FileSystems.ToArrayAsync(statusListener.CancellationToken);
        }

        protected override bool OnBeforeSave()
        {
            Volume model = Model;
            if (model is null)
                return false;
            model.DisplayName = DisplayName.AsWsNormalizedOrEmpty();
            VolumeIdentifier.TryParse(VolumeId, out VolumeIdentifier volumeIdentifier);
            model.Identifier = volumeIdentifier;
            model.MaxNameLength = ExplicitMaxNameLength ? MaxNameLengthValue : null;
            Notes = Notes.EmptyIfNullOrWhiteSpace();
            model.ReadOnly = RwFileSystemDefault ? null : ReadOnly;
            model.Status = SelectedVolumeStatus;
            model.Type = SelectedDriveType;
            model.VolumeName = VolumeName.AsWsNormalizedOrEmpty();
            model.FileSystem = SelectedFileSystem?.Model;
            return true;
        }

        ISimpleIdentityReference<Subdirectory> IHasSubdirectoryEntity.GetSubdirectoryEntity() => Dispatcher.CheckInvoke(() => RootDirectory);

        public async Task<ISimpleIdentityReference<Subdirectory>> GetSubdirectoryEntityAsync([DisallowNull] IWindowsStatusListener statusListener)
        {
            return await Dispatcher.InvokeAsync(() => RootDirectory);
        }
    }
}
