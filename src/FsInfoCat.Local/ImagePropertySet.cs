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
    public class ImagePropertySet : LocalDbEntity, ILocalImagePropertySet
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<uint?> _bitDepth;
        private readonly IPropertyChangeTracker<ushort?> _colorSpace;
        private readonly IPropertyChangeTracker<double?> _compressedBitsPerPixel;
        private readonly IPropertyChangeTracker<ushort?> _compression;
        private readonly IPropertyChangeTracker<string> _compressionText;
        private readonly IPropertyChangeTracker<double?> _horizontalResolution;
        private readonly IPropertyChangeTracker<uint?> _horizontalSize;
        private readonly IPropertyChangeTracker<string> _imageID;
        private readonly IPropertyChangeTracker<short?> _resolutionUnit;
        private readonly IPropertyChangeTracker<double?> _verticalResolution;
        private readonly IPropertyChangeTracker<uint?> _verticalSize;
        private HashSet<DbFile> _files = new();

        #endregion

        #region Properties

        public Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public uint? BitDepth { get => _bitDepth.GetValue(); set => _bitDepth.SetValue(value); }
        public ushort? ColorSpace { get => _colorSpace.GetValue(); set => _colorSpace.SetValue(value); }
        public double? CompressedBitsPerPixel { get => _compressedBitsPerPixel.GetValue(); set => _compressedBitsPerPixel.SetValue(value); }
        public ushort? Compression { get => _compression.GetValue(); set => _compression.SetValue(value); }
        public string CompressionText { get => _compressionText.GetValue(); set => _compressionText.SetValue(value); }
        public double? HorizontalResolution { get => _horizontalResolution.GetValue(); set => _horizontalResolution.SetValue(value); }
        public uint? HorizontalSize { get => _horizontalSize.GetValue(); set => _horizontalSize.SetValue(value); }
        public string ImageID { get => _imageID.GetValue(); set => _imageID.SetValue(value); }
        public short? ResolutionUnit { get => _resolutionUnit.GetValue(); set => _resolutionUnit.SetValue(value); }
        public double? VerticalResolution { get => _verticalResolution.GetValue(); set => _verticalResolution.SetValue(value); }
        public uint? VerticalSize { get => _verticalSize.GetValue(); set => _verticalSize.SetValue(value); }

        public HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        #endregion

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        #endregion

        public ImagePropertySet()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _bitDepth = AddChangeTracker<uint?>(nameof(BitDepth), null);
            _colorSpace = AddChangeTracker<ushort?>(nameof(ColorSpace), null);
            _compressedBitsPerPixel = AddChangeTracker<double?>(nameof(CompressedBitsPerPixel), null);
            _compression = AddChangeTracker<ushort?>(nameof(Compression), null);
            _compressionText = AddChangeTracker(nameof(CompressionText), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _horizontalResolution = AddChangeTracker<double?>(nameof(HorizontalResolution), null);
            _horizontalSize = AddChangeTracker<uint?>(nameof(HorizontalSize), null);
            _imageID = AddChangeTracker(nameof(ImageID), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _resolutionUnit = AddChangeTracker<short?>(nameof(ResolutionUnit), null);
            _verticalResolution = AddChangeTracker<double?>(nameof(VerticalResolution), null);
            _verticalSize = AddChangeTracker<uint?>(nameof(VerticalSize), null);
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
            ImagePropertySet oldPropertySet = (entity = entry.Entity).ImagePropertySetId.HasValue ?
                await entry.GetRelatedReferenceAsync(f => f.ImageProperties, cancellationToken) : null;
            IImageProperties currentProperties = await fileDetailProvider.GetImagePropertiesAsync(cancellationToken);
            if (FilePropertiesComparer.Equals(oldPropertySet, currentProperties))
                return;
            if (currentProperties.IsNullOrAllPropertiesEmpty())
                entity.ImageProperties = null;
            else
                entity.ImageProperties = await dbContext.GetMatchingAsync(currentProperties, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (oldPropertySet is null)
                return;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    Guid id = entity.Id;
                    if (!(await dbContext.Entry(oldPropertySet).GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                        dbContext.ImagePropertySets.Remove(oldPropertySet);
                    cancellationToken.ThrowIfCancellationRequested();
                    break;
            }
        }
    }
}
