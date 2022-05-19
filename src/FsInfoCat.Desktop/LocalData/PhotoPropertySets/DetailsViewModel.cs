using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
using System;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.PhotoPropertySets
{
    public class DetailsViewModel : PhotoPropertySetDetailsViewModel<PhotoPropertySet, FileWithBinaryPropertiesAndAncestorNames, FileWithBinaryPropertiesAndAncestorNamesViewModel>
    {
        public DetailsViewModel(PhotoPropertySet fs, PhotoPropertiesListItem entity) : base(fs, entity)
        {
        }
    }
}
