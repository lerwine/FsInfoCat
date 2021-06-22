namespace FsInfoCat.Model.Upstream
{
    public interface IUpstreamFileComparison : IFileComparison, IUpstreamTimeStampedEntity
    {
        new IUpstreamFile SourceFile { get; }

        new IUpstreamFile TargetFile { get; }
    }
}
