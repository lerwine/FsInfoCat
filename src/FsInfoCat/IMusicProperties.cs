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
        string[] Artist { get; }

        /// <summary>
        /// Gets the Composer
        /// </summary>
        /// <remarks>ID: {64440492-4C8B-11D1-8B70-080036B11A03}, 19 (MEDIAFILESUMMARYINFORMATION)</remarks>
        string[] Composer { get; }

        /// <summary>
        /// Gets the Conductor
        /// </summary>
        /// <remarks>ID: {56A3372E-CE9C-11D2-9F0E-006097C686F6}, 36 (MUSIC)</remarks>
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
        string[] Genre { get; }

        /// <summary>
        /// Gets the Part of Set
        /// </summary>
        /// <remarks>ID: {56A3372E-CE9C-11D2-9F0E-006097C686F6}, 37 (MUSIC)</remarks>
        string PartOfSet { get; }

        /// <summary>
        /// Gets the Period
        /// </summary>
        /// <remarks>ID: {64440492-4C8B-11D1-8B70-080036B11A03}, 31 (MEDIAFILESUMMARYINFORMATION)</remarks>
        string Period { get; }

        /// <summary>
        /// Gets the Track Number
        /// </summary>
        /// <remarks>ID: {56A3372E-CE9C-11D2-9F0E-006097C686F6}, 7 (MUSIC)</remarks>
        uint? TrackNumber { get; }
    }
}
