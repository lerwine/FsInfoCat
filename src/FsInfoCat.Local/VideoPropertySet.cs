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
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class VideoPropertySet : VideoPropertiesRow, ILocalVideoPropertySet, ISimpleIdentityReference<VideoPropertySet>, IEquatable<VideoPropertySet>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private HashSet<DbFile> _files = new();

        [NotNull]
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

        protected bool ArePropertiesEqual([DisallowNull] ILocalVideoPropertySet other) => ArePropertiesEqual((IVideoPropertySet)other) &&
            EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
            LastSynchronizedOn == other.LastSynchronizedOn;

        protected bool ArePropertiesEqual([DisallowNull] IVideoPropertySet other) => ArePropertiesEqual((IVideoProperties)other) && CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn;

        public bool Equals(VideoPropertySet other)
        {
            if (Id.Equals(Guid.Empty)) return other.Id.Equals(Guid.Empty) && ArePropertiesEqual(other);
            return Id.Equals(other.Id);
        }

        public bool Equals(IVideoPropertySet other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other is VideoPropertySet videoPropertySet) return Equals(videoPropertySet);
            if (other is ILocalVideoPropertySet localVideoPropertySet)
            {
                if (Id.Equals(Guid.Empty)) return localVideoPropertySet.Id.Equals(Guid.Empty) && ArePropertiesEqual(localVideoPropertySet);
                return Id.Equals(localVideoPropertySet.Id);
            }
            if (Id.Equals(Guid.Empty)) return other.Id.Equals(Guid.Empty) && ArePropertiesEqual(other);
            return Id.Equals(other.Id);
        }

        public override bool Equals(IVideoPropertiesRow other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other is VideoPropertySet videoPropertySet) return Equals(videoPropertySet);
            if (other is ILocalVideoPropertySet localVideoPropertySet)
            {
                if (Id.Equals(Guid.Empty)) return localVideoPropertySet.Id.Equals(Guid.Empty) && ArePropertiesEqual(localVideoPropertySet);
                return Id.Equals(localVideoPropertySet.Id);
            }
            if (Id.Equals(Guid.Empty)) return other.Id.Equals(Guid.Empty) && ArePropertiesEqual(other);
            return Id.Equals(other.Id);
        }

        public override bool Equals(IVideoProperties other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other is VideoPropertySet videoPropertySet) return Equals(videoPropertySet);
            if (other is ILocalVideoPropertySet localVideoPropertySet)
            {
                if (Id.Equals(Guid.Empty)) return localVideoPropertySet.Id.Equals(Guid.Empty) && ArePropertiesEqual(localVideoPropertySet);
                return Id.Equals(localVideoPropertySet.Id);
            }
            if (other is IVideoPropertySet iVideoPropertySet)
            {
                if (Id.Equals(Guid.Empty)) return iVideoPropertySet.Id.Equals(Guid.Empty) && ArePropertiesEqual(iVideoPropertySet);
                return Id.Equals(iVideoPropertySet.Id);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is VideoPropertySet videoPropertySet) return Equals(videoPropertySet);
            if (obj is IVideoPropertySet iVideoPropertySet)
            {
                if (Id.Equals(Guid.Empty)) return iVideoPropertySet.Id.Equals(Guid.Empty) && ArePropertiesEqual(iVideoPropertySet);
                return Id.Equals(iVideoPropertySet.Id);
            }
            if (obj is ILocalVideoPropertySet localVideoPropertySet)
            {
                if (Id.Equals(Guid.Empty)) return localVideoPropertySet.Id.Equals(Guid.Empty) && ArePropertiesEqual(localVideoPropertySet);
                return Id.Equals(localVideoPropertySet.Id);
            }
            return obj is IVideoProperties videoProperties && ArePropertiesEqual(videoProperties);
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers() { yield return Id; }
    }
}
