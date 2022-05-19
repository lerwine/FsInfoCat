namespace FsInfoCat.Desktop.ViewModel
{
    public interface IFileWithBinaryPropertiesAndAncestorNamesViewModel : IFileWithAncestorNamesViewModel
    {
        new Model.IFileListItemWithBinaryPropertiesAndAncestorNames Entity { get; }
    }
}
