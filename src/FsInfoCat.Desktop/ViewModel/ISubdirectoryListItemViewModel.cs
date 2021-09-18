namespace FsInfoCat.Desktop.ViewModel
{
    public interface ISubdirectoryListItemViewModel : IFsItemListItemViewModel, ISubdirectoryRowViewModel
    {
        new ISubdirectoryListItem Entity { get; }
    }
}
