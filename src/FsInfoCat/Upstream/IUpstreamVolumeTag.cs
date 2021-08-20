namespace FsInfoCat.Upstream
{
    public interface IUpstreamVolumeTag : IUpstreamItemTag, IVolumeTag
    {
        new IUpstreamVolume Tagged { get; }
    }
}
