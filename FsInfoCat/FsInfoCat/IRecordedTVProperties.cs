namespace FsInfoCat
{
    public interface IRecordedTVProperties : IPropertySet
    {
        /// <summary>
        /// Gets the Channel Number
        /// </summary>
        /// <remarks>Example: 42
        /// <para>ID: {6D748DE2-8D38-4CC3-AC60-F009B057C557}, 7</para></remarks>
        uint? ChannelNumber { get; set; }

        /// <summary>
        /// Gets the Episode Name
        /// </summary>
        /// <remarks>Example: &quot;Nowhere to Hyde&quot;
        /// <para>ID: {6D748DE2-8D38-4CC3-AC60-F009B057C557}, 2</para></remarks>
        string EpisodeName { get; set; }

        /// <summary>
        /// Indicates whether the video is DTV
        /// </summary>
        /// <remarks>ID: {6D748DE2-8D38-4CC3-AC60-F009B057C557}, 17</remarks>
        bool? IsDTVContent { get; set; }

        /// <summary>
        /// Indicates whether the video is HD
        /// </summary>
        /// <remarks>ID: {6D748DE2-8D38-4CC3-AC60-F009B057C557}, 18</remarks>
        bool? IsHDContent { get; set; }

        /// <summary>
        /// Gets the Network Affiliation
        /// </summary>
        /// <remarks>ID: {2C53C813-FB63-4E22-A1AB-0B331CA1E273}, 100</remarks>
        string NetworkAffiliation { get; set; }

        /// <summary>
        /// Gets the Original Broadcast Date
        /// </summary>
        /// <remarks>ID: {4684FE97-8765-4842-9C13-F006447B178C}, 100</remarks>
        System.DateTime? OriginalBroadcastDate { get; set; }

        /// <summary>
        /// Gets the Program Description
        /// </summary>
        /// <remarks>ID: {6D748DE2-8D38-4CC3-AC60-F009B057C557}, 3</remarks>
        string ProgramDescription { get; set; }

        /// <summary>
        /// Gets the Station Call Sign
        /// </summary>
        /// <remarks>Example: &quot;TOONP&quot;
        /// <para>ID: {6D748DE2-8D38-4CC3-AC60-F009B057C557}, 5</para></remarks>
        string StationCallSign { get; set; }

        /// <summary>
        /// Gets the Station Name
        /// </summary>
        /// <remarks>ID: {1B5439E7-EBA1-4AF8-BDD7-7AF1D4549493}, 100</remarks>
        string StationName { get; set; }
    }
}
