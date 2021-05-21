namespace FsInfoCat.Upstream
{
    public interface ISubdirectoryAction : IUpstreamDbEntity
    {
        IMitigationTask Task { get; set; }
    }
}
