using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for an entity that associates an <see cref="ILocalTagDefinition"/> with an <see cref="ILocalFile"/>, <see cref="ILocalSubdirectory"/>
    /// or <see cref="ILocalVolume"/>.
    /// </summary>
    /// <seealso cref="ILocalItemTagRow" />
    /// <seealso cref="M.IItemTag" />
    /// <seealso cref="Upstream.Model.IItemTag" />
    public interface ILocalItemTag : ILocalItemTagRow, M.IItemTag
    {
        /// <summary>
        /// Gets the tagged entity.
        /// </summary>
        /// <value>The entity that is associated with the <see cref="ILocalTagDefinition"/>.</value>
        new ILocalDbEntity Tagged { get; }

        /// <summary>
        /// Gets the tag definition.
        /// </summary>
        /// <value>The tag definition that is associated with the <see cref="ILocalDbEntity"/>.</value>
        new ILocalTagDefinition Definition { get; }
    }
}
