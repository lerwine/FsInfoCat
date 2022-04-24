using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Class RecordedTVPropertySet.
    /// Implements the <see cref="LocalDbEntity" />
    /// Implements the <see cref="ILocalRecordedTVPropertySet" />
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalRecordedTVPropertySet" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class RecordedTVPropertySet : RecordedTVPropertiesRow, ILocalRecordedTVPropertySet, ISimpleIdentityReference<RecordedTVPropertySet>, IEquatable<RecordedTVPropertySet>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private HashSet<DbFile> _files = new();

        [NotNull]
        [BackingField(nameof(_files))]
        public HashSet<DbFile> Files { get => _files; set => _files = value ?? new(); }

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        RecordedTVPropertySet IIdentityReference<RecordedTVPropertySet>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

        internal static async Task RefreshAsync([DisallowNull] EntityEntry<DbFile> entry, [DisallowNull] IFileDetailProvider fileDetailProvider, CancellationToken cancellationToken)
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
            RecordedTVPropertySet oldPropertySet = (entity = entry.Entity).RecordedTVPropertySetId.HasValue ?
                await entry.GetRelatedReferenceAsync(f => f.RecordedTVProperties, cancellationToken) : null;
            IRecordedTVProperties currentProperties = await fileDetailProvider.GetRecordedTVPropertiesAsync(cancellationToken);
            if (FilePropertiesComparer.Equals(oldPropertySet, currentProperties))
                return;
            entity.RecordedTVProperties = currentProperties.IsNullOrAllPropertiesEmpty() ? null : await dbContext.GetMatchingAsync(currentProperties, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (oldPropertySet is null)
                return;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    Guid id = entity.Id;
                    if (!(await dbContext.Entry(oldPropertySet).GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                        _ = dbContext.RecordedTVPropertySets.Remove(oldPropertySet);
                    cancellationToken.ThrowIfCancellationRequested();
                    break;
            }
        }

        public bool Equals(RecordedTVPropertySet other) => other is not null && (ReferenceEquals(this, other) || (TryGetId(out Guid id) ? id.Equals(other.Id) : !other.TryGetId(out _) && ArePropertiesEqual(other)));


        public bool Equals(IRecordedTVPropertySet other)
        {
            if (other is null) return false;
            if (other is RecordedTVPropertySet propertySet) return Equals(propertySet);
            if (TryGetId(out Guid id)) return id.Equals(other.Id);
            if (!other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalRecordedTVPropertiesRow localRow) return ArePropertiesEqual(localRow);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(IRecordedTVPropertiesRow other)
        {
            if (other is null) return false;
            if (other is RecordedTVPropertySet propertySet) return Equals(propertySet);
            if (TryGetId(out Guid id)) return id.Equals(other.Id);
            if (!other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalRecordedTVPropertiesRow localRow) return ArePropertiesEqual(localRow);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(IRecordedTVProperties other)
        {
            if (other is null) return false;
            if (other is RecordedTVPropertySet propertySet) return Equals(propertySet);
            if (other is IRecordedTVPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return id.Equals(row.Id);
                if (!row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalRecordedTVPropertiesRow localRow) return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is RecordedTVPropertySet other) return Equals(other);
            if (obj is IRecordedTVPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return id.Equals(row.Id);
                if (!row.Id.Equals(Guid.Empty)) return false;
                if (obj is ILocalRecordedTVPropertiesRow localRow) return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return obj is IRecordedTVProperties properties && ArePropertiesEqual(properties);
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers() { yield return Id; }
    }
}
