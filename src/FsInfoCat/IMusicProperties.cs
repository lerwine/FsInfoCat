using FsInfoCat.Collections;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>Represents extended file properties for music files.</summary>
    public interface IMusicProperties
    {
        /// <summary>Gets the Album Artist.</summary>
        /// <value>The Album Artist</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Album Artist</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>13</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-albumartist">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_AlbumArtist), ResourceType = typeof(Properties.Resources))]
        string AlbumArtist { get; }

        /// <summary>Gets the Album Title.</summary>
        /// <value>The Album Title</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Album Title</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>4</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-albumtitle">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_AlbumTitle), ResourceType = typeof(Properties.Resources))]
        string AlbumTitle { get; }

        /// <summary>Gets the Contributing Artist.</summary>
        /// <value>The Contributing Artist</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Contributing Artist</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>2</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-artist">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Artist), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Artist { get; }

        /// <summary>Gets the Channel Count.</summary>
        /// <value>Indicates the channel count for the audio file. Possible values are 1 for mono and 2 for stereo.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Channel Count</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440490-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>2</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-artist">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ChannelCount), ResourceType = typeof(Properties.Resources))]
        uint? ChannelCount { get; }

        /// <summary>Gets the Composer.</summary>
        /// <value>The Composer</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Composer</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>19</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-composer">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Composer), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Composer { get; }

        /// <summary>Gets the Conductor.</summary>
        /// <value>The Conductor</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Conductor</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>36</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-conductor">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Conductor), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Conductor { get; }

        /// <summary>Gets the Album Artist (best match of relevant properties).</summary>
        /// <value>The best representation of Album Artist for a given music file based upon AlbumArtist, ContributingArtist and compilation info.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <para>This property returns the best representation of the album artist for a specific music file based upon System.Music.AlbumArtist, System.Music.Artist, and System.Music.IsCompilation information.</para><list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Display Artist</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{FD122953-FA93-4EF7-92C3-04C946B2F7C8} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-displayartist">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayArtist), ResourceType = typeof(Properties.Resources))]
        string DisplayArtist { get; }

        /// <summary>Gets the Genre.</summary>
        /// <value>The Genre</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Genre</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>11</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-genre">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Genre), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Genre { get; }

        /// <summary>Gets the Part of the Set.</summary>
        /// <value>The Part of the Set</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Part of Set</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>37</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-partofset">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_PartOfSet), ResourceType = typeof(Properties.Resources))]
        string PartOfSet { get; }

        /// <summary>Gets the Period.</summary>
        /// <value>The Period</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Period</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>31</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-period">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Period), ResourceType = typeof(Properties.Resources))]
        string Period { get; }

        /// <summary>Gets the Track Number.</summary>
        /// <value>The Track Number</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>#</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>7</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-tracknumber">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_TrackNumber), ResourceType = typeof(Properties.Resources))]
        uint? TrackNumber { get; }
    }

}

