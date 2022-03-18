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
    public class RecordedTVPropertySet : RecordedTVPropertiesRow, ILocalRecordedTVPropertySet, ISimpleIdentityReference<RecordedTVPropertySet>, IEquatable<RecordedTVPropertySet>
    {
        private HashSet<DbFile> _files = new();

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

        protected bool ArePropertiesEqual([DisallowNull] ILocalRecordedTVPropertySet other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IRecordedTVPropertySet other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(RecordedTVPropertySet other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IRecordedTVPropertySet other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IRecordedTVProperties other)
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
                    int hash = EntityExtensions.HashNullable(ChannelNumber, 41, 47);
                    hash = hash * 47 + EpisodeName.GetHashCode();
                    hash = EntityExtensions.HashNullable(IsDTVContent, hash, 47);
                    hash = EntityExtensions.HashNullable(IsHDContent, hash, 47);
                    hash = hash * 47 + NetworkAffiliation.GetHashCode();
                    hash = EntityExtensions.HashNullable(OriginalBroadcastDate, hash, 47);
                    hash = hash * 47 + ProgramDescription.GetHashCode();
                    hash = hash * 47 + StationCallSign.GetHashCode();
                    hash = hash * 47 + StationName.GetHashCode();
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 47);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 47);
                    hash = hash * 47 + CreatedOn.GetHashCode();
                    hash = hash * 47 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers()
        {
            yield return Id;
        }
    }
}
