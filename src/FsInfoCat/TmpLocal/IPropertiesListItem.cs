using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="M.IPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IPropertiesListItem" />
    public interface ILocalPropertiesListItem : ILocalPropertiesRow, M.IPropertiesListItem { }
}
