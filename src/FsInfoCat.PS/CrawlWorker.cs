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
            if (job.FsRoots.TryFind(new FileUri(directoryInfo), out FsRoot fsRoot))
                job.WriteDebug($"Using existing FsRoot({fsRoot.RootPathName})");
            else
            {
                if (job.RegisteredVolumes.TryFind(directoryInfo, out IVolumeInfo volume))
                {
                    job.WriteDebug($"Adding new FsRoot({volume})");
                    fsRoot = new FsRoot(volume);
                }
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
            parent = ImportDirectory(fsRoot, new DirectoryInfo(fsRoot.RootUri.ToLocalPath()), directoryInfo);
            if (parent is null)
            {
                job.WriteDebug("Failed to import parent directory");
                totalItems = 0L;
                return false;
            }
            job.WriteDebug("Crawling subdirectories");
            CrawlWorker crawler = new CrawlWorker(maxItems, job);
            bool result = crawler.Crawl(parent, directoryInfo, job.MaxDepth);
            totalItems = crawler._totalItems;
            return result;
        }

        private static IFsDirectory ImportDirectory(FsRoot fsRoot, DirectoryInfo rootDir, DirectoryInfo directoryInfo)
        {
            if (directoryInfo is null)
                return null;
            if (rootDir.FullName.Equals(directoryInfo.FullName, StringComparison.InvariantCultureIgnoreCase))
                return fsRoot;
            IFsDirectory parent = ImportDirectory(fsRoot, rootDir, directoryInfo.Parent);
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
                result = childNodes.OfType<FsDirectory>().FirstOrDefault(d => fsRoot.GetNameComparer().Equals(d.Name, n));
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
            return result;
        }

        private bool Crawl(IFsDirectory parent, DirectoryInfo parentDirectoryInfo, int maxDepth)
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
                // TODO: Need to do case-appropriate name lookups. Utilize FsInfoCat.Models.Volumes.IVolumeSetProvider?
                FsFile fsFile = parent.ChildNodes.OfType<FsFile>().FirstOrDefault(f => f.Name.Equals(fileInfo.Name));
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
                        // TODO: Need to do case-appropriate name lookups. Utilize FsInfoCat.Models.Volumes.IVolumeSetProvider?
                        IEnumerable<string> skippedItems = files.Skip(i).Where(f => !parent.ChildNodes.OfType<FsFile>().Any(n => n.Name.Equals(f.Name))).Select(f => f.Name);
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
                // TODO: Need to do case-appropriate name lookups. Utilize FsInfoCat.Models.Volumes.IVolumeSetProvider?
                // TODO: Need to do check if subdirectory matches any existing registered volumes and do not crawl if so.
                FsDirectory fsDirectory = parent.ChildNodes.OfType<FsDirectory>().FirstOrDefault(f => f.Name.Equals(directoryInfo.Name));
                if (fsDirectory is null)
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
                else
                {
                    _job.WriteDebug($"Updating existing FsDirectory {{ Name = \"{fsDirectory.Name}\", CreationTime = {directoryInfo.CreationTimeUtc}, LastWriteTime = {directoryInfo.LastWriteTimeUtc}, Attributes = {(int)directoryInfo.Attributes} }}");
                    fsDirectory.CreationTime = directoryInfo.CreationTimeUtc;
                    fsDirectory.LastWriteTime = directoryInfo.LastWriteTimeUtc;
                    fsDirectory.Attributes = (int)directoryInfo.Attributes;
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
                if (!Crawl(fsDirectory, directoryInfo, maxDepth - 1))
                {
                    _job.WriteDebug("Subdirectory crawl returned false.");
                    // TODO: Need to do case-appropriate name lookups. Utilize FsInfoCat.Models.Volumes.IVolumeSetProvider?
                    IEnumerable<string> skippedItems = directories.Skip(i).Where(f => !parent.ChildNodes.OfType<FsDirectory>().Any(n => n.Name.Equals(f.Name))).Select(f => f.Name);
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
