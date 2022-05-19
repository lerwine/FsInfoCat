namespace FsInfoCat.Desktop.ViewModel
{
    public interface IFsItemRowViewModel : IDbEntityRowViewModel
    {
        new Model.IDbFsItemRow Entity { get; }
    }
}
