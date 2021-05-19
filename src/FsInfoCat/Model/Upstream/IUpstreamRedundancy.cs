namespace FsInfoCat.Model.Upstream
{
    public interface IUpstreamRedundancy : IRedundancy, IUpstreamTimeStampedEntity
    {
        new IUpstreamFile TargetFile { get; }

        new IUpstreamRedundantSet RedundantSet { get; }
    }
}
