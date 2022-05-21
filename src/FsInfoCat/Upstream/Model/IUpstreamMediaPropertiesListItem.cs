using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="IUpstreamMediaPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="IMediaPropertiesListItem" />
    /// <seealso cref="Local.Model.ILocalMediaPropertiesListItem" />
    public interface IUpstreamMediaPropertiesListItem : IUpstreamMediaPropertiesRow, IUpstreamPropertiesListItem, IMediaPropertiesListItem { }
}
