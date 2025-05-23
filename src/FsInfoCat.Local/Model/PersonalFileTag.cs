using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Generic interface for an <see cref="ILocalPersonalTagDefinition"/> that is associated with an <see cref="ILocalFile"/>.
/// </summary>
/// <seealso cref="PersonalFileTagListItem" />
/// <seealso cref="PersonalSubdirectoryTag" />
/// <seealso cref="PersonalVolumeTag" />
/// <seealso cref="LocalDbContext.PersonalFileTags" />
public class PersonalFileTag : ItemTag, IHasMembershipKeyReference<DbFile, PersonalTagDefinition>, ILocalPersonalFileTag, IEquatable<PersonalFileTag>
{
    private readonly FileReference _tagged;
    private readonly PersonalTagReference _definition;

    /// <summary>
    /// Gets the primary key value that references the tagged entity.
    /// </summary>
    /// <value>The <see cref="Guid">unique identifier</see> used as part of the current entity's primary key the database.</value>
    public override Guid TaggedId { get => _tagged.Id; set => _tagged.SetId(value); }

    /// <summary>
    /// Gets the tagged file.
    /// </summary>
    /// <value>The tagged <see cref="DbFile"/>.</value>
    [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_FileRequired),
        ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
    [Display(Name = nameof(FsInfoCat.Properties.Resources.TaggedFile), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public DbFile Tagged { get => _tagged.Entity; set => _tagged.Entity = value; }

    /// <summary>
    /// Gets the primary key value that references the <see cref="ITagDefinition"/> entity.
    /// </summary>
    /// <value>The <see cref="Guid">unique identifier</see> used as part of the current entity's primary key the database.</value>
    public override Guid DefinitionId { get => _definition.Id; set => _definition.SetId(value); }

    /// <summary>
    /// Gets the personal tag definition.
    /// </summary>
    /// <value>The personal tag definition that is associated with the <see cref="DbFile"/>.</value>
    [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_TagDefinitionRequired),
        ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
    [Display(Name = nameof(FsInfoCat.Properties.Resources.TagDefinition), ResourceType = typeof(FsInfoCat.Properties.Resources))]
    public PersonalTagDefinition Definition { get => _definition.Entity; set => _definition.Entity = value; }

    IForeignKeyReference<DbFile> IHasMembershipKeyReference<DbFile, PersonalTagDefinition>.Ref1 => _tagged;

    IForeignKeyReference<PersonalTagDefinition> IHasMembershipKeyReference<DbFile, PersonalTagDefinition>.Ref2 => _definition;

    ILocalPersonalTagDefinition ILocalPersonalTag.Definition => Definition;

    IPersonalTagDefinition IPersonalTag.Definition => Definition;

    ILocalFile ILocalFileTag.Tagged => Tagged;

    IFile IFileTag.Tagged => Tagged;

    IForeignKeyReference IHasMembershipKeyReference.Ref1 => _tagged;

    IForeignKeyReference IHasMembershipKeyReference.Ref2 => _definition;

    object ISynchronizable.SyncRoot => SyncRoot;

    IForeignKeyReference<IFile> IHasMembershipKeyReference<IFile, IPersonalTagDefinition>.Ref1 => _tagged;

    IForeignKeyReference<IPersonalTagDefinition> IHasMembershipKeyReference<IFile, IPersonalTagDefinition>.Ref2 => _definition;

    IForeignKeyReference<IFile> IHasMembershipKeyReference<IFile, ITagDefinition>.Ref1 => _tagged;

    IForeignKeyReference<ITagDefinition> IHasMembershipKeyReference<IFile, ITagDefinition>.Ref2 => _definition;

    IForeignKeyReference<ILocalFile> IHasMembershipKeyReference<ILocalFile, ILocalTagDefinition>.Ref1 => _tagged;

    IForeignKeyReference<ILocalTagDefinition> IHasMembershipKeyReference<ILocalFile, ILocalTagDefinition>.Ref2 => _definition;

    IForeignKeyReference<ILocalFile> IHasMembershipKeyReference<ILocalFile, ILocalPersonalTagDefinition>.Ref1 => _tagged;

    IForeignKeyReference<ILocalPersonalTagDefinition> IHasMembershipKeyReference<ILocalFile, ILocalPersonalTagDefinition>.Ref2 => _definition;

    /// <summary>
    /// Initializes a new PersonalFileTag entity.
    /// </summary>
    public PersonalFileTag()
    {
        _tagged = new(SyncRoot);
        _definition = new(SyncRoot);
    }

    /// <summary>
    /// Gets the tag definition.
    /// </summary>
    /// <returns>The <see cref="PersonalTagDefinition"/> that is associated with the current <see cref="PersonalFileTag"/>.</returns>
    protected override ILocalTagDefinition GetDefinition() => Definition;

    /// <summary>
    /// Gets the tagged entity.
    /// </summary>
    /// <returns>The <see cref="DbFile"/> that is associated with the current <see cref="PersonalFileTag"/>.</returns>
    protected override ILocalDbEntity GetTagged() => Tagged;

    internal static void OnBuildEntity(EntityTypeBuilder<PersonalFileTag> builder)
    {
        _ = builder.HasKey(nameof(TaggedId), nameof(DefinitionId));
        _ = builder.HasOne(pft => pft.Definition).WithMany(d => d.FileTags).HasForeignKey(nameof(DefinitionId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        _ = builder.HasOne(pft => pft.Tagged).WithMany(d => d.PersonalTags).HasForeignKey(nameof(TaggedId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="ILocalPersonalFileTag" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected bool ArePropertiesEqual([DisallowNull] ILocalPersonalFileTag other)
    {
        // TODO: Implement ArePropertiesEqual(ILocalPersonalFileTag)
        throw new NotImplementedException();
    }

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="IPersonalFileTag" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected bool ArePropertiesEqual([DisallowNull] IPersonalFileTag other)
    {
        // TODO: Implement ArePropertiesEqual(IPersonalFileTag)
        throw new NotImplementedException();
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public bool Equals(PersonalFileTag other) => other is not null && (ReferenceEquals(this, other) || ArePropertiesEqual(other));

    public bool Equals(ILocalPersonalFileTag other) => other is not null && ((other is PersonalFileTag tag) ? Equals(tag) : ArePropertiesEqual(other));

    public bool Equals(ILocalFileTag other)
    {
        if (other is null) return false;
        if (other is PersonalFileTag tag) return Equals(tag);
        return other is ILocalPersonalFileTag local && ArePropertiesEqual(local);
    }

    public bool Equals(IPersonalFileTag other)
    {
        if (other is null) return false;
        if (other is PersonalFileTag tag) return Equals(tag);
        return (other is ILocalPersonalFileTag local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (obj is PersonalFileTag tag) return Equals(tag);
        return obj is IPersonalFileTag other && ((other is ILocalPersonalFileTag local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other));
    }

    public override int GetHashCode()
    {
        Guid taggedId = TaggedId;
        Guid definitionId = DefinitionId;
        if (taggedId.Equals(Guid.Empty) && DefinitionId.Equals(Guid.Empty))
            // TODO: Implement GetHashCode()
            throw new NotImplementedException();
        return HashCode.Combine(taggedId, definitionId);
    }

    public override string ToString() => $@"{{ TaggedId={_tagged.IdValue}, DefinitionId={_definition.IdValue},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={LastSynchronizedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId} }}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Attempts to get the get primary key of the tag definition.
    /// </summary>
    /// <param name="definitionId">The <see cref="IHasSimpleIdentifier.Id"/> of the tag <see cref="PersonalTagDefinition"/>.</param>
    /// <returns><see langword="true"/> if the tag <see cref="PersonalTagDefinition"/> has a primary key value assigned; otherwise, <see langword="false"/>.</returns>
    public override bool TryGetDefinitionId(out Guid definitionId) => _definition.TryGetId(out definitionId);

    /// <summary>
    /// Attempts to get the get primary key of the tagged entity.
    /// </summary>
    /// <param name="taggedId">The <see cref="IHasSimpleIdentifier.Id"/> of the <see cref="DbFile"/> entity.</param>
    /// <returns><see langword="true"/> if the <see cref="DbFile"/> entity has a primary key value assigned; otherwise, <see langword="false"/>.</returns>
    public override bool TryGetTaggedId(out Guid taggedId) => _tagged.TryGetId(out taggedId);
}
