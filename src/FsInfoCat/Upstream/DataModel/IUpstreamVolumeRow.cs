namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities which represent a logical file system volume.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IVolumeRow" />
    /// <seealso cref="Local.ILocalVolumeRow" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamVolumeRow")]
    public interface IUpstreamVolumeRow : IUpstreamDbEntity, IVolumeRow { }
}
