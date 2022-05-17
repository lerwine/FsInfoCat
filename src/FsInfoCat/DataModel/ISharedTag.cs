namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for an entity that associates an <see cref="ISharedTagDefinition"/> with an <see cref="IFile"/>, <see cref="ISubdirectory"/> or <see cref="IVolume"/>.
    /// </summary>
    /// <seealso cref="IItemTag" />
    /// <seealso cref="Local.ILocalSharedTag" />
    /// <seealso cref="Upstream.IUpstreamSharedTag" />
    public interface ISharedTag : IItemTag
    {
        /// <summary>
        /// Gets the tag definition.
        /// </summary>
        /// <value>The tag definition that is associated with the <see cref="IDbEntity"/>.</value>
        new ISharedTagDefinition Definition { get; }
    }
}
