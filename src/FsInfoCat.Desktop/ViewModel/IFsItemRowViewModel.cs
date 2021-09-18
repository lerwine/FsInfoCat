namespace FsInfoCat.Desktop.ViewModel
{
    public interface IFsItemRowViewModel : IDbEntityRowViewModel
    {
        new IDbFsItemRow Entity { get; }
    }
}
