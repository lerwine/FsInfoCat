using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model;

/// <summary>
/// List item DB entity that associates a <see cref="PersonalTagDefinition"/> or <see cref="SharedTagDefinition"/> with an <see cref="DbFile"/>, <see cref="Subdirectory"/>
/// or <see cref="Volume"/>.
/// </summary>
/// <seealso cref="ItemTag" />
/// <seealso cref="PersonalFileTagListItem" />
/// <seealso cref="PersonalSubdirectoryTagListItem" />
/// <seealso cref="PersonalVolumeTagListItem" />
/// <seealso cref="LocalDbContext.PersonalFileTagListing" />
/// <seealso cref="LocalDbContext.PersonalSubdirectoryTagListing" />
/// <seealso cref="LocalDbContext.PersonalVolumeTagListing" />
public abstract class ItemTagListItem : ItemTagRow, ILocalItemTagListItem, IEquatable<ItemTagListItem>
{
    private string _name = string.Empty;
    private string _description = string.Empty;

    /// <summary>
    /// Gets the primary key value that references the tagged entity.
    /// </summary>
    /// <value>The <see cref="Guid">unique identifier</see> used as part of the current entity's primary key the database.</value>
    public override Guid TaggedId { get; set; }

    /// <summary>
    /// Gets the primary key value that references the <see cref="ILocalTagDefinition"/> entity.
    /// </summary>
    /// <value>The <see cref="Guid">unique identifier</see> used as part of the current entity's primary key the database.</value>
    public override Guid DefinitionId { get; set; }

    /// <summary>
    /// Gets the name of the tag.
    /// </summary>
    /// <value>The <see cref="ITagDefinitionRow.Name"/> of the tag.</value>
    [Required]
    [NotNull]
    [BackingField(nameof(_name))]
    public virtual string Name { get => _name; set => _name = value.AsWsNormalizedOrEmpty(); }

    /// <summary>
    /// Gets the description of the tag.
    /// </summary>
    /// <value>The <see cref="ITagDefinitionRow.Description"/> of the tag.</value>
    [Required(AllowEmptyStrings = true)]
    [NotNull]
    [BackingField(nameof(_description))]
    public virtual string Description { get => _description; set => _description = value.AsWsNormalizedOrEmpty(); }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public bool Equals(ItemTagListItem other)
    {
        // TODO: Implement Equals(ItemTagListItem)
        throw new NotImplementedException();
    }

    public bool Equals(IItemTagListItem other)
    {
        // TODO: Implement Equals(IItemTagListItem)
        throw new NotImplementedException();
    }

    public override bool Equals(object obj)
    {
        // TODO: Implement Equals(object)
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        Guid taggedId = TaggedId;
        Guid definitionId = DefinitionId;
        if (taggedId.Equals(Guid.Empty) && definitionId.Equals(Guid.Empty))
            throw new NotImplementedException();
        // TODO: Implement GetHashCode()
        return HashCode.Combine(taggedId, definitionId);
    }

    public override string ToString() => $@"{{ TaggedId={TaggedId}, DefinitionId={DefinitionId}, Name=""{ExtensionMethods.EscapeCsString(_name)}"",
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={LastSynchronizedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId},
    Description=""{ExtensionMethods.EscapeCsString(_description)}"" }}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
