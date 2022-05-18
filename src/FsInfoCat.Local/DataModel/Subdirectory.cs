using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Database entity that represents a subdirectory node within a file system on the local host machine.
    /// </summary>
    /// <seealso cref="SubdirectoryRow" />
    /// <seealso cref="ILocalSubdirectory" />
    /// <seealso cref="IEquatable{T}" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Subdirectory : SubdirectoryRow, ILocalSubdirectory, IEquatable<Subdirectory>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        #region Fields

        private readonly SubdirectoryReference _parent;
        private readonly VolumeReference _volume;
        private HashSet<DbFile> _files = new();
        private HashSet<Subdirectory> _subDirectories = new();
        private HashSet<SubdirectoryAccessError> _accessErrors = new();
        private HashSet<PersonalSubdirectoryTag> _personalTags = new();
        private HashSet<SharedSubdirectoryTag> _sharedTags = new();

        #endregion

        #region Properties

        public override Guid? ParentId { get => _parent.TryGetId(out Guid id) ? id : null; set => _parent.SetId(value); }

        public virtual Subdirectory Parent { get => _parent.Entity; set => _parent.Entity = value; }

        public override Guid? VolumeId { get => _volume.TryGetId(out Guid id) ? id : null; set => _volume.SetId(value); }

        public virtual Volume Volume { get => _volume.Entity; set => _volume.Entity = value; }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CrawlConfiguration), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public CrawlConfiguration CrawlConfiguration { get; set; }

        [NotNull]
        [BackingField(nameof(_files))]
        public virtual HashSet<DbFile> Files { get => _files; set => _files = value ?? new(); }

        [NotNull]
        [BackingField(nameof(_subDirectories))]
        public virtual HashSet<Subdirectory> SubDirectories { get => _subDirectories; set => _subDirectories = value ?? new(); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_accessErrors))]
        public virtual HashSet<SubdirectoryAccessError> AccessErrors { get => _accessErrors; set => _accessErrors = value ?? new(); }

        [NotNull]
        [BackingField(nameof(_personalTags))]
        public HashSet<PersonalSubdirectoryTag> PersonalTags { get => _personalTags; set => _personalTags = value ?? new(); }

        [NotNull]
        [BackingField(nameof(_sharedTags))]
        public HashSet<SharedSubdirectoryTag> SharedTags { get => _sharedTags; set => _sharedTags = value ?? new(); }

        #endregion

        #region Explicit Members

        ISubdirectory IDbFsItem.Parent { get => Parent; }

        ILocalVolume ILocalSubdirectory.Volume { get => Volume; }

        IVolume ISubdirectory.Volume { get => Volume; }

        IEnumerable<ILocalFile> ILocalSubdirectory.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> ISubdirectory.Files => Files.Cast<IFile>();

        IEnumerable<ILocalSubdirectory> ILocalSubdirectory.SubDirectories => SubDirectories.Cast<ILocalSubdirectory>();

        IEnumerable<ISubdirectory> ISubdirectory.SubDirectories => SubDirectories.Cast<ISubdirectory>();

        ILocalCrawlConfiguration ILocalSubdirectory.CrawlConfiguration { get => CrawlConfiguration; }

        ICrawlConfiguration ISubdirectory.CrawlConfiguration { get => CrawlConfiguration; }

        IEnumerable<ILocalSubdirectoryAccessError> ILocalSubdirectory.AccessErrors => AccessErrors.Cast<ILocalSubdirectoryAccessError>();

        IEnumerable<ISubdirectoryAccessError> ISubdirectory.AccessErrors => AccessErrors.Cast<ISubdirectoryAccessError>();

        ILocalSubdirectory ILocalDbFsItem.Parent => Parent;

        IEnumerable<ILocalAccessError> ILocalDbFsItem.AccessErrors => AccessErrors.Cast<ILocalAccessError>();

        IEnumerable<IAccessError> IDbFsItem.AccessErrors => AccessErrors.Cast<IAccessError>();

        ISubdirectory ISubdirectory.Parent => Parent;

        IEnumerable<ILocalPersonalSubdirectoryTag> ILocalSubdirectory.PersonalTags => PersonalTags.Cast<ILocalPersonalSubdirectoryTag>();

        IEnumerable<ILocalPersonalTag> ILocalDbFsItem.PersonalTags => PersonalTags.Cast<ILocalPersonalTag>();

        IEnumerable<IPersonalSubdirectoryTag> ISubdirectory.PersonalTags => PersonalTags.Cast<IPersonalSubdirectoryTag>();

        IEnumerable<IPersonalTag> IDbFsItem.PersonalTags => PersonalTags.Cast<IPersonalTag>();

        IEnumerable<ILocalSharedSubdirectoryTag> ILocalSubdirectory.SharedTags => SharedTags.Cast<ILocalSharedSubdirectoryTag>();

        IEnumerable<ILocalSharedTag> ILocalDbFsItem.SharedTags => SharedTags.Cast<ILocalSharedTag>();

        IEnumerable<ISharedSubdirectoryTag> ISubdirectory.SharedTags => SharedTags.Cast<ISharedSubdirectoryTag>();

        IEnumerable<ISharedTag> IDbFsItem.SharedTags => SharedTags.Cast<ISharedTag>();

        #endregion

        /// <summary>
        /// Creates a subdirectory entity.
        /// </summary>
        public Subdirectory()
        {
            _volume = new(SyncRoot);
            _parent = new(SyncRoot);
        }

        /// <summary>
        /// Asynchronously calculates the full path name of the current subdirectory.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="dbContext"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><see cref="EntityEntry.State"/> is either:
        /// <list type="bullet">
        ///     <item><see cref="EntityState.Deleted"/></item>
        ///     <item><see cref="EntityEntry.State"/> is <see cref="EntityState.Added"/> or <see cref="EntityState.Detached"/> and <see cref="ParentId"/> is <see langword="null"/></item>
        ///     <item>or <see cref="ParentId"/> and <see cref="VolumeId"/> are both <see langword="null"/></item>
        /// </list></exception>
        public async Task<string> GetFullNameAsync([DisallowNull] LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            EntityEntry<Subdirectory> entry = dbContext.Entry(this);
            Subdirectory parent = Parent;
            switch (entry.State)
            {
                case EntityState.Detached:
                case EntityState.Added:
                    if (parent is null)
                    {
                        Guid? parentId = ParentId;
                        if (!parentId.HasValue || (parent = await dbContext.Subdirectories.Where(d => d.Id == parentId).FirstOrDefaultAsync(cancellationToken)) is null)
                            throw new InvalidOperationException();
                        Parent = parent;
                    }
                    break;
                case EntityState.Deleted:
                    throw new InvalidOperationException();
                default:
                    if (parent is null)
                    {
                        if (ParentId.HasValue)
                        {
                            if ((parent = await entry.GetRelatedReferenceAsync(d => d.Parent, cancellationToken)) is null)
                                throw new InvalidOperationException();
                        }
                        else if (Volume is null)
                        {
                            Guid? volumeId = VolumeId;
                            if (!volumeId.HasValue || await entry.GetRelatedReferenceAsync(d => d.Volume, cancellationToken) is null)
                                throw new InvalidOperationException();
                        }
                    }
                    break;
            }
            if (parent is null)
                return Name;
            return Path.Combine(await parent.GetFullNameAsync(dbContext, cancellationToken), Name);
        }

        /// <summary>
        /// Asynchouusly finds the <see cref="Subdirectory"/> that matches the specified path.
        /// </summary>
        /// <param name="path">The file system path.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="onMatchSuccess">The optional callback to invoke if a match is found.</param>
        /// <returns>A <see cref="Task{Subdirectory}"/> that returns the matching <see cref="Subdirectory"/> or <see langword="null"/> if not match was found.</returns>
        public static Task<Subdirectory> FindByFullNameAsync(string path, CancellationToken cancellationToken, Action<LocalDbContext, Subdirectory, CancellationToken> onMatchSuccess = null)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrEmpty(path))
                return Task.FromResult<Subdirectory>(null);
            return FindByFullNameAsync(null, path, cancellationToken, onMatchSuccess);
        }

        /// <summary>
        /// Asynchouusly finds the <see cref="Subdirectory"/> that matches the specified path.
        /// </summary>
        /// <param name="path">The file system path.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{Subdirectory}"/> that returns the matching <see cref="Subdirectory"/> or <see langword="null"/> if not match was found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dbContext"/> is <see langword="null"/>.</exception>
        public static Task<Subdirectory> FindByFullNameAsync(string path, [DisallowNull] LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext));
            if (string.IsNullOrEmpty(path))
                return Task.FromResult<Subdirectory>(null);
            return FindByFullNameAsync(dbContext, path, cancellationToken);
        }

        /// <summary>
        /// Asynchouusly finds the <see cref="Subdirectory"/> that matches the specified path.
        /// </summary>
        /// <param name="dbContext">The database context or <see langword="null"/> to use a new database context.</param>
        /// <param name="path">The file system path.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="onMatchSuccess">The optional callback to invoke if a match is found or <see langword="null"/>.</param>
        /// <param name="toQueryable">The optional callback to query subdirectory entities or <see langword="null"/>.</param>
        /// <returns>A <see cref="Task{Subdirectory}"/> that returns the matching <see cref="Subdirectory"/> or <see langword="null"/> if not match was found.</returns>
        private static async Task<Subdirectory> FindByFullNameAsync(LocalDbContext dbContext, string path, CancellationToken cancellationToken,
            Action<LocalDbContext, Subdirectory, CancellationToken> onMatchSuccess = null, Func<DbSet<Subdirectory>, IQueryable<Subdirectory>> toQueryable = null)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!Path.IsPathFullyQualified(path))
                return null;
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            IFileSystemDetailService fileSystemDetailService = serviceScope.ServiceProvider.GetRequiredService<IFileSystemDetailService>();
            Subdirectory result;
            if (dbContext is null)
            {
                using LocalDbContext context = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                result = await FindByFullNameAsync((toQueryable is null) ? context.Subdirectories : toQueryable(context.Subdirectories), fileSystemDetailService, path, cancellationToken);
                if (result is not null)
                    onMatchSuccess?.Invoke(context, result, cancellationToken);
            }
            else if ((result = await FindByFullNameAsync((toQueryable is null) ? dbContext.Subdirectories : toQueryable(dbContext.Subdirectories), fileSystemDetailService, path, cancellationToken)) is not null)
                onMatchSuccess?.Invoke(dbContext, result, cancellationToken);
            return result;
        }

        private static Task<Subdirectory> FindByFullNameAsync<TProperty>(LocalDbContext dbContext, string path, CancellationToken cancellationToken, Func<DbSet<Subdirectory>, IQueryable<Subdirectory>> toQueryable) =>
            FindByFullNameAsync(dbContext, path, cancellationToken, null, toQueryable);

        //private static async Task<Subdirectory> FindByFullNameAsync([DisallowNull] LocalDbContext dbContext, IFileSystemDetailService fileSystemDetailService, string path, CancellationToken cancellationToken)
        //{
        //    cancellationToken.ThrowIfCancellationRequested();
        //    string directoryName = ExtensionMethods.SplitPath(path, out string leaf);
        //    Subdirectory subdirectory;
        //    if (string.IsNullOrEmpty(leaf))
        //    {
        //        if (fileSystemDetailService is not null)
        //        {
        //            ILogicalDiskInfo logicalDisk = (await fileSystemDetailService.GetLogicalDisksAsync(cancellationToken))
        //                .FirstOrDefault(d => string.Equals(d.Name, path, StringComparison.InvariantCultureIgnoreCase));
        //            if (logicalDisk is not null && logicalDisk.DriveType == DriveType.Network && !string.IsNullOrEmpty(logicalDisk.ProviderName))
        //                return await FindByFullNameAsync(dbContext, null, logicalDisk.ProviderName, cancellationToken);
        //        }
        //        return await (from d in dbContext.Subdirectories.Include(d => d.Parent) where d.VolumeId != null && d.Name == path select d).FirstOrDefaultAsync(cancellationToken);
        //    }
        //    subdirectory = await FindByFullNameAsync(dbContext, fileSystemDetailService, directoryName, cancellationToken);
        //    if (subdirectory is null)
        //        return null;
        //    Guid id = subdirectory.Id;
        //    return await (from d in dbContext.Subdirectories where d.ParentId == id && d.Name == leaf select d).FirstOrDefaultAsync(cancellationToken);
        //}

        private static async Task<Subdirectory> FindByFullNameAsync([DisallowNull] IQueryable<Subdirectory> subdirectories, IFileSystemDetailService fileSystemDetailService, string path, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string directoryName = ExtensionMethods.SplitPath(path, out string leaf);
            Subdirectory subdirectory;
            if (string.IsNullOrEmpty(leaf))
            {
                if (fileSystemDetailService is not null)
                {
                    ILogicalDiskInfo logicalDisk = (await fileSystemDetailService.GetLogicalDisksAsync(cancellationToken))
                        .FirstOrDefault(d => string.Equals(d.Name, path, StringComparison.InvariantCultureIgnoreCase));
                    if (logicalDisk is not null && logicalDisk.DriveType == DriveType.Network && !string.IsNullOrEmpty(logicalDisk.ProviderName))
                        return await FindByFullNameAsync(subdirectories, null, logicalDisk.ProviderName, cancellationToken);
                }
                return await subdirectories.Where(d => d.VolumeId != null && d.Name == path).FirstOrDefaultAsync(cancellationToken);
            }
            subdirectory = await FindByFullNameAsync(subdirectories, fileSystemDetailService, directoryName, cancellationToken);
            if (subdirectory is null)
                return null;
            Guid id = subdirectory.Id;
            return await subdirectories.Where(d => d.ParentId == id && d.Name == leaf).FirstOrDefaultAsync(cancellationToken);
        }

        [Obsolete("Use FsInfoCat.Local.Background.IDeleteBranchBackgroundService")]
        internal static async Task<bool> DeleteAsync([DisallowNull] Subdirectory target, [DisallowNull] LocalDbContext dbContext, CancellationToken cancellationToken,
            ItemDeletionOption deletionOption = ItemDeletionOption.Default)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext));
            EntityEntry<Subdirectory> entry = dbContext.Entry(target);
            if (!entry.ExistsInDb())
                return false;
            EntityEntry<CrawlConfiguration> oldCrawlConfiguration = await entry.GetRelatedTargetEntryAsync(e => e.CrawlConfiguration, cancellationToken);
            bool shouldDelete;
            switch (deletionOption)
            {
                case ItemDeletionOption.Default:
                    if (target.Options.HasFlag(DirectoryCrawlOptions.FlaggedForDeletion) || oldCrawlConfiguration is not null)
                    {
                        if (target.Status == DirectoryStatus.Deleted)
                            return false;
                        shouldDelete = false;
                        oldCrawlConfiguration = null;
                    }
                    else
                        shouldDelete = true;
                    break;
                case ItemDeletionOption.MarkAsDeleted:
                    if (target.Status == DirectoryStatus.Deleted)
                        return false;
                    shouldDelete = false;
                    oldCrawlConfiguration = null;
                    break;
                default:
                    shouldDelete = true;
                    break;
            }
            await entry.RemoveRelatedEntitiesAsync(e => e.PersonalTags, dbContext.PersonalSubdirectoryTags, cancellationToken);
            await entry.RemoveRelatedEntitiesAsync(e => e.SharedTags, dbContext.SharedSubdirectoryTags, cancellationToken);
            await entry.RemoveRelatedEntitiesAsync(e => e.AccessErrors, dbContext.SubdirectoryAccessErrors, cancellationToken);
            foreach (Subdirectory s in (await entry.GetRelatedCollectionAsync(e => e.SubDirectories, cancellationToken)))
            {
                if (!(await DeleteAsync(s, dbContext, cancellationToken, deletionOption)))
                    shouldDelete = false;
            }
            foreach (DbFile f in await entry.GetRelatedCollectionAsync(e => e.Files, cancellationToken))
            {
                if (!(await DbFile.DeleteAsync(f, dbContext, cancellationToken, deletionOption)))
                    shouldDelete = false;
            }
            if (shouldDelete)
            {
                if (oldCrawlConfiguration is not null)
                {
                    await CrawlConfiguration.RemoveAsync(oldCrawlConfiguration, cancellationToken);
                    dbContext.CrawlConfigurations.Remove(oldCrawlConfiguration.Entity);
                }
                return true;
            }
            target.Status = DirectoryStatus.Deleted;
            return false;
        }

        [Obsolete("Use FsInfoCat.Local.Background.IImportBranchBackgroundService")]
        public static async Task<EntityEntry<Subdirectory>> ImportBranchAsync([DisallowNull] DirectoryInfo directoryInfo, [DisallowNull] LocalDbContext dbContext,
            CancellationToken cancellationToken, bool markNewAsCompleted = false)
        {
            if (directoryInfo is null)
                throw new ArgumentNullException(nameof(directoryInfo));
            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext));

            Subdirectory result;
            if (directoryInfo.Parent is null)
            {
                EntityEntry<Volume> parentVolume = await Volume.ImportVolumeAsync(directoryInfo, dbContext, cancellationToken);
                result = await parentVolume.GetRelatedReferenceAsync(v => v.RootDirectory, cancellationToken);
                if (parentVolume.State == EntityState.Added)
                {
                    result = new Subdirectory
                    {
                        Id = Guid.NewGuid(),
                        Name = directoryInfo.Name,
                        CreatedOn = parentVolume.Entity.CreatedOn,
                        LastWriteTime = directoryInfo.LastWriteTime,
                        CreationTime = directoryInfo.CreationTime,
                        Volume = parentVolume.Entity,
                        Status = markNewAsCompleted ? DirectoryStatus.Complete : DirectoryStatus.Incomplete
                    };
                    _ = await dbContext.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    if (result is not null)
                        return dbContext.Entry(result);
                    result = new Subdirectory
                    {
                        Id = Guid.NewGuid(),
                        Name = directoryInfo.Name,
                        CreatedOn = DateTime.Now,
                        LastWriteTime = directoryInfo.LastWriteTime,
                        CreationTime = directoryInfo.CreationTime,
                        Volume = parentVolume.Entity,
                        Status = markNewAsCompleted ? DirectoryStatus.Complete : DirectoryStatus.Incomplete
                    };
                }
            }
            else
            {
                EntityEntry<Subdirectory> parent = await ImportBranchAsync(directoryInfo.Parent, dbContext, cancellationToken, true);
                if (parent.State == EntityState.Added)
                {
                    result = new Subdirectory
                    {
                        Id = Guid.NewGuid(),
                        Parent = parent.Entity,
                        Name = directoryInfo.Name,
                        CreatedOn = parent.Entity.CreatedOn,
                        LastWriteTime = directoryInfo.LastWriteTime,
                        CreationTime = directoryInfo.CreationTime,
                        Status = markNewAsCompleted ? DirectoryStatus.Complete : DirectoryStatus.Incomplete
                    };
                    _ = await dbContext.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    string name = directoryInfo.Name;
                    Guid parentId = parent.Entity.Id;
                    DbFile[] files = await (from f in dbContext.Files where f.ParentId == parentId && f.Name == name && f.Status != FileCorrelationStatus.Deleted select f).ToArrayAsync(cancellationToken);
                    if (files.Length > 0)
                    {
                        string[] names = directoryInfo.GetFiles().Select(f => f.Name).ToArray();
                        if (names.Length == 0 || (files = files.Where(f => !names.Contains(f.Name)).ToArray()).Length > 0)
                        {
                            foreach (DbFile f in files)
                                f.Status = FileCorrelationStatus.Deleted;
                            _ = await dbContext.SaveChangesAsync(cancellationToken);
                        }
                    }
                    Subdirectory[] subdirectories = await (from d in dbContext.Subdirectories where d.ParentId == parentId && d.Name == name select d).ToArrayAsync(cancellationToken);
                    if (subdirectories.Length == 1)
                    {
                        result = subdirectories[0];
                        if (result.Name == name && result.CreationTime == directoryInfo.CreationTime && result.LastWriteTime == directoryInfo.LastWriteTime)
                            return dbContext.Entry(result);
                        result.ModifiedOn = result.LastAccessed = DateTime.Now;
                    }
                    else if (subdirectories.Length > 1)
                    {
                        string[] names = directoryInfo.GetFiles().Select(f => f.Name).ToArray();
                        if ((result = subdirectories.FirstOrDefault(d => d.Name == name)) is not null)
                        {
                            if ((subdirectories = subdirectories.Where(d => d.Status != DirectoryStatus.Deleted && !(ReferenceEquals(d, result) || names.Contains(d.Name))).ToArray()).Length > 0)
                            {
                                foreach (Subdirectory d in subdirectories)
                                    await DeleteAsync(d, dbContext, cancellationToken);
                                await dbContext.SaveChangesAsync(cancellationToken);
                            }
                            if (result.Status == DirectoryStatus.Deleted)
                                result.Status = markNewAsCompleted ? DirectoryStatus.Complete : DirectoryStatus.Incomplete;
                            else if (result.CreationTime == directoryInfo.CreationTime && result.LastWriteTime == directoryInfo.LastWriteTime)
                                return dbContext.Entry(result);
                            result.ModifiedOn = result.LastAccessed = DateTime.Now;
                        }
                    }
                    else
                        result = null;
                    if (result is null)
                        result = new()
                        {
                            Id = Guid.NewGuid(),
                            Parent = parent.Entity,
                            Name = name,
                            CreationTime = directoryInfo.CreationTime,
                            LastWriteTime = directoryInfo.LastWriteTime,
                            CreatedOn = DateTime.Now,
                            Status = markNewAsCompleted ? DirectoryStatus.Complete : DirectoryStatus.Incomplete
                        };
                    else
                    {
                        result.Name = name;
                        result.CreationTime = directoryInfo.CreationTime;
                        result.LastWriteTime = directoryInfo.LastWriteTime;
                        result.ModifiedOn = result.LastAccessed = DateTime.Now;
                        return dbContext.Update(result);
                    }
                }
            }
            result.ModifiedOn = result.LastAccessed = result.CreatedOn;
            return dbContext.Subdirectories.Add(result);
        }

        [Obsolete("Use FsInfoCat.Local.Background.IMarkBranchIncompleteBackgroundService")]
        public static async Task MarkBranchIncompleteAsync(EntityEntry<Subdirectory> dbEntry, CancellationToken cancellationToken)
        {
            if (dbEntry.Context is not LocalDbContext dbContext)
                throw new InvalidOperationException();
            dbEntry.Entity.Status = DirectoryStatus.Incomplete;
            SubdirectoryAccessError[] accessErrors = (await dbEntry.GetRelatedCollectionAsync(d => d.AccessErrors, cancellationToken)).ToArray();
            if (accessErrors.Length > 0)
                dbContext.SubdirectoryAccessErrors.RemoveRange(accessErrors);
            foreach (Subdirectory subdirectory in await dbEntry.GetRelatedCollectionAsync(d => d.SubDirectories, cancellationToken))
                await MarkBranchIncompleteAsync(dbContext.Entry(subdirectory), cancellationToken);
            _ = await dbContext.SaveChangesAsync(cancellationToken);
        }

        [Obsolete("Use FsInfoCat.Local.Background.IMarkBranchIncompleteBackgroundService")]
        public async Task MarkBranchIncompleteAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            EntityEntry<Subdirectory> dbEntry = dbContext.Entry(this);
            switch (dbEntry.State)
            {
                case EntityState.Detached:
                case EntityState.Deleted:
                    throw new InvalidOperationException();
            }
            switch (Status)
            {
                case DirectoryStatus.Deleted:
                case DirectoryStatus.Incomplete:
                    return;
            }
            if (dbContext.Database.CurrentTransaction is null)
            {
                using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await MarkBranchIncompleteAsync(dbEntry, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await transaction.CommitAsync(cancellationToken);
            }
            else
                await MarkBranchIncompleteAsync(dbEntry, cancellationToken);
        }

        public void SetError(LocalDbContext dbContext, ErrorCode errorCode, Exception exception, string message = null)
        {
            _ = dbContext.SubdirectoryAccessErrors.Add(new SubdirectoryAccessError()
            {
                ErrorCode = errorCode,
                Message = message.GetDefaultIfNullOrWhiteSpace(() => $"An unexpected {exception.GetType().Name} has occurred."),
                Details = exception.ToString(),
                TargetId = Id
            });
            Status = DirectoryStatus.AccessError;
        }

        internal void SetUnauthorizedAccessError([DisallowNull] LocalDbContext dbContext, [DisallowNull] UnauthorizedAccessException exception) =>
            SetError(dbContext, ErrorCode.UnauthorizedAccess, exception);

        internal void SetSecurityError([DisallowNull] LocalDbContext dbContext, [DisallowNull] SecurityException exception) =>
            SetError(dbContext, ErrorCode.SecurityException, exception);

        internal void SetPathTooLongError([DisallowNull] LocalDbContext dbContext, [DisallowNull] PathTooLongException exception) =>
            SetError(dbContext, ErrorCode.PathTooLong, exception);

        internal void SetIOError([DisallowNull] LocalDbContext dbContext, [DisallowNull] IOException exception) =>
            SetError(dbContext, ErrorCode.IOError, exception);

        internal void SetUnspecifiedError([DisallowNull] LocalDbContext dbContext, [DisallowNull] Exception exception) =>
            SetError(dbContext, ErrorCode.Unexpected, exception);

        public record CrawlConfigWithFullRootPath<T>(string FullName, Guid SubdirectoryId, T Source);

        internal static void OnBuildEntity(EntityTypeBuilder<Subdirectory> builder)
        {
            _ = builder.HasOne(sn => sn.Parent).WithMany(d => d.SubDirectories).HasForeignKey(nameof(ParentId)).OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(sn => sn.Volume).WithOne(d => d.RootDirectory).HasForeignKey<Subdirectory>(nameof(VolumeId)).OnDelete(DeleteBehavior.Restrict);//.HasPrincipalKey<Volume>(nameof(Local.Volume.Id));
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            base.OnValidate(validationContext, results);
            if (string.IsNullOrWhiteSpace(validationContext.MemberName))
            {
                ValidationResult vr = results.FirstOrDefault(r => r.MemberNames.Contains(nameof(ParentId)));
                if (vr is not null)
                {
                    int index = results.IndexOf(vr);
                    _ = results.Remove(vr);
                    vr = new ValidationResult(vr.ErrorMessage, vr.MemberNames.Select(m => m == nameof(ParentId) ? nameof(Parent) : m).ToArray());
                    if (index < results.Count - 1)
                        results.Insert(index, vr);
                    else
                        results.Add(vr);
                }
                vr = results.FirstOrDefault(r => r.MemberNames.Contains(nameof(VolumeId)));
                if (vr is not null)
                {
                    int index = results.IndexOf(vr);
                    _ = results.Remove(vr);
                    vr = new ValidationResult(vr.ErrorMessage, vr.MemberNames.Select(m => m == nameof(VolumeId) ? nameof(Volume) : m).ToArray());
                    if (index < results.Count - 1)
                        results.Insert(index, vr);
                    else
                        results.Add(vr);
                }
            }
            else
                switch (validationContext.MemberName)
                {
                    case nameof(Parent):
                    case nameof(Volume):
                    case nameof(Name):
                        ValidateParentAndVolume(validationContext, results);
                        break;
                }
        }

        private void ValidateParentAndVolume(ValidationContext validationContext, List<ValidationResult> results)
        {
            Subdirectory parent = Parent;
            Volume volume = Volume;
            Guid id = Id;
            LocalDbContext dbContext;
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            if (parent is null)
            {
                if (volume is null)
                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_VolumeOrParentRequired, new string[] { nameof(Parent) }));
                else if ((dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>()) is not null)
                {
                    Guid volumeId = volume.Id;
                    var entities = from sn in dbContext.Subdirectories where id != sn.Id && sn.VolumeId == volumeId select sn;
                    if (entities.Any())
                        results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_VolumeHasRoot, new string[] { nameof(Volume) }));
                }
            }
            else if (volume is null)
            {
                if (Id.Equals(parent.Id))
                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_CircularReference, new string[] { nameof(Name) }));
                else if ((dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>()) is not null)
                {
                    string name = Name;
                    if (string.IsNullOrEmpty(name))
                        results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired, new string[] { nameof(Name) }));
                    else
                    {
                        Guid parentId = parent.Id;
                        var entities = from sn in dbContext.Subdirectories where id != sn.Id && sn.ParentId == parentId && sn.Name == name select sn;
                        if (entities.Any())
                            results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateName, new string[] { nameof(Name) }));
                        else
                            while (parent.ParentId.HasValue)
                            {
                                if (parent.ParentId.Value.Equals(Id))
                                {
                                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_CircularReference, new string[] { nameof(Name) }));
                                    break;
                                }
                                if ((parent = dbContext.Subdirectories.Find(parent.ParentId)) is null)
                                    break;
                            }
                    }
                }
            }
            else
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_VolumeAndParent, new string[] { nameof(Volume) }));
        }

        /// <summary>
        /// Tests whether the current database entity is equal to another.
        /// </summary>
        /// <param name="other">The <see cref="Subdirectory" /> to compare to.</param>
        /// <returns><see langword="true" /> if the <paramref name="other"/> entity is equal to the current entity; otherwise, <see langword="false" />.</returns>
        public bool Equals(Subdirectory other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        /// <summary>
        /// Tests whether the current database entity is equal to another.
        /// </summary>
        /// <param name="other">The <see cref="ILocalSubdirectory" /> to compare to.</param>
        /// <returns><see langword="true" /> if the <paramref name="other"/> entity is equal to the current entity; otherwise, <see langword="false" />.</returns>
        public bool Equals(ILocalSubdirectory other)
        {
            if (other is null) return false;
            if (other is Subdirectory subdirectory) return Equals(subdirectory);
            if (TryGetId(out Guid id1)) return other.TryGetId(out Guid id2) && id1.Equals(id2);
            return !other.TryGetId(out _) && ArePropertiesEqual(other);
        }

        /// <summary>
        /// Tests whether the current database entity is equal to another.
        /// </summary>
        /// <param name="other">The <see cref="ISubdirectory" /> to compare to.</param>
        /// <returns><see langword="true" /> if the <paramref name="other"/> entity is equal to the current entity; otherwise, <see langword="false" />.</returns>
        public bool Equals(ISubdirectory other)
        {
            if (other is null) return false;
            if (other is Subdirectory subdirectory) return Equals(subdirectory);
            if (TryGetId(out Guid id1)) return other.TryGetId(out Guid id2) && id1.Equals(id2);
            return !other.TryGetId(out _) && ((other is ILocalSubdirectory local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other));
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public override bool Equals(object obj)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (obj is null) return false;
            if (obj is Subdirectory subdirectory) return Equals(subdirectory);
            return obj is ISubdirectoryRow row && (TryGetId(out Guid id1) ? row.TryGetId(out Guid id2) && id1.Equals(id2) :
                (!row.TryGetId(out _) && ((row is ILocalSubdirectoryRow local) ? ArePropertiesEqual(local) : ArePropertiesEqual(row))));
        }

        public bool TryGetVolumeId(out Guid volumeId) => _volume.TryGetId(out volumeId);

        public bool TryGetParentId(out Guid subdirectoryId) => _parent.TryGetId(out subdirectoryId);

        protected class SubdirectoryReference : ForeignKeyReference<Subdirectory>, IForeignKeyReference<ILocalSubdirectory>, IForeignKeyReference<ISubdirectory>
        {
            internal SubdirectoryReference(object syncRoot) : base(syncRoot) { }

            ILocalSubdirectory IForeignKeyReference<ILocalSubdirectory>.Entity => Entity;

            ISubdirectory IForeignKeyReference<ISubdirectory>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<ILocalSubdirectory>>.Equals(IForeignKeyReference<ILocalSubdirectory> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ISubdirectory>>.Equals(IForeignKeyReference<ISubdirectory> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class VolumeReference : ForeignKeyReference<Volume>, IForeignKeyReference<ILocalVolume>, IForeignKeyReference<IVolume>
        {
            internal VolumeReference(object syncRoot) : base(syncRoot) { }

            ILocalVolume IForeignKeyReference<ILocalVolume>.Entity => Entity;

            IVolume IForeignKeyReference<IVolume>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<IVolume>>.Equals(IForeignKeyReference<IVolume> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalVolume>>.Equals(IForeignKeyReference<ILocalVolume> other)
            {
                throw new NotImplementedException();
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
