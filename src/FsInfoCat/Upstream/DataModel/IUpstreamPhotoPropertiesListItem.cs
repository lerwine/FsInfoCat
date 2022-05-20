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
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamPhotoPropertiesListItem")]
    public interface IUpstreamPhotoPropertiesListItem : IUpstreamPropertiesListItem, IPhotoPropertiesListItem { }
}
