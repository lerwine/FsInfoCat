using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model;

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
    /// Gets the primary key value of the related entity.
    /// </summary>
    /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
    [Display(Name = nameof(Properties.Resources.UniqueIdentifier), ResourceType = typeof(Properties.Resources))]
    Guid Id { get; }

    /// <summary>
    /// Attempts to get the primary key value of the related entity.
    /// </summary>
    /// <param name="id">Returns the primary key of the related entity.</param>
    /// <returns><see langword="true"/> if the primary key value has been set; otherwise, <see langword="false"/>.</returns>
    bool TryGetId(out Guid id);
}
