using FsInfoCat.Model;
using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
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

        [NotNull]
        [BackingField(nameof(_albumArtist))]
        public string AlbumArtist { get => _albumArtist; set => _albumArtist = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_albumTitle))]
        public string AlbumTitle { get => _albumTitle; set => _albumTitle = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue Artist { get; set; }

        public uint? ChannelCount { get; set; }

        public MultiStringValue Composer { get; set; }

        public MultiStringValue Conductor { get; set; }

        [NotNull]
        [BackingField(nameof(_displayArtist))]
        public string DisplayArtist { get => _displayArtist; set => _displayArtist = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue Genre { get; set; }

        [NotNull]
        [BackingField(nameof(_partOfSet))]
        public string PartOfSet { get => _partOfSet; set => _partOfSet = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_period))]
        public string Period { get => _period; set => _period = value.AsWsNormalizedOrEmpty(); }

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
}
