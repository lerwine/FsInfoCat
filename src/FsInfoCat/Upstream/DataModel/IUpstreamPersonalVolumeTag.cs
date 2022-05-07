namespace FsInfoCat.Upstream
{
    public interface IUpstreamPersonalVolumeTag : IUpstreamPersonalTag, IPersonalVolumeTag, IUpstreamVolumeTag, IHasMembershipKeyReference<IUpstreamVolume, IUpstreamPersonalTagDefinition> { }
}
