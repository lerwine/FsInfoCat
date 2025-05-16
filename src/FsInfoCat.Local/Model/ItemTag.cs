using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

/// <summary>
/// DB entity that associates a <see cref="PersonalTagDefinition"/> or <see cref="SharedTagDefinition"/> with a <see cref="DbFile"/>, <see cref="Subdirectory"/> or <see cref="Volume"/>.
/// </summary>
/// <seealso cref="ItemTagListItem" />
/// <seealso cref="PersonalFileTag" />
/// <seealso cref="PersonalSubdirectoryTag" />
/// <seealso cref="PersonalVolumeTag" />
/// <seealso cref="SharedFileTag" />
/// <seealso cref="SharedSubdirectoryTag" />
/// <seealso cref="SharedVolumeTag" />
/// <seealso cref="LocalDbContext.PersonalFileTags" />
/// <seealso cref="LocalDbContext.PersonalSubdirectoryTags" />
/// <seealso cref="LocalDbContext.PersonalVolumeTags" />
/// <seealso cref="LocalDbContext.SharedFileTags" />
/// <seealso cref="LocalDbContext.SharedSubdirectoryTags" />
/// <seealso cref="LocalDbContext.SharedVolumeTags" />
public abstract partial class ItemTag : ItemTagRow, ILocalItemTag
{
    /// <summary>
    /// Gets the tagged entity.
    /// </summary>
    /// <returns>The <see cref="ILocalDbEntity"/> that is associated with the current <see cref="ItemTag"/>.</returns>
    protected abstract ILocalDbEntity GetTagged();

    /// <summary>
    /// Gets the tag definition.
    /// </summary>
    /// <returns>The <see cref="ILocalTagDefinition"/> that is associated with the current <see cref="ItemTag"/>.</returns>
    protected abstract ILocalTagDefinition GetDefinition();

    /// <summary>
    /// Attempts to get the get primary key of the tag definition.
    /// </summary>
    /// <param name="definitionId">The <see cref="IHasSimpleIdentifier.Id"/> of the tag <see cref="ILocalTagDefinition"/>.</param>
    /// <returns><see langword="true"/> if the tag <see cref="ILocalTagDefinition"/> has a primary key value assigned; otherwise, <see langword="false"/>.</returns>
    public abstract bool TryGetDefinitionId(out Guid definitionId);

    /// <summary>
    /// Attempts to get the get primary key of the tagged entity.
    /// </summary>
    /// <param name="taggedId">The <see cref="IHasSimpleIdentifier.Id"/> of the <see cref="ILocalDbEntity"/> entity.</param>
    /// <returns><see langword="true"/> if the <see cref="ILocalDbEntity"/> entity has a primary key value assigned; otherwise, <see langword="false"/>.</returns>
    public abstract bool TryGetTaggedId(out Guid taggedId);

    IDbEntity IItemTag.Tagged => GetTagged();

    ITagDefinition IItemTag.Definition => GetDefinition();

    ILocalDbEntity ILocalItemTag.Tagged => GetTagged();

    ILocalTagDefinition ILocalItemTag.Definition => GetDefinition();
}
