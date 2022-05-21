using FsInfoCat.Model;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for list item entities which represent a logical file system volume and contains associated file system properties.
    /// </summary>
    /// <seealso cref="ILocalVolumeListItem" />
    /// <seealso cref="IVolumeListItemWithFileSystem" />
    /// <seealso cref="Upstream.Model.IUpstreamVolumeListItemWithFileSystem" />
    public interface ILocalVolumeListItemWithFileSystem : ILocalVolumeListItem, IVolumeListItemWithFileSystem { }
}
