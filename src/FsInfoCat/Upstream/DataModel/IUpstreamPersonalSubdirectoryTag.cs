namespace FsInfoCat.Upstream
{
    public interface IUpstreamPersonalSubdirectoryTag : IUpstreamPersonalTag, IPersonalSubdirectoryTag, IUpstreamSubdirectoryTag, IHasMembershipKeyReference<IUpstreamSubdirectory, IUpstreamPersonalTagDefinition> { }
}
