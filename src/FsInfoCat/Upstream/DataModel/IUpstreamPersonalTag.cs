namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for an entity that associates an <see cref="IUpstreamPersonalTagDefinition"/> with an <see cref="IUpstreamFile"/>, <see cref="IUpstreamSubdirectory"/>
    /// or <see cref="IUpstreamVolume"/>.
    /// </summary>
    /// <seealso cref="IUpstreamItemTag" />
    /// <seealso cref="IPersonalTag" />
    /// <seealso cref="Local.ILocalPersonalTag" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamPersonalTag")]
    public interface IUpstreamPersonalTag : IUpstreamItemTag, IPersonalTag
    {
        /// <summary>
        /// Gets the personal tag definition.
        /// </summary>
        /// <value>The personal tag definition that is associated with the <see cref="IUpstreamDbEntity"/>.</value>
        new IUpstreamPersonalTagDefinition Definition { get; }
    }
}
