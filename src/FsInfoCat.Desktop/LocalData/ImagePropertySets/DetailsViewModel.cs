using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
using System;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.ImagePropertySets
{
    public class DetailsViewModel : ImagePropertySetDetailsViewModel<ImagePropertySet, FileWithBinaryPropertiesAndAncestorNames, FileWithBinaryPropertiesAndAncestorNamesViewModel>
    {
        public DetailsViewModel(ImagePropertySet fs, ImagePropertiesListItem entity) : base(fs, entity)
        {
        }
    }
}
