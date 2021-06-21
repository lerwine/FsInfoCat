namespace FsInfoCat
{
    public interface IImageProperties : IPropertySet
    {
        /// <summary>
        /// Gets the Bit Depth
        /// </summary>
        /// <remarks>ID: {6444048F-4C8B-11D1-8B70-080036B11A03}, 7 (IMAGESUMMARYINFORMATION)</remarks>
        uint? BitDepth { get; set; }

        /// <summary>
        /// Gets the Color Space
        /// </summary>
        /// <remarks>PropertyTagExifColorSpace
        /// <para>ID: {14B81DA1-0135-4D31-96D9-6CBFC9671A99}, 40961 (ImageProperties)</para></remarks>
        ushort? ColorSpace { get; set; }

        /// <summary>
        /// Gets the Compressed Bits-per-Pixel
        /// </summary>
        /// <remarks>Calculated from PKEY_Image_CompressedBitsPerPixelNumerator and PKEY_Image_CompressedBitsPerPixelDenominator.
        /// <para>ID: {364B6FA9-37AB-482A-BE2B-AE02F60D4318}, 100</para></remarks>
        double? CompressedBitsPerPixel { get; set; }

        /// <summary>
        /// Indicates the image compression level.
        /// </summary>
        /// <remarks>PropertyTagCompression
        /// <para>ID: {14B81DA1-0135-4D31-96D9-6CBFC9671A99}, 259 (ImageProperties)</para></remarks>
        ushort? Compression { get; set; }

        /// <summary>
        /// This is the user-friendly form of System.Image.Compression.
        /// </summary>
        /// <remarks>Not intended to be parsed programmatically.
        /// <para>ID: {3F08E66F-2F44-4BB9-A682-AC35D2562322}, 100</para></remarks>
        string CompressionText { get; set; }

        /// <summary>
        /// Gets the Horizontal Resolution
        /// </summary>
        /// <remarks>ID: {6444048F-4C8B-11D1-8B70-080036B11A03}, 5 (IMAGESUMMARYINFORMATION)</remarks>
        double? HorizontalResolution { get; set; }

        /// <summary>
        /// Gets the Horizontal Size
        /// </summary>
        /// <remarks>ID: {6444048F-4C8B-11D1-8B70-080036B11A03}, 3 (IMAGESUMMARYINFORMATION)</remarks>
        uint? HorizontalSize { get; set; }

        /// <summary>
        /// Gets the Image ID
        /// </summary>
        /// <remarks>ID: {10DABE05-32AA-4C29-BF1A-63E2D220587F}, 100</remarks>
        string ImageID { get; set; }

        /// <summary>
        /// Gets the Resolution Unit
        /// </summary>
        /// <remarks>ID: {19B51FA6-1F92-4A5C-AB48-7DF0ABD67444}, 100</remarks>
        short? ResolutionUnit { get; set; }

        /// <summary>
        /// Gets the Vertical Resolution
        /// </summary>
        /// <remarks>ID: {6444048F-4C8B-11D1-8B70-080036B11A03}, 6 (IMAGESUMMARYINFORMATION)</remarks>
        double? VerticalResolution { get; set; }

        /// <summary>
        /// Gets the Vertical Size
        /// </summary>
        /// <remarks>ID: {6444048F-4C8B-11D1-8B70-080036B11A03}, 4 (IMAGESUMMARYINFORMATION)</remarks>
        uint? VerticalSize { get; set; }
    }
}
