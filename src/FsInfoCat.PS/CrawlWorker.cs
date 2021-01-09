using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using FsInfoCat.Models.Crawl;
using FsInfoCat.Models.HostDevices;

namespace FsInfoCat.PS
{
    public class CrawlWorker
    {
        private readonly CancellationToken _token;
        private readonly long _ttl;
        private readonly DateTime _stopAt;
        private readonly Stopwatch _stopWatch;
        private readonly FsCrawlJob _parentJob;
        private readonly Func<bool> _isExpired;
        private readonly int _maxDepth;
        private readonly long _maxItems;
        private readonly ReadOnlyCollection<string> _rootPaths;
        private int _currentItemIndex = -1;

        internal bool RanToCompletion { get; private set; } = false;

        internal CrawlWorker(CancellationToken token, int maxDepth, long maxItems, long ttl, Collection<string> rootPath, FsCrawlJob parentJob)
            : this(token, maxDepth, maxItems, (ttl < 0L) ? 0L : ttl, DateTime.MaxValue, rootPath, parentJob) { }

        internal CrawlWorker(CancellationToken token, int maxDepth, long maxItems, DateTime stopAt, Collection<string> rootPath, FsCrawlJob parentJob)
            : this(token, maxDepth, maxItems, -1L, stopAt, rootPath, parentJob) { }

        private CrawlWorker(CancellationToken token, int maxDepth, long maxItems, long ttl, DateTime stopAt, Collection<string> rootPath, FsCrawlJob parentJob)
        {
            _token = token;
            _maxDepth = (maxDepth < 0) ? 0 : maxDepth;
            _maxItems = (maxItems < 1L) ? long.MaxValue : maxItems;
            _rootPaths = new ReadOnlyCollection<string>((null == rootPath) ? new string[0] : rootPath.ToArray());
            _parentJob = parentJob;
            _ttl = ttl;
            _stopAt = stopAt;
            if (ttl < 0)
                _isExpired = new Func<bool>(() => _token.IsCancellationRequested || DateTime.Now >= _stopAt);
            else if (ttl == 0)
                _isExpired = new Func<bool>(() => _token.IsCancellationRequested);
            else
            {
                _stopWatch = new Stopwatch();
                _isExpired = new Func<bool>(() =>
                {
                    if (_token.IsCancellationRequested || _stopWatch.ElapsedMilliseconds >= _ttl)
                    {
                        if (_stopWatch.IsRunning)
                            _stopWatch.Stop();
                        return true;
                    }
                    return false;
                });
                return;
            }
            _stopWatch = null;
        }

        private CrawlError AddTargetError(object targetObject, Exception exception, MessageId errorId, ErrorCategory errorCategory, string activity, string reason, ErrorDetails details)
        {
            ErrorRecord errorRecord = new ErrorRecord(exception, Enum.GetName(typeof(MessageId), errorId), errorCategory, targetObject) { ErrorDetails = details };
            if (!string.IsNullOrWhiteSpace(activity))
                errorRecord.CategoryInfo.Activity = activity;
            if (!string.IsNullOrWhiteSpace(reason))
                errorRecord.CategoryInfo.Reason = reason;
            _parentJob.Error.Add(errorRecord);
            return new CrawlError(exception)
            {
                Message = errorRecord.ToString(),
                ID = errorId,
                Activity = errorRecord.CategoryInfo.Activity,
                Category = Enum.GetName(typeof(ErrorCategory), errorRecord.CategoryInfo.Category),
                Reason = errorRecord.CategoryInfo.Reason,
                TargetName = errorRecord.CategoryInfo.TargetName,
                TargetType = errorRecord.CategoryInfo.TargetType,
                RecommendedAction = (null == errorRecord.ErrorDetails) ? "" : errorRecord.ErrorDetails.RecommendedAction
            };
        }

        private CrawlError AddTargetError(object targetObject, Exception exception, MessageId errorId, ErrorCategory errorCategory, string activity, string reason)
        {
            return AddTargetError(targetObject, exception, errorId, errorCategory, activity, reason, null);
        }

        private CrawlError AddTargetError(object targetObject, Exception exception, MessageId errorId, ErrorCategory errorCategory, string activity, ErrorDetails details)
        {
            return AddTargetError(targetObject, exception, errorId, errorCategory, activity, null, details);
        }

        private CrawlError AddTargetError(object targetObject, Exception exception, MessageId errorId, ErrorCategory errorCategory, string activity)
        {
            return AddTargetError(targetObject, exception, errorId, errorCategory, activity, (string)null);
        }

        private CrawlError AddError(Exception exception, MessageId errorId, ErrorCategory errorCategory, string activity, string reason, ErrorDetails details)
        {
            return AddTargetError(null, exception, errorId, errorCategory, activity, reason, details);
        }

        private CrawlError AddError(Exception exception, MessageId errorId, ErrorCategory errorCategory, string activity, string reason)
        {
            return AddError(exception, errorId, errorCategory, activity, reason, null);
        }

        private CrawlError AddError(Exception exception, MessageId errorId, ErrorCategory errorCategory, string activity, ErrorDetails details)
        {
            return AddError(exception, errorId, errorCategory, activity, null, details);
        }

        private CrawlError AddError(Exception exception, MessageId errorId, ErrorCategory errorCategory, string activity)
        {
            return AddError(exception, errorId, errorCategory, activity, (string)null);
        }

        private CrawlError AddError(Exception exception, MessageId errorId, ErrorCategory errorCategory, ErrorDetails details)
        {
            return AddError(exception, errorId, errorCategory, null, details);
        }

        private CrawlError AddError(Exception exception, MessageId errorId, ErrorCategory errorCategory)
        {
            return AddError(exception, errorId, errorCategory, (string)null);
        }

        private CrawlWarning AddWarning(string message, MessageId warningId)
        {
            WarningRecord warningRecord = new WarningRecord(Enum.GetName(typeof(MessageId), warningId), message);
            _parentJob.Warning.Add(warningRecord);
            return new CrawlWarning(message, warningId);
        }

        public bool ProcessNext(out FsHost result)
        {
            if (_isExpired() || _currentItemIndex == _rootPaths.Count)
            {
                if (null != _stopWatch && _stopWatch.IsRunning)
                    _stopWatch.Stop();
                result = null;
                return false;
            }
            if (++_currentItemIndex == _rootPaths.Count)
            {
                if (null != _stopWatch && _stopWatch.IsRunning)
                    _stopWatch.Stop();
                RanToCompletion = true;
                if (_currentItemIndex == 0)
                    _parentJob.Warning.Add(new WarningRecord(Enum.GetName(typeof(MessageId), MessageId.NoPathProvided), "No paths were provided"));
                result = null;
                return false;
            }
            string currentPath = _rootPaths[_currentItemIndex];
            result = new FsHost { MachineName = Environment.MachineName };
            try { result.MachineIdentifier = HostDeviceRegRequest.GetLocalMachineIdentifier(); }
            catch (Exception exc)
            {
                result.Messages.Add(AddTargetError(result.MachineName, exc, MessageId.ErrorGettingMachineIdentifier, ErrorCategory.ResourceUnavailable, "Getting machine identifier"));
                result.Messages.Add(new CrawlWarning(MessageId.CrawlOperationStopped));
                return !_isExpired();
            }
            if (_isExpired())
            {
                result.Messages.Add(new CrawlWarning(MessageId.CrawlOperationStopped));
                return false;
            }
            try
            {
                if (null == currentPath)
                    throw new PSArgumentNullException("RootPath", "Path value is null at index " + _currentItemIndex);
                if (currentPath.Trim().Length == 0)
                    throw new PSArgumentException("Path value is empty at index" + _currentItemIndex, "RootPath");
            }
            catch (Exception exc)
            {
                result.Messages.Add(AddError(exc, MessageId.NoPathProvided, ErrorCategory.NotSpecified, "Validating path"));
                result.Messages.Add(new CrawlWarning(MessageId.CrawlOperationStopped));
                return !_isExpired();
            }
            DirectoryInfo currentDirectory;
            try { currentDirectory = (File.Exists(currentPath)) ? null : new DirectoryInfo(currentPath); }
            catch (Exception exc)
            {
                result.Messages.Add(AddTargetError(currentPath, exc, MessageId.InvalidPath, ErrorCategory.InvalidArgument, "Validating path"));
                result.Messages.Add(new CrawlWarning(MessageId.CrawlOperationStopped));
                return !_isExpired();
            }
            if (_isExpired())
            {
                result.Messages.Add(new CrawlWarning(MessageId.CrawlOperationStopped));
                return false;
            }
            IFsDirectory parentBranch;
            FsRoot fsRoot;
            ErrorCategory category = ErrorCategory.InvalidArgument;
            MessageId messageId = MessageId.InvalidPath;
            try
            {
                if (null == currentDirectory)
                {
                    category = ErrorCategory.NotSpecified;
                    messageId = MessageId.NoPathProvided;
                    throw new PSArgumentException("Path is not a subdirectory at index" + _currentItemIndex, "RootPath");
                }
                currentPath = currentDirectory.FullName;
                if (!currentDirectory.Exists)
                {
                    category = ErrorCategory.ObjectNotFound;
                    messageId = MessageId.PathNotFound;
                    throw new PSArgumentException("Directory not found at index" + _currentItemIndex, "RootPath");
                }
                fsRoot = FsDirectory.GetRoot(result, currentDirectory, out parentBranch);
            }
            catch (Exception exc)
            {
                result.Messages.Add(AddTargetError(currentPath, exc, messageId, category, "Validating path"));
                result.Messages.Add(new CrawlWarning(MessageId.CrawlOperationStopped));
                return !_isExpired();
            }
            if (_isExpired())
            {
                parentBranch.Messages.Add(new CrawlWarning(MessageId.CrawlOperationStopped));
                return false;
            }
            parentBranch.ChildNodes.Clear();
            Crawl(currentDirectory, parentBranch, fsRoot, _maxDepth);
            if (_isExpired())
            {
                parentBranch.Messages.Add(new CrawlWarning(MessageId.CrawlOperationStopped));
                return false;
            }
            return true;
        }

        private void Import(FileInfo file, int index, int count, IFsDirectory container, string parentPath)
        {
            string name;
            try
            {
                _parentJob.Debug.Add(new DebugRecord("Importing file " + file.FullName));
                name = file.Name;
            }
            catch (Exception exc)
            {
                container.Messages.Add(AddTargetError(file, exc, MessageId.FileSystemInfoPropertyAccessError, ErrorCategory.PermissionDenied, "Importing file #" + (index + 1) + " of " + count,
                    new ErrorDetails("Unable to obtain name of file #" + (index + 1) + " in " + parentPath)));
                return;
            }
            FsFile item = new FsFile { Name = name };
            string currentOperation = "Importing file " + name;
            try { item.Length = file.Length; }
            catch (Exception exc)
            {
                item.Length = -1L;
                item.Messages.Add(AddTargetError(file, exc, MessageId.FileSystemInfoPropertyAccessError, ErrorCategory.PermissionDenied, currentOperation,
                    new ErrorDetails("Unable to obtain length of file " + file.FullName)));
            }
            try { item.CreationTime = file.CreationTimeUtc; }
            catch (Exception exc)
            {
                item.Messages.Add(AddTargetError(file, exc, MessageId.CreationTimeAccessError, ErrorCategory.PermissionDenied, currentOperation,
                    new ErrorDetails("Unable to obtain creation time of file " + file.FullName)));
            }
            try { item.LastWriteTime = file.LastWriteTimeUtc; }
            catch (Exception exc)
            {
                item.Messages.Add(AddTargetError(file, exc, MessageId.LastWriteTimeAccessError, ErrorCategory.PermissionDenied, currentOperation,
                    new ErrorDetails("Unable to obtain lastWrite time of file " + file.FullName)));
            }
            try { item.Attributes = (int)file.Attributes; }
            catch (Exception exc)
            {
                item.Messages.Add(AddTargetError(file, exc, MessageId.AttributesAccessError, ErrorCategory.PermissionDenied, currentOperation,
                    new ErrorDetails("Unable to obtain file system attributes of file " + file.FullName)));
            }
            container.ChildNodes.Add(item);
            _parentJob.Debug.Add(new DebugRecord("Imported file " + file.FullName));
        }

        private FsDirectory Import(DirectoryInfo directory, int index, int count, IFsDirectory container, string parentPath)
        {
            string name;
            try
            {
                _parentJob.Debug.Add(new DebugRecord("Importing directory " + directory.FullName));
                name = directory.Name;
            }
            catch (Exception exc)
            {
                container.Messages.Add(AddTargetError(directory, exc, MessageId.FileSystemInfoPropertyAccessError, ErrorCategory.PermissionDenied, "Importing subdirectory #" + (index + 1) + " of " + count,
                    new ErrorDetails("Unable to obtain name of subdirectory #" + (index + 1) + " in " + parentPath)));
                return null;
            }
            string currentOperation = "Importing subdirectory " + name;
            FsDirectory item = new FsDirectory { Name = name };
            try { item.CreationTime = directory.CreationTimeUtc; }
            catch (Exception exc)
            {
                item.Messages.Add(AddTargetError(directory, exc, MessageId.CreationTimeAccessError, ErrorCategory.PermissionDenied, currentOperation,
                    new ErrorDetails("Unable to obtain creation time of subdirectory " + directory.FullName)));
            }
            try { item.LastWriteTime = directory.LastWriteTimeUtc; }
            catch (Exception exc)
            {
                item.Messages.Add(AddTargetError(directory, exc, MessageId.LastWriteTimeAccessError, ErrorCategory.PermissionDenied, currentOperation,
                    new ErrorDetails("Unable to obtain last write time of subdirectory " + directory.FullName)));
            }
            try { item.Attributes = (int)directory.Attributes; }
            catch (Exception exc)
            {
                item.Messages.Add(AddTargetError(directory, exc, MessageId.LastWriteTimeAccessError, ErrorCategory.PermissionDenied, currentOperation,
                    new ErrorDetails("Unable to obtain file system attributes of subdirectory " + directory.FullName)));
            }
            container.ChildNodes.Add(item);
            _parentJob.Debug.Add(new DebugRecord("Imported directory " + directory.FullName));
            return item;
        }

        private void Crawl(DirectoryInfo currentDirectory, IFsDirectory parentBranch, FsRoot fsRoot, int maxDepth)
        {
            if (_isExpired())
            {
                parentBranch.Messages.Add(new CrawlWarning(MessageId.CrawlOperationStopped));
                return;
            }
            _parentJob.Progress.Add(new ProgressRecord(FsCrawlJob.ACTIVITY_ID, FsCrawlJob.ACTIVITY, "Crawling " + currentDirectory.FullName));
            FileInfo[] filesArray;
            try
            {
                if (null == (filesArray = currentDirectory.GetFiles()))
                    filesArray = new FileInfo[0];
            }
            catch (Exception exc)
            {
                filesArray = new FileInfo[0];
                AddTargetError(currentDirectory, exc, MessageId.DirectoryFilesAccessError, ErrorCategory.ReadError, "Enumerating files for " + currentDirectory.FullName);
            }
            int index = -1;
            foreach (FileInfo file in filesArray)
            {
                if (_isExpired())
                {
                    parentBranch.Messages.Add(new CrawlWarning(MessageId.CrawlOperationStopped));
                    return;
                }

                Import(file, ++index, filesArray.Length, parentBranch, currentDirectory.FullName);
            }
            DirectoryInfo[] directoryArray;
            try
            {
                if (null == (directoryArray = currentDirectory.GetDirectories()))
                    directoryArray = new DirectoryInfo[0];
            }
            catch (Exception exc)
            {
                CrawlError crawlError = AddTargetError(currentDirectory, exc, MessageId.SubdirectoriesAccessError, ErrorCategory.ReadError, "Enumerating subdirectories for " + currentDirectory.FullName);
                if (maxDepth < 1)
                {
                    parentBranch.Messages.Add(AddWarning("Maximum depth reached " + currentDirectory.FullName + ". Could not dtermine if directories were skipped due to exception.", MessageId.MaxDepthReached));
                    return;
                }
                parentBranch.Messages.Add(crawlError);
                directoryArray = new DirectoryInfo[0];
            }
            if (maxDepth < 1)
            {
                if (directoryArray.Length == 1)
                    parentBranch.Messages.Add(AddWarning("Maximum depth reached for " + currentDirectory.FullName + ". 1 subdirectory skipped.", MessageId.MaxDepthReached));
                else if (directoryArray.Length > 1)
                    parentBranch.Messages.Add(AddWarning("Maximum depth reached " + currentDirectory.FullName + ". " + directoryArray.Length + " subdirectories skipped.", MessageId.MaxDepthReached));
                return;
            }

            index = -1;
            maxDepth--;
            foreach (DirectoryInfo directory in directoryArray)
            {
                if (_isExpired())
                {
                    parentBranch.Messages.Add(new CrawlWarning(MessageId.CrawlOperationStopped));
                    return;
                }
                Crawl(directory, Import(directory, ++index, directoryArray.Length, parentBranch, currentDirectory.FullName), fsRoot, maxDepth);
            }
        }
    }
}
