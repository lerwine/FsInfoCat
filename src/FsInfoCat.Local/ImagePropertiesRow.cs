namespace FsInfoCat.Local
{
    public class ImagePropertiesRow : PropertiesRow, IImageProperties
    {
        #region Fields

        private string _compressionText = string.Empty;
        private string _imageID = string.Empty;

        #endregion

        #region Properties

        public uint? BitDepth { get; set; }

        public ushort? ColorSpace { get; set; }

        public double? CompressedBitsPerPixel { get; set; }

        public ushort? Compression { get; set; }

        public string CompressionText { get => _compressionText; set => _compressionText = value.AsWsNormalizedOrEmpty(); }

        public double? HorizontalResolution { get; set; }

        public uint? HorizontalSize { get; set; }

        public string ImageID { get => _imageID; set => _imageID = value.AsWsNormalizedOrEmpty(); }

        public short? ResolutionUnit { get; set; }

        public double? VerticalResolution { get; set; }

        public uint? VerticalSize { get; set; }


        #endregion
    }
}
