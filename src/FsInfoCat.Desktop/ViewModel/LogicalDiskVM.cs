using FsInfoCat.Desktop.WMI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class LogicalDiskVM : DependencyObject
    {

        private static readonly DependencyPropertyKey NamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Name), typeof(string), typeof(LogicalDiskVM), new PropertyMetadata(""));

        public static readonly DependencyProperty NameProperty = NamePropertyKey.DependencyProperty;

        public string Name
        {
            get => GetValue(NameProperty) as string;
            private set => SetValue(NamePropertyKey, value);
        }

        private static readonly DependencyPropertyKey PathPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Path), typeof(string), typeof(LogicalDiskVM),
                new PropertyMetadata(""));

        public static readonly DependencyProperty PathProperty = PathPropertyKey.DependencyProperty;

        public string Path
        {
            get => GetValue(PathProperty) as string;
            private set => SetValue(PathPropertyKey, value);
        }

        private static readonly DependencyPropertyKey TypePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Type), typeof(DriveType), typeof(LogicalDiskVM),
                new PropertyMetadata(DriveType.Unknown));

        public static readonly DependencyProperty TypeProperty = TypePropertyKey.DependencyProperty;

        public DriveType Type
        {
            get => (DriveType)GetValue(TypeProperty);
            private set => SetValue(TypePropertyKey, value);
        }

        public static readonly DependencyProperty FileSystemProperty =
            DependencyProperty.Register(nameof(FileSystem), typeof(string), typeof(LogicalDiskVM),
                new PropertyMetadata(""));

        public string FileSystem
        {
            get => GetValue(FileSystemProperty) as string;
            set => SetValue(FileSystemProperty, value);
        }

        private static readonly DependencyPropertyKey VolumeNamePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(VolumeName), typeof(string), typeof(LogicalDiskVM),
                new PropertyMetadata(""));

        public static readonly DependencyProperty VolumeNameProperty = VolumeNamePropertyKey.DependencyProperty;

        public string VolumeName
        {
            get => GetValue(VolumeNameProperty) as string;
            private set => SetValue(VolumeNamePropertyKey, value);
        }

        private static readonly DependencyPropertyKey VolumeSerialNumberPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(VolumeSerialNumber), typeof(string), typeof(LogicalDiskVM),
                new PropertyMetadata(""));

        public static readonly DependencyProperty VolumeSerialNumberProperty = VolumeSerialNumberPropertyKey.DependencyProperty;

        public string VolumeSerialNumber
        {
            get => GetValue(VolumeSerialNumberProperty) as string;
            private set => SetValue(VolumeSerialNumberPropertyKey, value);
        }

        private static readonly DependencyPropertyKey IsReadOnlyPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsReadOnly), typeof(bool), typeof(LogicalDiskVM),
                new PropertyMetadata(false));

        public static readonly DependencyProperty IsReadOnlyProperty = IsReadOnlyPropertyKey.DependencyProperty;

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            private set => SetValue(IsReadOnlyPropertyKey, value);
        }

        private static readonly DependencyPropertyKey CompressedPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Compressed), typeof(bool), typeof(LogicalDiskVM),
                new PropertyMetadata(false));

        public static readonly DependencyProperty CompressedProperty = CompressedPropertyKey.DependencyProperty;

        public bool Compressed
        {
            get => (bool)GetValue(CompressedProperty);
            private set => SetValue(CompressedPropertyKey, value);
        }

        private static readonly DependencyPropertyKey FreeSpacePropertyKey = DependencyProperty.RegisterReadOnly(nameof(FreeSpace), typeof(ulong), typeof(LogicalDiskVM), new PropertyMetadata(0UL));

        public static readonly DependencyProperty FreeSpaceProperty = FreeSpacePropertyKey.DependencyProperty;

        public ulong FreeSpace
        {
            get => (ulong)GetValue(FreeSpaceProperty);
            private set => SetValue(FreeSpacePropertyKey, value);
        }

        private static readonly DependencyPropertyKey SizePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Size), typeof(ulong), typeof(LogicalDiskVM), new PropertyMetadata(0UL));

        public static readonly DependencyProperty SizeProperty = SizePropertyKey.DependencyProperty;

        public ulong Size
        {
            get => (ulong)GetValue(SizeProperty);
            private set => SetValue(SizePropertyKey, value);
        }

        private static readonly DependencyPropertyKey AttributesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Attributes), typeof(FileAttributes), typeof(LogicalDiskVM), new PropertyMetadata(FileAttributes.Normal));

        public static readonly DependencyProperty AttributesProperty = AttributesPropertyKey.DependencyProperty;

        public FileAttributes Attributes
        {
            get => (FileAttributes)GetValue(AttributesProperty);
            private set => SetValue(AttributesPropertyKey, value);
        }

        private static readonly DependencyPropertyKey CreationTimePropertyKey = DependencyProperty.RegisterReadOnly(nameof(CreationTime), typeof(DateTime), typeof(LogicalDiskVM), new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty CreationTimeProperty = CreationTimePropertyKey.DependencyProperty;

        public DateTime CreationTime
        {
            get => (DateTime)GetValue(CreationTimeProperty);
            private set => SetValue(CreationTimePropertyKey, value);
        }

        public static readonly DependencyProperty LastWriteTimeProperty = DependencyProperty.Register(nameof(LastWriteTime), typeof(DateTime), typeof(LogicalDiskVM), new PropertyMetadata(DateTime.Now));

        public DateTime LastWriteTime
        {
            get => (DateTime)GetValue(LastWriteTimeProperty);
            set => SetValue(LastWriteTimeProperty, value);
        }

        #region Folders Property Members

        private static readonly DependencyPropertyKey InnerFoldersPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(InnerFolders), typeof(ObservableCollection<FolderVM>), typeof(LogicalDiskVM),
                new PropertyMetadata(new ObservableCollection<FolderVM>()));

        private static readonly DependencyPropertyKey FoldersPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Folders), typeof(ReadOnlyObservableCollection<FolderVM>), typeof(LogicalDiskVM),
                new PropertyMetadata(null));

        protected static readonly DependencyProperty InnerFoldersProperty = InnerFoldersPropertyKey.DependencyProperty;

        public static readonly DependencyProperty FoldersProperty = FoldersPropertyKey.DependencyProperty;

        protected ObservableCollection<FolderVM> InnerFolders
        {
            get => (ObservableCollection<FolderVM>)GetValue(InnerFoldersProperty);
            private set => SetValue(InnerFoldersPropertyKey, value);
        }

        public ReadOnlyObservableCollection<FolderVM> Folders
        {
            get => (ReadOnlyObservableCollection<FolderVM>)GetValue(FoldersProperty);
            private set => SetValue(FoldersPropertyKey, value);
        }

        #endregion

        #region Files Property Members

        private static readonly DependencyPropertyKey InnerFilesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(InnerFiles), typeof(ObservableCollection<FileVM>), typeof(LogicalDiskVM), new PropertyMetadata(new ObservableCollection<FileVM>()));

        private static readonly DependencyPropertyKey FilesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Files), typeof(ReadOnlyObservableCollection<FileVM>), typeof(LogicalDiskVM), new PropertyMetadata(null));

        protected static readonly DependencyProperty InnerFilesProperty = InnerFilesPropertyKey.DependencyProperty;

        public static readonly DependencyProperty FilesProperty = FilesPropertyKey.DependencyProperty;

        protected ObservableCollection<FileVM> InnerFiles
        {
            get => (ObservableCollection<FileVM>)GetValue(InnerFilesProperty);
            private set => SetValue(InnerFilesPropertyKey, value);
        }

        public ReadOnlyObservableCollection<FileVM> Files
        {
            get => (ReadOnlyObservableCollection<FileVM>)GetValue(FilesProperty);
            private set => SetValue(FilesPropertyKey, value);
        }

        #endregion

        internal LogicalDiskVM(Win32_LogicalDisk logicalDisk, int preloadDepth)
        {
            Name = logicalDisk.DisplayName;
            Path = logicalDisk.RootDirectory.Path;
            Type = logicalDisk.DriveType;
            FileSystem = logicalDisk.FileSystem;
            VolumeName = logicalDisk.VolumeName;
            VolumeSerialNumber = logicalDisk.VolumeSerialNumber;
            IsReadOnly = logicalDisk.IsReadOnly;
            Compressed = logicalDisk.Compressed;
            FreeSpace = logicalDisk.FreeSpace;
            Size = logicalDisk.Size;
            Folders = new ReadOnlyObservableCollection<FolderVM>(InnerFolders);
            Files = new ReadOnlyObservableCollection<FileVM>(InnerFiles);
            DirectoryInfo rootDirectory = new(Path);
            Attributes = rootDirectory.Attributes;
            CreationTime = rootDirectory.CreationTime;
            LastWriteTime = rootDirectory.LastWriteTime;
            if (preloadDepth > 0)
            {
                preloadDepth--;
                foreach (DirectoryInfo c in rootDirectory.GetDirectories())
                    InnerFolders.Add(new FolderVM(c, preloadDepth));
            }
            foreach (FileInfo c in rootDirectory.GetFiles())
                InnerFiles.Add(new FileVM(c));
        }
    }
}
