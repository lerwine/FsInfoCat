namespace FsInfoCat.Desktop.ViewModel
{
    public interface IFileRowViewModel : IFsItemRowViewModel
    {
        new Model.IFileRow Entity { get; }
    }
}
