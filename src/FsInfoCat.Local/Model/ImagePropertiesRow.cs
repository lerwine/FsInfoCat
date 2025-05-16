using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Base class for entities containing extended file properties for image files.
/// </summary>
/// <seealso cref="ImagePropertiesListItem" />
/// <seealso cref="ImagePropertySet" />
/// <seealso cref="LocalDbContext.ImagePropertySets" />
/// <seealso cref="LocalDbContext.ImagePropertiesListing" />
public abstract class ImagePropertiesRow : PropertiesRow, ILocalImagePropertiesRow
{
    #region Fields

    private string _compressionText = string.Empty;
    private string _imageID = string.Empty;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the Bit Depth.
    /// </summary>
    /// <value>Indicates how many bits are used in each pixel of the image.</value>
    /// <remarks>
    /// (Usually 8, 16, 24, or 32).
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Bit Depth</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{6444048F-4C8B-11D1-8B70-080036B11A03} (ImageSummaryInformation)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>7</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-bitdepth">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.BitDepth), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public uint? BitDepth { get; set; }

    /// <summary>
    /// Gets the Color Space.
    /// </summary>
    /// <value>PropertyTagExifColorSpace The colorspace embedded in the image.</value>
    /// <remarks>
    /// Taken from the Exchangeable Image File (EXIF) information.
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Color Representation</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>40961</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-colorspace">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.ColorSpace), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public ushort? ColorSpace { get; set; }

    /// <summary>
    /// Gets the Compressed Bits-per-Pixel.
    /// </summary>
    /// <value>Calculated from PKEY_Image_CompressedBitsPerPixelNumerator and PKEY_Image_CompressedBitsPerPixelDenominator.</value>
    /// <remarks>
    /// Indicates the image compression level.
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Compressed Bits-per-Pixel</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{364B6FA9-37AB-482A-BE2B-AE02F60D4318} (Format)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>100</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-compressedbitsperpixel">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.CompressedBitsPerPixel), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public double? CompressedBitsPerPixel { get; set; }

    /// <summary>
    /// Indicates the image compression level.
    /// </summary>
    /// <value>PropertyTagCompression The algorithm used to compress the image.</value>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Compression</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>259</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-compression">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Compression), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public ushort? Compression { get; set; }

    /// <summary>
    /// This is the user-friendly form of System.Image.Compression.
    /// </summary>
    /// <value>Not intended to be parsed programmatically.</value>
    /// <remarks>
    /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
    /// <para>The user-friendly form of System.Image.Compression. Not intended to be parsed programmatically.</para><list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Compression</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{3F08E66F-2F44-4BB9-A682-AC35D2562322} (Format)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>100</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-compressiontext">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Compression), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_compressionText))]
    public string CompressionText { get => _compressionText; set => _compressionText = value.AsWsNormalizedOrEmpty(); }

    /// <summary>
    /// Gets the Horizontal Resolution.
    /// </summary>
    /// <value>Indicates the number of pixels per resolution unit in the image width.</value>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Horizontal Resolution</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{6444048F-4C8B-11D1-8B70-080036B11A03} (ImageSummaryInformation)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>5</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-horizontalresolution">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.HorizontalResolution), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public double? HorizontalResolution { get; set; }

    /// <summary>
    /// Gets the Horizontal Size.
    /// </summary>
    /// <value>The horizontal size of the image, in pixels.</value>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Width</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{6444048F-4C8B-11D1-8B70-080036B11A03} (ImageSummaryInformation)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>3</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-horizontalsize">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.HorizontalSize), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public uint? HorizontalSize { get; set; }

    /// <summary>
    /// Gets the Image ID.
    /// </summary>
    /// <value>The Image ID.</value>
    /// <remarks>
    /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Image ID</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{10DABE05-32AA-4C29-BF1A-63E2D220587F} (Format)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>100</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-imageid">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.ImageID), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_imageID))]
    public string ImageID { get => _imageID; set => _imageID = value.AsWsNormalizedOrEmpty(); }

    /// <summary>
    /// Gets the Resolution Unit.
    /// </summary>
    /// <value>Indicates the resolution units.</value>
    /// <remarks>
    /// Used for images with a non-square aspect ratio, but without meaningful absolute dimensions. 1 = No absolute unit of measurement. 2 = Inches. 3 = Centimeters.
    /// The default value is 2 (Inches).
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Resolution Unit</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{19B51FA6-1F92-4A5C-AB48-7DF0ABD67444} (Format)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>100</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-resolutionunit">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.ResolutionUnit), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public short? ResolutionUnit { get; set; }

    /// <summary>
    /// Gets the Vertical Resolution.
    /// </summary>
    /// <value>Indicates the number of pixels per resolution unit in the image height.</value>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Vertical Resolution</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{6444048F-4C8B-11D1-8B70-080036B11A03} (ImageSummaryInformation)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>6</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-verticalresolution">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.VerticalResolution), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public double? VerticalResolution { get; set; }

    /// /// <summary>
    /// Gets the Vertical Size.
    /// </summary>
    /// <value>The vertical size of the image, in pixels.</value>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>Height</description>
    ///     </item>
    ///     <item>
    ///         <term>Format ID</term>
    ///         <description>{6444048F-4C8B-11D1-8B70-080036B11A03} (ImageSummaryInformation)</description>
    ///     </item>
    ///     <item>
    ///         <term>Property ID</term>
    ///         <description>4</description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-verticalsize">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.VerticalSize), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public uint? VerticalSize { get; set; }

    #endregion

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="ILocalImagePropertiesRow" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected bool ArePropertiesEqual([DisallowNull] ILocalImagePropertiesRow other) => ArePropertiesEqual((IImagePropertiesRow)other) &&
        EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
        LastSynchronizedOn == other.LastSynchronizedOn;

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="IImagePropertiesRow" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected bool ArePropertiesEqual([DisallowNull] IImagePropertiesRow other) => ArePropertiesEqual((IImageProperties)other) &&
        CreatedOn == other.CreatedOn &&
        ModifiedOn == other.ModifiedOn;

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="IImageProperties" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected bool ArePropertiesEqual([DisallowNull] IImageProperties other) => _compressionText == other.CompressionText &&
        _imageID == other.ImageID &&
        BitDepth == other.BitDepth &&
        ColorSpace == other.ColorSpace &&
        CompressedBitsPerPixel == other.CompressedBitsPerPixel &&
        Compression == other.Compression &&
        HorizontalResolution == other.HorizontalResolution &&
        HorizontalSize == other.HorizontalSize &&
        ResolutionUnit == other.ResolutionUnit &&
        VerticalResolution == other.VerticalResolution &&
        VerticalSize == other.VerticalSize;
    //EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
    //LastSynchronizedOn == other.LastSynchronizedOn &&
    //CreatedOn == other.CreatedOn &&
    //ModifiedOn == other.ModifiedOn;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract bool Equals(IImagePropertiesRow other);

    public abstract bool Equals(IImageProperties other);

    public override int GetHashCode()
    {
        if (TryGetId(out Guid id)) return id.GetHashCode();
        HashCode hash = new();
        hash.Add(_compressionText);
        hash.Add(_imageID);
        hash.Add(BitDepth);
        hash.Add(ColorSpace);
        hash.Add(CompressedBitsPerPixel);
        hash.Add(Compression);
        hash.Add(HorizontalResolution);
        hash.Add(HorizontalSize);
        hash.Add(ResolutionUnit);
        hash.Add(VerticalResolution);
        hash.Add(VerticalSize);
        hash.Add(UpstreamId);
        hash.Add(LastSynchronizedOn);
        hash.Add(CreatedOn);
        hash.Add(ModifiedOn);
        return hash.ToHashCode();
    }

    protected virtual string PropertiesToString() => $@"ImageID=""{ExtensionMethods.EscapeCsString(_imageID)}"", Compression={Compression}, CompressionText=""{ExtensionMethods.EscapeCsString(_compressionText)}"",
    HorizontalSize={HorizontalSize}, VerticalSize={VerticalSize}, HorizontalResolution={HorizontalResolution}, VerticalResolution={VerticalResolution},
    ResolutionUnit={ResolutionUnit}, BitDepth={BitDepth}, ColorSpace={ColorSpace}, CompressedBitsPerPixel={CompressedBitsPerPixel}";

    public override string ToString() => $@"{{ Id={(TryGetId(out Guid id) ? id : null)}, {PropertiesToString()},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId} }}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
