using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
using System;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.DRMPropertySets
{
    public class DetailsViewModel : DRMPropertySetDetailsViewModel<DRMPropertySet, FileWithBinaryPropertiesAndAncestorNames, FileWithBinaryPropertiesAndAncestorNamesViewModel>
    {
        public DetailsViewModel(DRMPropertySet fs, DRMPropertiesListItem entity) : base(fs, entity)
        {
        }
    }
}
