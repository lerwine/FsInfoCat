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
    public class VolumesPageVM : DbEntityListingPageVM<Volume, VolumeItemVM>
    {
    }
    public class VolumeItemVM : DbEntityItemVM<Volume>
    {
        #region DisplayName Property Members

        private static readonly DependencyPropertyKey DisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(DisplayName), typeof(string), typeof(VolumeItemVM), new PropertyMetadata(""));

        public static readonly DependencyProperty DisplayNameProperty = DisplayNamePropertyKey.DependencyProperty;

        public string DisplayName
        {
            get => GetValue(DisplayNameProperty) as string;
            private set => SetValue(DisplayNamePropertyKey, value);
        }

        #endregion
        #region Notes Property Members

        private static readonly DependencyPropertyKey NotesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Notes), typeof(string), typeof(VolumeItemVM), new PropertyMetadata(""));

        public static readonly DependencyProperty NotesProperty = NotesPropertyKey.DependencyProperty;

        public string Notes
        {
            get => GetValue(NotesProperty) as string;
            private set => SetValue(NotesPropertyKey, value);
        }

        #endregion

        internal VolumeItemVM([DisallowNull] Volume model)
            : base(model)
        {
            DisplayName = model.DisplayName;
            Notes = model.Notes;
            // TODO: Initialize properties
        }

        protected override void OnModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Volume.DisplayName):
                    Dispatcher.Invoke(() => DisplayName = Model.DisplayName);
                    break;
                case nameof(Volume.Notes):
                    Dispatcher.Invoke(() => Notes = Model.Notes);
                    break;
                    // TODO: Check for remainder of properties
            }
        }
    }
    public class EditVolumeVM : EditDbEntityVM<Volume>
    {
        #region Other Property Members

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

        #region VolumeStatusOptions Property Members

        private static readonly DependencyPropertyKey VolumeStatusOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeStatusOptions), typeof(ReadOnlyObservableCollection<VolumeStatus>), typeof(EditVolumeVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty VolumeStatusOptionsProperty = VolumeStatusOptionsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<VolumeStatus> VolumeStatusOptions => (ReadOnlyObservableCollection<VolumeStatus>)GetValue(VolumeStatusOptionsProperty);

        #endregion

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

        #region DriveTypeOptions Property Members

        private static readonly DependencyPropertyKey DriveTypeOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(DriveTypeOptions),
            typeof(ReadOnlyObservableCollection<DriveType>), typeof(EditVolumeVM), new PropertyMetadata(null));

        public static readonly DependencyProperty DriveTypeOptionsProperty = DriveTypeOptionsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<DriveType> DriveTypeOptions => (ReadOnlyObservableCollection<DriveType>)GetValue(DriveTypeOptionsProperty);

        #endregion

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

        public EditVolumeVM()
        {
            SetValue(VolumeIdentifierTypeOptionsPropertyKey, new ReadOnlyObservableCollection<VolumeIdType>(new(Enum.GetValues<VolumeIdType>())));
            SetValue(VolumeStatusOptionsPropertyKey, new ReadOnlyObservableCollection<VolumeStatus>(new(Enum.GetValues<VolumeStatus>())));
        }

        protected override void Initialize(Volume model, EntityState state)
        {
            base.Initialize(model, state);
            DisplayName = model.DisplayName.AsWsNormalizedOrEmpty();
            VolumeIdentifier vid = model.Identifier;
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
            MaxNameLengthValue = model.MaxNameLength ?? DbConstants.DbColDefaultValue_MaxNameLength;
            ExplicitMaxNameLength = model.MaxNameLength.HasValue;
            Notes = model.Notes.EmptyIfNullOrWhiteSpace();
            bool? b = model.ReadOnly;
            if (b.HasValue)
            {
                if (b.Value)
                    ReadOnly = true;
                else
                    ReadWrite = true;
            }
            else
                RwFileSystemDefault = true;
            SelectedVolumeStatus = model.Status;
            SelectedDriveType = model.Type;
            VolumeName = model.VolumeName.AsWsNormalizedOrEmpty();
            // TODO: Load related
            //RootDirectory = model.RootDirectory;
            //AccessErrors = model.AccessErrors;
            //FileSystem = model.FileSystem;
        }

        protected override Volume InitializeNewModel() => new Volume
        {
            Id = Guid.NewGuid(),
            CreatedOn = DateTime.Now
        };

        protected override DbSet<Volume> GetDbSet(LocalDbContext dbContext) => dbContext.Volumes;

        protected override void UpdateModelForSave(Volume model, bool isNew)
        {
            model.DisplayName = DisplayName.AsWsNormalizedOrEmpty();
            VolumeIdentifier.TryParse(VolumeId, out VolumeIdentifier volumeIdentifier);
            model.Identifier = volumeIdentifier;
            model.MaxNameLength = ExplicitMaxNameLength ? MaxNameLengthValue : null;
            Notes = Notes.EmptyIfNullOrWhiteSpace();
            model.ReadOnly = RwFileSystemDefault ? null : ReadOnly;
            model.Status = SelectedVolumeStatus;
            model.Type = SelectedDriveType;
            model.VolumeName = VolumeName.AsWsNormalizedOrEmpty();
            // TODO: Apply related
            //model.RootDirectory = RootDirectory;
            //model.AccessErrors = AccessErrors;
            //model.FileSystem = FileSystem;
        }
    }
}
