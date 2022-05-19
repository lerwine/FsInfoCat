using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
using System;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.GPSPropertySets
{
    public class DetailsViewModel : GPSPropertySetDetailsViewModel<GPSPropertySet, FileWithBinaryPropertiesAndAncestorNames, FileWithBinaryPropertiesAndAncestorNamesViewModel>
    {
        public DetailsViewModel(GPSPropertySet fs, GPSPropertiesListItem entity) : base(fs, entity)
        {
        }
    }
}
