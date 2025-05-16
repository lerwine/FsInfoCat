using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.MediaPropertySets
{
    public class DetailsViewModel(MediaPropertySet fs, MediaPropertiesListItem entity) : MediaPropertySetDetailsViewModel<MediaPropertySet, FileWithBinaryPropertiesAndAncestorNames, FileWithBinaryPropertiesAndAncestorNamesViewModel>(fs, entity)
    {
    }
}
