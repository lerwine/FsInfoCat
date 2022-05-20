namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for list item entities which represent a logical file system volume and contains associated file system properties.
    /// </summary>
    /// <seealso cref="ILocalVolumeListItem" />
    /// <seealso cref="IVolumeListItemWithFileSystem" />
    /// <seealso cref="Upstream.IUpstreamVolumeListItemWithFileSystem" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalVolumeListItemWithFileSystem")]
    public interface ILocalVolumeListItemWithFileSystem : ILocalVolumeListItem, IVolumeListItemWithFileSystem { }
}
