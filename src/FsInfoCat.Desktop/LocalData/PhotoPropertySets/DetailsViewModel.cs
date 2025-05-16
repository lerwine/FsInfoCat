using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.PhotoPropertySets
{
    public class DetailsViewModel(PhotoPropertySet fs, PhotoPropertiesListItem entity) : PhotoPropertySetDetailsViewModel<PhotoPropertySet, FileWithBinaryPropertiesAndAncestorNames, FileWithBinaryPropertiesAndAncestorNamesViewModel>(fs, entity)
    {
    }
}
