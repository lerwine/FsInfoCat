using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities which represent a logical file system volume.
    /// </summary>
    /// <seealso cref="IUpstreamVolumeRow" />
    /// <seealso cref="IVolumeListItem" />
    /// <seealso cref="Local.Model.ILocalVolumeListItem" />
    public interface IUpstreamVolumeListItem : IUpstreamVolumeRow, IVolumeListItem { }
}
