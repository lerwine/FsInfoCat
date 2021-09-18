namespace FsInfoCat.Desktop.ViewModel
{
    public interface IFileRowViewModel : IFsItemRowViewModel
    {
        new IFileRow Entity { get; }
    }
}
