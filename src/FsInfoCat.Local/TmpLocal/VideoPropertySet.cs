using M = FsInfoCat.Model;
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

namespace FsInfoCat.Local.Model
{
    // TODO: Document VideoPropertySet class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class VideoPropertySet : VideoPropertiesRow, ILocalVideoPropertySet, IEquatable<VideoPropertySet>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private HashSet<DbFile> _files = new();

        [NotNull]
        [BackingField(nameof(_files))]
        public HashSet<DbFile> Files { get => _files; set => _files = value ?? new(); }

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<M.IFile> M.IPropertySet.Files => Files.Cast<M.IFile>();

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
            M.IVideoProperties currentProperties = await fileDetailProvider.GetVideoPropertiesAsync(cancellationToken);
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

        public bool Equals(VideoPropertySet other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(ILocalVideoPropertySet other)
        {
            if (other is null) return false;
            if (other is VideoPropertySet videoPropertySet) return Equals(videoPropertySet);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && ArePropertiesEqual(other);
        }

        public bool Equals(M.IVideoPropertySet other)
        {
            if (other is null) return false;
            if (other is VideoPropertySet videoPropertySet) return Equals(videoPropertySet);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalVideoPropertySet local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(M.IVideoPropertiesRow other)
        {
            if (other is null) return false;
            if (other is VideoPropertySet propertySet) return Equals(propertySet);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalVideoPropertiesRow local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(M.IVideoProperties other)
        {
            if (other is null) return false;
            if (other is VideoPropertySet propertySet) return Equals(propertySet);
            if (other is M.IVideoPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalVideoPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is VideoPropertySet other) return Equals(other);
            if (obj is M.IVideoPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalVideoPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return obj is M.IVideoProperties properties && ArePropertiesEqual(properties);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
