using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.Volumes
{
    public sealed class FileSystemListItemViewModel : FileSystemListItemViewModel<FileSystemListItem>
    {
        public FileSystemListItemViewModel(FileSystemListItem entity) : base(entity) { }
    }
}
