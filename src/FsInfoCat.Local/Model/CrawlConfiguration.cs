using FsInfoCat.Model;
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

namespace FsInfoCat.Local.Model;

/// <summary>
/// Specifies the configuration of a file system crawl.
/// </summary>
/// <seealso cref="LocalDbContext.CrawlConfigurations" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    // CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
public partial class CrawlConfiguration : CrawlConfigurationRow, ILocalCrawlConfiguration, IEquatable<CrawlConfiguration>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
{
    #region Fields

    private readonly SubdirectoryReference _root;
    private HashSet<CrawlJobLog> _logs = [];

    #endregion

    #region Properties

    /// <summary>
    /// Gets the primary key of the root directory entity.
    /// </summary>
    /// <value>The primary key value of the root <see cref="Subdirectory"/> entity.</value>
    public override Guid RootId { get => _root.Id; set => _root.SetId(value); }

    /// <summary>
    /// Gets the starting subdirectory for the configured subdirectory crawl.
    /// </summary>
    /// <value>The root subdirectory of the configured subdirectory crawl.</value>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.RootSubdirectory), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public Subdirectory Root { get => _root.Entity; set => _root.Entity = value; }

    /// <summary>
    /// Gets the crawl log entries.
    /// </summary>
    /// <value>The crawl log entries.</value>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Logs), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [BackingField(nameof(_logs))]
    [NotNull]
    public virtual HashSet<CrawlJobLog> Logs { get => _logs; set => _logs = value ?? []; }

    #endregion

    ILocalSubdirectory ILocalCrawlConfiguration.Root => Root;

    ISubdirectory ICrawlConfiguration.Root => Root;

    IEnumerable<ICrawlJobLog> ICrawlConfiguration.Logs => Logs.Cast<ICrawlJobLog>();

    IEnumerable<ILocalCrawlJobLog> ILocalCrawlConfiguration.Logs => Logs.Cast<ILocalCrawlJobLog>();

    /// <summary>
    /// Initializes a new CrawlConfiguration entity.
    /// </summary>
    public CrawlConfiguration() { _root = new(SyncRoot); }

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
        // TODO: Implement CrawlAsync<T>(LocalDbContext, T, Subdirectory, IActivityProgress, Func<T, Subdirectory, (FileInfo[] Files, T[] Directories, CrawlStatus? Status)>)
        throw new NotImplementedException();
    }

    /// <summary>
    /// Asynchronously starts a new filesystem crawl.
    /// </summary>
    /// <param name="crawlConfigurationId">The unique identifier of the <see cref="CrawlConfiguration"/> that defines what to crawl.</param>
    /// <param name="progress">The object that is used for reporting progress.</param>
    /// <returns>A task that returns the <see cref="CrawlConfiguration"/> that defines what has been crawled.</returns>
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
        // TODO: Implement CrawlAsync(Guid, IActivityProgress)
        throw new NotImplementedException();
    }

    /// <summary>
    /// Asynchronously removes a <see cref="CrawlConfiguration"/> from the database.
    /// </summary>
    /// <param name="entry">The entity entry of the <see cref="CrawlConfiguration"/> to remove.</param>
    /// <param name="cancellationToken">The token that is observed to determine if the asynchronous task should be canceled.</param>
    /// <returns>The <see cref="Task"/> for the asynchronous delete operation.</returns>
    public static async Task RemoveAsync([DisallowNull] EntityEntry<CrawlConfiguration> entry, CancellationToken cancellationToken)
    {
        if ((entry ?? throw new ArgumentNullException(nameof(entry))).Context is not LocalDbContext dbContext)
            throw new InvalidOperationException();
        CrawlJobLog[] logs = (await entry.GetRelatedCollectionAsync(e => e.Logs, cancellationToken)).ToArray();
        if (logs.Length > 0)
            dbContext.CrawlJobLogs.RemoveRange(logs);
        dbContext.CrawlConfigurations.Remove(entry.Entity);
    }

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="ILocalCrawlConfiguration" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected virtual bool ArePropertiesEqual([DisallowNull] ILocalCrawlConfiguration other) => ArePropertiesEqual((ILocalCrawlConfigurationRow)other) &&
        RootId.Equals(other.RootId);

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="ICrawlConfiguration" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected virtual bool ArePropertiesEqual([DisallowNull] ICrawlConfiguration other)
    {
        if (ArePropertiesEqual((ICrawlConfigurationRow)other))
        {
            Guid? id = other.Root?.Id;
            return id.HasValue ? id.Value.Equals(RootId) : RootId.Equals(Guid.Empty);
        }
        return false;
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public bool Equals(CrawlConfiguration other) => other is not null && (ReferenceEquals(this, other) ||
        (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

    public bool Equals(ILocalCrawlConfiguration other)
    {
        if (other is null) return false;
        if (other is CrawlConfiguration crawlConfiguration) return Equals(crawlConfiguration);
        if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
        return !other.TryGetId(out _) && ArePropertiesEqual(other);
    }

    public bool Equals(ICrawlConfiguration other)
    {
        if (other is null) return false;
        if (other is CrawlConfiguration crawlConfiguration) return Equals(crawlConfiguration);
        if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
        if (other.TryGetId(out _)) return false;
        if (other is ILocalCrawlConfiguration localCrawlConfig) return ArePropertiesEqual(localCrawlConfig);
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

    protected override string PropertiesToString() => $@"RootId{_root.IdValue},
    {base.PropertiesToString()}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Gets the unique identifier of the <see cref="Root" /> entity if it has been assigned.
    /// </summary>
    /// <param name="rootId">Receives the unique identifier value.</param>
    /// <returns><see langword="true" /> if the unique identifier of the <see cref="Root" /> entity has been set; otherwise, <see langword="false" />.</returns>
    public bool TryGetRootId(out Guid rootId) => _root.TryGetId(out rootId);
}
