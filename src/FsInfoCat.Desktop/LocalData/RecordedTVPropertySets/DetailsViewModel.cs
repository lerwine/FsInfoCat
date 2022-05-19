using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
using System;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.RecordedTVPropertySets
{
    public class DetailsViewModel : RecordedTVPropertySetDetailsViewModel<RecordedTVPropertySet, FileWithBinaryPropertiesAndAncestorNames, FileWithBinaryPropertiesAndAncestorNamesViewModel>
    {
        public DetailsViewModel(RecordedTVPropertySet fs, RecordedTVPropertiesListItem entity) : base(fs, entity)
        {
        }
    }
}
