using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.Volumes
{
    public sealed class PersonalTagViewModel : ItemTagListItemViewModel<PersonalVolumeTagListItem>
    {
        public PersonalTagViewModel(PersonalVolumeTagListItem entity) : base(entity) { }
    }
}
