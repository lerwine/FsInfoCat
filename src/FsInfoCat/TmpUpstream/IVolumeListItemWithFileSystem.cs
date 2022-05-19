using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for list item entities which represent a logical file system volume and contains associated file system properties.
    /// </summary>
    /// <seealso cref="IUpstreamVolumeListItem" />
    /// <seealso cref="M.IVolumeListItemWithFileSystem" />
    /// <seealso cref="Local.Model.IVolumeListItemWithFileSystem" />
    public interface IUpstreamVolumeListItemWithFileSystem : IUpstreamVolumeListItem, M.IVolumeListItemWithFileSystem { }
}
