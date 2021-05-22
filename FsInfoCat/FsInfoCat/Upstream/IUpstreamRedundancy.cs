namespace FsInfoCat.Upstream
{
    public interface IUpstreamRedundancy : IRedundancy, IUpstreamDbEntity
    {
        new IUpstreamRedundantSet RedundantSet { get; set; }
    }
}
