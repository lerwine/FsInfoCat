namespace FsInfoCat.Desktop.ViewModel
{
    public interface IFileWithBinaryPropertiesViewModel : IFileRowViewModel, ICrudEntityRowViewModel
    {
        new Model.IFileListItemWithBinaryProperties Entity { get; }
    }
}
