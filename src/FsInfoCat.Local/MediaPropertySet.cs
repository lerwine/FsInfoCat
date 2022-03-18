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
    /// Class MediaPropertySet.
    /// Implements the <see cref="LocalDbEntity" />
    /// Implements the <see cref="ILocalMediaPropertySet" />
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalMediaPropertySet" />
    public class MediaPropertySet : MediaPropertiesRow, ILocalMediaPropertySet, ISimpleIdentityReference<MediaPropertySet>, IEquatable<MediaPropertySet>
    {
        private HashSet<DbFile> _files = new();

        public HashSet<DbFile> Files { get => _files; set => _files = value ?? new(); }

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        MediaPropertySet IIdentityReference<MediaPropertySet>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<MediaPropertySet> builder)
        {
            if (builder is null)
                throw new ArgumentOutOfRangeException(nameof(builder));
            _ = builder.Property(nameof(Producer)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Writer)).HasConversion(MultiStringValue.Converter);
        }

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
            MediaPropertySet oldPropertySet = (entity = entry.Entity).MediaPropertySetId.HasValue ?
                await entry.GetRelatedReferenceAsync(f => f.MediaProperties, cancellationToken) : null;
            IMediaProperties currentProperties = await fileDetailProvider.GetMediaPropertiesAsync(cancellationToken);
            if (FilePropertiesComparer.Equals(oldPropertySet, currentProperties))
                return;
            entity.MediaProperties = currentProperties.IsNullOrAllPropertiesEmpty() ? null : await dbContext.GetMatchingAsync(currentProperties, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (oldPropertySet is null)
                return;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    Guid id = entity.Id;
                    if (!(await dbContext.Entry(oldPropertySet).GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                        _ = dbContext.MediaPropertySets.Remove(oldPropertySet);
                    cancellationToken.ThrowIfCancellationRequested();
                    break;
            }
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalMediaPropertySet other)
        {
            throw new System.NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IMediaPropertySet other)
        {
            throw new System.NotImplementedException();
        }

        public bool Equals(MediaPropertySet other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

        public bool Equals(IMediaPropertySet other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IMediaProperties other)
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
                    int hash = 61;
                    hash = hash * 71 + ContentDistributor.GetHashCode();
                    hash = hash * 71 + CreatorApplication.GetHashCode();
                    hash = hash * 71 + CreatorApplicationVersion.GetHashCode();
                    hash = hash * 71 + DateReleased.GetHashCode();
                    hash = EntityExtensions.HashNullable(Duration, hash, 71);
                    hash = hash * 71 + DVDID.GetHashCode();
                    hash = EntityExtensions.HashNullable(FrameCount, hash, 71);
                    hash = EntityExtensions.HashObject(Producer, hash, 71);
                    hash = hash * 71 + ProtectionType.GetHashCode();
                    hash = hash * 71 + ProviderRating.GetHashCode();
                    hash = hash * 71 + ProviderStyle.GetHashCode();
                    hash = hash * 71 + Publisher.GetHashCode();
                    hash = hash * 71 + Subtitle.GetHashCode();
                    hash = EntityExtensions.HashObject(Writer, hash, 71);
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 71);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 71);
                    hash = hash * 71 + CreatedOn.GetHashCode();
                    hash = hash * 71 + ModifiedOn.GetHashCode();
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
