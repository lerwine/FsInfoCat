using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for entities that has a single <see cref="Guid"/> value as the primary key.
    /// </summary>
    /// <seealso cref="IAccessError" />
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IBinaryPropertySet" />
    /// <seealso cref="ICrawlConfigurationRow" />
    /// <seealso cref="ICrawlJobLogRow" />
    /// <seealso cref="IDbFsItemRow" />
    /// <seealso cref="IDbFsItemAncestorName" />
    /// <seealso cref="IFileSystemRow" />
    /// <seealso cref="ITagDefinitionRow" />
    /// <seealso cref="IRedundantSetRow" />
    /// <seealso cref="ISymbolicNameRow" />
    /// <seealso cref="IVolumeRow" />
    public interface IHasSimpleIdentifier
    {
        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Display(Name = nameof(Properties.Resources.UniqueIdentifier), ResourceType = typeof(Properties.Resources))]
        Guid Id { get; }

        /// <summary>
        /// Attempts to get the primary key value.
        /// </summary>
        /// <param name="id">The primary key value.</param>
        /// <returns><see langword="true"/> if the primary key value ahas been set; otherwise, <see langword="false"/>.</returns>
        bool TryGetId(out Guid id);
    }
}
