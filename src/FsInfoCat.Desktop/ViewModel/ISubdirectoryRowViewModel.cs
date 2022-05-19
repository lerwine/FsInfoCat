namespace FsInfoCat.Desktop.ViewModel
{
    public interface ISubdirectoryRowViewModel : IFsItemRowViewModel
    {
        new Model.ISubdirectoryRow Entity { get; }
    }
}
