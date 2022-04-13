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
    public class GPSPropertySet : GPSPropertiesRow, ILocalGPSPropertySet, ISimpleIdentityReference<GPSPropertySet>, IEquatable<GPSPropertySet>
    {
        private HashSet<DbFile> _files = new();

        [NotNull]
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

        protected bool ArePropertiesEqual([DisallowNull] ILocalGPSPropertySet other)
        {
            throw new System.NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IGPSPropertySet other)
        {
            throw new System.NotImplementedException();
        }

        public bool Equals(GPSPropertySet other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

        public bool Equals(IGPSPropertySet other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IGPSPropertiesRow other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IGPSProperties other)
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
                    int hash = 53;
                    hash = hash * 61 + AreaInformation.GetHashCode();
                    hash = EntityExtensions.HashNullable(LatitudeDegrees, hash, 61);
                    hash = EntityExtensions.HashNullable(LatitudeMinutes, hash, 61);
                    hash = EntityExtensions.HashNullable(LatitudeSeconds, hash, 61);
                    hash = hash * 61 + LatitudeRef.GetHashCode();
                    hash = EntityExtensions.HashNullable(LongitudeDegrees, hash, 61);
                    hash = EntityExtensions.HashNullable(LongitudeMinutes, hash, 61);
                    hash = EntityExtensions.HashNullable(LongitudeSeconds, hash, 61);
                    hash = hash * 61 + LongitudeRef.GetHashCode();
                    hash = hash * 61 + MeasureMode.GetHashCode();
                    hash = hash * 61 + ProcessingMethod.GetHashCode();
                    hash = EntityExtensions.HashObject(VersionID, hash, 61);
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 61);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 61);
                    hash = hash * 61 + CreatedOn.GetHashCode();
                    hash = hash * 61 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers() { yield return Id; }
    }
}
