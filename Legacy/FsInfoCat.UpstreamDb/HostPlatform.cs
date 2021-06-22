using FsInfoCat.Model;
using FsInfoCat.Model.Upstream;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.UpstreamDb
{
    public class HostPlatform : IHostPlatform
    {
        private string _displayName = "";
        private string _notes = "";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        internal static void BuildEntity(EntityTypeBuilder<HostPlatform> builder)
        {
            builder.Property(nameof(Notes)).HasDefaultValue("").HasColumnType("nvarchar(max)").IsRequired();
            builder.HasOne(d => d.DefaultFSType).WithMany(u => u.HostPlatformDefaults).HasForeignKey(nameof(DefaultFsTypeId)).IsRequired();
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedHostPlatforms).HasForeignKey(nameof(CreatedById)).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedHostPlatforms).HasForeignKey(nameof(ModifiedById)).IsRequired();
        }

        public HostPlatform()
        {
            HostDevices = new HashSet<HostDevice>();
        }

        #region Column Properties

        public Guid Id { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_DisplayName), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_DisplayNameRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [MaxLength(DbConstants.DbColMaxLen_DisplayName, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string DisplayName { get => _displayName; set => _displayName = value ?? ""; }

        [Required]
        public PlatformType Type { get; set; }

        public Guid? DefaultFsTypeId { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Notes { get => _notes; set => _notes = value ?? ""; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_IsInactive), ResourceType = typeof(ModelResources))]
        public bool IsInactive { get; set; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_CreatedOn), ResourceType = typeof(ModelResources))]
        public DateTime CreatedOn { get; set; }

        public Guid CreatedById { get; set; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_ModifiedOn), ResourceType = typeof(ModelResources))]
        public DateTime ModifiedOn { get; set; }

        public Guid ModifiedById { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = nameof(ModelResources.DisplayName_DefaultFSType), ResourceType = typeof(ModelResources))]
        public FileSystem DefaultFSType { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_HostDevices), ResourceType = typeof(ModelResources))]
        public HashSet<HostDevice> HostDevices { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_CreatedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile CreatedBy { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ModifiedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile ModifiedBy { get; set; }

        #endregion

        #region Explicit Members

        IUpstreamFileSystem IHostPlatform.DefaultFsType => DefaultFSType;

        IReadOnlyCollection<IHostDevice> IHostPlatform.HostDevices => HostDevices;

        IUserProfile IUpstreamTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IUpstreamTimeStampedEntity.ModifiedBy => ModifiedBy;

        #endregion
    }
}
