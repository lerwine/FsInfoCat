namespace FsInfoCat.Desktop.ViewModel
{
    public interface ISubdirectoryListItemViewModel : IFsItemListItemViewModel, ISubdirectoryRowViewModel
    {
        new Model.ISubdirectoryListItem Entity { get; }
    }
}
