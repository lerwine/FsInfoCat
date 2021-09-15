using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.Volumes
{
    public sealed class SharedTagViewModel : ItemTagListItemViewModel<SharedVolumeTagListItem>
    {
        public SharedTagViewModel(SharedVolumeTagListItem entity) : base(entity) { }
    }
}
