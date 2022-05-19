using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="IUpstreamMediaPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="M.IMediaPropertiesListItem" />
    /// <seealso cref="Local.Model.IMediaPropertiesListItem" />
    public interface IUpstreamMediaPropertiesListItem : IUpstreamMediaPropertiesRow, IUpstreamPropertiesListItem, M.IMediaPropertiesListItem { }
}
