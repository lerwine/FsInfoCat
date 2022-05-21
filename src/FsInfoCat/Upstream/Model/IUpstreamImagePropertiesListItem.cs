using FsInfoCat.Model;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="IUpstreamImagePropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="IImagePropertiesListItem" />
    /// <seealso cref="Local.Model.ILocalImagePropertiesListItem" />
    public interface IUpstreamImagePropertiesListItem : IUpstreamImagePropertiesRow, IUpstreamPropertiesListItem, IImagePropertiesListItem { }
}
