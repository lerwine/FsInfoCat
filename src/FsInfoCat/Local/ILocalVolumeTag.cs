namespace FsInfoCat.Local
{
    public interface ILocalVolumeTag : ILocalItemTag, IVolumeTag
    {
        new ILocalVolume Tagged { get; }
    }
}
