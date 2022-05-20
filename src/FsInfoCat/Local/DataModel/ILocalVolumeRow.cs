namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities which represent a logical file system volume.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IVolumeRow" />
    /// <seealso cref="Upstream.IUpstreamVolumeRow" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalVolumeRow")]
    public interface ILocalVolumeRow : ILocalDbEntity, IVolumeRow { }
}
