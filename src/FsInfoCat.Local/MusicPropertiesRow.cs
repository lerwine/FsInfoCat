using FsInfoCat.Collections;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
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
        public string AlbumArtist { get => _albumArtist; set => _albumArtist = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string AlbumTitle { get => _albumTitle; set => _albumTitle = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue Artist { get; set; }

        public uint? ChannelCount { get; set; }

        public MultiStringValue Composer { get; set; }

        public MultiStringValue Conductor { get; set; }

        [NotNull]
        public string DisplayArtist { get => _displayArtist; set => _displayArtist = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue Genre { get; set; }

        [NotNull]
        public string PartOfSet { get => _partOfSet; set => _partOfSet = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string Period { get => _period; set => _period = value.AsWsNormalizedOrEmpty(); }

        public uint? TrackNumber { get; set; }

        #endregion

        protected bool ArePropertiesEqual([DisallowNull] IMusicProperties other)
        {
            throw new NotImplementedException();
        }

        public abstract bool Equals(IMusicPropertiesRow other);

        public abstract bool Equals(IMusicProperties other);
    }
}
