using FsInfoCat.Desktop.Model;
using FsInfoCat.Model.Local;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class LocalVolumeVM : DependencyObject
    {
        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(nameof(DisplayName), typeof(string), typeof(LocalVolumeVM),
            new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as LocalVolumeVM).OnDisplayNamePropertyChanged(e.OldValue as string, e.NewValue as string),
                (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        public string DisplayName
        {
            get { return GetValue(DisplayNameProperty) as string; }
            set { SetValue(DisplayNameProperty, value); }
        }

        protected virtual void OnDisplayNamePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnDisplayNamePropertyChanged Logic
        }

        private static readonly DependencyPropertyKey VolumeNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeName), typeof(string), typeof(LocalVolumeVM),
            new PropertyMetadata("", null, (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        public static readonly DependencyProperty VolumeNameProperty = VolumeNamePropertyKey.DependencyProperty;

        public string VolumeName
        {
            get { return GetValue(VolumeNameProperty) as string; }
            private set { SetValue(VolumeNamePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey RootPathNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(RootPathName), typeof(string), typeof(LocalVolumeVM),
            new PropertyMetadata("", null, (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        public static readonly DependencyProperty RootPathNameProperty = RootPathNamePropertyKey.DependencyProperty;

        public string RootPathName
        {
            get { return GetValue(RootPathNameProperty) as string; }
            private set { SetValue(RootPathNamePropertyKey, value); }
        }

        public static readonly DependencyProperty IsInactiveProperty =
            DependencyProperty.Register(nameof(IsInactive), typeof(bool), typeof(LocalVolumeVM),
                new PropertyMetadata(false));

        public bool IsInactive
        {
            get { return (bool)GetValue(IsInactiveProperty); }
            set { SetValue(IsInactiveProperty, value); }
        }

        private static readonly DependencyPropertyKey DriveFormatPropertyKey = DependencyProperty.RegisterReadOnly(nameof(DriveFormat), typeof(string), typeof(LocalVolumeVM),
            new PropertyMetadata("", null, (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        public static readonly DependencyProperty DriveFormatProperty = DriveFormatPropertyKey.DependencyProperty;

        public string DriveFormat
        {
            get { return GetValue(DriveFormatProperty) as string; }
            private set { SetValue(DriveFormatPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey IdentifierPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Identifier), typeof(VolumeIdentifier), typeof(LocalVolumeVM),
                new PropertyMetadata(VolumeIdentifier.Empty));

        public static readonly DependencyProperty IdentifierProperty = IdentifierPropertyKey.DependencyProperty;

        public VolumeIdentifier Identifier
        {
            get { return (VolumeIdentifier)GetValue(IdentifierProperty); }
            private set { SetValue(IdentifierPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey MaxNameLengthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MaxNameLength), typeof(long), typeof(LocalVolumeVM),
                new PropertyMetadata(0L, null, (DependencyObject d, object baseValue) => (baseValue is long v) ? ((v < 0L) ? 0L : v) : 0L));

        public static readonly DependencyProperty MaxNameLengthProperty = MaxNameLengthPropertyKey.DependencyProperty;

        public long MaxNameLength
        {
            get { return (long)GetValue(MaxNameLengthProperty); }
            private set { SetValue(MaxNameLengthPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey CaseSensitivePropertyKey = DependencyProperty.RegisterReadOnly(nameof(CaseSensitive), typeof(bool), typeof(LocalVolumeVM),
            new PropertyMetadata(false));

        public static readonly DependencyProperty CaseSensitiveProperty = CaseSensitivePropertyKey.DependencyProperty;

        public bool CaseSensitive
        {
            get { return (bool)GetValue(CaseSensitiveProperty); }
            private set { SetValue(CaseSensitivePropertyKey, value); }
        }

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register(nameof(Id), typeof(Guid?), typeof(LocalVolumeVM),
                new PropertyMetadata(null));

        public Guid? Id
        {
            get { return (Guid?)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        private static readonly DependencyPropertyKey CreatedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CreatedOn), typeof(DateTime), typeof(LocalVolumeVM),
                new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty CreatedOnProperty = CreatedOnPropertyKey.DependencyProperty;

        public DateTime CreatedOn
        {
            get { return (DateTime)GetValue(CreatedOnProperty); }
            private set { SetValue(CreatedOnPropertyKey, value); }
        }


        private static readonly DependencyPropertyKey ModifiedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ModifiedOn), typeof(DateTime), typeof(LocalVolumeVM),
                new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty ModifiedOnProperty = ModifiedOnPropertyKey.DependencyProperty;

        public DateTime ModifiedOn
        {
            get { return (DateTime)GetValue(ModifiedOnProperty); }
            private set { SetValue(ModifiedOnPropertyKey, value); }
        }

        public static readonly DependencyProperty NotesProperty = DependencyProperty.Register(nameof(Notes), typeof(string), typeof(LocalVolumeVM),
            new PropertyMetadata("", null, (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        public string Notes
        {
            get { return GetValue(NotesProperty) as string; }
            set { SetValue(NotesProperty, value); }
        }

        private static readonly DependencyPropertyKey IsModifiedPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsModified), typeof(bool), typeof(LocalVolumeVM),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsModifiedProperty = IsModifiedPropertyKey.DependencyProperty;

        public bool IsModified
        {
            get { return (bool)GetValue(IsModifiedProperty); }
            private set { SetValue(IsModifiedPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey DriveTypePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(DriveType), typeof(DriveType), typeof(LocalVolumeVM),
                new PropertyMetadata(DriveType.Unknown));

        public static readonly DependencyProperty DriveTypeProperty = DriveTypePropertyKey.DependencyProperty;

        public DriveType DriveType
        {
            get { return (DriveType)GetValue(DriveTypeProperty); }
            private set { SetValue(DriveTypePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey AvailabilityPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Availability), typeof(Win32_DeviceAvailability), typeof(LocalVolumeVM),
                new PropertyMetadata(Win32_DeviceAvailability.Unknown));

        public static readonly DependencyProperty AvailabilityProperty = AvailabilityPropertyKey.DependencyProperty;

        public Win32_DeviceAvailability Availability
        {
            get { return (Win32_DeviceAvailability)GetValue(AvailabilityProperty); }
            private set { SetValue(AvailabilityPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey StatusPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Status), typeof(string), typeof(LocalVolumeVM),
                new PropertyMetadata(""));

        public static readonly DependencyProperty StatusProperty = StatusPropertyKey.DependencyProperty;

        public string Status
        {
            get { return GetValue(StatusProperty) as string; }
            private set { SetValue(StatusPropertyKey, value); }
        }

        internal void InitializeFromModel(ILocalVolume volume)
        {
            if (volume is null)
                throw new ArgumentNullException(nameof(volume));
            //DisplayName = volume.DisplayName;
            //VolumeName = volume.VolumeName;
            //RootPathName = volume.GetRootPathName();
            //DriveType = volume.Type;
            //IsInactive = volume.IsInactive;
            //DriveFormat = volume.GetDriveFormat();
            //Identifier = VolumeIdentifier.TryCreate(volume.Identifier, out VolumeIdentifier volumeIdentifier) ? volumeIdentifier : VolumeIdentifier.Empty;
            //MaxNameLength = volume.GetEffectiveMaxNameLength();
            //CaseSensitive = volume.GetEffectiveCaseSensitiveSearch();
            //Id = volume.Id;
            //CreatedOn = volume.CreatedOn;
            //ModifiedOn = volume.ModifiedOn;
            //Notes = volume.Notes;
            throw new NotImplementedException();
            IsModified = false;
        }

        internal void InitializeFromModel(Win32_LogicalDiskRootDirectory volume)
        {
            if (volume is null)
                throw new ArgumentNullException(nameof(volume));
            Win32_LogicalDisk logicalDisk = volume.GroupComponent;
            DisplayName = string.IsNullOrWhiteSpace(logicalDisk.Caption) ? logicalDisk.Name : logicalDisk.Caption;
            VolumeName = logicalDisk.VolumeName;
            VolumeInfo volumeInfo = new VolumeInfo(volume.PartComponent.Name);
            RootPathName = volumeInfo.FullName;
            IsInactive = false;
            DriveFormat = volumeInfo.FileSystemName;
            DriveType = volumeInfo.Type;
            if (volumeInfo.Type == DriveType.Network)
                Identifier = VolumeIdentifier.TryCreate(logicalDisk, out VolumeIdentifier volumeIdentifier) ? volumeIdentifier : VolumeIdentifier.Empty;
            else
                Identifier = new VolumeIdentifier(volumeInfo.SerialNumber);
            MaxNameLength = volumeInfo.MaxNameLength;
            CaseSensitive = volumeInfo.Features.HasFlag(FileSystemFeature.CaseSensitiveSearch);
            Id = null;
            CreatedOn = DateTime.Now;
            ModifiedOn = CreatedOn;
            Notes = "";
            IsModified = true;
        }

        internal void UpdateFromModel(Win32_LogicalDiskRootDirectory volume)
        {
            if (volume is null)
                throw new ArgumentNullException(nameof(volume));
            Win32_LogicalDisk logicalDisk = volume.GroupComponent;
            Win32_Directory rootDir = volume.PartComponent;
            VolumeInfo volumeInfo = new VolumeInfo(rootDir.Name);
            VolumeIdentifier id;
            if (volumeInfo.Type == DriveType.Network)
                id = VolumeIdentifier.TryCreate(logicalDisk, out VolumeIdentifier volumeIdentifier) ? volumeIdentifier : VolumeIdentifier.Empty;
            else
                id = new VolumeIdentifier(volumeInfo.SerialNumber);
            if (id != Identifier)
                throw new ArgumentOutOfRangeException(nameof(volume));
            bool isModified = VolumeName != logicalDisk.VolumeName;
            if (isModified)
                VolumeName = logicalDisk.VolumeName;
            if (RootPathName != volumeInfo.FullName)
            {
                isModified = true;
                RootPathName = volumeInfo.FullName;
            }
            if (DriveFormat != volumeInfo.FileSystemName)
            {
                isModified = true;
                DriveFormat = volumeInfo.FileSystemName;
            }
            if (MaxNameLength != volumeInfo.MaxNameLength)
            {
                isModified = true;
                MaxNameLength = volumeInfo.MaxNameLength;
            }
            bool caseSensitive = volumeInfo.Features.HasFlag(FileSystemFeature.CaseSensitiveSearch);
            if (CaseSensitive != caseSensitive)
            {
                CaseSensitive = caseSensitive;
                isModified = true;
            }
            if (DriveType != volumeInfo.Type)
            {
                DriveType = volumeInfo.Type;
                isModified = true;
            }
            IsModified = isModified;
            Availability = logicalDisk.Availability ?? Win32_DeviceAvailability.Unknown;
            Status = logicalDisk.Status;
        }

        internal static void Refresh(ReadOnlyObservableCollection<LocalVolumeVM> localVolumes, List<Win32_LogicalDiskRootDirectory> result)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<LocalVolumeVM> GetAllLocalVolumes(IEnumerable<ILocalVolume> dbVolumes, List<Win32_LogicalDiskRootDirectory> logicalDisks)
        {
            Dictionary<VolumeIdentifier, Win32_LogicalDiskRootDirectory> byVolumeIdentifer = new Dictionary<VolumeIdentifier, Win32_LogicalDiskRootDirectory>();
            if (!(logicalDisks is  null))
                foreach (Win32_LogicalDiskRootDirectory sv in logicalDisks)
                {
                    if (VolumeIdentifier.TryCreate(sv, out VolumeIdentifier id))
                    {
                        if (byVolumeIdentifer.ContainsKey(id))
                            byVolumeIdentifer[id] = sv;
                        else
                            byVolumeIdentifer.Add(id, sv);
                    }
                }
            if (!(dbVolumes is null))
                foreach (ILocalVolume lv in dbVolumes)
                {
                    if (lv is null)
                        continue;
                    LocalVolumeVM vm = new LocalVolumeVM();
                    vm.InitializeFromModel(lv);
                    throw new NotImplementedException();
                    //if (VolumeIdentifier.TryCreate(lv.Identifier, out VolumeIdentifier id) && byVolumeIdentifer.ContainsKey(id))
                    //{
                    //    vm.UpdateFromModel(byVolumeIdentifer[id]);
                    //    byVolumeIdentifer.Remove(id);
                    //}
                    //else
                    //{
                    //    vm.Availability = Win32_DeviceAvailability.Other;
                    //    vm.Status = "";
                    //}
                    //yield return vm;
                }
            foreach (Win32_LogicalDiskRootDirectory sv in byVolumeIdentifer.Values)
            {
                LocalVolumeVM vm = new LocalVolumeVM();
                vm.InitializeFromModel(sv);
                yield return vm;
            }
        }
    }
}
