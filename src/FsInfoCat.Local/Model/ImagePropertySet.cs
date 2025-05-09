using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Class ImagePropertySet.
    /// Implements the <see cref="LocalDbEntity" />
    /// Implements the <see cref="ILocalImagePropertySet" />
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalImagePropertySet" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class ImagePropertySet : ImagePropertiesRow, ILocalImagePropertySet, IEquatable<ImagePropertySet>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        // TODO: Document ImagePropertySet class members
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        private HashSet<DbFile> _files = [];

        [NotNull]
        [BackingField(nameof(_files))]
        public HashSet<DbFile> Files { get => _files; set => _files = value ?? []; }

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

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
            ImagePropertySet oldPropertySet = (entity = entry.Entity).ImagePropertySetId.HasValue ?
                await entry.GetRelatedReferenceAsync(f => f.ImageProperties, cancellationToken) : null;
            IImageProperties currentProperties = await fileDetailProvider.GetImagePropertiesAsync(cancellationToken);
            if (FilePropertiesComparer.Equals(oldPropertySet, currentProperties))
                return;
            entity.ImageProperties = currentProperties.IsNullOrAllPropertiesEmpty() ? null : await dbContext.GetMatchingAsync(currentProperties, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (oldPropertySet is null)
                return;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    Guid id = entity.Id;
                    if (!(await dbContext.Entry(oldPropertySet).GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                        _ = dbContext.ImagePropertySets.Remove(oldPropertySet);
                    cancellationToken.ThrowIfCancellationRequested();
                    break;
            }
        }

        public bool Equals(ImagePropertySet other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(ILocalImagePropertySet other)
        {
            if (other is null) return false;
            if (other is ImagePropertySet propertySet) return Equals(propertySet);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && ArePropertiesEqual(other);
        }

        public bool Equals(IImagePropertySet other)
        {
            if (other is null) return false;
            if (other is ImagePropertySet propertySet) return Equals(propertySet);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalImagePropertySet local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(IImagePropertiesRow other)
        {
            if (other is null) return false;
            if (other is ImagePropertySet propertySet) return Equals(propertySet);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalImagePropertiesRow local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(IImageProperties other)
        {
            if (other is null) return false;
            if (other is ImagePropertySet propertySet) return Equals(propertySet);
            if (other is IImagePropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalImagePropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is ImagePropertySet other) return Equals(other);
            if (obj is IImagePropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalImagePropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return obj is IImageProperties properties && ArePropertiesEqual(properties);
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
