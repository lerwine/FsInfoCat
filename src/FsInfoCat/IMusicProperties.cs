namespace FsInfoCat
{
    public interface IMusicProperties
    {
        /// <summary>
        /// Gets the Album Artist
        /// </summary>
        /// <remarks>ID: {56A3372E-CE9C-11D2-9F0E-006097C686F6}, 13 (MUSIC)</remarks>
        string AlbumArtist { get; }

        /// <summary>
        /// Gets the Album Title
        /// </summary>
        /// <remarks>ID: {56A3372E-CE9C-11D2-9F0E-006097C686F6}, 4 (MUSIC)</remarks>
        string AlbumTitle { get; }

        /// <summary>
        /// Gets the Artist
        /// </summary>
        /// <remarks>ID: {56A3372E-CE9C-11D2-9F0E-006097C686F6}, 2 (MUSIC)</remarks>
        // BUG: Can't be stored in database this way
        string[] Artist { get; }

        /// <summary>Indicates the channel count for the audio file.</summary>
        /// <value>
        ///   <c>1</c> for monaural audio; <c>2</c> for stereo; otherwise, <span class="keyword"><span class="languageSpecificText"><span class="cs">null</span><span class="vb">Nothing</span><span class="cpp">nullptr</span></span></span><span class="nu">a null reference (<span class="keyword">Nothing</span> in Visual Basic)</span> if this value is not specified.
        /// <list type="bullet">
        /// <item><term>Reference</term><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-channelcount">System.Audio.ChannelCount</a></description></item>
        /// <item><term>Format ID</term><description>64440490-4C8B-11D1-8B70-080036B11A03</description></item>
        /// <item><term>Property ID</term><description>7</description></item>
        /// </list></value>
        uint? ChannelCount { get; }

        /// <summary>
        /// Gets the Composer
        /// </summary>
        /// <remarks>ID: {64440492-4C8B-11D1-8B70-080036B11A03}, 19 (MEDIAFILESUMMARYINFORMATION)</remarks>
        // BUG: Can't be stored in database this way
        string[] Composer { get; }

        /// <summary>
        /// Gets the Conductor
        /// </summary>
        /// <remarks>ID: {56A3372E-CE9C-11D2-9F0E-006097C686F6}, 36 (MUSIC)</remarks>
        // BUG: Can't be stored in database this way
        string[] Conductor { get; }

        /// <summary>
        /// This property returns the best representation of Album Artist for a given music file based upon AlbumArtist, ContributingArtist and compilation info.
        /// </summary>
        /// <remarks>ID: {FD122953-FA93-4EF7-92C3-04C946B2F7C8}, 100</remarks>
        string DisplayArtist { get; }

        /// <summary>
        /// Gets the Genre
        /// </summary>
        /// <remarks>ID: {56A3372E-CE9C-11D2-9F0E-006097C686F6}, 11 (MUSIC)</remarks>
        // BUG: Can't be stored in database this way
        string[] Genre { get; }

        /// <summary>
        /// Gets the Part of Set
        /// </summary>
        /// <remarks>ID: {56A3372E-CE9C-11D2-9F0E-006097C686F6}, 37 (MUSIC)</remarks>
        string PartOfSet { get; }

        /// <summary>Gets the Period</summary>
        /// <remarks>
        /// ID: {64440492-4C8B-11D1-8B70-080036B11A03}, 31 (MEDIAFILESUMMARYINFORMATION)</remarks>
        string Period { get; }

        /// <summary>
        /// Gets the Track Number
        /// </summary>
        /// <remarks>ID: {56A3372E-CE9C-11D2-9F0E-006097C686F6}, 7 (MUSIC)</remarks>
        uint? TrackNumber { get; }
    }
}
