namespace FsInfoCat.Desktop.ViewModel
{
    public interface IFsItemListItemViewModel : IFsItemRowViewModel
    {
        new IDbFsItemListItem Entity { get; }
    }
}
