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

        #region SubFolders Property Members

        private static readonly DependencyPropertyKey InnerSubFoldersPropertyKey = DependencyProperty.RegisterReadOnly(nameof(InnerSubFolders), typeof(ObservableCollection<FolderVM>), typeof(FolderVM), new PropertyMetadata(new ObservableCollection<FolderVM>()));

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

        private static readonly DependencyPropertyKey InnerFilesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(InnerFiles), typeof(ObservableCollection<FileVM>), typeof(FolderVM), new PropertyMetadata(new ObservableCollection<FileVM>()));

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

        internal FolderVM(DirectoryInfo directoryInfo, int preloadDepth)
        {
            Name = directoryInfo.Name;
            Attributes = directoryInfo.Attributes;
            CreationTime = directoryInfo.CreationTime;
            LastWriteTime = directoryInfo.LastWriteTime;
            if (preloadDepth > 0)
            {
                preloadDepth--;
                foreach (DirectoryInfo c in directoryInfo.GetDirectories())
                    InnerSubFolders.Add(new FolderVM(c, preloadDepth));
            }
            foreach (FileInfo c in directoryInfo.GetFiles())
                InnerFiles.Add(new FileVM(c));
        }
    }
}
