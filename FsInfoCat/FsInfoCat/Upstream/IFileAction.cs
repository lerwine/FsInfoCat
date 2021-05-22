namespace FsInfoCat.Upstream
{
    public interface IFileAction : IUpstreamDbEntity
    {
        IMitigationTask Task { get; set; }

        IUpstreamFile Source { get; set; }

        IUpstreamSubdirectory Target { get; set; }
    }
}
