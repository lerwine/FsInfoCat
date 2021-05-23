namespace FsInfoCat.Upstream
{
    public interface IUpstreamRedundancy : IRedundancy, IUpstreamDbEntity
    {
        new IUpstreamFile File { get; set; }

        new IUpstreamRedundantSet RedundantSet { get; set; }
    }
}
