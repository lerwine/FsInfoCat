namespace FsInfoCat.Desktop.FileSystemDetail
{
    public record ImagePropertiesRecord : Model.IImageProperties
    {
        public uint? BitDepth { get; init; }

        public ushort? ColorSpace { get; init; }

        public double? CompressedBitsPerPixel { get; init; }

        public ushort? Compression { get; init; }

        public string CompressionText { get; init; }

        public double? HorizontalResolution { get; init; }

        public uint? HorizontalSize { get; init; }

        public string ImageID { get; init; }

        public short? ResolutionUnit { get; init; }

        public double? VerticalResolution { get; init; }

        public uint? VerticalSize { get; init; }

        public bool Equals(Model.IImageProperties other)
        {
            // TODO: Implement Equals(IImageProperties);
            throw new System.NotImplementedException();
        }
    }
}
