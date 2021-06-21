using System;
using System.Collections.Generic;

namespace FsInfoCat.PS.Export
{
    public class RecordedTVProperties : ExportSet.ExtendedPropertiesBase
    {
        /// <summary>
        /// Gets the Channel Number
        /// </summary>
        /// <remarks>Example: 42</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemRecordedTV.ChannelNumber"/>
        public uint? ChannelNumber { get; set; }

        /// <summary>
        /// Gets the Episode Name
        /// </summary>
        /// <remarks>Example: &quot;Nowhere to Hyde&quot;</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemRecordedTV.EpisodeName"/>
        public string EpisodeName { get; set; }

        /// <summary>
        /// Indicates whether the video is DTV
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemRecordedTV.IsDTVContent"/>
        public bool? IsDTVContent { get; set; }

        /// <summary>
        /// Indicates whether the video is HD
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemRecordedTV.IsHDContent"/>
        public bool? IsHDContent { get; set; }

        /// <summary>
        /// Gets the Network Affiliation
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemRecordedTV.NetworkAffiliation"/>
        public string NetworkAffiliation { get; set; }

        /// <summary>
        /// Gets the Original Broadcast Date
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemRecordedTV.OriginalBroadcastDate"/>
        public System.DateTime? OriginalBroadcastDate { get; set; }

        /// <summary>
        /// Gets the Program Description
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemRecordedTV.ProgramDescription"/>
        public string ProgramDescription { get; set; }

        /// <summary>
        /// Gets the Station Call Sign
        /// </summary>
        /// <remarks>Example: &quot;TOONP&quot;</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemRecordedTV.StationCallSign"/>
        public string StationCallSign { get; set; }

        /// <summary>
        /// Gets the Station Name
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemRecordedTV.StationName"/>
        public string StationName { get; set; }

        public IEnumerable<File> GetFiles() => throw new NotImplementedException();

        internal static RecordedTVProperties Create(System.IO.FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }
    }
}
