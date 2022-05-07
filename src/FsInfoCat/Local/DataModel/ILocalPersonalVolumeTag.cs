namespace FsInfoCat.Local
{
    public interface ILocalPersonalVolumeTag : ILocalPersonalTag, IPersonalVolumeTag, ILocalVolumeTag, IHasMembershipKeyReference<ILocalVolume, ILocalPersonalTagDefinition> { }
}
