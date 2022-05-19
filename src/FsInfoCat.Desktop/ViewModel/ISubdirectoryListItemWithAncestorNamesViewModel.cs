namespace FsInfoCat.Desktop.ViewModel
{
    public interface ISubdirectoryListItemWithAncestorNamesViewModel : ISubdirectoryListItemViewModel
    {
        new Model.ISubdirectoryListItemWithAncestorNames Entity { get; }
    }
}
