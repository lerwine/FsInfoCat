using System;
using System.Collections.Generic;

namespace FsInfoCat.PS.Export
{
    public class ImageProperties : ExportSet.ExtendedPropertiesBase
    {
        /// <summary>
        /// Gets the Bit Depth
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemImage.BitDepth"/>
        public uint? BitDepth { get; set; }

        /// <summary>
        /// Gets the Color Space
        /// </summary>
        /// <remarks>PropertyTagExifColorSpace</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemImage.ColorSpace"/>
        public ushort? ColorSpace { get; set; }

        /// <summary>
        /// Gets the Compressed Bits-per-Pixel
        /// </summary>
        /// <remarks>Calculated from PKEY_Image_CompressedBitsPerPixelNumerator and PKEY_Image_CompressedBitsPerPixelDenominator.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemImage.CompressedBitsPerPixel"/>
        public double? CompressedBitsPerPixel { get; set; }

        /// <summary>
        /// Indicates the image compression level.
        /// </summary>
        /// <remarks>PropertyTagCompression</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemImage.Compression"/>
        public ushort? Compression { get; set; }

        /// <summary>
        /// This is the user-friendly form of System.Image.Compression.
        /// </summary>
        /// <remarks>Not intended to be parsed programmatically.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemImage.CompressionText"/>
        public string CompressionText { get; set; }

        /// <summary>
        /// Gets the Horizontal Resolution
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemImage.HorizontalResolution"/>
        public double? HorizontalResolution { get; set; }

        /// <summary>
        /// Gets the Horizontal Size
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemImage.HorizontalSize"/>
        public uint? HorizontalSize { get; set; }

        /// <summary>
        /// Gets the Image ID
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemImage.ImageID"/>
        public string ImageID { get; set; }

        /// <summary>
        /// Gets the Resolution Unit
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemImage.ResolutionUnit"/>
        public short? ResolutionUnit { get; set; }

        /// <summary>
        /// Gets the Vertical Resolution
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemImage.VerticalResolution"/>
        public double? VerticalResolution { get; set; }

        /// <summary>
        /// Gets the Vertical Size
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemImage.VerticalSize"/>
        public uint? VerticalSize { get; set; }

        public IEnumerable<File> GetFiles() => throw new NotImplementedException();

        internal static ImageProperties Create(System.IO.FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }
    }
}
