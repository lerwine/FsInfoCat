namespace FsInfoCat
{
    public interface IVolumeTag : IItemTag
    {
        new IVolume Tagged { get; }
    }
}
