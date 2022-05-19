using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="M.ISummaryPropertiesRow" />
    /// <seealso cref="Local.Model.ISummaryPropertiesRow" />
    public interface IUpstreamSummaryPropertiesRow : IUpstreamPropertiesRow, M.ISummaryPropertiesRow { }
}
