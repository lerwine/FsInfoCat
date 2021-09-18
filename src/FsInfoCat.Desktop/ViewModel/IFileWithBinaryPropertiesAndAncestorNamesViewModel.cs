namespace FsInfoCat.Desktop.ViewModel
{
    public interface IFileWithBinaryPropertiesAndAncestorNamesViewModel : IFileWithAncestorNamesViewModel
    {
        new IFileListItemWithBinaryPropertiesAndAncestorNames Entity { get; }
    }
}
