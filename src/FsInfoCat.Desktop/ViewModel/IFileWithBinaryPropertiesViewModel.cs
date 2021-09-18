namespace FsInfoCat.Desktop.ViewModel
{
    public interface IFileWithBinaryPropertiesViewModel : IFileRowViewModel, ICrudEntityRowViewModel
    {
        new IFileListItemWithBinaryProperties Entity { get; }
    }
}
