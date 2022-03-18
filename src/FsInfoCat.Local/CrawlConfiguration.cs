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
    public class CrawlConfiguration : CrawlConfigurationRow, ILocalCrawlConfiguration, ISimpleIdentityReference<CrawlConfiguration>, IEquatable<CrawlConfiguration>
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

        protected bool ArePropertiesEqual([DisallowNull] ILocalCrawlConfiguration other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] ICrawlConfiguration other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(CrawlConfiguration other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

        public bool Equals(ICrawlConfiguration other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            Guid id = Id;
            if (id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 59;
                    hash = hash * 67 + DisplayName.GetHashCode();
                    hash = hash * 67 + MaxRecursionDepth.GetHashCode();
                    hash = EntityExtensions.HashNullable(MaxTotalItems, hash, 67);
                    hash = EntityExtensions.HashNullable(TTL, hash, 67);
                    hash = hash * 67 + Notes.GetHashCode();
                    hash = hash * 67 + StatusValue.GetHashCode();
                    hash = EntityExtensions.HashNullable(LastCrawlStart, hash, 67);
                    hash = EntityExtensions.HashNullable(LastCrawlEnd, hash, 67);
                    hash = EntityExtensions.HashNullable(NextScheduledStart, hash, 67);
                    hash = EntityExtensions.HashNullable(RescheduleInterval, hash, 67);
                    hash = hash * 67 + RescheduleFromJobEnd.GetHashCode();
                    hash = hash * 67 + RescheduleAfterFail.GetHashCode();
                    hash = EntityExtensions.HashGuid(RootId, hash, 67);
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 67);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 67);
                    hash = hash * 67 + CreatedOn.GetHashCode();
                    hash = hash * 67 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }
    }
}
