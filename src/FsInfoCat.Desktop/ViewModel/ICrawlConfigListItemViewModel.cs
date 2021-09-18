namespace FsInfoCat.Desktop.ViewModel
{
    public interface ICrawlConfigListItemViewModel : ICrawlConfigurationRowViewModel
    {
        new ICrawlConfigurationListItem Entity { get; }
    }
}
