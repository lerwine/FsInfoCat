using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
using System;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.MusicPropertySets
{
    public class DetailsViewModel : MusicPropertySetDetailsViewModel<MusicPropertySet, FileWithBinaryPropertiesAndAncestorNames, FileWithBinaryPropertiesAndAncestorNamesViewModel>
    {
        public DetailsViewModel(MusicPropertySet fs, MusicPropertiesListItem entity) : base(fs, entity)
        {
        }
    }
}
