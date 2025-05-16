using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.ImagePropertySets
{
    public class DetailsViewModel(ImagePropertySet fs, ImagePropertiesListItem entity) : ImagePropertySetDetailsViewModel<ImagePropertySet, FileWithBinaryPropertiesAndAncestorNames, FileWithBinaryPropertiesAndAncestorNamesViewModel>(fs, entity)
    {
    }
}
