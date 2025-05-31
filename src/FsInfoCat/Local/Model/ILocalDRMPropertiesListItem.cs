using FsInfoCat.Model;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Generic interface for list item entities containing extended file DRM information properties.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamDRMPropertiesListItem" />
public interface ILocalDRMPropertiesListItem : ILocalDRMPropertiesRow, ILocalPropertiesListItem, IDRMPropertiesListItem { }
