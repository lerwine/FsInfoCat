namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for an <see cref="ITagDefinition"/> that is associated with an <see cref="IFile"/>.
    /// </summary>
    /// <seealso cref="IItemTag" />
    /// <seealso cref="IHasMembershipKeyReference{IFile, ITagDefinition}" />
    public interface IFileTag : IItemTag, IHasMembershipKeyReference<IFile, ITagDefinition>
    {
        /// <summary>
        /// Gets the tagged file.
        /// </summary>
        /// <value>The tagged <see cref="IFile"/>.</value>
        new IFile Tagged { get; }
    }
}