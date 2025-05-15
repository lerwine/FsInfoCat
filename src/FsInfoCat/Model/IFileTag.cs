namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an <see cref="ITagDefinition"/> that is associated with an <see cref="IFile"/>.
    /// </summary>
    /// <seealso cref="IPersonalFileTag" />
    /// <seealso cref="ISharedFileTag" />
    /// <seealso cref="IPersonalTag" />
    /// <seealso cref="ISubdirectoryTag" />
    /// <seealso cref="IVolumeTag" />
    /// <seealso cref="ISharedTag" />
    public interface IFileTag : IItemTag, IHasMembershipKeyReference<IFile, ITagDefinition>
    {
        /// <summary>
        /// Gets the tagged file.
        /// </summary>
        /// <value>The tagged <see cref="IFile"/>.</value>
        new IFile Tagged { get; }
    }
}
