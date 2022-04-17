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
    /// Class PhotoPropertySet.
    /// Implements the <see cref="LocalDbEntity" />
    /// Implements the <see cref="ILocalPhotoPropertySet" />
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalPhotoPropertySet" />
    public class PhotoPropertySet : PhotoPropertiesRow, ILocalPhotoPropertySet, ISimpleIdentityReference<PhotoPropertySet>, IEquatable<PhotoPropertySet>
    {
        private HashSet<DbFile> _files = new();

        [NotNull]
        [BackingField(nameof(_files))]
        public HashSet<DbFile> Files { get => _files; set => _files = value ?? new(); }

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        PhotoPropertySet IIdentityReference<PhotoPropertySet>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<PhotoPropertySet> builder)
        {
            if (builder is null)
                throw new ArgumentOutOfRangeException(nameof(builder));
            _ = builder.Property(nameof(Event)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(PeopleNames)).HasConversion(MultiStringValue.Converter);
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
            PhotoPropertySet oldPropertySet = (entity = entry.Entity).PhotoPropertySetId.HasValue ?
                await entry.GetRelatedReferenceAsync(f => f.PhotoProperties, cancellationToken) : null;
            IPhotoProperties currentProperties = await fileDetailProvider.GetPhotoPropertiesAsync(cancellationToken);
            if (FilePropertiesComparer.Equals(oldPropertySet, currentProperties))
                return;
            entity.PhotoProperties = currentProperties.IsNullOrAllPropertiesEmpty() ? null : await dbContext.GetMatchingAsync(currentProperties, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (oldPropertySet is null)
                return;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    Guid id = entity.Id;
                    if (!(await dbContext.Entry(oldPropertySet).GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                        _ = dbContext.PhotoPropertySets.Remove(oldPropertySet);
                    cancellationToken.ThrowIfCancellationRequested();
                    break;
            }
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalPhotoPropertySet other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IPhotoPropertySet other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(PhotoPropertySet other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IPhotoPropertySet other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IPhotoPropertiesRow other)
        {
            throw new NotImplementedException();
        }
        public override bool Equals(IPhotoProperties other)
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
                    int hash = 37;
                    hash = hash * 43 + CameraManufacturer.GetHashCode();
                    hash = hash * 43 + CameraModel.GetHashCode();
                    hash = EntityExtensions.HashNullable(DateTaken, hash, 43);
                    hash = EntityExtensions.HashObject(Event, hash, 43);
                    hash = hash * 43 + EXIFVersion.GetHashCode();
                    hash = EntityExtensions.HashNullable(Orientation, hash, 43);
                    hash = hash * 43 + OrientationText.GetHashCode();
                    hash = EntityExtensions.HashObject(PeopleNames, hash, 43);
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 43);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 43);
                    hash = hash * 43 + CreatedOn.GetHashCode();
                    hash = hash * 43 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers() { yield return Id; }
    }
}
