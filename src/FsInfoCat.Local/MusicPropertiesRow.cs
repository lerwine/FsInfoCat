using FsInfoCat.Collections;

namespace FsInfoCat.Local
{
    public class MusicPropertiesRow : PropertiesRow, IMusicProperties
    {
        #region Fields

        private readonly IPropertyChangeTracker<string> _albumArtist;
        private readonly IPropertyChangeTracker<string> _albumTitle;
        private readonly IPropertyChangeTracker<MultiStringValue> _artist;
        private readonly IPropertyChangeTracker<uint?> _channelCount;
        private readonly IPropertyChangeTracker<MultiStringValue> _composer;
        private readonly IPropertyChangeTracker<MultiStringValue> _conductor;
        private readonly IPropertyChangeTracker<string> _displayArtist;
        private readonly IPropertyChangeTracker<MultiStringValue> _genre;
        private readonly IPropertyChangeTracker<string> _partOfSet;
        private readonly IPropertyChangeTracker<string> _period;
        private readonly IPropertyChangeTracker<uint?> _trackNumber;

        #endregion

        #region Properties

        public string AlbumArtist { get => _albumArtist.GetValue(); set => _albumArtist.SetValue(value); }
        public string AlbumTitle { get => _albumTitle.GetValue(); set => _albumTitle.SetValue(value); }
        public MultiStringValue Artist { get => _artist.GetValue(); set => _artist.SetValue(value); }
        public uint? ChannelCount { get => _channelCount.GetValue(); set => _channelCount.SetValue(value); }
        public MultiStringValue Composer { get => _composer.GetValue(); set => _composer.SetValue(value); }
        public MultiStringValue Conductor { get => _conductor.GetValue(); set => _conductor.SetValue(value); }
        public string DisplayArtist { get => _displayArtist.GetValue(); set => _displayArtist.SetValue(value); }
        public MultiStringValue Genre { get => _genre.GetValue(); set => _genre.SetValue(value); }
        public string PartOfSet { get => _partOfSet.GetValue(); set => _partOfSet.SetValue(value); }
        public string Period { get => _period.GetValue(); set => _period.SetValue(value); }
        public uint? TrackNumber { get => _trackNumber.GetValue(); set => _trackNumber.SetValue(value); }

        #endregion
        
        public MusicPropertiesRow()
        {
            _albumArtist = AddChangeTracker(nameof(AlbumArtist), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _albumTitle = AddChangeTracker(nameof(AlbumTitle), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _artist = AddChangeTracker<MultiStringValue>(nameof(Artist), null);
            _channelCount = AddChangeTracker<uint?>(nameof(ChannelCount), null);
            _composer = AddChangeTracker<MultiStringValue>(nameof(Composer), null);
            _conductor = AddChangeTracker<MultiStringValue>(nameof(Conductor), null);
            _displayArtist = AddChangeTracker(nameof(DisplayArtist), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _genre = AddChangeTracker<MultiStringValue>(nameof(Genre), null);
            _partOfSet = AddChangeTracker(nameof(PartOfSet), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _period = AddChangeTracker(nameof(Period), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _trackNumber = AddChangeTracker<uint?>(nameof(TrackNumber), null);
        }
    }
}
