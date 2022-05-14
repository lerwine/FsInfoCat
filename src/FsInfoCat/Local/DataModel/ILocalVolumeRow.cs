namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities which represent a logical file system volume.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IVolumeRow" />
    public interface ILocalVolumeRow : ILocalDbEntity, IVolumeRow { }
}
