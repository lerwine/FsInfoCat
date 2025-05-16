using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.DRMPropertySets
{
    public class DetailsViewModel(DRMPropertySet fs, DRMPropertiesListItem entity) : DRMPropertySetDetailsViewModel<DRMPropertySet, FileWithBinaryPropertiesAndAncestorNames, FileWithBinaryPropertiesAndAncestorNamesViewModel>(fs, entity)
    {
    }
}
