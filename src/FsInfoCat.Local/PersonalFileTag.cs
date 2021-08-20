using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public class PersonalFileTag : ItemTag, ILocalPersonalFileTag, IPersonalFileTag
    {
        private readonly IPropertyChangeTracker<DbFile> _tagged;
        private readonly IPropertyChangeTracker<PersonalTagDefinition> _definition;

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_FileRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Tagged_File), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public DbFile Tagged
        {
            get => _tagged.GetValue();
            set
            {
                if (_tagged.SetValue(value))
                    SetTaggedId((value is null) ? Guid.Empty : value.Id);
            }
        }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_TagDefinitionRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_TagDefinition), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public PersonalTagDefinition Definition
        {
            get => _definition.GetValue();
            set
            {
                if (_definition.SetValue(value))
                    SetDefinitionId((value is null) ? Guid.Empty : value.Id);
            }
        }

        ILocalPersonalTagDefinition ILocalPersonalTag.Definition => Definition;

        IPersonalTagDefinition IPersonalTag.Definition => Definition;

        ILocalFile ILocalFileTag.Tagged => Tagged;

        IFile IFileTag.Tagged => Tagged;

        public PersonalFileTag()
        {
            _tagged = AddChangeTracker<DbFile>(nameof(Tagged), null);
            _definition = AddChangeTracker<PersonalTagDefinition>(nameof(Definition), null);
        }

        protected override ILocalTagDefinition GetDefinition() => Definition;

        protected override ILocalDbEntity GetTagged() => Tagged;

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalFileTag> builder)
        {
            builder.HasOne(pft => pft.Definition).WithMany(d => d.FileTags).HasForeignKey(nameof(DefinitionId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pft => pft.Tagged).WithMany(d => d.PersonalTags).HasForeignKey(nameof(TaggedId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnTaggedIdChanged(Guid value)
        {
            DbFile nav = _tagged.GetValue();
            if (!(nav is null || nav.Id.Equals(value)))
                _tagged.SetValue(null);
        }

        protected override void OnDefinitionIdChanged(Guid value)
        {
            PersonalTagDefinition nav = _definition.GetValue();
            if (!(nav is null || nav.Id.Equals(value)))
                _definition.SetValue(null);
        }
    }
}
