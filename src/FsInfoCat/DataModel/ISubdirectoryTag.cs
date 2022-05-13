namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for an <see cref="ITagDefinition"/> that is associated with an <see cref="ISubdirectory"/>.
    /// </summary>
    /// <seealso cref="IItemTag" />
    /// <seealso cref="IHasMembershipKeyReference{ISubdirectory, ITagDefinition}" />
    public interface ISubdirectoryTag : IItemTag, IHasMembershipKeyReference<ISubdirectory, ITagDefinition>
    {
        /// <summary>
        /// Gets the tagged subdirectory.
        /// </summary>
        /// <value>The tagged <see cref="ISubdirectory"/>.</value>
        new ISubdirectory Tagged { get; }
    }
}
