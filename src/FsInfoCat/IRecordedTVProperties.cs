namespace FsInfoCat
{
    /// <summary>
    /// Represents extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="IRecordedTVPropertySet"/>
    /// <seealso cref="Local.ILocalRecordedTVPropertySet"/>
    /// <seealso cref="Upstream.IUpstreamRecordedTVPropertySet"/>
    /// <seealso cref="FilePropertiesComparer.Equals(IRecordedTVProperties, IRecordedTVProperties)"/>
    /// <seealso cref="Local.IFileDetailProvider.GetRecordedTVPropertiesAsync(System.Threading.CancellationToken)"/>
    /// <seealso cref="IDbContext.FindMatchingAsync(IRecordedTVProperties, System.Threading.CancellationToken)"/>
    public interface IRecordedTVProperties
    {
        /// <summary>
        /// Gets the Channel Number
        /// </summary>
        /// <value>
        /// Example: 42 The recorded TV channels.
        /// </value>
        /// <remarks>
        /// For example, 42, 5, 53.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Channel Number</description></item>
        /// <item><term>Format ID</term><description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description></item>
        /// <item><term>Property ID</term><description>7</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-channelnumber">[Reference Link]</a></description></item>
        /// </list></remarks>
        uint? ChannelNumber { get; }

        /// <summary>
        /// Gets the Episode Name
        /// </summary>
        /// <value>
        /// Example: "Nowhere to Hyde" The names of recorded TV episodes.
        /// </value>
        /// <remarks>
        /// For example, "Nowhere to Hyde".
        /// <list type="bullet">
        /// <item><term>Name</term><description>Episode Name</description></item>
        /// <item><term>Format ID</term><description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description></item>
        /// <item><term>Property ID</term><description>2</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-episodename">[Reference Link]</a></description></item>
        /// </list></remarks>
        string EpisodeName { get; }

        /// <summary>
        /// Indicates whether the video is DTV
        /// </summary>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Is DTV Content</description></item>
        /// <item><term>Format ID</term><description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description></item>
        /// <item><term>Property ID</term><description>17</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-isdtvcontent">[Reference Link]</a></description></item>
        /// </list></remarks>
        bool? IsDTVContent { get; }

        /// <summary>
        /// Indicates whether the video is HDTV
        /// </summary>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Is HDTV Content</description></item>
        /// <item><term>Format ID</term><description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description></item>
        /// <item><term>Property ID</term><description>18</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-ishdcontent">[Reference Link]</a></description></item>
        /// </list></remarks>
        bool? IsHDContent { get; }

        /// <summary>
        /// Gets the Network Affiliation
        /// </summary>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>TV Network Affiliation</description></item>
        /// <item><term>Format ID</term><description>{2C53C813-FB63-4E22-A1AB-0B331CA1E273} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-networkaffiliation">[Reference Link]</a></description></item>
        /// </list></remarks>
        string NetworkAffiliation { get; }

        /// <summary>
        /// Gets the Original Broadcast Date
        /// </summary>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Original Broadcast Date</description></item>
        /// <item><term>Format ID</term><description>{4684FE97-8765-4842-9C13-F006447B178C} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-originalbroadcastdate">[Reference Link]</a></description></item>
        /// </list></remarks>
        System.DateTime? OriginalBroadcastDate { get; }

        /// <summary>
        /// Gets the Program Description
        /// </summary>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Program Description</description></item>
        /// <item><term>Format ID</term><description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description></item>
        /// <item><term>Property ID</term><description>3</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-programdescription">[Reference Link]</a></description></item>
        /// </list></remarks>
        string ProgramDescription { get; }
        
        /// <summary>
        /// Gets the Station Call Sign
        /// </summary>
        /// <value>
        /// Example: "TOONP" Any recorded station call signs.
        /// </value>
        /// <remarks>
        /// For example, "TOONP".
        /// <list type="bullet">
        /// <item><term>Name</term><description>Station Call Sign</description></item>
        /// <item><term>Format ID</term><description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description></item>
        /// <item><term>Property ID</term><description>5</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-stationcallsign">[Reference Link]</a></description></item>
        /// </list></remarks>
        string StationCallSign { get; }

        /// <summary>
        /// Gets the Station Name.
        /// </summary>
        /// <value>The  name of the broadcast station or <see langword="null"/> if this value is not specified.</value>
        /// <remarks>ID: {1B5439E7-EBA1-4AF8-BDD7-7AF1D4549493}, 100</remarks>
        /// <summary>
        /// Gets the Station Name
        /// </summary>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Station Name</description></item>
        /// <item><term>Format ID</term><description>{1B5439E7-EBA1-4AF8-BDD7-7AF1D4549493} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-stationname">[Reference Link]</a></description></item>
        /// </list></remarks>
        string StationName { get; }
    }
}
