using FsInfoCat.Desktop.WMI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FolderVM : DependencyObject
    {

        private static readonly DependencyPropertyKey NamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Name), typeof(string), typeof(FolderVM), new PropertyMetadata(""));

        public static readonly DependencyProperty NameProperty = NamePropertyKey.DependencyProperty;

        public string Name
        {
            get => GetValue(NameProperty) as string;
            private set => SetValue(NamePropertyKey, value);
        }

        private static readonly DependencyPropertyKey PathPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Path), typeof(string), typeof(FolderVM), new PropertyMetadata(""));

        public static readonly DependencyProperty PathProperty = PathPropertyKey.DependencyProperty;

        public string Path
        {
            get => GetValue(PathProperty) as string;
            private set => SetValue(PathPropertyKey, value);
        }

        private static readonly DependencyPropertyKey AttributesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Attributes), typeof(FileAttributes), typeof(FolderVM), new PropertyMetadata(FileAttributes.Normal));

        public static readonly DependencyProperty AttributesProperty = AttributesPropertyKey.DependencyProperty;

        public FileAttributes Attributes
        {
            get => (FileAttributes)GetValue(AttributesProperty);
            private set => SetValue(AttributesPropertyKey, value);
        }

        private static readonly DependencyPropertyKey CreationTimePropertyKey = DependencyProperty.RegisterReadOnly(nameof(CreationTime), typeof(DateTime), typeof(FolderVM), new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty CreationTimeProperty = CreationTimePropertyKey.DependencyProperty;

        public DateTime CreationTime
        {
            get => (DateTime)GetValue(CreationTimeProperty);
            private set => SetValue(CreationTimePropertyKey, value);
        }

        private static readonly DependencyPropertyKey LastWriteTimePropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastWriteTime), typeof(DateTime), typeof(FolderVM), new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty LastWriteTimeProperty = LastWriteTimePropertyKey.DependencyProperty;

        public DateTime LastWriteTime
        {
            get => (DateTime)GetValue(LastWriteTimeProperty);
            private set => SetValue(LastWriteTimePropertyKey, value);
        }

        public event DependencyPropertyChangedEventHandler IsSelectedPropertyChanged;

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(FolderVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as FolderVM).OnIsSelectedPropertyChanged(e)));

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        protected virtual void OnIsSelectedPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnIsSelectedPropertyChanged((bool)args.OldValue, (bool)args.NewValue); }
            finally { IsSelectedPropertyChanged?.Invoke(this, args); }
        }

        protected virtual void OnIsSelectedPropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnIsSelectedPropertyChanged Logic
        }

        public event DependencyPropertyChangedEventHandler IsExpandedPropertyChanged;

        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(FolderVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as FolderVM).OnIsExpandedPropertyChanged(e)));

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        protected virtual void OnIsExpandedPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnIsExpandedPropertyChanged((bool)args.OldValue, (bool)args.NewValue); }
            finally { IsExpandedPropertyChanged?.Invoke(this, args); }
        }

        protected virtual void OnIsExpandedPropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnIsExpandedPropertyChanged Logic
        }

        #region SubFolders Property Members

        private static readonly DependencyPropertyKey InnerSubFoldersPropertyKey = DependencyProperty.RegisterReadOnly(nameof(InnerSubFolders), typeof(ObservableCollection<FolderVM>), typeof(FolderVM), new PropertyMetadata(null));

        private static readonly DependencyPropertyKey SubFoldersPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SubFolders), typeof(ReadOnlyObservableCollection<FolderVM>), typeof(FolderVM), new PropertyMetadata(null));

        protected static readonly DependencyProperty InnerSubFoldersProperty = InnerSubFoldersPropertyKey.DependencyProperty;

        public static readonly DependencyProperty SubFoldersProperty = SubFoldersPropertyKey.DependencyProperty;

        protected ObservableCollection<FolderVM> InnerSubFolders
        {
            get => (ObservableCollection<FolderVM>)GetValue(InnerSubFoldersProperty);
            private set => SetValue(InnerSubFoldersPropertyKey, value);
        }

        public ReadOnlyObservableCollection<FolderVM> SubFolders
        {
            get => (ReadOnlyObservableCollection<FolderVM>)GetValue(SubFoldersProperty);
            private set => SetValue(SubFoldersPropertyKey, value);
        }

        #endregion

        #region Files Property Members

        private static readonly DependencyPropertyKey InnerFilesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(InnerFiles), typeof(ObservableCollection<FileVM>), typeof(FolderVM), new PropertyMetadata(null));

        private static readonly DependencyPropertyKey FilesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Files), typeof(ReadOnlyObservableCollection<FileVM>), typeof(FolderVM), new PropertyMetadata(null));

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

        private static readonly DependencyPropertyKey AccessErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AccessError), typeof(string), typeof(FolderVM), new PropertyMetadata(""));

        public static readonly DependencyProperty AccessErrorProperty = AccessErrorPropertyKey.DependencyProperty;

        public string AccessError
        {
            get => GetValue(AccessErrorProperty) as string;
            private set => SetValue(AccessErrorPropertyKey, value);
        }

        private FolderVM(string name, DirectoryInfo directoryInfo, int preloadDepth)
        {
            Name = name;
            Path = directoryInfo.FullName;
            Attributes = directoryInfo.Attributes;
            CreationTime = directoryInfo.CreationTime;
            LastWriteTime = directoryInfo.LastWriteTime;
            InnerSubFolders = new();
            InnerFiles = new();
            SubFolders = new(InnerSubFolders);
            Files = new(InnerFiles);
            DirectoryInfo[] directories;
            FileInfo[] files;
            try
            {
                directories = (preloadDepth-- > 0) ? directoryInfo.GetDirectories() : Array.Empty<DirectoryInfo>();
                files = directoryInfo.GetFiles();
            }
            catch (Exception exception)
            {
                AccessError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
                directories = Array.Empty<DirectoryInfo>();
                files = Array.Empty<FileInfo>();
            }
            foreach (DirectoryInfo c in directories)
                InnerSubFolders.Add(new FolderVM(c, preloadDepth));
            foreach (FileInfo c in files)
                InnerFiles.Add(new FileVM(c));
        }

        internal FolderVM(Win32_LogicalDisk logicalDisk, int preloadDepth) : this(string.IsNullOrWhiteSpace(logicalDisk.VolumeName) ? logicalDisk.DisplayName : $"{logicalDisk.DisplayName} ({logicalDisk.VolumeName})",
            new DirectoryInfo(string.IsNullOrEmpty(logicalDisk.RootDirectory.Path) ? logicalDisk.RootDirectory.Name : logicalDisk.RootDirectory.Path), preloadDepth) { }

        internal FolderVM(DirectoryInfo directoryInfo, int preloadDepth) : this(directoryInfo.Name, directoryInfo, preloadDepth) { }
    }
}
