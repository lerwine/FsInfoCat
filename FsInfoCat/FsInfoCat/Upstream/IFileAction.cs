namespace FsInfoCat.Upstream
{
    public interface IFileAction : IUpstreamDbEntity
    {
        IMitigationTask Task { get; set; }
    }
}
