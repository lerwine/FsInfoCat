using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="M.IMediaPropertiesRow" />
    /// <seealso cref="Upstream.Model.IMediaPropertiesRow" />
    public interface ILocalMediaPropertiesRow : ILocalPropertiesRow, M.IMediaPropertiesRow { }
}
