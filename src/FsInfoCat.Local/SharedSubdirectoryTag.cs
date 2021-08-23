using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public class SharedSubdirectoryTag : ItemTag, ILocalSharedSubdirectoryTag, ISharedSubdirectoryTag
    {
        private readonly IPropertyChangeTracker<Subdirectory> _tagged;
        private readonly IPropertyChangeTracker<SharedTagDefinition> _definition;

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_SubdirectoryRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Tagged_Subdirectory), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public Subdirectory Tagged
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
        public SharedTagDefinition Definition
        {
            get => _definition.GetValue();
            set
            {
                if (_definition.SetValue(value))
                    SetDefinitionId((value is null) ? Guid.Empty : value.Id);
            }
        }

        ILocalSharedTagDefinition ILocalSharedTag.Definition => Definition;

        ISharedTagDefinition ISharedTag.Definition => Definition;

        ILocalSubdirectory ILocalSubdirectoryTag.Tagged => Tagged;

        ISubdirectory ISubdirectoryTag.Tagged => Tagged;

        public SharedSubdirectoryTag()
        {
            _tagged = AddChangeTracker<Subdirectory>(nameof(Tagged), null);
            _definition = AddChangeTracker<SharedTagDefinition>(nameof(Definition), null);
        }

        protected override ILocalTagDefinition GetDefinition() => Definition;

        protected override ILocalDbEntity GetTagged() => Tagged;

        internal static void OnBuildEntity(EntityTypeBuilder<SharedSubdirectoryTag> builder)
        {
            builder.HasKey(nameof(TaggedId), nameof(DefinitionId));
            builder.HasOne(pft => pft.Definition).WithMany(d => d.SubdirectoryTags).HasForeignKey(nameof(DefinitionId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pft => pft.Tagged).WithMany(d => d.SharedTags).HasForeignKey(nameof(TaggedId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnTaggedIdChanged(Guid value)
        {
            Subdirectory nav = _tagged.GetValue();
            if (!(nav is null || nav.Id.Equals(value)))
                _tagged.SetValue(null);
        }

        protected override void OnDefinitionIdChanged(Guid value)
        {
            SharedTagDefinition nav = _definition.GetValue();
            if (!(nav is null || nav.Id.Equals(value)))
                _definition.SetValue(null);
        }
    }
}
