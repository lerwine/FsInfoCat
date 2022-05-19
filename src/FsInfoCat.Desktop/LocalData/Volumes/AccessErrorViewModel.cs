using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.Volumes
{
    public sealed class AccessErrorViewModel : AccessErrorRowViewModel<VolumeAccessError>
    {
        public AccessErrorViewModel(VolumeAccessError entity) : base(entity) { }
    }
}
