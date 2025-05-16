using FsInfoCat.Model;
using FsInfoCat.Activities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Interface for entities which represent a specific file system type.
/// </summary>
/// <seealso cref="FileSystemListItem" />
/// <seealso cref="LocalDbContext.FileSystems" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
// CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
public class FileSystem : FileSystemRow, ILocalFileSystem, IEquatable<FileSystem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
{
    #region Fields

    private HashSet<Volume> _volumes = [];
    private HashSet<SymbolicName> _symbolicNames = [];

    #endregion

    #region Properties

    /// <summary>
    /// Gets the volumes that share this file system type.
    /// </summary>
    /// <value>The volumes that share this file system type.</value>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Volumes), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_volumes))]
    public virtual HashSet<Volume> Volumes { get => _volumes; set => _volumes = value ?? []; }

    /// <summary>
    /// Gets the symbolic names for the current file system type.
    /// </summary>
    /// <value>The symbolic names that are used to identify the current file system type.</value>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.SymbolicNames), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_symbolicNames))]
    public virtual HashSet<SymbolicName> SymbolicNames { get => _symbolicNames; set => _symbolicNames = value ?? []; }

    #endregion

    #region Explicit Members

    IEnumerable<ILocalVolume> ILocalFileSystem.Volumes => _volumes.Cast<ILocalVolume>();

    IEnumerable<ILocalSymbolicName> ILocalFileSystem.SymbolicNames => _volumes.Cast<ILocalSymbolicName>();

    IEnumerable<IVolume> IFileSystem.Volumes => _volumes.Cast<IVolume>();

    IEnumerable<ISymbolicName> IFileSystem.SymbolicNames => _volumes.Cast<ISymbolicName>();

    #endregion

    /// <summary>
    /// Asynchronously imports file system information into the database.
    /// </summary>
    /// <param name="diskInfo">The object containing logical disk information.</param>
    /// <param name="volumeIdentifier">The identifier of the volume.</param>
    /// <param name="dbContext">The database to add the file system to.</param>
    /// <param name="fileSystemDetailService">The object that can read extended file properties.</param>
    /// <param name="cancellationToken">The token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that returns a tuple which contains the database entity that was added and the symbolic name of the file system type.</returns>
    public static async Task<(EntityEntry<FileSystem> Entry, SymbolicName SymbolicName)> ImportFileSystemAsync([AllowNull] ILogicalDiskInfo diskInfo, VolumeIdentifier volumeIdentifier, [DisallowNull] LocalDbContext dbContext,
        [DisallowNull] IFileSystemDetailService fileSystemDetailService, CancellationToken cancellationToken)
    {
        string name;
        FileSystem fileSystem;
        SymbolicName symbolicName;
        if (diskInfo is null)
        {
            if (!volumeIdentifier.Location.IsUnc)
                throw new ArgumentOutOfRangeException(nameof(volumeIdentifier));
            (IFileSystemProperties Properties, string SymbolicName) genericNetworkFsType = fileSystemDetailService.GetGenericNetworkShareFileSystem();
            name = genericNetworkFsType.SymbolicName;

            symbolicName = await (from sn in dbContext.SymbolicNames.Include(n => n.FileSystem) where sn.Name == name select sn).FirstOrDefaultAsync(cancellationToken);
            if (symbolicName is not null)
                return (dbContext.Entry(symbolicName.FileSystem), symbolicName);
            fileSystem = new()
            {
                Id = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                DisplayName = "Network File System",
                DefaultDriveType = genericNetworkFsType.Properties.DefaultDriveType,
                MaxNameLength = genericNetworkFsType.Properties.MaxNameLength,
                ReadOnly = genericNetworkFsType.Properties.ReadOnly
            };
        }
        else
        {
            name = diskInfo.FileSystemName;
            symbolicName = await (from sn in dbContext.SymbolicNames.Include(n => n.FileSystem) where sn.Name == name select sn).FirstOrDefaultAsync(cancellationToken);
            if (symbolicName is not null)
                return (dbContext.Entry(symbolicName.FileSystem), symbolicName);
            fileSystem = new()
            {
                Id = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                DisplayName = "Network File System",
                DefaultDriveType = diskInfo.DriveType,
                MaxNameLength = diskInfo.MaxNameLength,
                ReadOnly = diskInfo.IsReadOnly
            };
        }
        fileSystem.ModifiedOn = fileSystem.CreatedOn;

        return (dbContext.FileSystems.Add(fileSystem), new()
        {
            Id = Guid.NewGuid(),
            CreatedOn = fileSystem.CreatedOn,
            ModifiedOn = fileSystem.ModifiedOn,
            Name = name,
            FileSystem = fileSystem,
            Priority = 0
        });
    }

    /// <summary>
    /// Asynchronously deletes a <see cref="FileSystem"/> entity from the database.
    /// </summary>
    /// <param name="target">The filesystem to be deleted.</param>
    /// <param name="dbContext">The target database.</param>
    /// <param name="progress">An object that is used to report asynchronous operation progress.</param>
    /// <param name="logger">The object for logging asynchronous activity.</param>
    /// <returns>A task that returns the number of database records affected by the operation.</returns>
    public static async Task<int> DeleteAsync([DisallowNull] FileSystem target, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IActivityProgress progress, [DisallowNull] ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(dbContext);
        ArgumentNullException.ThrowIfNull(progress);
        ArgumentNullException.ThrowIfNull(logger);
        using IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
        using (logger.BeginScope(target.Id))
        {
            progress.Report($"Removing file system definition: {target.DisplayName}");
            EntityEntry<FileSystem> entry = dbContext.Entry(target);
            logger.LogDebug("Removing dependant records for Subdirectory {{ Id = {Id}; DisplayName = \"{DisplayName}\" }}", target.Id, target.DisplayName);
            SymbolicName[] symbolicNames = (await entry.GetRelatedCollectionAsync(e => e.SymbolicNames, progress.Token)).ToArray();
            int result;
            if (symbolicNames.Length > 0)
            {
                dbContext.RemoveRange(symbolicNames);
                result = await dbContext.SaveChangesAsync(progress.Token);
            }
            else
                result = 0;
            _ = dbContext.FileSystems.Remove(target);
            result += await dbContext.SaveChangesAsync(progress.Token);
            await transaction.CommitAsync(progress.Token);
            return result;
        }
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public bool Equals(FileSystem other) => other is not null && (ReferenceEquals(this, other) ||
        (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

    public bool Equals(ILocalFileSystem other)
    {
        if (other is null) return false;
        if (other is FileSystem fileSystem) return Equals(fileSystem);
        if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
        return !other.TryGetId(out _) && ArePropertiesEqual(other);
    }

    public bool Equals(IFileSystem other)
    {
        if (other is null) return false;
        if (other is FileSystem fileSystem) return Equals(fileSystem);
        if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
        return !other.TryGetId(out _) && (other is ILocalFileSystem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (obj is FileSystem fileSystem) return Equals(fileSystem);
        if (obj is IFileSystem other)
        {
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalFileSystem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }
        return false;
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
