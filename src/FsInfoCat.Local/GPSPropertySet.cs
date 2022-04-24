using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Class GPSPropertySet.
    /// Implements the <see cref="LocalDbEntity" />
    /// Implements the <see cref="ILocalGPSPropertySet" />
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalGPSPropertySet" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class GPSPropertySet : GPSPropertiesRow, ILocalGPSPropertySet, ISimpleIdentityReference<GPSPropertySet>, IEquatable<GPSPropertySet>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private HashSet<DbFile> _files = new();

        [NotNull]
        [BackingField(nameof(_files))]
        public HashSet<DbFile> Files { get => _files; set => _files = value ?? new(); }

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        GPSPropertySet IIdentityReference<GPSPropertySet>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<GPSPropertySet> builder) =>
            (builder ?? throw new ArgumentOutOfRangeException(nameof(builder))).Property(nameof(VersionID)).HasConversion(ByteValues.Converter);

        internal static async Task RefreshAsync([DisallowNull] EntityEntry<DbFile> entry, [DisallowNull] IFileDetailProvider fileDetailProvider,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (entry is null)
                throw new ArgumentNullException(nameof(entry));
            if (fileDetailProvider is null)
                throw new ArgumentNullException(nameof(fileDetailProvider));
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
            GPSPropertySet oldPropertySet = (entity = entry.Entity).GPSPropertySetId.HasValue ?
                await entry.GetRelatedReferenceAsync(f => f.GPSProperties, cancellationToken) : null;
            IGPSProperties currentProperties = await fileDetailProvider.GetGPSPropertiesAsync(cancellationToken);
            if (FilePropertiesComparer.Equals(oldPropertySet, currentProperties))
                return;
            entity.GPSProperties = currentProperties.IsNullOrAllPropertiesEmpty() ? null : await dbContext.GetMatchingAsync(currentProperties, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (oldPropertySet is null)
                return;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    Guid id = entity.Id;
                    if (!(await dbContext.Entry(oldPropertySet).GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                        _ = dbContext.GPSPropertySets.Remove(oldPropertySet);
                    cancellationToken.ThrowIfCancellationRequested();
                    break;
            }
        }

        public bool Equals(GPSPropertySet other) => other is not null && (ReferenceEquals(this, other) || (TryGetId(out Guid id) ? id.Equals(other.Id) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(IGPSPropertySet other)
        {
            if (other is null) return false;
            if (other is GPSPropertySet propertySet) return Equals(propertySet);
            if (TryGetId(out Guid id)) return id.Equals(other.Id);
            if (!other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalGPSPropertiesRow localRow) return ArePropertiesEqual(localRow);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(IGPSPropertiesRow other)
        {
            if (other is null) return false;
            if (other is GPSPropertySet propertySet) return Equals(propertySet);
            if (TryGetId(out Guid id)) return id.Equals(other.Id);
            if (!other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalGPSPropertiesRow localRow) return ArePropertiesEqual(localRow);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(IGPSProperties other)
        {
            if (other is null) return false;
            if (other is GPSPropertySet propertySet) return Equals(propertySet);
            if (other is IGPSPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return id.Equals(row.Id);
                if (!row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalGPSPropertiesRow localRow) return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is GPSPropertySet other) return Equals(other);
            if (obj is IGPSPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return id.Equals(row.Id);
                if (!row.Id.Equals(Guid.Empty)) return false;
                if (obj is ILocalGPSPropertiesRow localRow) return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return obj is IGPSProperties properties && ArePropertiesEqual(properties);
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers() { yield return Id; }
    }
}
