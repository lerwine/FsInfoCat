using FsInfoCat.Model;
using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Base class for entities containing extended file properties for music files.
/// </summary>
/// <seealso cref="MusicPropertiesListItem" />
/// <seealso cref="MusicPropertySet" />
/// <seealso cref="LocalDbContext.MusicPropertySets" />
/// <seealso cref="LocalDbContext.MusicPropertiesListing" />
public abstract class MusicPropertiesRow : PropertiesRow, ILocalMusicPropertiesRow
{
    #region Fields

    private string _albumArtist = string.Empty;
    private string _albumTitle = string.Empty;
    private string _displayArtist = string.Empty;
    private string _partOfSet = string.Empty;
    private string _period = string.Empty;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the Album Artist.
    /// </summary>
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
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-albumartist">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.AlbumArtist), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_albumArtist))]
    public string AlbumArtist { get => _albumArtist; set => _albumArtist = value.AsWsNormalizedOrEmpty(); }

    /// <summary>
    /// Gets the Album Title.
    /// </summary>
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
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-albumtitle">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.AlbumTitle), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_albumTitle))]
    public string AlbumTitle { get => _albumTitle; set => _albumTitle = value.AsWsNormalizedOrEmpty(); }

    /// <summary>
    /// Gets the Contributing Artist.
    /// </summary>
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
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-artist">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Artist), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public MultiStringValue Artist { get; set; }

    /// <summary>
    /// Gets the Channel Count.
    /// </summary>
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
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-artist">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.ChannelCount), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public uint? ChannelCount { get; set; }

    /// <summary>
    /// Gets the Composer.
    /// </summary>
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
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-composer">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Composer), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public MultiStringValue Composer { get; set; }

    /// <summary>
    /// Gets the Conductor.
    /// </summary>
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
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-conductor">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Conductor), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public MultiStringValue Conductor { get; set; }

    /// <summary>
    /// Gets the Album Artist (best match of relevant properties).
    /// </summary>
    /// <value>The best representation of Album Artist for a given music file based upon AlbumArtist, ContributingArtist and compilation info.</value>
    /// <remarks>
    /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
    /// <para>This property returns the best representation of the album artist for a specific music file based upon System.Music.AlbumArtist, System.Music.Artist,
    /// and System.Music.IsCompilation information.</para><list type="bullet">
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
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-displayartist">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayArtist), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_displayArtist))]
    public string DisplayArtist { get => _displayArtist; set => _displayArtist = value.AsWsNormalizedOrEmpty(); }

    /// <summary>
    /// Gets the Genre.
    /// </summary>
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
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-genre">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Genre), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public MultiStringValue Genre { get; set; }

    /// <summary>
    /// Gets the Part of the Set.
    /// </summary>
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
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-partofset">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.PartOfSet), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_partOfSet))]
    public string PartOfSet { get => _partOfSet; set => _partOfSet = value.AsWsNormalizedOrEmpty(); }

    /// <summary>
    /// Gets the Period.
    /// </summary>
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
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-period">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.Period), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    [NotNull]
    [BackingField(nameof(_period))]
    public string Period { get => _period; set => _period = value.AsWsNormalizedOrEmpty(); }

    /// <summary>
    /// Gets the Track Number.
    /// </summary>
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
    ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-tracknumber">[Reference Link]</see>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Display(Name = nameof(FsInfoCat.Properties.Resources.TrackNumber), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public uint? TrackNumber { get; set; }

    #endregion

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="ILocalMusicPropertiesRow" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected bool ArePropertiesEqual([DisallowNull] ILocalMusicPropertiesRow other) => ArePropertiesEqual((IMusicPropertiesRow)other) &&
        EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
        LastSynchronizedOn == other.LastSynchronizedOn;

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="IMusicPropertiesRow" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected bool ArePropertiesEqual([DisallowNull] IMusicPropertiesRow other) => ArePropertiesEqual((IMusicProperties)other) &&
        CreatedOn == other.CreatedOn &&
        ModifiedOn == other.ModifiedOn;

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="IMusicProperties" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected bool ArePropertiesEqual([DisallowNull] IMusicProperties other) => _albumArtist == other.AlbumArtist &&
        _albumTitle == other.AlbumTitle &&
        _displayArtist == other.DisplayArtist &&
        _partOfSet == other.PartOfSet &&
        _period == other.Period &&
        EqualityComparer<MultiStringValue>.Default.Equals(Artist, other.Artist) &&
        ChannelCount == other.ChannelCount &&
        EqualityComparer<MultiStringValue>.Default.Equals(Composer, other.Composer) &&
        EqualityComparer<MultiStringValue>.Default.Equals(Conductor, other.Conductor) &&
        EqualityComparer<MultiStringValue>.Default.Equals(Genre, other.Genre) &&
        TrackNumber == other.TrackNumber;
    //EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
    //LastSynchronizedOn == other.LastSynchronizedOn &&
    //CreatedOn == other.CreatedOn &&
    //ModifiedOn == other.ModifiedOn;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract bool Equals(IMusicPropertiesRow other);

    public abstract bool Equals(IMusicProperties other);

    public override int GetHashCode()
    {
        if (TryGetId(out Guid id)) return id.GetHashCode();
        HashCode hash = new();
        hash.Add(_albumArtist);
        hash.Add(_albumTitle);
        hash.Add(_displayArtist);
        hash.Add(_partOfSet);
        hash.Add(_period);
        hash.Add(Artist);
        hash.Add(ChannelCount);
        hash.Add(Composer);
        hash.Add(Conductor);
        hash.Add(Genre);
        hash.Add(TrackNumber);
        hash.Add(UpstreamId);
        hash.Add(LastSynchronizedOn);
        hash.Add(CreatedOn);
        hash.Add(ModifiedOn);
        return hash.ToHashCode();
    }

    protected virtual string PropertiesToString() => $@"AlbumTitle=""{ExtensionMethods.EscapeCsString(_albumTitle)}"", DisplayArtist=""{ExtensionMethods.EscapeCsString(_displayArtist)}"",
    ChannelCount={ChannelCount}, TrackNumber={TrackNumber}, PartOfSet=""{ExtensionMethods.EscapeCsString(_partOfSet)}"", Period=""{ExtensionMethods.EscapeCsString(_period)}"", Genre={Genre.ToCsString()},
    AlbumArtist=""{ExtensionMethods.EscapeCsString(_albumArtist)}"", Artist={Artist.ToCsString()}, Composer={Composer.ToCsString()}, Conductor={Conductor.ToCsString()}";

    public override string ToString() => $@"{{ Id={(TryGetId(out Guid id) ? id : null)}, {PropertiesToString()},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId} }}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
