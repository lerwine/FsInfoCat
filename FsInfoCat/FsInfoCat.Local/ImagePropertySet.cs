using System;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Local
{
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
            _compressionText = AddChangeTracker<string>(nameof(CompressionText), null);
            _horizontalResolution = AddChangeTracker<double?>(nameof(HorizontalResolution), null);
            _horizontalSize = AddChangeTracker<uint?>(nameof(HorizontalSize), null);
            _imageID = AddChangeTracker<string>(nameof(ImageID), null);
            _resolutionUnit = AddChangeTracker<short?>(nameof(ResolutionUnit), null);
            _verticalResolution = AddChangeTracker<double?>(nameof(VerticalResolution), null);
            _verticalSize = AddChangeTracker<uint?>(nameof(VerticalSize), null);
        }
    }
}
