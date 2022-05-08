namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for an entity that associates a <see cref="ILocalTagDefinition"/> with an <see cref="ILocalFile"/>, <see cref="ILocalSubdirectory"/> or <see cref="ILocalVolume"/>.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IItemTagRow" />
    public interface ILocalItemTagRow : ILocalDbEntity, IItemTagRow { }

    /// <summary>
    /// Generic interface for a list item entity that associates an <see cref="ILocalTagDefinition"/> with an <see cref="ILocalFile"/>, <see cref="ILocalSubdirectory"/> or <see cref="ILocalVolume"/>.
    /// </summary>
    /// <seealso cref="IItemTagListItem" />
    /// <seealso cref="ILocalItemTagRow" />
    public interface ILocalItemTagListItem : IItemTagListItem, ILocalItemTagRow { }

    /// <summary>
    /// Generic interface for an entity that associates an <see cref="ILocalTagDefinition"/> with an <see cref="ILocalFile"/>, <see cref="ILocalSubdirectory"/> or <see cref="ILocalVolume"/>.
    /// </summary>
    /// <seealso cref="ILocalItemTagRow" />
    /// <seealso cref="IItemTag" />
    public interface ILocalItemTag : ILocalItemTagRow, IItemTag
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
