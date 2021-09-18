namespace FsInfoCat.Desktop.ViewModel
{
    public interface ISubdirectoryRowViewModel : IFsItemRowViewModel
    {
        new ISubdirectoryRow Entity { get; }
    }
}
