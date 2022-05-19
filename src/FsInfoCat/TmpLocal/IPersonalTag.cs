using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for an entity that associates an <see cref="ILocalPersonalTagDefinition"/> with an <see cref="ILocalFile"/>, <see cref="ILocalSubdirectory"/>
    /// or <see cref="ILocalVolume"/>.
    /// </summary>
    /// <seealso cref="ILocalItemTag" />
    /// <seealso cref="M.IPersonalTag" />
    /// <seealso cref="Upstream.Model.IPersonalTag" />
    public interface ILocalPersonalTag : ILocalItemTag, M.IPersonalTag
    {
        /// <summary>
        /// Gets the personal tag definition.
        /// </summary>
        /// <value>The personal tag definition that is associated with the <see cref="ILocalDbEntity"/>.</value>
        new ILocalPersonalTagDefinition Definition { get; }
    }
}
