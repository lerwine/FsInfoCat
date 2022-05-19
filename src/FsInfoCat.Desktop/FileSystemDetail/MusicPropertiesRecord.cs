using FsInfoCat.Collections;

namespace FsInfoCat.Desktop.FileSystemDetail
{
    public record MusicPropertiesRecord : Model.IMusicProperties
    {
        public string AlbumArtist { get; init; }

        public string AlbumTitle { get; init; }

        public MultiStringValue Artist { get; init; }

        public uint? ChannelCount { get; init; }

        public MultiStringValue Composer { get; init; }

        public MultiStringValue Conductor { get; init; }

        public string DisplayArtist { get; init; }

        public MultiStringValue Genre { get; init; }

        public string PartOfSet { get; init; }

        public string Period { get; init; }

        public uint? TrackNumber { get; init; }

        public bool Equals(Model.IMusicProperties other)
        {
            // TODO: Implement Equals(IMusicProperties);
            throw new System.NotImplementedException();
        }
    }
}
