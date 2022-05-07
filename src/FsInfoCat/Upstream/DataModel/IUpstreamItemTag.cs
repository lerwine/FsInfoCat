namespace FsInfoCat.Upstream
{
    public interface IUpstreamItemTagRow : IUpstreamDbEntity, IItemTagRow { }

    public interface IUpstreamItemTagListItem : IItemTagListItem, IUpstreamItemTagRow { }

    public interface IUpstreamItemTag : IUpstreamItemTagRow, IItemTag
    {
        new IUpstreamDbEntity Tagged { get; }

        new IUpstreamTagDefinition Definition { get; }
    }
}
