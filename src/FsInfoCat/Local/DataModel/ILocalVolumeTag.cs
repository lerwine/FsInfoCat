namespace FsInfoCat.Local
{
    public interface ILocalVolumeTag : ILocalItemTag, IVolumeTag, IHasMembershipKeyReference<ILocalVolume, ITagDefinition>
    {
        new ILocalVolume Tagged { get; }
    }
}
