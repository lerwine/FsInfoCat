namespace FsInfoCat.Upstream
{
    public interface IUpstreamFileTag : IUpstreamItemTag, IFileTag, IHasMembershipKeyReference<IUpstreamFile, IUpstreamTagDefinition>
    {
        new IUpstreamFile Tagged { get; }
    }
}
