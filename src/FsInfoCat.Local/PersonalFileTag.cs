using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local
{
    public class PersonalFileTag : ItemTag, ILocalPersonalFileTag, IPersonalFileTag, IEquatable<PersonalFileTag>
    {
        private DbFile _tagged;
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

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_FileRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Tagged_File), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public DbFile Tagged
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

        ILocalFile ILocalFileTag.Tagged => Tagged;

        IFile IFileTag.Tagged => Tagged;

        protected override ILocalTagDefinition GetDefinition() => Definition;

        protected override ILocalDbEntity GetTagged() => Tagged;

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalFileTag> builder)
        {
            _ = builder.HasKey(nameof(TaggedId), nameof(DefinitionId));
            _ = builder.HasOne(pft => pft.Definition).WithMany(d => d.FileTags).HasForeignKey(nameof(DefinitionId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(pft => pft.Tagged).WithMany(d => d.PersonalTags).HasForeignKey(nameof(TaggedId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalPersonalFileTag other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IPersonalFileTag other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(PersonalFileTag other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IPersonalFileTag other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (Tagged is null) ? (TaggedId.Equals(Guid.Empty) ? hash * 109 : hash * 109 + TaggedId.GetHashCode()) : hash * 109 + (Tagged?.GetHashCode() ?? 0);
                hash = (Definition is null) ? (DefinitionId.Equals(Guid.Empty) ? hash * 109 : hash * 109 + DefinitionId.GetHashCode()) : hash * 109 + (Definition?.GetHashCode() ?? 0);
                hash = hash * 23 + Notes.GetHashCode();
                hash = UpstreamId.HasValue ? hash * 23 + (UpstreamId ?? default).GetHashCode() : hash * 23;
                hash = LastSynchronizedOn.HasValue ? hash * 23 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 23;
                hash = hash * 23 + CreatedOn.GetHashCode();
                hash = hash * 23 + ModifiedOn.GetHashCode();
                return hash;
            }
        }
    }
}
