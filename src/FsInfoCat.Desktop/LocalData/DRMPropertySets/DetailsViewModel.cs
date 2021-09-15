using FsInfoCat.Local;
using System;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.DRMPropertySets
{
    public class DetailsViewModel : ViewModel.DRMPropertySetDetailsViewModel<DRMPropertySet, FileWithBinaryPropertiesAndAncestorNames, FileWithBinaryPropertiesAndAncestorNamesViewModel>
    {
        public DetailsViewModel(DRMPropertySet fs, DRMPropertiesListItem entity) : base(fs, entity)
        {
        }
    }
}
