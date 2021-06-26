namespace FsInfoCat.Upstream
{
    public interface IUpstreamComparison : IComparison, IUpstreamDbEntity
    {
        new IUpstreamFile Baseline { get; set; }

        new IUpstreamFile Correlative { get; set; }
    }
}
