namespace FsInfoCat
{
    public interface IRecordedTVProperties
    {
        /// <summary>
        /// Gets the Channel Number.
        /// </summary>
        /// <value>The channel number or <see langword="null"/> if this value is not specified.</value>
        /// <remarks>Example: 42
        /// <para>ID: {6D748DE2-8D38-4CC3-AC60-F009B057C557}, 7</para></remarks>
        uint? ChannelNumber { get; }

        /// <summary>
        /// Gets the Episode Name.
        /// </summary>
        /// <value>The name of the episode or <see langword="null"/> if this value is not specified.</value>
        /// <remarks>Example: &quot;Nowhere to Hyde&quot;
        /// <para>ID: {6D748DE2-8D38-4CC3-AC60-F009B057C557}, 2</para></remarks>
        string EpisodeName { get; }

        /// <summary>
        /// Indicates whether the video is DTV.
        /// </summary>
        /// <value><see langword="true"/> if the video is DTV; <see langword="false"/> if it is not DTV; otherwise, <see langword="null"/> if this value is not specified.</value>
        /// <remarks>ID: {6D748DE2-8D38-4CC3-AC60-F009B057C557}, 17</remarks>
        bool? IsDTVContent { get; }

        /// <summary>
        /// Indicates whether the video is HD.
        /// </summary>
        /// <value><see langword="true"/> if the video is HD; <see langword="false"/> if it is not HD; otherwise, <see langword="null"/> if this value is not specified.</value>
        /// <remarks>ID: {6D748DE2-8D38-4CC3-AC60-F009B057C557}, 18</remarks>
        bool? IsHDContent { get; }

        /// <summary>
        /// Gets the Network Affiliation.
        /// </summary>
        /// <value>The name of the affiliated network or <see langword="null"/> if this value is not specified.</value>
        /// <remarks>ID: {2C53C813-FB63-4E22-A1AB-0B331CA1E273}, 100</remarks>
        string NetworkAffiliation { get; }

        /// <summary>
        /// Gets the Original Broadcast Date.
        /// </summary>
        /// <value>The original broadcast date or <see langword="null"/> if this value is not specified.</value>
        /// <remarks>ID: {4684FE97-8765-4842-9C13-F006447B178C}, 100</remarks>
        System.DateTime? OriginalBroadcastDate { get; }

        /// <summary>
        /// Gets the Program Description.
        /// </summary>
        /// <value>The program description or <see langword="null"/> if this value is not specified.</value>
        /// <remarks>ID: {6D748DE2-8D38-4CC3-AC60-F009B057C557}, 3</remarks>
        string ProgramDescription { get; }

        /// <summary>
        /// Gets the Station Call Sign.
        /// </summary>
        /// <value>The call sign of the broadcast station or <see langword="null"/> if this value is not specified.</value>
        /// <remarks>Example: &quot;TOONP&quot;
        /// <para>ID: {6D748DE2-8D38-4CC3-AC60-F009B057C557}, 5</para></remarks>
        string StationCallSign { get; }

        /// <summary>
        /// Gets the Station Name.
        /// </summary>
        /// <value>The  name of the broadcast station or <see langword="null"/> if this value is not specified.</value>
        /// <remarks>ID: {1B5439E7-EBA1-4AF8-BDD7-7AF1D4549493}, 100</remarks>
        string StationName { get; }
    }
}
