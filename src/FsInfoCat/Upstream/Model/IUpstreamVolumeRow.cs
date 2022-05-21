using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for entities which represent a logical file system volume.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IVolumeRow" />
    /// <seealso cref="Local.Model.ILocalVolumeRow" />
    public interface IUpstreamVolumeRow : IUpstreamDbEntity, IVolumeRow { }
}
