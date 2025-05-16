using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.GPSPropertySets
{
    public class DetailsViewModel(GPSPropertySet fs, GPSPropertiesListItem entity) : GPSPropertySetDetailsViewModel<GPSPropertySet, FileWithBinaryPropertiesAndAncestorNames, FileWithBinaryPropertiesAndAncestorNamesViewModel>(fs, entity)
    {
    }
}
