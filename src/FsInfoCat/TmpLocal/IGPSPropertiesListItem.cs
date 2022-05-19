using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="ILocalGPSPropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="M.IGPSPropertiesListItem" />
    /// <seealso cref="Upstream.Model.IGPSPropertiesListItem" />
    public interface ILocalGPSPropertiesListItem : ILocalGPSPropertiesRow, ILocalPropertiesListItem, M.IGPSPropertiesListItem { }
}
