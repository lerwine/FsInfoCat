namespace FsInfoCat.Upstream
{
    public interface IHostCrawlConfiguration : ICrawlConfiguration, IUpstreamDbEntity
    {
        new IUpstreamSubdirectory Root { get; }
    }
}
