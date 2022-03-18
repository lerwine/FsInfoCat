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
    public class AudioPropertySet : AudioPropertiesRow, ILocalAudioPropertySet, ISimpleIdentityReference<AudioPropertySet>, IEquatable<AudioPropertySet>
    {
        private HashSet<DbFile> _files = new();

        public HashSet<DbFile> Files { get => _files; set => _files = value ?? new(); }

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        AudioPropertySet IIdentityReference<AudioPropertySet>.Entity => this;

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
            AudioPropertySet oldPropertySet = (entity = entry.Entity).AudioPropertySetId.HasValue ?
                await entry.GetRelatedReferenceAsync(f => f.AudioProperties, cancellationToken) : null;
            IAudioProperties currentProperties = await fileDetailProvider.GetAudioPropertiesAsync(cancellationToken);
            if (FilePropertiesComparer.Equals(oldPropertySet, currentProperties))
                return;
            entity.AudioProperties = currentProperties.IsNullOrAllPropertiesEmpty() ? null : await dbContext.GetMatchingAsync(currentProperties, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (oldPropertySet is null)
                return;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    Guid id = entity.Id;
                    if (!(await dbContext.Entry(oldPropertySet).GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                        _ = dbContext.AudioPropertySets.Remove(oldPropertySet);
                    cancellationToken.ThrowIfCancellationRequested();
                    break;
            }
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalAudioPropertySet other)
        {
            throw new System.NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IAudioPropertySet other)
        {
            throw new System.NotImplementedException();
        }

        public bool Equals(AudioPropertySet other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

        public bool Equals(IAudioPropertySet other) => other is not null && (other is AudioPropertySet p) ? Equals(p) : ArePropertiesEqual(other);

        public override bool Equals(IAudioProperties other) => other is not null && (other is AudioPropertySet p) ? Equals(p) : (other is ILocalAudioPropertySet l) ? ArePropertiesEqual(l) : (other is IAudioPropertySet a) ? ArePropertiesEqual(a) : ArePropertiesEqual(other);

        public override bool Equals(object obj) => obj is IAudioProperties i && ((obj is AudioPropertySet p) ? Equals(p) : (obj is ILocalAudioPropertySet l) ? ArePropertiesEqual(l) : (obj is IAudioPropertySet a) ? ArePropertiesEqual(a) : ArePropertiesEqual(i));

        public override int GetHashCode()
        {
            Guid id = Id;
            if (id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 37;
                    hash = hash * 43 + Compression.GetHashCode();
                    hash = EntityExtensions.HashNullable(EncodingBitrate, hash, 43);
                    hash = hash * 43 + Format.GetHashCode();
                    hash = EntityExtensions.HashNullable(IsVariableBitrate, hash, 43);
                    hash = EntityExtensions.HashNullable(SampleRate, hash, 43);
                    hash = EntityExtensions.HashNullable(SampleSize, hash, 43);
                    hash = hash * 43 + StreamName.GetHashCode();
                    hash = EntityExtensions.HashNullable(StreamNumber, hash, 43);
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 43);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 43);
                    hash = hash * 43 + CreatedOn.GetHashCode();
                    hash = hash * 43 + ModifiedOn.GetHashCode();
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
