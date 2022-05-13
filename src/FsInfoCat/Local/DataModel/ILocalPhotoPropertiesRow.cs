namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for photo files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IPhotoPropertiesRow" />
    public interface ILocalPhotoPropertiesRow : ILocalPropertiesRow, IPhotoPropertiesRow { }
}
