using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for video files.
    /// </summary>
    /// <seealso cref="IUpstreamVideoPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="IVideoPropertiesListItem" />
    /// <seealso cref="Local.Model.ILocalVideoPropertiesListItem" />
    public interface IUpstreamVideoPropertiesListItem : IUpstreamVideoPropertiesRow, IUpstreamPropertiesListItem, IVideoPropertiesListItem { }
}
