namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an entity that associates an <see cref="ISharedTagDefinition"/> with an <see cref="IFile"/>, <see cref="ISubdirectory"/> or <see cref="IVolume"/>.
    /// </summary>
    /// <seealso cref="ISharedFileTag" />
    /// <seealso cref="ISharedSubdirectoryTag" />
    /// <seealso cref="ISharedVolumeTag" />
    /// <seealso cref="IFileTag" />
    /// <seealso cref="IPersonalTag" />
    /// <seealso cref="ISubdirectoryTag" />
    /// <seealso cref="IVolumeTag" />
    public interface ISharedTag : IItemTag
    {
        /// <summary>
        /// Gets the tag definition.
        /// </summary>
        /// <value>The tag definition that is associated with the <see cref="IDbEntity"/>.</value>
        new ISharedTagDefinition Definition { get; }
    }
}
