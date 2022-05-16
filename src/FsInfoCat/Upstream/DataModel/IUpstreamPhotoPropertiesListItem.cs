namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for photo files.
    /// Implements the <see cref="IPhotoPropertiesListItem" />
    /// </summary>
    /// <seealso cref="IUpstreamPhotoPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="IPhotoPropertiesListItem" />
    /// <seealso cref="Local.ILocalPhotoPropertiesListItem" />
    public interface IUpstreamPhotoPropertiesListItem : IUpstreamPropertiesListItem, IPhotoPropertiesListItem { }
}
