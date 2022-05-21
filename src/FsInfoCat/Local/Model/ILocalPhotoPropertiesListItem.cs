using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for photo files.
    /// Implements the <see cref="IPhotoPropertiesListItem" />
    /// </summary>
    /// <seealso cref="ILocalPhotoPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="M.IPhotoPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamPhotoPropertiesListItem" />
    public interface ILocalPhotoPropertiesListItem : ILocalPhotoPropertiesRow, ILocalPropertiesListItem, M.IPhotoPropertiesListItem { }
}
