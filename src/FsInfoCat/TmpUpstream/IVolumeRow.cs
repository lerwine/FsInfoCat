using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities which represent a logical file system volume.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="M.IVolumeRow" />
    /// <seealso cref="Local.Model.IVolumeRow" />
    public interface IUpstreamVolumeRow : IUpstreamDbEntity, M.IVolumeRow { }
}
