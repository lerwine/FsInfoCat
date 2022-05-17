namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for list item entities which represent a logical file system volume.
    /// </summary>
    /// <seealso cref="ILocalVolumeRow" />
    /// <seealso cref="IVolumeListItem" />
    /// <seealso cref="Upstream.IUpstreamVolumeListItem" />
    public interface ILocalVolumeListItem : ILocalVolumeRow, IVolumeListItem { }
}
