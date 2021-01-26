using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using FsInfoCat.Models.Crawl;

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
        /// <param name="startingDirectory">The filesystem path to be crawled.</param>
        /// <param name="maxItems">The maximum number of items to crawl.</param>
        /// <param name="job">The job running this crawler.</param>
        /// <param name="totalItems">The number of new items crawled.</param>
        /// <returns>True if ran to completion; otherwise, false.</returns>
        internal static bool Run(string startingDirectory, long maxItems, FsCrawlJob job, out long totalItems)
        {
            IFsDirectory parent = FsRoot.ImportDirectory(job.FsRoots, startingDirectory, job.GetVolumes, out string realPath);
            if (null == parent)
            {
                totalItems = 0L;
                return true;
            }
            if (job.IsExpired())
            {
                totalItems = 0L;
                return false;
            }
            CrawlWorker crawler = new CrawlWorker(maxItems, job);
            bool result = crawler.Crawl(parent, new DirectoryInfo(realPath), job.MaxDepth);
            totalItems = crawler._totalItems;
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
                    ErrorDetails = new ErrorDetails("Error reading files from " + parentDirectoryInfo.FullName)
                });
                files = new FileInfo[0];
                try { directories = parentDirectoryInfo.GetDirectories(); } catch { directories = new DirectoryInfo[0]; }
            }
            if (null == directories)
            {
                try { directories = parentDirectoryInfo.GetDirectories(); }
                catch (Exception exc)
                {
                    _job.Error.Add(new ErrorRecord(exc, MessageId.SubdirectoriesAccessError.ToString(), ErrorCategory.ReadError, parentDirectoryInfo)
                    {
                        ErrorDetails = new ErrorDetails("Error reading subdirectories from " + parentDirectoryInfo.FullName)
                    });
                    directories = new DirectoryInfo[0];
                }
            }
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fileInfo = files[i];
                FsFile fsFile = parent.ChildNodes.OfType<FsFile>().FirstOrDefault(f => f.Name.Equals(fileInfo.Name));
                if (null == fsFile)
                {
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
                    fsFile.Length = fileInfo.Length;
                    fsFile.CreationTime = fileInfo.CreationTimeUtc;
                    fsFile.LastWriteTime = fileInfo.LastWriteTimeUtc;
                    fsFile.Attributes = (int)fileInfo.Attributes;
                }
            }
            if (maxDepth < 1)
            {
                if (directories.Length > 0)
                    parent.Messages.Add(new CrawlWarning(MessageId.MaxDepthReached));
                return true;
            }
            for (int i = 0; i < directories.Length; i++)
            {
                DirectoryInfo directoryInfo = directories[i];
                FsDirectory fsDirectory = parent.ChildNodes.OfType<FsDirectory>().FirstOrDefault(f => f.Name.Equals(directoryInfo.Name));
                if (null == fsDirectory)
                {
                    ++_totalItems;
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
                    fsDirectory.CreationTime = directoryInfo.CreationTimeUtc;
                    fsDirectory.LastWriteTime = directoryInfo.LastWriteTimeUtc;
                    fsDirectory.Attributes = (int)directoryInfo.Attributes;
                }
                if (_totalItems == _maxItems || _job.IsExpired() || !Crawl(fsDirectory, directoryInfo, maxDepth - 1))
                {
                    IEnumerable<string> skippedItems = directories.Skip(i).Where(f => !parent.ChildNodes.OfType<FsDirectory>().Any(n => n.Name.Equals(f.Name))).Select(f => f.Name);
                    if (!skippedItems.Any())
                        return true;
                    parent.Messages.Add(new PartialCrawlWarning(MessageId.MaxItemsReached, skippedItems));
                    return false;
                }
            }
            return true;
        }
    }
}
