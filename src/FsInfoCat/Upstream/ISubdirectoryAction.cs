namespace FsInfoCat.Upstream
{
    public interface ISubdirectoryAction : IUpstreamDbEntity
    {
        IMitigationTask Task { get; set; }

        IUpstreamSubdirectory Source { get; set; }

        IUpstreamSubdirectory Target { get; set; }
    }
}
