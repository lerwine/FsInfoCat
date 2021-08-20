using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Local
{
    public class PersonalTagDefinition : LocalDbEntity, ILocalPersonalTagDefinition
    {
        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _name;
        private readonly IPropertyChangeTracker<string> _description;
        private readonly IPropertyChangeTracker<bool> _isInactive;
        private HashSet<PersonalFileTag> _fileTags = new();
        private HashSet<PersonalSubdirectoryTag> _subdirectoryTags = new();
        private HashSet<PersonalVolumeTag> _volumeTags = new();

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_SimpleName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string Name { get => _name.GetValue(); set => _name.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public string Description { get => _description.GetValue(); set => _description.SetValue(value); }

        [Required]
        public bool IsInactive { get => _isInactive.GetValue(); set => _isInactive.SetValue(value); }

        public virtual HashSet<PersonalFileTag> FileTags
        {
            get => _fileTags;
            set => CheckHashSetChanged(_fileTags, value, h => _fileTags = h);
        }

        public virtual HashSet<PersonalSubdirectoryTag> SubdirectoryTags
        {
            get => _subdirectoryTags;
            set => CheckHashSetChanged(_subdirectoryTags, value, h => _subdirectoryTags = h);
        }

        public virtual HashSet<PersonalVolumeTag> VolumeTags
        {
            get => _volumeTags;
            set => CheckHashSetChanged(_volumeTags, value, h => _volumeTags = h);
        }

        IEnumerable<ILocalFileTag> ILocalTagDefinition.FileTags => FileTags.Cast<ILocalFileTag>();

        IEnumerable<IPersonalFileTag> IPersonalTagDefinition.FileTags => FileTags.Cast<IPersonalFileTag>();

        IEnumerable<IFileTag> ITagDefinition.FileTags => FileTags.Cast<IFileTag>();

        IEnumerable<ILocalPersonalFileTag> ILocalPersonalTagDefinition.FileTags => FileTags.Cast<ILocalPersonalFileTag>();

        IEnumerable<ILocalSubdirectoryTag> ILocalTagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<ILocalSubdirectoryTag>();

        IEnumerable<IPersonalSubdirectoryTag> IPersonalTagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<IPersonalSubdirectoryTag>();

        IEnumerable<ISubdirectoryTag> ITagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<ISubdirectoryTag>();

        IEnumerable<ILocalPersonalSubdirectoryTag> ILocalPersonalTagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<ILocalPersonalSubdirectoryTag>();

        IEnumerable<ILocalVolumeTag> ILocalTagDefinition.VolumeTags => VolumeTags.Cast<ILocalVolumeTag>();

        IEnumerable<IPersonalVolumeTag> IPersonalTagDefinition.VolumeTags => VolumeTags.Cast<IPersonalVolumeTag>();

        IEnumerable<IVolumeTag> ITagDefinition.VolumeTags => VolumeTags.Cast<IVolumeTag>();

        IEnumerable<ILocalPersonalVolumeTag> ILocalPersonalTagDefinition.VolumeTags => VolumeTags.Cast<ILocalPersonalVolumeTag>();

        public PersonalTagDefinition()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _name = AddChangeTracker(nameof(Name), "", NonNullStringCoersion.Default);
            _description = AddChangeTracker(nameof(Description), "", NonWhiteSpaceOrEmptyStringCoersion.Default);
            _isInactive = AddChangeTracker(nameof(IsInactive), false);
        }
    }
}
