using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.RecordedTVPropertySets
{
    public class DetailsViewModel(RecordedTVPropertySet fs, RecordedTVPropertiesListItem entity) : RecordedTVPropertySetDetailsViewModel<RecordedTVPropertySet, FileWithBinaryPropertiesAndAncestorNames, FileWithBinaryPropertiesAndAncestorNamesViewModel>(fs, entity)
    {
    }
}
