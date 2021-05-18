namespace FsInfoCat.Model.Upstream
{
    public interface IUpstreamFileComparison : IFileComparison, IUpstreamTimeStampedEntity
    {
        new IUpstreamFile File1 { get; }
        new IUpstreamFile File2 { get; }
    }
}
