namespace FsInfoCat.Upstream
{
    public interface IUpstreamItemTag : IUpstreamItemTagRow, IItemTag
    {
        new IUpstreamDbEntity Tagged { get; }

        new IUpstreamTagDefinition Definition { get; }
    }
}
