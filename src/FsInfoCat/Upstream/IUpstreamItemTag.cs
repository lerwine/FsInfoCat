namespace FsInfoCat.Upstream
{
    public interface IUpstreamItemTag : IUpstreamDbEntity, IItemTag
    {
        new IUpstreamDbEntity Tagged { get; }

        new IUpstreamTagDefinition Definition { get; }
    }
}
