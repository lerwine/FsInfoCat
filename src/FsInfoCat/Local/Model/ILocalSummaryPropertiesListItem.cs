using FsInfoCat.Model;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Generic interface for entities containing extended file summary properties.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamSummaryPropertiesListItem" />
public interface ILocalSummaryPropertiesListItem : ILocalSummaryPropertiesRow, ILocalPropertiesListItem, ISummaryPropertiesListItem { }
