using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="IUpstreamSummaryPropertiesRow" />
    /// <seealso cref="IUpstreamPropertiesListItem" />
    /// <seealso cref="ISummaryPropertiesListItem" />
    /// <seealso cref="Local.Model.ISummaryPropertiesListItem" />
    public interface IUpstreamSummaryPropertiesListItem : IUpstreamSummaryPropertiesRow, IUpstreamPropertiesListItem, ISummaryPropertiesListItem { }
}
