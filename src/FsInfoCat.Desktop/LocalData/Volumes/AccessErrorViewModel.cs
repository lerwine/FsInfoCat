using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.Volumes
{
    public sealed class AccessErrorViewModel(VolumeAccessError entity) : AccessErrorRowViewModel<VolumeAccessError>(entity)
    {
    }
}
