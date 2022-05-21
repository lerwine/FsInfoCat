using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file summary properties.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="ISummaryPropertiesRow" />
    /// <seealso cref="Local.Model.ILocalSummaryPropertiesRow" />
    public interface IUpstreamSummaryPropertiesRow : IUpstreamPropertiesRow, ISummaryPropertiesRow { }
}
