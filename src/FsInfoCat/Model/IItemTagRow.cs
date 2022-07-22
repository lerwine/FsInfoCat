using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an entity that associates a <see cref="ITagDefinition"/> with an <see cref="IFile"/>, <see cref="ISubdirectory"/> or <see cref="IVolume"/>.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="IHasIdentifierPair" />
    /// <seealso cref="Local.Model.ILocalItemTagRow" />
    /// <seealso cref="Upstream.Model.IUpstreamItemTagRow" />
    public interface IItemTagRow : IDbEntity, IHasIdentifierPair
    {
        /// <summary>
        /// Gets the primary key value that references the tagged entity.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as part of the current entity's primary key the database.</value>
        Guid TaggedId { get; }

        /// <summary>
        /// Gets the primary key value that references the <see cref="ITagDefinition"/> entity.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as part of the current entity's primary key the database.</value>
        Guid DefinitionId { get; }

        /// <summary>
        /// Gets custom notes to be associated with the current file system item.
        /// </summary>
        /// <value>The custom notes to associate with the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }
    }
}
