namespace FsInfoCat.Upstream
{
    public interface IUpstreamSubdirectoryTag : IUpstreamItemTag, ISubdirectoryTag
    {
        new IUpstreamSubdirectory Tagged { get; }
    }
}
