using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities which represent a logical file system volume.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="M.IVolumeRow" />
    /// <seealso cref="Upstream.Model.IVolumeRow" />
    public interface ILocalVolumeRow : ILocalDbEntity, M.IVolumeRow { }
}
