using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    // TODO: Document CrawlJob.CrawlContext class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public partial class CrawlJob
    {
        public abstract class CrawlContext
        {
            public CrawlJob Job { get; }

            public LocalDbContext DbContext { get; }

            public EntityEntry<Subdirectory> Entry { get; }

            public DirectoryInfo ParentDirectory { get; }

            public ushort Depth { get; }

            protected abstract CrawlContext Create(Subdirectory entity, DirectoryInfo subdir);

            protected CrawlContext(CrawlContext parentContext, Subdirectory entity, DirectoryInfo subdir)
            {
                (Job, DbContext) = (parentContext.Job, parentContext.DbContext);
                Entry = parentContext.DbContext.Entry(entity);
                ParentDirectory = subdir;
                Depth = (ushort)(parentContext.Depth + 1);
            }

            public virtual async Task<(CrawlContext[] Directories, IList<(DbFile Entity, FileInfo FileInfo)> Files, bool StopCrawling)> GetChildItemsAsync()
            {
                CancellationToken token = Job._progress.Token;
                DbFile[] fileEntities = (await Entry.GetRelatedCollectionAsync(e => e.Files, token)).ToArray();
                Subdirectory[] directoryEntities = (await Entry.GetRelatedCollectionAsync(e => e.SubDirectories, token)).ToArray();
                if (Entry.Entity.Options.HasFlag(DirectoryCrawlOptions.Skip))
                {
                    foreach (Subdirectory d in directoryEntities)
                        await Subdirectory.DeleteAsync(d, DbContext, token);
                    foreach (DbFile f in fileEntities)
                        await DbFile.DeleteAsync(f, DbContext, token);
                    return (Array.Empty<CrawlContext>(), Array.Empty<(DbFile, FileInfo)>(), false);
                }
                FileInfo[] fileInfos = ParentDirectory.GetFiles();
                DirectoryInfo[] childDirectories;
                if (Entry.Entity.Options.HasFlag(DirectoryCrawlOptions.SkipSubdirectories))
                {
                    if (directoryEntities.Length > 0)
                    {
                        foreach (Subdirectory d in directoryEntities)
                            await Subdirectory.DeleteAsync(d, DbContext, token);
                        directoryEntities = Array.Empty<Subdirectory>();
                    }
                    childDirectories = Array.Empty<DirectoryInfo>();
                }
                else
                {
                    childDirectories = ParentDirectory.GetDirectories();
                    if (Depth >= Job.MaxRecursionDepth)
                    {
                        if (directoryEntities.Length > 0)
                        {
                            foreach (Subdirectory d in directoryEntities)
                                await Subdirectory.DeleteAsync(d, DbContext, token);
                            directoryEntities = Array.Empty<Subdirectory>();
                        }
                        if (childDirectories.Length > 0)
                        {
                            // TODO: Log and write depth reached warning to db
                            childDirectories = Array.Empty<DirectoryInfo>();
                        }
                    }
                }
                List<(DbFile Entity, FileInfo FileInfo)> filePairs = fileEntities.ToMatchedPairs(fileInfos, out List<DbFile> uDbFile, out List<FileInfo> uFsFile);
                foreach ((DbFile entity, FileInfo fsItem) in filePairs)
                {
                    // TODO: Update entity
                }
                foreach (DbFile entity in uDbFile)
                    await DbFile.DeleteAsync(entity, DbContext, token, ItemDeletionOption.MarkAsDeleted);
                foreach (FileInfo fsItem in uFsFile)
                {
                    // TODO: Insert entity
                    //filePairs.Add((entity, fsItem));
                }

                ;
                List<(Subdirectory Entity, DirectoryInfo FileInfo)> dirPairs = directoryEntities.ToMatchedPairs(childDirectories, out List<Subdirectory> uDbDir, out List<DirectoryInfo> uFsDir);
                foreach ((Subdirectory entity, DirectoryInfo fsItem) in dirPairs)
                {
                    // TODO: Update entity
                }
                foreach (Subdirectory entity in uDbDir)
                    await Subdirectory.DeleteAsync(entity, DbContext, token, ItemDeletionOption.MarkAsDeleted);
                foreach (DirectoryInfo fsItem in uFsDir)
                {
                    // TODO: Insert entity
                    //dirPairs.Add((entity, fsItem));
                }
                return (dirPairs.Select(p => Create(p.Entity, p.FileInfo)).ToArray(), filePairs, false);
            }
        }

        public abstract class UnlimitedCrawlContext : CrawlContext
        {
            protected UnlimitedCrawlContext(UnlimitedCrawlContext parentContext, Subdirectory entity, DirectoryInfo subdir) : base(parentContext, entity, subdir)
            {
            }
        }

        public class ItemLimitedCrawlContext : CrawlContext
        {
            protected ItemLimitedCrawlContext(ItemLimitedCrawlContext parentContext, Subdirectory entity, DirectoryInfo subdir) : base(parentContext, entity, subdir)
            {
            }

            protected override CrawlContext Create(Subdirectory entity, DirectoryInfo subdir) => new ItemLimitedCrawlContext(this, entity, subdir);

            public async override Task<(CrawlContext[] Directories, IList<(DbFile Entity, FileInfo FileInfo)> Files, bool StopCrawling)> GetChildItemsAsync()
            {
                (CrawlContext[] childDirectories, IList<(DbFile Entity, FileInfo FileInfo)> files, bool stopCrawling) = await base.GetChildItemsAsync();
                if (stopCrawling)
                    return (childDirectories, files, true);
                (DbFile Entity, FileInfo FileInfo)[] skippedFiles;
                CrawlContext[] skippedSubdirs;
                if (Job._remainingtotalItems < (ulong)files.Count)
                {
                    skippedFiles = files.Skip((int)Job._remainingtotalItems).ToArray();
                    skippedSubdirs = childDirectories;
                    childDirectories = Array.Empty<CrawlContext>();
                    files = files.Take((int)Job._remainingtotalItems).ToArray();
                    Job._remainingtotalItems = 0UL;
                }
                else
                {
                    Job._remainingtotalItems -= (ulong)files.Count;
                    if (Job._remainingtotalItems < (ulong)childDirectories.Length)
                    {
                        skippedSubdirs = childDirectories.Skip((int)Job._remainingtotalItems).ToArray();
                        childDirectories = childDirectories.Take((int)Job._remainingtotalItems).ToArray();
                    }
                    else
                        return (childDirectories, files, (Job._remainingtotalItems -= (ulong)childDirectories.Length) == 0UL);
                }

                // TODO: Write warning to db about skipped subdirs and/or files.
                return (childDirectories, files, true);
            }
        }

        public sealed class LimitedCrawlContext : ItemLimitedCrawlContext
        {
            private LimitedCrawlContext(LimitedCrawlContext parentContext, Subdirectory entity, DirectoryInfo subdir) : base(parentContext, entity, subdir)
            {
            }

            protected override CrawlContext Create(Subdirectory entity, DirectoryInfo subdir) => new LimitedCrawlContext(this, entity, subdir);

            public async override Task<(CrawlContext[] Directories, IList<(DbFile Entity, FileInfo FileInfo)> Files, bool StopCrawling)> GetChildItemsAsync()
            {
                if (DateTime.Now > Job._stopAt)
                {
                    // TODO: Write warning to db
                    return (Array.Empty<CrawlContext>(), Array.Empty<(DbFile, FileInfo)>(), true);
                }
                (CrawlContext[] childDirectories, IList<(DbFile Entity, FileInfo FileInfo)> files, bool stopCrawling) = await base.GetChildItemsAsync();
                if (stopCrawling)
                    return (childDirectories, files, true);
                // TODO: Implement GetChildItemsAsync()
                throw new NotImplementedException();
            }
        }

        public class TimeLimitedCrawlContext : CrawlContext
        {
            protected TimeLimitedCrawlContext(TimeLimitedCrawlContext parentContext, Subdirectory entity, DirectoryInfo subdir) : base(parentContext, entity, subdir)
            {
            }

            protected override CrawlContext Create(Subdirectory entity, DirectoryInfo subdir) => new TimeLimitedCrawlContext(this, entity, subdir);

            public async override Task<(CrawlContext[] Directories, IList<(DbFile Entity, FileInfo FileInfo)> Files, bool StopCrawling)> GetChildItemsAsync()
            {
                if (DateTime.Now > Job._stopAt)
                {
                    // TODO: Write warning to db
                    return (Array.Empty<CrawlContext>(), Array.Empty<(DbFile, FileInfo)>(), true);
                }
                (CrawlContext[] childDirectories, IList<(DbFile Entity, FileInfo FileInfo)> files, bool stopCrawling) = await base.GetChildItemsAsync();
                if (stopCrawling)
                    return (childDirectories, files, true);
                // TODO: Implement GetChildItemsAsync()
                throw new NotImplementedException();
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
