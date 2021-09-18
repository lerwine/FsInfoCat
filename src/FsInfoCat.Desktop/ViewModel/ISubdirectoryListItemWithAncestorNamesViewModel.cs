namespace FsInfoCat.Desktop.ViewModel
{
    public interface ISubdirectoryListItemWithAncestorNamesViewModel : ISubdirectoryListItemViewModel
    {
        new ISubdirectoryListItemWithAncestorNames Entity { get; }
    }
}
