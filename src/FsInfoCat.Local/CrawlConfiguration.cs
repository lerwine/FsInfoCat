using FsInfoCat.Activities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{

    /// <summary>Specifies the configuration of a file system crawl.</summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalCrawlConfiguration" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class CrawlConfiguration : CrawlConfigurationRow, ILocalCrawlConfiguration, ISimpleIdentityReference<CrawlConfiguration>, IEquatable<CrawlConfiguration>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        #region Fields

        private Guid? _rootId;
        private Subdirectory _root;
        private HashSet<CrawlJobLog> _logs = new();

        #endregion

        #region Properties

        public Guid RootId
        {
            get => _root?.Id ?? _rootId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_root is not null)
                    {
                        if (_root.Id.Equals(value)) return;
                        _root = null;
                    }
                    _rootId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        /// <summary>Gets the starting subdirectory for the configured subdirectory crawl.</summary>
        /// <value>The root subdirectory of the configured subdirectory crawl.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Root), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [BackingField(nameof(_root))]
        public Subdirectory Root
        {
            get => _root;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is not null && _root is not null && ReferenceEquals(value, _root)) return;
                    _rootId = null;
                    _root = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        /// <summary>Gets the crawl log entries.</summary>
        /// <value>The crawl log entries.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Logs), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [BackingField(nameof(_logs))]
        [NotNull]
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
                    status = await CrawlAsync(dbContext, ctx, subdir, progress, crawl);
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

        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalCrawlConfiguration other) => ArePropertiesEqual((ILocalCrawlConfigurationRow)other) &&
            RootId.Equals(other.RootId);

        protected virtual bool ArePropertiesEqual([DisallowNull] ICrawlConfiguration other)
        {
            if (ArePropertiesEqual((ICrawlConfigurationRow)other))
            {
                Guid? id = other.Root?.Id;
                return id.HasValue ? id.Value.Equals(RootId) : RootId.Equals(Guid.Empty);
            }
            return false;
        }

        public bool Equals(CrawlConfiguration other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (TryGetId(out Guid id))
                return other.TryGetId(out Guid g) && id.Equals(g);
            return !other.TryGetId(out _) && ArePropertiesEqual(other);
        }

        public bool Equals(ICrawlConfiguration other)
        {
            if (other is null) return false;
            if (other is CrawlConfiguration crawlConfiguration) return Equals(crawlConfiguration);
            if (TryGetId(out Guid id)) id.Equals(other.Id);
            if (!other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalCrawlConfiguration localCrawlConfig)
                return ArePropertiesEqual(localCrawlConfig);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is CrawlConfiguration crawlConfiguration) return Equals(crawlConfiguration);
            if (obj is ICrawlConfigurationRow row)
            {
                if (TryGetId(out Guid id)) id.Equals(row.Id);
                if (!row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalCrawlConfiguration localCrawlConfig)
                    return ArePropertiesEqual(localCrawlConfig);
                if (row is ICrawlConfiguration crawlConfig)
                    return ArePropertiesEqual(crawlConfig);
                if (row is (ILocalCrawlConfigurationRow localRow))
                    return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return false;
        }

        public bool TryGetRootId(out Guid rootId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_root is null)
                {
                    if (_rootId.HasValue)
                    {
                        rootId = _rootId.Value;
                        return true;
                    }
                }
                else
                    return _root.TryGetId(out rootId);
            }
            finally { Monitor.Exit(SyncRoot); }
            rootId = Guid.Empty;
            return false;
        }
    }
}
