namespace FsInfoCat.Upstream
{
    public interface IUpstreamSharedTag : IUpstreamItemTag, ISharedTag
    {
        new IUpstreamSharedTagDefinition Definition { get; }
    }
}
