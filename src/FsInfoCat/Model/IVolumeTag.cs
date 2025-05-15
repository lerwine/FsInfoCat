namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an <see cref="ITagDefinition"/> that is associated with an <see cref="IVolume"/>.
    /// </summary>
    /// <seealso cref="IPersonalVolumeTag" />
    /// <seealso cref="ISharedVolumeTag" />
    /// <seealso cref="IFileTag" />
    /// <seealso cref="IPersonalTag" />
    /// <seealso cref="ISubdirectoryTag" />
    /// <seealso cref="ISharedTag" />
    public interface IVolumeTag : IItemTag, IHasMembershipKeyReference<IVolume, ITagDefinition>
    {
        /// <summary>
        /// Gets the tagged volume.
        /// </summary>
        /// <value>The tagged <see cref="IVolume"/>.</value>
        new IVolume Tagged { get; }
    }
}
