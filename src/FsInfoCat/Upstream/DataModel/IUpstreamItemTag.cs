namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for an entity that associates an <see cref="IUpstreamTagDefinition"/> with an <see cref="IUpstreamFile"/>, <see cref="IUpstreamSubdirectory"/>
    /// or <see cref="IUpstreamVolume"/>.
    /// </summary>
    /// <seealso cref="IUpstreamItemTagRow" />
    /// <seealso cref="IItemTag" />
    /// <seealso cref="Local.ILocalItemTag" />
    public interface IUpstreamItemTag : IUpstreamItemTagRow, IItemTag
    {
        /// <summary>
        /// Gets the tagged entity.
        /// </summary>
        /// <value>The entity that is associated with the <see cref="IUpstreamTagDefinition"/>.</value>
        new IUpstreamDbEntity Tagged { get; }

        /// <summary>
        /// Gets the tag definition.
        /// </summary>
        /// <value>The tag definition that is associated with the <see cref="IUpstreamDbEntity"/>.</value>
        new IUpstreamTagDefinition Definition { get; }
    }
}
