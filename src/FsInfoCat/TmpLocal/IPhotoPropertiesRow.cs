using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for photo files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="M.IPhotoPropertiesRow" />
    /// <seealso cref="Upstream.Model.IPhotoPropertiesRow" />
    public interface ILocalPhotoPropertiesRow : ILocalPropertiesRow, M.IPhotoPropertiesRow { }
}
