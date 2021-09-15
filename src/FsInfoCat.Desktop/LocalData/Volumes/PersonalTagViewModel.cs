using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.Volumes
{
    public sealed class PersonalTagViewModel : ItemTagListItemViewModel<PersonalVolumeTagListItem>
    {
        public PersonalTagViewModel(PersonalVolumeTagListItem entity) : base(entity) { }
    }
}
