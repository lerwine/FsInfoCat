namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IMediaPropertiesRow" />
    public interface ILocalMediaPropertiesRow : ILocalPropertiesRow, IMediaPropertiesRow { }
}
