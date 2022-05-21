using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for list item entities which represent a logical file system volume.
    /// </summary>
    /// <seealso cref="ILocalVolumeRow" />
    /// <seealso cref="IVolumeListItem" />
    /// <seealso cref="Upstream.Model.IVolumeListItem" />
    public interface ILocalVolumeListItem : ILocalVolumeRow, IVolumeListItem { }
}
