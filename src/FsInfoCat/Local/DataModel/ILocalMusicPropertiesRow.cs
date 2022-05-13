namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for music files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IMusicPropertiesRow" />
    public interface ILocalMusicPropertiesRow : ILocalPropertiesRow, IMusicPropertiesRow { }
}
