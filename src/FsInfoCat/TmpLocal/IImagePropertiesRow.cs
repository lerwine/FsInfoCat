using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="M.IImagePropertiesRow" />
    /// <seealso cref="Upstream.Model.IImagePropertiesRow" />
    public interface ILocalImagePropertiesRow : ILocalPropertiesRow, M.IImagePropertiesRow { }
}
