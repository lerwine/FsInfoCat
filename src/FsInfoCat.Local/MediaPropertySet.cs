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

        public bool Equals(MediaPropertySet other)
        {
            throw new NotImplementedException();
        }

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
            if (Id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 61;
                    hash = hash * 71 + ContentDistributor.GetHashCode();
                    hash = hash * 71 + CreatorApplication.GetHashCode();
                    hash = hash * 71 + CreatorApplicationVersion.GetHashCode();
                    hash = hash * 71 + DateReleased.GetHashCode();
                    hash = Duration.HasValue ? hash * 71 + (Duration ?? default).GetHashCode() : hash * 71;
                    hash = hash * 71 + DVDID.GetHashCode();
                    hash = FrameCount.HasValue ? hash * 71 + (FrameCount ?? default).GetHashCode() : hash * 71;
                    hash = (Producer is null) ? hash * 71 : hash * 71 + (Producer?.GetHashCode() ?? 0);
                    hash = hash * 71 + ProtectionType.GetHashCode();
                    hash = hash * 71 + ProviderRating.GetHashCode();
                    hash = hash * 71 + ProviderStyle.GetHashCode();
                    hash = hash * 71 + Publisher.GetHashCode();
                    hash = hash * 71 + Subtitle.GetHashCode();
                    hash = (Writer is null) ? hash * 71 : hash * 71 + (Writer?.GetHashCode() ?? 0);
                    hash = UpstreamId.HasValue ? hash * 71 + (UpstreamId ?? default).GetHashCode() : hash * 71;
                    hash = LastSynchronizedOn.HasValue ? hash * 71 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 71;
                    hash = hash * 71 + CreatedOn.GetHashCode();
                    hash = hash * 71 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return Id.GetHashCode();
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers()
        {
            yield return Id;
        }
    }
}
