namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities which represent a logical file system volume.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IVolumeRow" />
    /// <seealso cref="Local.ILocalVolumeRow" />
    public interface IUpstreamVolumeRow : IUpstreamDbEntity, IVolumeRow { }
}
