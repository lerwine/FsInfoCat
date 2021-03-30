using FsInfoCat.Models.Crawl;
using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace FsInfoCat.PS
{
    public class CrawlWorker
    {
        private readonly long _maxItems;
        private readonly FsCrawlJob _job;
        private long _totalItems = 0L;

        internal CrawlWorker(long maxItems, FsCrawlJob job)
        {
            _maxItems = maxItems;
            _job = job;
        }

        /// <summary>
        /// Craws a subdirectory.
        /// </summary>
        /// <param name="startingDirectory">The starting subdirectory to be crawled.</param>
        /// <param name="maxItems">The maximum number of items to crawl.</param>
        /// <param name="job">The job running this crawler.</param>
        /// <param name="totalItems">The number of new items crawled.</param>
        /// <returns>True if ran to completion; otherwise, false.</returns>
        internal static bool Run(string startingDirectory, long maxItems, FsCrawlJob job, out long totalItems)
        {
            job.WriteDebug($"CrawlWorker processing \"{startingDirectory}\"; maxItems: {maxItems}");
            DirectoryInfo directoryInfo = new DirectoryInfo(startingDirectory);
            if (!directoryInfo.Exists)
            {
                job.WriteDebug("Directory does not exist.");
                totalItems = 0L;
                return true;
            }
            IFsDirectory parent;
            if (job.FsRoots.TryGetByChildURI(directoryInfo, out FsRoot fsRoot))
                job.WriteDebug($"Using existing FsRoot({fsRoot.RootPathName})");
            else
            {
                if (job.FsRoots.TryImportRoot(directoryInfo, out fsRoot))
                    job.WriteDebug($"Added new FsRoot({fsRoot})");
                else
                {
                    job.WriteDebug($"No existing FsRoot or registered volume found for \"{startingDirectory}\".");
                    totalItems = 0L;
                    return true;
                }
            }
            if (job.IsExpired())
            {
                job.WriteDebug("Job has reached time limit.");
                totalItems = 0L;
                return false;
            }
            parent = ImportDirectory(fsRoot, new DirectoryInfo(fsRoot.RootUri.ToLocalPath()).GetSegmentCount(), directoryInfo, out FileUri fileUri);
            if (parent is null)
            {
                job.WriteDebug("Failed to import parent directory");
                totalItems = 0L;
                return false;
            }
            job.WriteDebug("Crawling subdirectories");
            CrawlWorker crawler = new CrawlWorker(maxItems, job);
            bool result = crawler.Crawl(parent, directoryInfo, fileUri, job.MaxDepth, fsRoot.GetNameComparer());
            totalItems = crawler._totalItems;
            return result;
        }

        /// <summary>
        /// Imports a directory and its parents up to the specified root directory.
        /// </summary>
        /// <param name="fsRoot"></param>
        /// <param name="rootDir"></param>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        private static IFsDirectory ImportDirectory(FsRoot fsRoot, int minSegmentCount, DirectoryInfo directoryInfo, out FileUri fileUri)
        {
            if (directoryInfo is null)
            {
                fileUri = null;
                return null;
            }
            if (directoryInfo.GetSegmentCount() == minSegmentCount)
            {
                fileUri = fsRoot.RootUri;
                return fsRoot;
            }
            IFsDirectory parent = ImportDirectory(fsRoot, minSegmentCount, directoryInfo.Parent, out fileUri);
            if (parent is null)
                return null;
            string n = directoryInfo.Name;
            IList<IFsChildNode> childNodes = parent.ChildNodes;
            FsDirectory result;
            if (childNodes is null)
            {
                childNodes = new Collection<IFsChildNode>();
                parent.ChildNodes = childNodes;
            }
            else
            {
                IEqualityComparer<string> nameComparer = fsRoot.GetNameComparer();
                result = childNodes.OfType<FsDirectory>().FirstOrDefault(d => nameComparer.Equals(d.Name, n));
                if (!(result is null))
                    return result;
            }
            result = new FsDirectory
            {
                CreationTime = directoryInfo.CreationTimeUtc,
                LastWriteTime = directoryInfo.LastWriteTimeUtc,
                Attributes = (int)directoryInfo.Attributes,
                Name = directoryInfo.Name
            };
            childNodes.Add(result);
            fileUri = new FileUri(fileUri, result.Name);
            return result;
        }

        private bool Crawl(IFsDirectory parent, DirectoryInfo parentDirectoryInfo, FileUri parentUri, int maxDepth, IEqualityComparer<string> nameComparer)
        {
            _job.Progress.Add(new ProgressRecord(FsCrawlJob.ACTIVITY_ID, FsCrawlJob.ACTIVITY, "Reading directory contents") { CurrentOperation = parentDirectoryInfo.FullName });
            FileInfo[] files;
            DirectoryInfo[] directories;
            try
            {
                files = parentDirectoryInfo.GetFiles();
                directories = null;
            }
            catch (Exception exc)
            {
                _job.Error.Add(new ErrorRecord(exc, MessageId.DirectoryFilesAccessError.ToString(), ErrorCategory.ReadError, parentDirectoryInfo)
                {
                    ErrorDetails = new ErrorDetails($"Error reading files from {parentDirectoryInfo.FullName}")
                });
                files = new FileInfo[0];
                _job.WriteDebug("Reading subdirectories");
                try { directories = parentDirectoryInfo.GetDirectories(); } catch { directories = new DirectoryInfo[0]; }
            }
            if (directories is null)
            {
                _job.WriteDebug("Reading subdirectories");
                try { directories = parentDirectoryInfo.GetDirectories(); }
                catch (Exception exc)
                {
                    _job.Error.Add(new ErrorRecord(exc, MessageId.SubdirectoriesAccessError.ToString(), ErrorCategory.ReadError, parentDirectoryInfo)
                    {
                        ErrorDetails = new ErrorDetails($"Error reading subdirectories from {parentDirectoryInfo.FullName}")
                    });
                    directories = new DirectoryInfo[0];
                }
            }
            for (int i = 0; i < files.Length; i++)
            {
                _job.WriteDebug($"Importing file #{i}");
                FileInfo fileInfo = files[i];
                FsFile fsFile = parent.ChildNodes.OfType<FsFile>().FirstOrDefault(f => nameComparer.Equals(f.Name, fileInfo.Name));
                if (fsFile is null)
                {
                    _job.WriteDebug($"Adding new FsFile {{ Name = \"{fileInfo.Name}\", Length = {fileInfo.Length}, CreationTime = {fileInfo.CreationTimeUtc}, LastWriteTime = {fileInfo.LastWriteTimeUtc}, Attributes = {(int)fileInfo.Attributes} }}");
                    parent.ChildNodes.Add(new FsFile
                    {
                        Name = fileInfo.Name,
                        Length = fileInfo.Length,
                        CreationTime = fileInfo.CreationTimeUtc,
                        LastWriteTime = fileInfo.LastWriteTimeUtc,
                        Attributes = (int)fileInfo.Attributes
                    });
                    if (++_totalItems == _maxItems)
                    {
                        _job.WriteDebug("Reached item limit");
                        IEnumerable<string> skippedItems = files.Skip(i).Where(f => !parent.ChildNodes.OfType<FsFile>().Any(n => nameComparer.Equals(n.Name, f.Name)))
                            .Select(f => f.Name);
                        if (maxDepth > 0)
                            skippedItems = directories.Select(d => d.Name).Concat(skippedItems);
                        else if (directories.Length > 0)
                            parent.Messages.Add(new CrawlWarning(MessageId.MaxDepthReached));
                        if (!skippedItems.Any())
                            return true;
                        parent.Messages.Add(new PartialCrawlWarning(MessageId.MaxItemsReached, skippedItems));
                        return false;
                    }
                }
                else
                {
                    _job.WriteDebug($"Updating existing FsFile {{ Name = \"{fsFile.Name}\", Length = {fileInfo.Length}, CreationTime = {fileInfo.CreationTimeUtc}, LastWriteTime = {fileInfo.LastWriteTimeUtc}, Attributes = {(int)fileInfo.Attributes} }}");
                    fsFile.Length = fileInfo.Length;
                    fsFile.CreationTime = fileInfo.CreationTimeUtc;
                    fsFile.LastWriteTime = fileInfo.LastWriteTimeUtc;
                    fsFile.Attributes = (int)fileInfo.Attributes;
                }
            }
            if (maxDepth < 1)
            {
                _job.WriteDebug("Depth limit");
                if (directories.Length > 0)
                    parent.Messages.Add(new CrawlWarning(MessageId.MaxDepthReached));
                return true;
            }
            for (int i = 0; i < directories.Length; i++)
            {
                _job.WriteDebug($"Importing subdirectory #{i}");
                DirectoryInfo directoryInfo = directories[i];
                FileUri childUri = new FileUri(parentUri, directoryInfo.Name);
                if (_job.FsRoots.TryGetValue(childUri, out FsRoot v))
                {
                    _job.WriteDebug($"Skipping subdirectory #{i} because it is the root of volume {v.VolumeName} ({v.Identifier}).");
                    continue;
                }
                if (parent.ChildNodes.OfType<FsDirectory>().TryFindByName(directoryInfo.Name, nameComparer, out FsDirectory fsDirectory))
                {
                    _job.WriteDebug($"Updating existing FsDirectory {{ Name = \"{fsDirectory.Name}\", CreationTime = {directoryInfo.CreationTimeUtc}, LastWriteTime = {directoryInfo.LastWriteTimeUtc}, Attributes = {(int)directoryInfo.Attributes} }}");
                    fsDirectory.CreationTime = directoryInfo.CreationTimeUtc;
                    fsDirectory.LastWriteTime = directoryInfo.LastWriteTimeUtc;
                    fsDirectory.Attributes = (int)directoryInfo.Attributes;
                }
                else
                {
                    ++_totalItems;
                    _job.WriteDebug($"Adding new FsDirectory {{ Name = \"{directoryInfo.Name}\", CreationTime = {directoryInfo.CreationTimeUtc}, LastWriteTime = {directoryInfo.LastWriteTimeUtc}, Attributes = {(int)directoryInfo.Attributes} }}");
                    fsDirectory = new FsDirectory
                    {
                        Name = directoryInfo.Name,
                        CreationTime = directoryInfo.CreationTimeUtc,
                        LastWriteTime = directoryInfo.LastWriteTimeUtc,
                        Attributes = (int)directoryInfo.Attributes
                    };
                    parent.ChildNodes.Add(fsDirectory);
                }
                if (_totalItems == _maxItems)
                {
                    _job.WriteDebug("Reached item limit");
                    return false;
                }
                else if (_job.IsExpired())
                {
                    _job.WriteDebug("Job has reached time limit.");
                    return false;
                }
                if (!Crawl(fsDirectory, directoryInfo, childUri, maxDepth - 1, nameComparer))
                {
                    _job.WriteDebug("Subdirectory crawl returned false.");
                    IEnumerable<string> skippedItems = directories.Skip(i).Where(f => !parent.ChildNodes.OfType<FsDirectory>().Any(n => nameComparer.Equals(n.Name, f.Name)))
                        .Select(f => f.Name);
                    if (!skippedItems.Any())
                        return true;
                    parent.Messages.Add(new PartialCrawlWarning(MessageId.MaxItemsReached, skippedItems));
                    return false;
                }
                _job.WriteDebug("Subdirectory crawl returned true.");
            }
            return true;
        }
    }
}
