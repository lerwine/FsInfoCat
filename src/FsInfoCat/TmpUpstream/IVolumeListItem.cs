using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities which represent a logical file system volume.
    /// </summary>
    /// <seealso cref="IUpstreamVolumeRow" />
    /// <seealso cref="M.IVolumeListItem" />
    /// <seealso cref="Local.Model.IVolumeListItem" />
    public interface IUpstreamVolumeListItem : IUpstreamVolumeRow, M.IVolumeListItem { }
}
