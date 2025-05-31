using FsInfoCat.Model;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Generic interface for list item entities containing extended file properties for image files.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamImagePropertiesListItem" />
public interface ILocalImagePropertiesListItem : ILocalImagePropertiesRow, ILocalPropertiesListItem, IImagePropertiesListItem { }
