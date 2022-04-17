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
    /// Class ImagePropertySet.
    /// Implements the <see cref="LocalDbEntity" />
    /// Implements the <see cref="ILocalImagePropertySet" />
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalImagePropertySet" />
    public class ImagePropertySet : ImagePropertiesRow, ILocalImagePropertySet, ISimpleIdentityReference<ImagePropertySet>, IEquatable<ImagePropertySet>
    {
        private HashSet<DbFile> _files = new();

        [NotNull]
        [BackingField(nameof(_files))]
        public HashSet<DbFile> Files { get => _files; set => _files = value ?? new(); }

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        ImagePropertySet IIdentityReference<ImagePropertySet>.Entity => this;

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

        protected bool ArePropertiesEqual([DisallowNull] ILocalImagePropertySet other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IImagePropertySet other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ImagePropertySet other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

        public bool Equals(IImagePropertySet other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IImagePropertiesRow other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IImageProperties other)
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
                    int hash = EntityExtensions.HashNullable(BitDepth, 47, 59);
                    hash = EntityExtensions.HashNullable(ColorSpace, hash, 59);
                    hash = EntityExtensions.HashNullable(CompressedBitsPerPixel, hash, 59);
                    hash = EntityExtensions.HashNullable(Compression, hash, 59);
                    hash = hash * 59 + CompressionText.GetHashCode();
                    hash = EntityExtensions.HashNullable(HorizontalResolution, hash, 59);
                    hash = EntityExtensions.HashNullable(HorizontalSize, hash, 59);
                    hash = hash * 59 + ImageID.GetHashCode();
                    hash = EntityExtensions.HashNullable(ResolutionUnit, hash, 59);
                    hash = EntityExtensions.HashNullable(VerticalResolution, hash, 59);
                    hash = EntityExtensions.HashNullable(VerticalSize, hash, 59);
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 59);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 59);
                    hash = hash * 59 + CreatedOn.GetHashCode();
                    hash = hash * 59 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers() { yield return Id; }
    }
}
