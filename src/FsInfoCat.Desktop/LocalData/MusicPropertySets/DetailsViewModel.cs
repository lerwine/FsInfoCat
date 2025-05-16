using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.MusicPropertySets
{
    public class DetailsViewModel(MusicPropertySet fs, MusicPropertiesListItem entity) : MusicPropertySetDetailsViewModel<MusicPropertySet, FileWithBinaryPropertiesAndAncestorNames, FileWithBinaryPropertiesAndAncestorNamesViewModel>(fs, entity)
    {
    }
}
