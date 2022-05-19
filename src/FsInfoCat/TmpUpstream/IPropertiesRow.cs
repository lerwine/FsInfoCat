using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="M.IPropertiesRow" />
    /// <seealso cref="Local.Model.IPropertiesRow" />
    public interface IUpstreamPropertiesRow : IUpstreamDbEntity, M.IPropertiesRow { }
}
