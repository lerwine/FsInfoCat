namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for list item entities which represent a logical file system volume and contains associated file system properties.
    /// </summary>
    /// <seealso cref="IUpstreamVolumeListItem" />
    /// <seealso cref="IVolumeListItemWithFileSystem" />
    /// <seealso cref="Local.ILocalVolumeListItemWithFileSystem" />
    public interface IUpstreamVolumeListItemWithFileSystem : IUpstreamVolumeListItem, IVolumeListItemWithFileSystem { }
}
