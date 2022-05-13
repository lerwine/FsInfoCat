namespace FsInfoCat.Upstream
{
    public interface IUpstreamSubdirectoryTag : IUpstreamItemTag, ISubdirectoryTag, IHasMembershipKeyReference<IUpstreamSubdirectory, IUpstreamTagDefinition>
    {
        new IUpstreamSubdirectory Tagged { get; }
    }
}
