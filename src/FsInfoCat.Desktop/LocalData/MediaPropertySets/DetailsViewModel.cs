using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.MediaPropertySets
{
    public class DetailsViewModel : MediaPropertySetDetailsViewModel<MediaPropertySet, FileWithBinaryPropertiesAndAncestorNames, FileWithBinaryPropertiesAndAncestorNamesViewModel>
    {
        public DetailsViewModel(MediaPropertySet fs, MediaPropertiesListItem entity) : base(fs, entity)
        {
        }
    }
}
