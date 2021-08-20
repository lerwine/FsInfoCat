using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Local
{
    public class SharedTagDefinition : LocalDbEntity, ILocalSharedTagDefinition
    {
        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _name;
        private readonly IPropertyChangeTracker<string> _description;
        private readonly IPropertyChangeTracker<bool> _isInactive;
        private HashSet<SharedFileTag> _fileTags = new();
        private HashSet<SharedSubdirectoryTag> _subdirectoryTags = new();
        private HashSet<SharedVolumeTag> _volumeTags = new();

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

        public HashSet<SharedFileTag> FileTags
        {
            get => _fileTags;
            set => CheckHashSetChanged(_fileTags, value, h => _fileTags = h);
        }

        public HashSet<SharedSubdirectoryTag> SubdirectoryTags
        {
            get => _subdirectoryTags;
            set => CheckHashSetChanged(_subdirectoryTags, value, h => _subdirectoryTags = h);
        }

        public HashSet<SharedVolumeTag> VolumeTags
        {
            get => _volumeTags;
            set => CheckHashSetChanged(_volumeTags, value, h => _volumeTags = h);
        }

        IEnumerable<ILocalFileTag> ILocalTagDefinition.FileTags => FileTags.Cast<ILocalFileTag>();

        IEnumerable<ISharedFileTag> ISharedTagDefinition.FileTags => FileTags.Cast<ISharedFileTag>();

        IEnumerable<IFileTag> ITagDefinition.FileTags => FileTags.Cast<IFileTag>();

        IEnumerable<ILocalSharedFileTag> ILocalSharedTagDefinition.FileTags => FileTags.Cast<ILocalSharedFileTag>();

        IEnumerable<ILocalSubdirectoryTag> ILocalTagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<ILocalSubdirectoryTag>();

        IEnumerable<ISharedSubdirectoryTag> ISharedTagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<ISharedSubdirectoryTag>();

        IEnumerable<ISubdirectoryTag> ITagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<ISubdirectoryTag>();

        IEnumerable<ILocalSharedSubdirectoryTag> ILocalSharedTagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<ILocalSharedSubdirectoryTag>();

        IEnumerable<ILocalVolumeTag> ILocalTagDefinition.VolumeTags => VolumeTags.Cast<ILocalVolumeTag>();

        IEnumerable<ISharedVolumeTag> ISharedTagDefinition.VolumeTags => VolumeTags.Cast<ISharedVolumeTag>();

        IEnumerable<IVolumeTag> ITagDefinition.VolumeTags => VolumeTags.Cast<IVolumeTag>();

        IEnumerable<ILocalSharedVolumeTag> ILocalSharedTagDefinition.VolumeTags => VolumeTags.Cast<ILocalSharedVolumeTag>();

        public SharedTagDefinition()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _name = AddChangeTracker(nameof(Name), "", NonNullStringCoersion.Default);
            _description = AddChangeTracker(nameof(Description), "", NonWhiteSpaceOrEmptyStringCoersion.Default);
            _isInactive = AddChangeTracker(nameof(IsInactive), false);
        }
    }
}
