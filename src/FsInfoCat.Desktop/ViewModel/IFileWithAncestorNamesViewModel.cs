namespace FsInfoCat.Desktop.ViewModel
{
    public interface IFileWithAncestorNamesViewModel : IFileRowViewModel, ICrudEntityRowViewModel
    {
        new Model.IFileListItemWithAncestorNames Entity { get; }
    }
}
