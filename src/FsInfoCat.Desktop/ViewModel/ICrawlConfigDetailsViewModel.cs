namespace FsInfoCat.Desktop.ViewModel
{
    public interface ICrawlConfigDetailsViewModel : ICrawlConfigurationRowViewModel
    {
        new Model.ICrawlConfiguration Entity { get; }
    }
}
