using FsInfoCat.Model;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Generic interface for list item entities containing extended file properties for photo files.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamPhotoPropertiesListItem" />
public interface ILocalPhotoPropertiesListItem : ILocalPhotoPropertiesRow, ILocalPropertiesListItem, IPhotoPropertiesListItem { }
