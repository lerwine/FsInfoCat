using FsInfoCat.Desktop.WMI;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FolderVM : DependencyObject
    {
        private readonly object _syncRoot = new();
        private CancellableTask<Task, int> _preload;

        private static readonly DependencyPropertyKey NamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Name), typeof(string), typeof(FolderVM), new PropertyMetadata(""));

        public static readonly DependencyProperty NameProperty = NamePropertyKey.DependencyProperty;
        private readonly ILogger<FolderVM> _logger;

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

        internal static Task<LinkedList<FolderVM>> FindByPathAsync(IEnumerable<FolderVM> logicalDisks, string path) => Task.Factory.StartNew(() => FindByPath(logicalDisks, path));

        private static LinkedList<FolderVM> FindByPath(IEnumerable<FolderVM> logicalDisks, string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            string name = System.IO.Path.GetFileName(path);
            string parentPath = System.IO.Path.GetDirectoryName(path);
            while (string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(parentPath))
            {
                name = System.IO.Path.GetFileName(parentPath);
                parentPath = System.IO.Path.GetDirectoryName(parentPath);
            }
            LinkedList<FolderVM> list;
            FolderVM item;
            if (string.IsNullOrEmpty(parentPath) || string.IsNullOrEmpty(name))
            {
                item = logicalDisks.FirstOrDefault(d => d.Dispatcher.Invoke(() => d.Path == path));
                if (item is null && path.Any(c => char.IsLetter(c)))
                {
                    StringComparer comparer = StringComparer.InvariantCultureIgnoreCase;
                    item = logicalDisks.FirstOrDefault(d => d.Dispatcher.Invoke(() => comparer.Equals(d.Path, path)));
                }
                if (item is null)
                    return null;
                list = new();
                _ = list.AddLast(item);
            }
            else if ((list = FindByPath(logicalDisks, parentPath)) is not null)
            {
                FolderVM result = list.Last.Value;
                ReadOnlyObservableCollection<FolderVM> subFolders = result.Dispatcher.Invoke(() => result.SubFolders);
                if (subFolders.Count == 0)
                {
                    if (!(result._preload?.Task.IsCompleted ?? false))
                    {
                        result.PreloadAsync(0).Wait();
                        subFolders = result.Dispatcher.Invoke(() => result.SubFolders);
                    }
                    if (subFolders.Count == 0)
                        return null;
                }
                item = result.Dispatcher.Invoke(() => subFolders.FirstOrDefault(d => d.Name == name));
                if (item is null && name.Any(c => char.IsLetter(c)))
                {
                    StringComparer comparer = StringComparer.InvariantCultureIgnoreCase;
                    item = result.Dispatcher.Invoke(() => subFolders.FirstOrDefault(d => comparer.Equals(d.Name, name)));
                }
                if (item is null)
                    return null;
                _ = list.AddLast(item);
            }
            return list;
        }

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

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(FolderVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as FolderVM).OnIsSelectedPropertyChanged(e)));

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        protected virtual void OnIsSelectedPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            if ((bool)args.NewValue)
                _ = PreloadAsync();
        }

        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(FolderVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as FolderVM).OnIsExpandedPropertyChanged(e)));

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        protected virtual void OnIsExpandedPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            if ((bool)args.NewValue)
                _ = PreloadAsync();
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

        private FolderVM(string name, DirectoryInfo directoryInfo)
        {
            _logger = App.GetLogger(this);
            Name = name;
            Path = directoryInfo.FullName;
            Attributes = directoryInfo.Attributes;
            CreationTime = directoryInfo.CreationTime;
            LastWriteTime = directoryInfo.LastWriteTime;
            InnerSubFolders = new();
            SubFolders = new(InnerSubFolders);
            InnerFiles = new();
            Files = new(InnerFiles);
        }

        private void Preload(int depth, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string path = Dispatcher.Invoke(() => Path);
            using IDisposable scope = _logger.BeginScope("Pre-loading {Path}; Depth={Depth}", path, depth);
            FolderVM[] subFolders;
            if (path is not null)
            {
                DirectoryInfo[] directories;
                FileInfo[] files;
                try
                {
                    DirectoryInfo directory = new(path);
                    directories = directory.GetDirectories();
                    files = directory.GetFiles();
                }
                catch (UnauthorizedAccessException unauthorizedAccessException)
                {
                    _logger.LogError(ErrorCode.UnauthorizedAccess.ToEventId(), unauthorizedAccessException, "Access to \"{Path}\" not authorized.", path);
                    Dispatcher.Invoke(() => AccessError = string.IsNullOrWhiteSpace(unauthorizedAccessException.Message) ? unauthorizedAccessException.ToString() : unauthorizedAccessException.Message);
                    return;
                }
                catch (SecurityException securityException)
                {
                    _logger.LogError(ErrorCode.SecurityException.ToEventId(), securityException, "Security error while trying to access \"{Path}\".", path);
                    Dispatcher.Invoke(() => AccessError = string.IsNullOrWhiteSpace(securityException.Message) ? securityException.ToString() : securityException.Message);
                    return;
                }
                catch (PathTooLongException pathTooLongException)
                {
                    _logger.LogError(ErrorCode.PathTooLong.ToEventId(), pathTooLongException, "Path too long: \"{Path}\".", path);
                    Dispatcher.Invoke(() => AccessError = string.IsNullOrWhiteSpace(pathTooLongException.Message) ? pathTooLongException.ToString() : pathTooLongException.Message);
                    return;
                }
                catch (DirectoryNotFoundException argumentException)
                {
                    _logger.LogError(ErrorCode.InvalidPath.ToEventId(), argumentException, "Directory does not exist: \"{Path}\".", path);
                    Dispatcher.Invoke(() => AccessError = string.IsNullOrWhiteSpace(argumentException.Message) ? argumentException.ToString() : argumentException.Message);
                    return;
                }
                catch (ArgumentException argumentException)
                {
                    _logger.LogError(ErrorCode.InvalidPath.ToEventId(), argumentException, "Possible invalid path: \"{Path}\".", path);
                    Dispatcher.Invoke(() => AccessError = string.IsNullOrWhiteSpace(argumentException.Message) ? argumentException.ToString() : argumentException.Message);
                    return;
                }
                catch (IOException ioException)
                {
                    _logger.LogError(ErrorCode.IOError.ToEventId(), ioException, "I/O error while trying to access \"{Path}\"", path);
                    Dispatcher.Invoke(() => AccessError = string.IsNullOrWhiteSpace(ioException.Message) ? ioException.ToString() : ioException.Message);
                    return;
                }
                catch (Exception exception)
                {
                    _logger.LogError(ErrorCode.Unexpected.ToEventId(), exception, "Unknown error while trying to access \"{Path}\"", path);
                    Dispatcher.Invoke(() => AccessError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message);
                    return;
                }
                cancellationToken.ThrowIfCancellationRequested();
                if (depth < 1)
                {
                    Dispatcher.Invoke(() =>
                    {
                        _logger.LogDebug("Populating SubFolders collection");
                        foreach (DirectoryInfo d in directories)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            InnerSubFolders.Add(new(d));
                        }
                        _logger.LogDebug("Populating Files collection");
                        foreach (FileInfo f in files)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            InnerFiles.Add(new(f));
                        }
                    });
                    return;
                }
                subFolders = Dispatcher.Invoke(() =>
                {
                    _logger.LogDebug("Populating SubFolders collection");
                    FolderVM[] folders = directories.Select(d =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        return new FolderVM(d);
                    }).ToArray();
                    foreach (FolderVM f in folders)
                        InnerSubFolders.Add(f);
                    _logger.LogDebug("Populating Files collection");
                    foreach (FileInfo f in files)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        InnerFiles.Add(new(f));
                    }
                    return folders;
                });
            }
            else
            {
                if (depth < 1)
                    return;
                _logger.LogDebug("Retrieving items from SubFolders collection");
                subFolders = Dispatcher.Invoke(() =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return InnerSubFolders.ToArray();
                });
            }
            if (subFolders.Length > 0)
            {
                _logger.LogDebug("Pre-loading {Count} SubFolders", subFolders.Length);
                depth--;
                foreach (FolderVM folder in subFolders)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    folder.PreloadAsync(depth).Wait();
                }
            }
            else
                _logger.LogDebug("No SubFolders to pre-load");
        }

        private void PreloadSubdirectories(int depth, CancellationToken cancellationToken)
        {
            using IDisposable scope = _logger.BeginScope("Pre-loading child items of {Path}; Depth={Depth}", Dispatcher.Invoke(() => Path), depth);
            FolderVM[] subFolders = Dispatcher.Invoke(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return InnerSubFolders.ToArray();
            });
            if (subFolders.Length > 0)
            {
                _logger.LogDebug("Pre-loading {Count} SubFolders", subFolders.Length);
                foreach (FolderVM folder in subFolders)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    folder.PreloadAsync(depth).Wait();
                }
            }
            else
                _logger.LogDebug("No SubFolders to pre-load");
        }

        internal Task PreloadAsync(int depth = 2)
        {
            CancellableTask<Task, int> preload;
            lock (_syncRoot)
            {
                if (_preload is null)
                {
                    CancellationTokenSource tokenSource = new();
                    _preload = preload = new CancellableTask<Task, int>
                    {
                        Task = Task.Factory.StartNew(() => Preload(depth, tokenSource.Token)),
                        TokenSource = tokenSource,
                        State = depth
                    };
                }
                else if (_preload.State < depth)
                {
                    CancellationTokenSource tokenSource = new();
                    _preload = preload = new CancellableTask<Task, int>
                    {
                        Task = Task.Factory.StartNew(() => PreloadSubdirectories(depth - 1, tokenSource.Token)),
                        TokenSource = tokenSource,
                        State = depth
                    };
                }
                else
                    return _preload.Task;
            }
            _ = preload.Task.ContinueWith(task =>
              {
                  lock (_syncRoot)
                  {
                      if (_preload is not null && ReferenceEquals(_preload, preload))
                          _preload = preload with { TokenSource = null };
                  }
                  preload.TokenSource.Dispose();
              });
            return preload.Task;
        }

        internal void CancelPreload(bool throwOnFirstException)
        {
            CancellableTask<Task, int> preload;
            lock (_syncRoot)
            {
                preload = _preload;
                _preload = null;
            }
            if (preload is not null && !(preload.TokenSource?.IsCancellationRequested ?? false))
                preload.TokenSource.Cancel(throwOnFirstException);
            if (Dispatcher.CheckAccess())
            {
                foreach (FolderVM folder in SubFolders)
                    folder.CancelPreload(throwOnFirstException);
            }
            else
                Dispatcher.Invoke(() =>
                {
                    foreach (FolderVM folder in SubFolders)
                        folder.CancelPreload(throwOnFirstException);
                });
        }

        internal FolderVM(Win32_LogicalDisk logicalDisk) : this((logicalDisk.DriveType == DriveType.Network && !string.IsNullOrWhiteSpace(logicalDisk.ProviderName)) ? $"{logicalDisk.DisplayName} ({logicalDisk.ProviderName})" :
            string.IsNullOrWhiteSpace(logicalDisk.VolumeName) ? logicalDisk.DisplayName : $"{logicalDisk.DisplayName} ({logicalDisk.VolumeName})",
            new DirectoryInfo(string.IsNullOrEmpty(logicalDisk.RootDirectory.Path) ? logicalDisk.RootDirectory.Name : logicalDisk.RootDirectory.Path)) { }

        internal FolderVM(DirectoryInfo directoryInfo) : this(directoryInfo.Name, directoryInfo) { }
    }
}
