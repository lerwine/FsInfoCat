namespace FsInfoCat.Upstream
{
    public interface IUpstreamVolumeTag : IUpstreamItemTag, IVolumeTag, IHasMembershipKeyReference<IUpstreamVolume, IUpstreamTagDefinition>
    {
        new IUpstreamVolume Tagged { get; }
    }
}
