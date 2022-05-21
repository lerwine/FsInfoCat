using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IGPSPropertiesRow" />
    /// <seealso cref="Local.Model.ILocalGPSPropertiesRow" />
    public interface IUpstreamGPSPropertiesRow : IUpstreamPropertiesRow, IGPSPropertiesRow { }
}
