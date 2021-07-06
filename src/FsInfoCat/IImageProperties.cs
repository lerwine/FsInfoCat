namespace FsInfoCat
{
    /// <summary>
    /// Represents extended file properties for image files.
    /// </summary>
    /// <seealso cref="IImagePropertySet"/>
    /// <seealso cref="Local.ILocalImagePropertySet"/>
    /// <seealso cref="Upstream.IUpstreamImagePropertySet"/>
    /// <seealso cref="FilePropertiesComparer.Equals(IImageProperties, IImageProperties)"/>
    /// <seealso cref="Local.IFileDetailProvider.GetImagePropertiesAsync(System.Threading.CancellationToken)"/>
    /// <seealso cref="IDbContext.FindMatchingAsync(IImageProperties, System.Threading.CancellationToken)"/>
    public interface IImageProperties
    {
        /// <summary>
        /// Gets the Bit Depth
        /// </summary>
        /// <value>
        /// Indicates how many bits are used in each pixel of the image.
        /// </value>
        /// <remarks>
        /// (Usually 8, 16, 24, or 32).
        /// <list type="bullet">
        /// <item><term>Name</term><description>Bit Depth</description></item>
        /// <item><term>Format ID</term><description>{6444048F-4C8B-11D1-8B70-080036B11A03} (ImageSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>7</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-bitdepth">[Reference Link]</a></description></item>
        /// </list></remarks>
        uint? BitDepth { get; }

        /// <summary>
        /// Gets the Color Space
        /// </summary>
        /// <value>
        /// PropertyTagExifColorSpace The colorspace embedded in the image.
        /// </value>
        /// <remarks>
        /// Taken from the Exchangeable Image File (EXIF) information.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Color Representation</description></item>
        /// <item><term>Format ID</term><description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description></item>
        /// <item><term>Property ID</term><description>40961</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-colorspace">[Reference Link]</a></description></item>
        /// </list></remarks>
        ushort? ColorSpace { get; }

        /// <summary>
        /// Gets the Compressed Bits-per-Pixel
        /// </summary>
        /// <value>
        /// Calculated from PKEY_Image_CompressedBitsPerPixelNumerator and PKEY_Image_CompressedBitsPerPixelDenominator.
        /// </value>
        /// <remarks>
        /// Indicates the image compression level.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Compressed Bits-per-Pixel</description></item>
        /// <item><term>Format ID</term><description>{364B6FA9-37AB-482A-BE2B-AE02F60D4318} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-compressedbitsperpixel">[Reference Link]</a></description></item>
        /// </list></remarks>
        double? CompressedBitsPerPixel { get; }

        /// <summary>
        /// Indicates the image compression level.
        /// </summary>
        /// <value>
        /// PropertyTagCompression The algorithm used to compress the image.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Compression</description></item>
        /// <item><term>Format ID</term><description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description></item>
        /// <item><term>Property ID</term><description>259</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-compression">[Reference Link]</a></description></item>
        /// </list></remarks>
        ushort? Compression { get; }

        /// <summary>
        /// This is the user-friendly form of System.Image.Compression.
        /// </summary>
        /// <value>
        /// Not intended to be parsed programmatically.
        /// </value>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <para>The user-friendly form of System.Image.Compression. Not intended to be parsed programmatically.</para>
        /// <list type="bullet">
        /// <item><term>Name</term><description>Compression</description></item>
        /// <item><term>Format ID</term><description>{3F08E66F-2F44-4BB9-A682-AC35D2562322} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-compressiontext">[Reference Link]</a></description></item>
        /// </list></remarks>
        string CompressionText { get; }

        /// <summary>
        /// Gets the Horizontal Resolution
        /// </summary>
        /// <value>
        /// Indicates the number of pixels per resolution unit in the image width.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Horizontal Resolution</description></item>
        /// <item><term>Format ID</term><description>{6444048F-4C8B-11D1-8B70-080036B11A03} (ImageSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>5</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-horizontalresolution">[Reference Link]</a></description></item>
        /// </list></remarks>
        double? HorizontalResolution { get; }

        /// <summary>
        /// Gets the Horizontal Size
        /// </summary>
        /// <value>
        /// The horizontal size of the image, in pixels.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Width</description></item>
        /// <item><term>Format ID</term><description>{6444048F-4C8B-11D1-8B70-080036B11A03} (ImageSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>3</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-horizontalsize">[Reference Link]</a></description></item>
        /// </list></remarks>
        uint? HorizontalSize { get; }

        /// <summary>
        /// Gets the Image ID
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Image ID</description></item>
        /// <item><term>Format ID</term><description>{10DABE05-32AA-4C29-BF1A-63E2D220587F} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-imageid">[Reference Link]</a></description></item>
        /// </list></remarks>
        string ImageID { get; }

        /// <summary>
        /// Gets the Resolution Unit
        /// </summary>
        /// <value>
        /// Indicates the resolution units.
        /// </value>
        /// <remarks>
        /// Used for images with a non-square aspect ratio, but without meaningful absolute dimensions. 1 = No absolute unit of measurement. 2 = Inches. 3 = Centimeters. The default value is 2 (Inches).
        /// <list type="bullet">
        /// <item><term>Name</term><description>Resolution Unit</description></item>
        /// <item><term>Format ID</term><description>{19B51FA6-1F92-4A5C-AB48-7DF0ABD67444} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-resolutionunit">[Reference Link]</a></description></item>
        /// </list></remarks>
        short? ResolutionUnit { get; }

        /// <summary>
        /// Gets the Vertical Resolution
        /// </summary>
        /// <value>
        /// Indicates the number of pixels per resolution unit in the image height.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Vertical Resolution</description></item>
        /// <item><term>Format ID</term><description>{6444048F-4C8B-11D1-8B70-080036B11A03} (ImageSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>6</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-verticalresolution">[Reference Link]</a></description></item>
        /// </list></remarks>
        double? VerticalResolution { get; }

        /// <summary>
        /// Gets the Vertical Size
        /// </summary>
        /// <value>
        /// The vertical size of the image, in pixels.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Height</description></item>
        /// <item><term>Format ID</term><description>{6444048F-4C8B-11D1-8B70-080036B11A03} (ImageSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>4</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-verticalsize">[Reference Link]</a></description></item>
        /// </list></remarks>
        uint? VerticalSize { get; }
    }
}
