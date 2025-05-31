using FsInfoCat.Model;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Generic interface for list item entities containing extended file GPS information properties.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamGPSPropertiesListItem" />
public interface ILocalGPSPropertiesListItem : ILocalGPSPropertiesRow, ILocalPropertiesListItem, IGPSPropertiesListItem { }
