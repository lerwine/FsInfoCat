namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for an entity that associates an <see cref="ILocalSharedTagDefinition"/> with an <see cref="ILocalFile"/>, <see cref="ILocalSubdirectory"/>
    /// or <see cref="ILocalVolume"/>.
    /// </summary>
    /// <seealso cref="ILocalItemTag" />
    /// <seealso cref="ISharedTag" />
    /// <seealso cref="Upstream.IUpstreamSharedTag" />
    public interface ILocalSharedTag : ILocalItemTag, ISharedTag
    {
        /// <summary>
        /// Gets the tag definition.
        /// </summary>
        /// <value>The tag definition that is associated with the <see cref="ILocalDbEntity"/>.</value>
        new ILocalSharedTagDefinition Definition { get; }
    }
}
