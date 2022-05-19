using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for photo files.
    /// </summary>
    /// <seealso cref="IUpstreamPhotoPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="M.IPhotoPropertiesListItem" />
    /// <seealso cref="Local.Model.IPhotoPropertiesListItem" />
    public interface IUpstreamPhotoPropertiesListItem : IUpstreamPropertiesListItem, M.IPhotoPropertiesListItem { }
}
