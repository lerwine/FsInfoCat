namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for an <see cref="ITagDefinition"/> that is associated with an <see cref="IVolume"/>.
    /// </summary>
    /// <seealso cref="IItemTag" />
    /// <seealso cref="IHasMembershipKeyReference{IVolume, ITagDefinition}" />
    public interface IVolumeTag : IItemTag, IHasMembershipKeyReference<IVolume, ITagDefinition>
    {
        /// <summary>
        /// Gets the tagged volume.
        /// </summary>
        /// <value>The tagged <see cref="IVolume"/>.</value>
        new IVolume Tagged { get; }
    }
}
