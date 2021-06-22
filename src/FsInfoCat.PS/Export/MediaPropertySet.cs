using System;
using System.Collections.Generic;

namespace FsInfoCat.PS.Export
{
    public class MediaPropertySet : ExportSet.ExtendedPropertySetBase
    {
        /// <summary>
        /// Gets the Content Distributor
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMedia.ContentDistributor"/>
        public string ContentDistributor { get; set; }

        /// <summary>
        /// Gets the Creator Application
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMedia.CreatorApplication"/>
        public string CreatorApplication { get; set; }

        /// <summary>
        /// Gets the Creator Application Version
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMedia.CreatorApplicationVersion"/>
        public string CreatorApplicationVersion { get; set; }

        /// <summary>
        /// Gets the Date Released
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMedia.DateReleased"/>
        public string DateReleased { get; set; }

        /// <summary>
        /// Gets the duration
        /// </summary>
        /// <remarks>100ns units, not milliseconds</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMedia.Duration"/>
        public ulong? Duration { get; set; }

        /// <summary>
        /// Gets the DVD ID
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMedia.DVDID"/>
        public string DVDID { get; set; }

        /// <summary>
        /// Indicates the frame count for the image.
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMedia.FrameCount"/>
        public uint? FrameCount { get; set; }

        /// <summary>
        /// Gets the Producer
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMedia.Producer"/>
        public string[] Producer { get; set; }

        /// <summary>
        /// Gets the Protection Type
        /// </summary>
        /// <remarks>If media is protected, how is it protected?</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMedia.ProtectionType"/>
        public string ProtectionType { get; set; }

        /// <summary>
        /// Gets the Provider Rating
        /// </summary>
        /// <remarks>Rating value ranges from 0 to 99, supplied by metadata provider</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMedia.ProviderRating"/>
        public string ProviderRating { get; set; }

        /// <summary>
        /// Style of music or video
        /// </summary>
        /// <remarks>Supplied by metadata provider</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMedia.ProviderStyle"/>
        public string ProviderStyle { get; set; }

        /// <summary>
        /// Gets the Publisher
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMedia.Publisher"/>
        public string Publisher { get; set; }

        /// <summary>
        /// Gets the Subtitle
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMedia.Subtitle"/>
        public string Subtitle { get; set; }

        /// <summary>
        /// Gets the Writer
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMedia.Writer"/>
        public string[] Writer { get; set; }

        /// <summary>
        /// Gets the Year
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMedia.Year"/>
        public uint? Year { get; set; }

        public IEnumerable<File> GetFiles() => throw new NotImplementedException();

        internal static MediaPropertySet Create(System.IO.FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }
    }
}
