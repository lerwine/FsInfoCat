namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for list item entities which represent a logical file system volume.
    /// </summary>
    /// <seealso cref="IUpstreamVolumeRow" />
    /// <seealso cref="IVolumeListItem" />
    /// <seealso cref="Local.ILocalVolumeListItem" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamVolumeListItem")]
    public interface IUpstreamVolumeListItem : IUpstreamVolumeRow, IVolumeListItem { }
}
