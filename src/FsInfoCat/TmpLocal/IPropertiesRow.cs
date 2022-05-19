using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="M.IPropertiesRow" />
    /// <seealso cref="Upstream.Model.IPropertiesRow" />
    public interface ILocalPropertiesRow : ILocalDbEntity, M.IPropertiesRow { }
}
