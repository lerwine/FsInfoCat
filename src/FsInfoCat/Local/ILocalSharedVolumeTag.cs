namespace FsInfoCat.Local
{
    public interface ILocalSharedVolumeTag : ILocalSharedTag, ISharedVolumeTag, ILocalVolumeTag, IHasMembershipKeyReference<ILocalVolume, ILocalSharedTagDefinition> { }
}
