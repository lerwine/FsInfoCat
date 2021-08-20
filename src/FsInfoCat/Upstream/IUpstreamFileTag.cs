namespace FsInfoCat.Upstream
{
    public interface IUpstreamFileTag : IUpstreamItemTag, IFileTag
    {
        new IUpstreamFile Tagged { get; }
    }
}
