using System;
using System.Collections.Generic;

namespace FsInfoCat.PS.Export
{
    public class VideoPropertySet : ExportSet.ExtendedPropertySetBase
    {
        /// <summary>
        /// Indicates the level of compression for the video stream.
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemVideo.Compression"/>
        public string Compression { get; set; }

        /// <summary>
        /// Gets the Director
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemVideo.Director"/>
        public string[] Director { get; set; }

        /// <summary>
        /// Indicates the data rate in "bits per second" for the video stream.
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemVideo.EncodingBitrate"/>
        public uint? EncodingBitrate { get; set; }

        /// <summary>
        /// Indicates the frame height for the video stream.
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemVideo.FrameHeight"/>
        public uint? FrameHeight { get; set; }

        /// <summary>
        /// Indicates the frame rate in "frames per millisecond" for the video stream.
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemVideo.FrameRate"/>
        public uint? FrameRate { get; set; }

        /// <summary>
        /// Indicates the frame width for the video stream.
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemVideo.FrameWidth"/>
        public uint? FrameWidth { get; set; }

        /// <summary>
        /// Indicates the horizontal portion of the aspect ratio.
        /// </summary>
        /// <remarks>The X portion of XX:YY, like 16:9.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemVideo.HorizontalAspectRatio"/>
        public uint? HorizontalAspectRatio { get; set; }

        /// <summary>
        /// Indicates the name for the video stream.
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemVideo.StreamName"/>
        public string StreamName { get; set; }

        /// <summary>
        /// Gets the Stream Number
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemVideo.StreamNumber"/>
        public ushort? StreamNumber { get; set; }

        /// <summary>
        /// Indicates the vertical portion of the aspect ratio
        /// </summary>
        /// <remarks>The Y portion of XX:YY, like 16:9.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemVideo.VerticalAspectRatio"/>
        public uint? VerticalAspectRatio { get; set; }

        public IEnumerable<File> GetFiles() => throw new NotImplementedException();

        internal static VideoPropertySet Create(System.IO.FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }
    }
}
