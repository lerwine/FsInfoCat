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
    /// Class DRMPropertySet.
    /// Implements the <see cref="LocalDbEntity" />
    /// Implements the <see cref="ILocalDRMPropertySet" />
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalDRMPropertySet" />
    public class DRMPropertySet : DRMPropertiesRow, ILocalDRMPropertySet, ISimpleIdentityReference<DRMPropertySet>, IEquatable<DRMPropertySet>
    {
        private HashSet<DbFile> _files = new();

        [NotNull]
        [BackingField(nameof(_files))]
        public HashSet<DbFile> Files { get => _files; set => _files = value ?? new(); }

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        DRMPropertySet IIdentityReference<DRMPropertySet>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

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

        protected bool ArePropertiesEqual([DisallowNull] ILocalDRMPropertySet other)
        {
            throw new System.NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IDRMPropertySet other)
        {
            throw new System.NotImplementedException();
        }

        public bool Equals(DRMPropertySet other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

        public bool Equals(IDRMPropertySet other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IDRMPropertiesRow other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IDRMProperties other)
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
                    int hash = 23;
                    hash = EntityExtensions.HashNullable(DatePlayExpires, hash, 31);
                    hash = EntityExtensions.HashNullable(DatePlayStarts, hash, 31);
                    hash = hash * 31 + Description.GetHashCode();
                    hash = hash * 31 + IsProtected.GetHashCode();
                    hash = EntityExtensions.HashNullable(PlayCount, hash, 31);
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 31);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 31);
                    hash = hash * 31 + CreatedOn.GetHashCode();
                    hash = hash * 31 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers() { yield return Id; }
    }
}
