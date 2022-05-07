namespace FsInfoCat.Upstream
{
    public interface IUpstreamPersonalTag : IUpstreamItemTag, IPersonalTag
    {
        new IUpstreamPersonalTagDefinition Definition { get; }
    }
}
