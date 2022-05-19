using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for an entity that associates an <see cref="ILocalSharedTagDefinition"/> with an <see cref="ILocalFile"/>, <see cref="ILocalSubdirectory"/>
    /// or <see cref="ILocalVolume"/>.
    /// </summary>
    /// <seealso cref="ILocalItemTag" />
    /// <seealso cref="M.ISharedTag" />
    /// <seealso cref="Upstream.Model.ISharedTag" />
    public interface ILocalSharedTag : ILocalItemTag, M.ISharedTag
    {
        /// <summary>
        /// Gets the tag definition.
        /// </summary>
        /// <value>The tag definition that is associated with the <see cref="ILocalDbEntity"/>.</value>
        new ILocalSharedTagDefinition Definition { get; }
    }
}
