using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for an entity that associates an <see cref="IUpstreamSharedTagDefinition"/> with an <see cref="IUpstreamFile"/>, <see cref="IUpstreamSubdirectory"/>
    /// or <see cref="IUpstreamVolume"/>.
    /// </summary>
    /// <seealso cref="IUpstreamItemTag" />
    /// <seealso cref="ISharedTag" />
    /// <seealso cref="Local.Model.ISharedTag" />
    public interface IUpstreamSharedTag : IUpstreamItemTag, ISharedTag
    {
        /// <summary>
        /// Gets the tag definition.
        /// </summary>
        /// <value>The tag definition that is associated with the <see cref="IUpstreamDbEntity"/>.</value>
        new IUpstreamSharedTagDefinition Definition { get; }
    }
}
