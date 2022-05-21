using FsInfoCat.Model;
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

            public Model.LocalDbContext DbContext { get; }

            public EntityEntry<Model.Subdirectory> Entry { get; }

            public DirectoryInfo ParentDirectory { get; }

            public ushort Depth { get; }

            protected abstract CrawlContext Create(Model.Subdirectory entity, DirectoryInfo subdir);

            protected CrawlContext(CrawlContext parentContext, Model.Subdirectory entity, DirectoryInfo subdir)
            {
                (Job, DbContext) = (parentContext.Job, parentContext.DbContext);
                Entry = parentContext.DbContext.Entry(entity);
                ParentDirectory = subdir;
                Depth = (ushort)(parentContext.Depth + 1);
            }

            public virtual async Task<(CrawlContext[] Directories, IList<(Model.DbFile Entity, FileInfo FileInfo)> Files, bool StopCrawling)> GetChildItemsAsync()
            {
                CancellationToken token = Job._progress.Token;
                Model.DbFile[] fileEntities = (await Entry.GetRelatedCollectionAsync(e => e.Files, token)).ToArray();
                Model.Subdirectory[] directoryEntities = (await Entry.GetRelatedCollectionAsync(e => e.SubDirectories, token)).ToArray();
                if (Entry.Entity.Options.HasFlag(DirectoryCrawlOptions.Skip))
                {
                    foreach (Model.Subdirectory d in directoryEntities)
                        await Model.Subdirectory.DeleteAsync(d, DbContext, token);
                    foreach (Model.DbFile f in fileEntities)
                        await Model.DbFile.DeleteAsync(f, DbContext, token);
                    return (Array.Empty<CrawlContext>(), Array.Empty<(Model.DbFile, FileInfo)>(), false);
                }
                FileInfo[] fileInfos = ParentDirectory.GetFiles();
                DirectoryInfo[] childDirectories;
                if (Entry.Entity.Options.HasFlag(DirectoryCrawlOptions.SkipSubdirectories))
                {
                    if (directoryEntities.Length > 0)
                    {
                        foreach (Model.Subdirectory d in directoryEntities)
                            await Model.Subdirectory.DeleteAsync(d, DbContext, token);
                        directoryEntities = Array.Empty<Model.Subdirectory>();
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
                            foreach (Model.Subdirectory d in directoryEntities)
                                await Model.Subdirectory.DeleteAsync(d, DbContext, token);
                            directoryEntities = Array.Empty<Model.Subdirectory>();
                        }
                        if (childDirectories.Length > 0)
                        {
                            // TODO: Log and write depth reached warning to db
                            childDirectories = Array.Empty<DirectoryInfo>();
                        }
                    }
                }
                List<(Model.DbFile Entity, FileInfo FileInfo)> filePairs = fileEntities.ToMatchedPairs(fileInfos, out List<Model.DbFile> uDbFile, out List<FileInfo> uFsFile);
                foreach ((Model.DbFile entity, FileInfo fsItem) in filePairs)
                {
                    // TODO: Update entity
                }
                foreach (Model.DbFile entity in uDbFile)
                    await Model.DbFile.DeleteAsync(entity, DbContext, token, ItemDeletionOption.MarkAsDeleted);
                foreach (FileInfo fsItem in uFsFile)
                {
                    // TODO: Insert entity
                    //filePairs.Add((entity, fsItem));
                }

                ;
                List<(Model.Subdirectory Entity, DirectoryInfo FileInfo)> dirPairs = directoryEntities.ToMatchedPairs(childDirectories, out List<Model.Subdirectory> uDbDir, out List<DirectoryInfo> uFsDir);
                foreach ((Model.Subdirectory entity, DirectoryInfo fsItem) in dirPairs)
                {
                    // TODO: Update entity
                }
                foreach (Model.Subdirectory entity in uDbDir)
                    await Model.Subdirectory.DeleteAsync(entity, DbContext, token, ItemDeletionOption.MarkAsDeleted);
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
            protected UnlimitedCrawlContext(UnlimitedCrawlContext parentContext, Model.Subdirectory entity, DirectoryInfo subdir) : base(parentContext, entity, subdir)
            {
            }
        }

        public class ItemLimitedCrawlContext : CrawlContext
        {
            protected ItemLimitedCrawlContext(ItemLimitedCrawlContext parentContext, Model.Subdirectory entity, DirectoryInfo subdir) : base(parentContext, entity, subdir)
            {
            }

            protected override CrawlContext Create(Model.Subdirectory entity, DirectoryInfo subdir) => new ItemLimitedCrawlContext(this, entity, subdir);

            public async override Task<(CrawlContext[] Directories, IList<(Model.DbFile Entity, FileInfo FileInfo)> Files, bool StopCrawling)> GetChildItemsAsync()
            {
                (CrawlContext[] childDirectories, IList<(Model.DbFile Entity, FileInfo FileInfo)> files, bool stopCrawling) = await base.GetChildItemsAsync();
                if (stopCrawling)
                    return (childDirectories, files, true);
                (Model.DbFile Entity, FileInfo FileInfo)[] skippedFiles;
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
            private LimitedCrawlContext(LimitedCrawlContext parentContext, Model.Subdirectory entity, DirectoryInfo subdir) : base(parentContext, entity, subdir)
            {
            }

            protected override CrawlContext Create(Model.Subdirectory entity, DirectoryInfo subdir) => new LimitedCrawlContext(this, entity, subdir);

            public async override Task<(CrawlContext[] Directories, IList<(Model.DbFile Entity, FileInfo FileInfo)> Files, bool StopCrawling)> GetChildItemsAsync()
            {
                if (DateTime.Now > Job._stopAt)
                {
                    // TODO: Write warning to db
                    return (Array.Empty<CrawlContext>(), Array.Empty<(Model.DbFile, FileInfo)>(), true);
                }
                (CrawlContext[] childDirectories, IList<(Model.DbFile Entity, FileInfo FileInfo)> files, bool stopCrawling) = await base.GetChildItemsAsync();
                if (stopCrawling)
                    return (childDirectories, files, true);
                // TODO: Implement GetChildItemsAsync()
                throw new NotImplementedException();
            }
        }

        public class TimeLimitedCrawlContext : CrawlContext
        {
            protected TimeLimitedCrawlContext(TimeLimitedCrawlContext parentContext, Model.Subdirectory entity, DirectoryInfo subdir) : base(parentContext, entity, subdir)
            {
            }

            protected override CrawlContext Create(Model.Subdirectory entity, DirectoryInfo subdir) => new TimeLimitedCrawlContext(this, entity, subdir);

            public async override Task<(CrawlContext[] Directories, IList<(Model.DbFile Entity, FileInfo FileInfo)> Files, bool StopCrawling)> GetChildItemsAsync()
            {
                if (DateTime.Now > Job._stopAt)
                {
                    // TODO: Write warning to db
                    return (Array.Empty<CrawlContext>(), Array.Empty<(Model.DbFile, FileInfo)>(), true);
                }
                (CrawlContext[] childDirectories, IList<(Model.DbFile Entity, FileInfo FileInfo)> files, bool stopCrawling) = await base.GetChildItemsAsync();
                if (stopCrawling)
                    return (childDirectories, files, true);
                // TODO: Implement GetChildItemsAsync()
                throw new NotImplementedException();
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
