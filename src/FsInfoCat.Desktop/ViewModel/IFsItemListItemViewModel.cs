namespace FsInfoCat.Desktop.ViewModel
{
    public interface IFsItemListItemViewModel : IFsItemRowViewModel
    {
        new Model.IDbFsItemListItem Entity { get; }
    }
}
