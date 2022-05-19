using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="ILocalImagePropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="M.IImagePropertiesListItem" />
    /// <seealso cref="Upstream.Model.IImagePropertiesListItem" />
    public interface ILocalImagePropertiesListItem : ILocalImagePropertiesRow, ILocalPropertiesListItem, M.IImagePropertiesListItem { }
}
