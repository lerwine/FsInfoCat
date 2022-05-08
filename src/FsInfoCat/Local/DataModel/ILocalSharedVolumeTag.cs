namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for an <see cref="ILocalSharedTagDefinition"/> that is associated with an <see cref="ILocalVolume"/>.
    /// </summary>
    /// <seealso cref="ILocalSharedTag" />
    /// <seealso cref="ISharedVolumeTag" />
    /// <seealso cref="ILocalVolumeTag" />
    /// <seealso cref="IHasMembershipKeyReference{ILocalVolume, ILocalSharedTagDefinition}" />
    public interface ILocalSharedVolumeTag : ILocalSharedTag, ISharedVolumeTag, ILocalVolumeTag, IHasMembershipKeyReference<ILocalVolume, ILocalSharedTagDefinition> { }
}
