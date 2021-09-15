using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.Volumes
{
    public sealed class AccessErrorViewModel : AccessErrorRowViewModel<VolumeAccessError>
    {
        public AccessErrorViewModel(VolumeAccessError entity) : base(entity) { }
    }
}
