using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IGPSPropertiesRow" />
    /// <seealso cref="Upstream.Model.IGPSPropertiesRow" />
    public interface ILocalGPSPropertiesRow : ILocalPropertiesRow, IGPSPropertiesRow { }
}
