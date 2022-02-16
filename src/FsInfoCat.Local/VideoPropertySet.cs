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
    public class VideoPropertySet : VideoPropertiesRow, ILocalVideoPropertySet, ISimpleIdentityReference<VideoPropertySet>, IEquatable<VideoPropertySet>
    {
        private HashSet<DbFile> _files = new();

        public HashSet<DbFile> Files { get => _files; set => _files = value ?? new(); }

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        VideoPropertySet IIdentityReference<VideoPropertySet>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<VideoPropertySet> builder) =>
            (builder ?? throw new ArgumentOutOfRangeException(nameof(builder))).Property(nameof(Director)).HasConversion(MultiStringValue.Converter);

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
            VideoPropertySet oldPropertySet = (entity = entry.Entity).VideoPropertySetId.HasValue ?
                await entry.GetRelatedReferenceAsync(f => f.VideoProperties, cancellationToken) : null;
            IVideoProperties currentProperties = await fileDetailProvider.GetVideoPropertiesAsync(cancellationToken);
            if (FilePropertiesComparer.Equals(oldPropertySet, currentProperties))
                return;
            entity.VideoProperties = currentProperties.IsNullOrAllPropertiesEmpty() ? null : await dbContext.GetMatchingAsync(currentProperties, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (oldPropertySet is null)
                return;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    Guid id = entity.Id;
                    if (!(await dbContext.Entry(oldPropertySet).GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                        _ = dbContext.VideoPropertySets.Remove(oldPropertySet);
                    cancellationToken.ThrowIfCancellationRequested();
                    break;
            }
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalVideoPropertySet other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IVideoPropertySet other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(VideoPropertySet other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IVideoPropertySet other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IVideoProperties other)
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
                    int hash = 43;
                    hash = hash * 53 + Compression.GetHashCode();
                    hash = (Director is null) ? hash * 53 : hash * 53 + (Director?.GetHashCode() ?? 0);
                    hash = EncodingBitrate.HasValue ? hash * 53 + (EncodingBitrate ?? default).GetHashCode() : hash * 53;
                    hash = FrameHeight.HasValue ? hash * 53 + (FrameHeight ?? default).GetHashCode() : hash * 53;
                    hash = FrameRate.HasValue ? hash * 53 + (FrameRate ?? default).GetHashCode() : hash * 53;
                    hash = FrameWidth.HasValue ? hash * 53 + (FrameWidth ?? default).GetHashCode() : hash * 53;
                    hash = HorizontalAspectRatio.HasValue ? hash * 53 + (HorizontalAspectRatio ?? default).GetHashCode() : hash * 53;
                    hash = hash * 53 + StreamName.GetHashCode();
                    hash = StreamNumber.HasValue ? hash * 53 + (StreamNumber ?? default).GetHashCode() : hash * 53;
                    hash = VerticalAspectRatio.HasValue ? hash * 53 + (VerticalAspectRatio ?? default).GetHashCode() : hash * 53;
                    hash = UpstreamId.HasValue ? hash * 53 + (UpstreamId ?? default).GetHashCode() : hash * 53;
                    hash = LastSynchronizedOn.HasValue ? hash * 53 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 53;
                    hash = hash * 53 + CreatedOn.GetHashCode();
                    hash = hash * 53 + ModifiedOn.GetHashCode();
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
