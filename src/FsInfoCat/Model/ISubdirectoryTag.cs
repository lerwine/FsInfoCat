namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an <see cref="ITagDefinition"/> that is associated with an <see cref="ISubdirectory"/>.
    /// </summary>
    /// <seealso cref="IPersonalSubdirectoryTag" />
    /// <seealso cref="ISharedSubdirectoryTag" />
    /// <seealso cref="IFileTag" />
    /// <seealso cref="IPersonalTag" />
    /// <seealso cref="IVolumeTag" />
    /// <seealso cref="ISharedTag" />
    public interface ISubdirectoryTag : IItemTag, IHasMembershipKeyReference<ISubdirectory, ITagDefinition>
    {
        /// <summary>
        /// Gets the tagged subdirectory.
        /// </summary>
        /// <value>The tagged <see cref="ISubdirectory"/>.</value>
        new ISubdirectory Tagged { get; }
    }
}
