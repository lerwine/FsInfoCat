namespace FsInfoCat
{
    public interface IVolumeTag : IItemTag, IHasMembershipKeyReference<IVolume, ITagDefinition>
    {
        new IVolume Tagged { get; }
    }
}
