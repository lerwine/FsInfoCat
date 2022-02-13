using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace FsInfoCat.Local
{
    public class PersonalSubdirectoryTag : ItemTag, ILocalPersonalSubdirectoryTag, IPersonalSubdirectoryTag
    {
        private Subdirectory _tagged;
        private PersonalTagDefinition _definition;

        public override Guid TaggedId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _tagged?.Id;
                    if (id.HasValue && id.Value != base.TaggedId)
                    {
                        base.TaggedId = id.Value;
                        return id.Value;
                    }
                    return base.TaggedId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _tagged?.Id;
                    if (id.HasValue && id.Value != value)
                        _tagged = null;
                    base.TaggedId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_SubdirectoryRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Tagged_Subdirectory), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public Subdirectory Tagged
        {
            get => _tagged;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_tagged is not null)
                            base.TaggedId = Guid.Empty;
                    }
                    else
                    {
                        base.TaggedId = value.Id;
                        _tagged = value;
                    }
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid DefinitionId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _definition?.Id;
                    if (id.HasValue && id.Value != base.DefinitionId)
                    {
                        base.DefinitionId = id.Value;
                        return id.Value;
                    }
                    return base.DefinitionId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _definition?.Id;
                    if (id.HasValue && id.Value != value)
                        _definition = null;
                    base.DefinitionId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_TagDefinitionRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_TagDefinition), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public PersonalTagDefinition Definition
        {
            get => _definition;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_definition is not null)
                            base.DefinitionId = Guid.Empty;
                    }
                    else
                    {
                        base.DefinitionId = value.Id;
                        _definition = value;
                    }
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        ILocalPersonalTagDefinition ILocalPersonalTag.Definition => Definition;

        IPersonalTagDefinition IPersonalTag.Definition => Definition;

        ILocalSubdirectory ILocalSubdirectoryTag.Tagged => Tagged;

        ISubdirectory ISubdirectoryTag.Tagged => Tagged;

        protected override ILocalTagDefinition GetDefinition() => Definition;

        protected override ILocalDbEntity GetTagged() => Tagged;

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalSubdirectoryTag> builder)
        {
            _ = builder.HasKey(nameof(TaggedId), nameof(DefinitionId));
            _ = builder.HasOne(pft => pft.Definition).WithMany(d => d.SubdirectoryTags).HasForeignKey(nameof(DefinitionId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(pft => pft.Tagged).WithMany(d => d.PersonalTags).HasForeignKey(nameof(TaggedId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
