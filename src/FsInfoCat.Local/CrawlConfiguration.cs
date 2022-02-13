using FsInfoCat.Activities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class CrawlJob : ICrawlJob
    {
        private DateTime _stopAt;
        private ulong _remainingtotalItems;
        private IActivityProgress _progress;

        public Guid? LogEntityId { get; }

        public Guid ConfigurationId { get; }

        public DateTime CrawlStart { get; }

        public string StatusMessage { get; private set; }

        public string StatusDetail { get; private set; }

        public long FoldersProcessed { get; private set; }

        public long FilesProcessed { get; private set; }

        public ulong TotalCount { get; private set; }

        public ushort MaxRecursionDepth { get; }

        public ulong? MaxTotalItems { get; private set; }

        public long? TTL { get; }

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
                    return (Array.Empty<CrawlContext>(), Array.Empty<(DbFile, FileInfo) >(), false);
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
                    return (Array.Empty<CrawlContext>(), Array.Empty<(DbFile, FileInfo) >(), true);
                }
                (CrawlContext[] childDirectories, IList<(DbFile Entity, FileInfo FileInfo)> files, bool stopCrawling) = await base.GetChildItemsAsync();
                if (stopCrawling)
                    return (childDirectories, files, true);
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
                    return (Array.Empty<CrawlContext>(), Array.Empty<(DbFile, FileInfo) >(), true);
                }
                (CrawlContext[] childDirectories, IList<(DbFile Entity, FileInfo FileInfo)> files, bool stopCrawling) = await base.GetChildItemsAsync();
                if (stopCrawling)
                    return (childDirectories, files, true);
                throw new NotImplementedException();
            }
        }
    }

    /// <summary>Specifies the configuration of a file system crawl.</summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalCrawlConfiguration" />
    public class CrawlConfiguration : CrawlConfigurationRow, ILocalCrawlConfiguration, ISimpleIdentityReference<CrawlConfiguration>
    {
        #region Fields

        private Subdirectory _root;
        private HashSet<CrawlJobLog> _logs = new();

        #endregion

        #region Properties

        public override Guid RootId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _root?.Id;
                    if (id.HasValue && id.Value != base.RootId)
                    {
                        base.RootId = id.Value;
                        return id.Value;
                    }
                    return base.RootId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _root?.Id;
                    if (id.HasValue && id.Value != value)
                        _root = null;
                    base.RootId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        /// <summary>Gets the starting subdirectory for the configured subdirectory crawl.</summary>
        /// <value>The root subdirectory of the configured subdirectory crawl.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Root), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public Subdirectory Root
        {
            get => _root;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_root is not null)
                            base.RootId = Guid.Empty;
                    }
                    else
                    {
                        base.RootId = value.Id;
                        _root = value;
                    }
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        /// <summary>Gets the crawl log entries.</summary>
        /// <value>The crawl log entries.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Logs), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<CrawlJobLog> Logs { get => _logs; set => _logs = value ?? new(); }

        #endregion

        ILocalSubdirectory ILocalCrawlConfiguration.Root => Root;

        ISubdirectory ICrawlConfiguration.Root => Root;

        IEnumerable<ICrawlJobLog> ICrawlConfiguration.Logs => Logs.Cast<ICrawlJobLog>();

        IEnumerable<ILocalCrawlJobLog> ILocalCrawlConfiguration.Logs => Logs.Cast<ILocalCrawlJobLog>();

        CrawlConfiguration IIdentityReference<CrawlConfiguration>.Entity => this;

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<CrawlConfiguration> builder)
        {
            _ = builder.HasOne(s => s.Root).WithOne(c => c.CrawlConfiguration).HasForeignKey<CrawlConfiguration>(nameof(RootId)).OnDelete(DeleteBehavior.Restrict);
        }

        record CrawlContext(DirectoryInfo Current, int Depth, long TotalCount);

        interface ICrawlContext
        {
            DirectoryInfo Directory { get; }
        }

        private async Task<CrawlStatus?> CrawlAsync<T>(LocalDbContext dbContext, T crawlContext, Subdirectory entity, IActivityProgress progress, Func<T, Subdirectory, (FileInfo[] Files, T[] Directories, CrawlStatus? Status)> crawl)
            where T : ICrawlContext
        {
            progress.Token.ThrowIfCancellationRequested();
            (FileInfo[] files, T[] subdirectories, CrawlStatus? status) = crawl(crawlContext, entity);
            if (status.HasValue)
                return status.Value;
            Guid parentId = entity.Id;
            Subdirectory[] subdirEntities = (await dbContext.Entry(entity).GetRelatedCollectionAsync(d => d.SubDirectories, progress.Token)).ToArray();
            DbFile[] fileEntities = (await dbContext.Entry(entity).GetRelatedCollectionAsync(d => d.Files, progress.Token)).ToArray();

            foreach (T ctx in subdirectories)
            {
                string name = ctx.Directory.Name;
                Subdirectory subdir = dbContext.Subdirectories.FirstOrDefault(e => e.ParentId == parentId && e.Name == name);
                if (subdir is null)
                {
                    subdir = new Subdirectory
                    {
                        Name = name,
                        CreationTime = ctx.Directory.CreationTime,
                        LastWriteTime = ctx.Directory.LastWriteTime,
                        Parent = entity,
                        Status = DirectoryStatus.Incomplete
                    };
                    dbContext.Subdirectories.Add(subdir);
                    await dbContext.SaveChangesAsync(progress.Token);
                }
                else
                status = await CrawlAsync<T>(dbContext, ctx, subdir, progress, crawl);
                if (status.HasValue)
                    return status.Value;
            }
            throw new NotImplementedException();
        }

        public static async Task<CrawlConfiguration> CrawlAsync(Guid crawlConfigurationId, IActivityProgress progress)
        {
            using IServiceScope scope = Hosting.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            CrawlConfiguration crawlConfiguration = dbContext.CrawlConfigurations.FirstOrDefault(e => e.Id == crawlConfigurationId);
            Guid id = crawlConfiguration.RootId;
            Subdirectory subdirectory = await dbContext.Subdirectories.FirstOrDefaultAsync(s => s.Id == id);
            string path = await subdirectory.GetFullNameAsync(dbContext, progress.Token);
            crawlConfiguration.StatusValue = CrawlStatus.InProgress;
            //try
            //{
            //    crawlConfiguration.StatusValue = await crawlConfiguration.CrawlAsync(dbContext, new DirectoryInfo(path), subdirectory, progress);
            //}
            //catch (OperationCanceledException)
            //{
            //    crawlConfiguration.StatusValue = CrawlStatus.Canceled;
            //}
            //catch (Exception error)
            //{
            //    crawlConfiguration.StatusValue = CrawlStatus.Failed;
            //}
            //finally
            //{
            //    dbContext.CrawlConfigurations.Update(crawlConfiguration);
            //    await dbContext.SaveChangesAsync(progress.Token);
            //}
            throw new NotImplementedException();
        }


        public static async Task RemoveAsync([DisallowNull] EntityEntry<CrawlConfiguration> entry, CancellationToken cancellationToken)
        {
            if ((entry ?? throw new ArgumentNullException(nameof(entry))).Context is not LocalDbContext dbContext)
                throw new InvalidOperationException();
            CrawlJobLog[] logs = (await entry.GetRelatedCollectionAsync(e => e.Logs, cancellationToken)).ToArray();
            if (logs.Length > 0)
                dbContext.CrawlJobLogs.RemoveRange(logs);
            dbContext.CrawlConfigurations.Remove(entry.Entity);
        }
    }
}
