namespace FsInfoCat.Desktop.ViewModel
{
    public interface ICrawlJobListItemViewModel : ICrawlJobRowViewModel
    {
        new Model.ICrawlJobListItem Entity { get; }
    }
}
