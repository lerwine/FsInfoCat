using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for an entity that associates an <see cref="IUpstreamTagDefinition"/> with an <see cref="IUpstreamFile"/>, <see cref="IUpstreamSubdirectory"/>
    /// or <see cref="IUpstreamVolume"/>.
    /// </summary>
    /// <seealso cref="IUpstreamItemTagRow" />
    /// <seealso cref="M.IItemTag" />
    /// <seealso cref="Local.Model.IItemTag" />
    public interface IUpstreamItemTag : IUpstreamItemTagRow, M.IItemTag
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
