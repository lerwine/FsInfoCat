using FsInfoCat.Collections;

namespace FsInfoCat
{
    /// <summary>
    /// Represents extended file properties for music files.
    /// </summary>
    /// <seealso cref="IMusicPropertySet"/>
    /// <seealso cref="Local.ILocalMusicPropertySet"/>
    /// <seealso cref="Upstream.IUpstreamMusicPropertySet"/>
    /// <seealso cref="FilePropertiesComparer.Equals(IMusicProperties, IMusicProperties)"/>
    /// <seealso cref="Local.IFileDetailProvider.GetMusicPropertiesAsync(System.Threading.CancellationToken)"/>
    /// <seealso cref="IDbContext.FindMatchingAsync(IMusicProperties, System.Threading.CancellationToken)"/>
    public interface IMusicProperties
    {
        /// <summary>
        /// Gets the Album Artist
        /// </summary>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Album Artist</description></item>
        /// <item><term>Format ID</term><description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description></item>
        /// <item><term>Property ID</term><description>13</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-albumartist">[Reference Link]</a></description></item>
        /// </list></remarks>
        string AlbumArtist { get; }

        /// <summary>
        /// Gets the Album Title
        /// </summary>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Album Title</description></item>
        /// <item><term>Format ID</term><description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description></item>
        /// <item><term>Property ID</term><description>4</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-albumtitle">[Reference Link]</a></description></item>
        /// </list></remarks>
        string AlbumTitle { get; }

        /// <summary>
        /// Gets the Contributing Artist
        /// </summary>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Contributing Artist</description></item>
        /// <item><term>Format ID</term><description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description></item>
        /// <item><term>Property ID</term><description>2</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-artist">[Reference Link]</a></description></item>
        /// </list></remarks>
        MultiStringValue Artist { get; }

        /// <summary>
        /// Gets the Channel Count.
        /// </summary>
        /// <value>Indicates the channel count for the audio file. Possible values are 1 for mono and 2 for stereo.</value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Channel Count</description></item>
        /// <item><term>Format ID</term><description>{64440490-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>2</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-artist">[Reference Link]</a></description></item>
        /// </list></remarks>
        uint? ChannelCount { get; }

        /// <summary>
        /// Gets the Composer
        /// </summary>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Composer</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>19</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-composer">[Reference Link]</a></description></item>
        /// </list></remarks>
        MultiStringValue Composer { get; }

        /// <summary>
        /// Gets the Conductor
        /// </summary>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Conductor</description></item>
        /// <item><term>Format ID</term><description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description></item>
        /// <item><term>Property ID</term><description>36</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-conductor">[Reference Link]</a></description></item>
        /// </list></remarks>
        MultiStringValue Conductor { get; }

        /// <summary>
        /// Gets the Album Artist (best match of relevant properties).
        /// </summary>
        /// <value>
        /// The best representation of Album Artist for a given music file based upon AlbumArtist, ContributingArtist and compilation info.
        /// </value>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <para>This property returns the best representation of the album artist for a specific music file based upon System.Music.AlbumArtist, System.Music.Artist, and System.Music.IsCompilation information.</para>
        /// <list type="bullet">
        /// <item><term>Name</term><description>Display Artist</description></item>
        /// <item><term>Format ID</term><description>{FD122953-FA93-4EF7-92C3-04C946B2F7C8} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-displayartist">[Reference Link]</a></description></item>
        /// </list></remarks>
        string DisplayArtist { get; }

        /// <summary>
        /// Gets the Genre
        /// </summary>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Genre</description></item>
        /// <item><term>Format ID</term><description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description></item>
        /// <item><term>Property ID</term><description>11</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-genre">[Reference Link]</a></description></item>
        /// </list></remarks>
        MultiStringValue Genre { get; }

        /// <summary>
        /// Gets the Part of Set
        /// </summary>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Part of Set</description></item>
        /// <item><term>Format ID</term><description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description></item>
        /// <item><term>Property ID</term><description>37</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-partofset">[Reference Link]</a></description></item>
        /// </list></remarks>
        string PartOfSet { get; }

        /// <summary>
        /// Gets the Period
        /// </summary>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Period</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>31</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-period">[Reference Link]</a></description></item>
        /// </list></remarks>
        string Period { get; }

        /// <summary>
        /// Gets the Track Number
        /// </summary>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>#</description></item>
        /// <item><term>Format ID</term><description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description></item>
        /// <item><term>Property ID</term><description>7</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-tracknumber">[Reference Link]</a></description></item>
        /// </list></remarks>
        /// <remarks>ID: {56A3372E-CE9C-11D2-9F0E-006097C686F6}, 7 (MUSIC)</remarks>
        uint? TrackNumber { get; }
    }
}
