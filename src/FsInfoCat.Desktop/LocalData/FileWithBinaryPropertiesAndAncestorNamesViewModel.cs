using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.LocalData
{
    public class FileWithBinaryPropertiesAndAncestorNamesViewModel([DisallowNull] FileWithBinaryPropertiesAndAncestorNames entity) : FileWithBinaryPropertiesAndAncestorNamesViewModel<FileWithBinaryPropertiesAndAncestorNames>(entity)
    {
    }
}
