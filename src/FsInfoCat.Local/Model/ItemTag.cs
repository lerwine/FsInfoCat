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
    protected abstract ILocalDbEntity GetTagged();

    protected abstract ILocalTagDefinition GetDefinition();

    public abstract bool TryGetDefinitionId(out Guid definitionId);

    public abstract bool TryGetTaggedId(out Guid taggedId);

    IDbEntity IItemTag.Tagged => GetTagged();

    ITagDefinition IItemTag.Definition => GetDefinition();

    ILocalDbEntity ILocalItemTag.Tagged => GetTagged();

    ILocalTagDefinition ILocalItemTag.Definition => GetDefinition();
}
