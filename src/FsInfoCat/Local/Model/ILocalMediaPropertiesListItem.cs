using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="ILocalMediaPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IMediaPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamMediaPropertiesListItem" />
    public interface ILocalMediaPropertiesListItem : ILocalMediaPropertiesRow, ILocalPropertiesListItem, IMediaPropertiesListItem { }
}
