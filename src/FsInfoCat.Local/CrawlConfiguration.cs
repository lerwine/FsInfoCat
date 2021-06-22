using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class CrawlConfiguration : LocalDbEntity, ILocalCrawlConfiguration
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _displayName;
        private readonly IPropertyChangeTracker<ushort> _maxRecursionDepth;
        private readonly IPropertyChangeTracker<ulong> _totalMaxItems;
        private readonly IPropertyChangeTracker<long?> _ttl;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<bool> _isInactive;
        private readonly IPropertyChangeTracker<Subdirectory> _root;

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_LongName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string DisplayName { get => _displayName.GetValue(); set => _displayName.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MaxNameLength), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual ushort MaxRecursionDepth { get => _maxRecursionDepth.GetValue(); set => _maxRecursionDepth.SetValue(value); }

        public virtual ulong MaxTotalItems { get => _totalMaxItems.GetValue(); set => _totalMaxItems.SetValue(value); }

        [Range(1, long.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual long? TTL { get => _ttl.GetValue(); set => _ttl.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool IsInactive { get => _isInactive.GetValue(); set => _isInactive.SetValue(value); }

        public Subdirectory Root { get => _root.GetValue(); set => _root.SetValue(value); }

        #endregion

        ILocalSubdirectory ILocalCrawlConfiguration.Root => Root;

        ISubdirectory ICrawlConfiguration.Root => Root;

        public CrawlConfiguration()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _displayName = AddChangeTracker(nameof(DisplayName), "", TrimmedNonNullStringCoersion.Default);
            _maxRecursionDepth = AddChangeTracker(nameof(MaxRecursionDepth), DbConstants.DbColDefaultValue_MaxRecursionDepth);
            _totalMaxItems = AddChangeTracker(nameof(MaxTotalItems), DbConstants.DbColDefaultValue_MaxTotalItems);
            _ttl = AddChangeTracker<long?>(nameof(TTL), null);
            _notes = AddChangeTracker(nameof(Notes), "", NonWhiteSpaceOrEmptyStringCoersion.Default);
            _isInactive = AddChangeTracker(nameof(IsInactive), false);
            _root = AddChangeTracker<Subdirectory>(nameof(Root), null);
        }
    }
}
