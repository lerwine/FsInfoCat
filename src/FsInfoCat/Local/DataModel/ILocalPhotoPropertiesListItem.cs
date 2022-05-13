namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for photo files.
    /// Implements the <see cref="IPhotoPropertiesListItem" />
    /// </summary>
    /// <seealso cref="ILocalPhotoPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IPhotoPropertiesListItem" />
    public interface ILocalPhotoPropertiesListItem : ILocalPhotoPropertiesRow, ILocalPropertiesListItem, IPhotoPropertiesListItem { }
}
