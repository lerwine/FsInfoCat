using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Class DRMPropertySet.
/// </summary>
/// <seealso cref="DRMPropertiesListItem" />
/// <seealso cref="LocalDbContext.DRMPropertySets" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
// CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
public class DRMPropertySet : DRMPropertiesRow, ILocalDRMPropertySet, IEquatable<DRMPropertySet>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
{
    private HashSet<DbFile> _files = [];

    /// <summary>
    /// Gets the files that share the same property values as this property set.
    /// </summary>
    /// <value>The <see cref="DbFile">files</see> that share the same property values as this property set.</value>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Files), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_files))]
    public HashSet<DbFile> Files { get => _files; set => _files = value ?? []; }

    #region Explicit Members

    IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

    IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

    #endregion

    internal static async Task RefreshAsync([DisallowNull] EntityEntry<DbFile> entry, [DisallowNull] IFileDetailProvider fileDetailProvider,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(entry);
        ArgumentNullException.ThrowIfNull(fileDetailProvider);
        switch (entry.State)
        {
            case EntityState.Detached:
                throw new ArgumentOutOfRangeException(nameof(entry), $"{nameof(DbFile)} is detached");
            case EntityState.Deleted:
                throw new ArgumentOutOfRangeException(nameof(entry), $"{nameof(DbFile)} is flagged for deletion");
        }
        if (entry.Context is not LocalDbContext dbContext)
            throw new ArgumentOutOfRangeException(nameof(entry), "Invalid database context");
        DbFile entity;
        DRMPropertySet oldPropertySet = (entity = entry.Entity).DRMPropertySetId.HasValue ?
            await entry.GetRelatedReferenceAsync(f => f.DRMProperties, cancellationToken) : null;
        IDRMProperties currentProperties = await fileDetailProvider.GetDRMPropertiesAsync(cancellationToken);
        if (FilePropertiesComparer.Equals(oldPropertySet, currentProperties))
            return;
        entity.DRMProperties = currentProperties.IsNullOrAllPropertiesEmpty() ? null : await dbContext.GetMatchingAsync(currentProperties, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        if (oldPropertySet is null)
            return;
        switch (entry.State)
        {
            case EntityState.Unchanged:
            case EntityState.Modified:
                Guid id = entity.Id;
                if (!(await dbContext.Entry(oldPropertySet).GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                    _ = dbContext.DRMPropertySets.Remove(oldPropertySet);
                cancellationToken.ThrowIfCancellationRequested();
                break;
        }
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public bool Equals(DRMPropertySet other) => other is not null && (ReferenceEquals(this, other) ||
        (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

    public bool Equals(ILocalDRMPropertySet other)
    {
        if (other is null) return false;
        if (other is DRMPropertySet propertySet) return Equals(propertySet);
        if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
        return !other.TryGetId(out _) && ArePropertiesEqual(other);
    }

    public bool Equals(IDRMPropertySet other)
    {
        if (other is null) return false;
        if (other is DRMPropertySet propertySet) return Equals(propertySet);
        if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
        return !other.TryGetId(out _) && (other is ILocalDRMPropertySet local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
    }

    public override bool Equals(IDRMPropertiesRow other)
    {
        if (other is null) return false;
        if (other is DRMPropertySet propertySet) return Equals(propertySet);
        if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
        return !other.TryGetId(out _) && (other is ILocalDRMPropertiesRow local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
    }

    public override bool Equals(IDRMProperties other)
    {
        if (other is null) return false;
        if (other is DRMPropertySet propertySet) return Equals(propertySet);
        if (other is IDRMPropertiesRow row)
        {
            if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
            return !row.TryGetId(out _) && (row is ILocalDRMPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
        }
        return ArePropertiesEqual(other);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (obj is DRMPropertySet other) return Equals(other);
        if (obj is IDRMPropertiesRow row)
        {
            if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
            return !row.TryGetId(out _) && (row is ILocalDRMPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
        }
        return obj is IDRMProperties properties && ArePropertiesEqual(properties);
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
