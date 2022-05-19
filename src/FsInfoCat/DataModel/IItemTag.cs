using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for an entity that associates an <see cref="ITagDefinition"/> with an <see cref="IFile"/>, <see cref="ISubdirectory"/> or <see cref="IVolume"/>.
    /// </summary>
    /// <seealso cref="IItemTagRow" />
    /// <seealso cref="Local.ILocalItemTag" />
    /// <seealso cref="Upstream.IUpstreamItemTag" />
    /// <seealso cref="ITagDefinition.FileTags" />
    /// <seealso cref="ITagDefinition.SubdirectoryTags" />
    /// <seealso cref="ITagDefinition.VolumeTags" />
    [System.Obsolete("Use FsInfoCat.Model.IItemTag")]
    public interface IItemTag : IItemTagRow
    {
        /// <summary>
        /// Gets the tagged entity.
        /// </summary>
        /// <value>The entity that is associated with the <see cref="ITagDefinition"/>.</value>
        IDbEntity Tagged { get; }

        /// <summary>
        /// Gets the tag definition.
        /// </summary>
        /// <value>The tag definition that is associated with the <see cref="IDbEntity"/>.</value>
        ITagDefinition Definition { get; }

        /// <summary>
        /// Attempts to get the get primary key of the tagged entity.
        /// </summary>
        /// <param name="taggedId">The <see cref="IHasSimpleIdentifier.Id"/> of the <see cref="Tagged"/> entity.</param>
        /// <returns><see langword="true"/> if the <see cref="Tagged"/> entity has a primary key value assigned; otherwise, <see langword="false"/>.</returns>
        bool TryGetTaggedId(out Guid taggedId);

        /// <summary>
        /// Attempts to get the get primary key of the tag definition.
        /// </summary>
        /// <param name="definitionId">The <see cref="IHasSimpleIdentifier.Id"/> of the tag <see cref="Definition"/>.</param>
        /// <returns><see langword="true"/> if the tag <see cref="Definition"/> has a primary key value assigned; otherwise, <see langword="false"/>.</returns>
        bool TryGetDefinitionId(out Guid definitionId);
    }
}
