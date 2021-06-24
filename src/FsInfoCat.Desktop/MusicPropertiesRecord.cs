namespace FsInfoCat.Desktop
{
    public record MusicPropertiesRecord : IMusicProperties
    {
        public string AlbumArtist { get; init; }

        public string AlbumTitle { get; init; }

        public string[] Artist { get; init; }

        public string[] Composer { get; init; }

        public string[] Conductor { get; init; }

        public string DisplayArtist { get; init; }

        public string[] Genre { get; init; }

        public string PartOfSet { get; init; }

        public string Period { get; init; }

        public uint? TrackNumber { get; init; }
    }
}
