using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for an entity that associates an <see cref="IUpstreamPersonalTagDefinition"/> with an <see cref="IUpstreamFile"/>, <see cref="IUpstreamSubdirectory"/>
    /// or <see cref="IUpstreamVolume"/>.
    /// </summary>
    /// <seealso cref="IUpstreamItemTag" />
    /// <seealso cref="M.IPersonalTag" />
    /// <seealso cref="Local.Model.IPersonalTag" />
    public interface IUpstreamPersonalTag : IUpstreamItemTag, M.IPersonalTag
    {
        /// <summary>
        /// Gets the personal tag definition.
        /// </summary>
        /// <value>The personal tag definition that is associated with the <see cref="IUpstreamDbEntity"/>.</value>
        new IUpstreamPersonalTagDefinition Definition { get; }
    }
}
