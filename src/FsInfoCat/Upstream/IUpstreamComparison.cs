namespace FsInfoCat.Upstream
{
    public interface IUpstreamComparison : IComparison, IUpstreamDbEntity
    {
        new IUpstreamFile SourceFile { get; set; }

        new IUpstreamFile TargetFile { get; set; }
    }
}
