using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="Local.Model.IPropertiesRow" />
    public interface IUpstreamPropertiesRow : IUpstreamDbEntity, IPropertiesRow { }
}
