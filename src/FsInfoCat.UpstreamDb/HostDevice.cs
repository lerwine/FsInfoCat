using FsInfoCat.Model;
using FsInfoCat.Model.Upstream;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.UpstreamDb
{
    public class HostDevice : IHostDevice
    {
        private string _displayName = "";
        private string _notes = "";
        private string _machineIdentifier = "";
        private string _machineName = "";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        internal static void BuildEntity(EntityTypeBuilder<HostDevice> builder)
        {
            builder.HasOne(d => d.Platform).WithMany(p => p.HostDevices).IsRequired();
            builder.Property(nameof(Notes)).HasDefaultValue("").HasColumnType("nvarchar(max)").IsRequired();
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedHostDevices).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedHostDevices).IsRequired();
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedHostDevices).HasForeignKey(nameof(CreatedById)).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedHostDevices).HasForeignKey(nameof(ModifiedById)).IsRequired();
            throw new NotImplementedException();
        }

        public HostDevice()
        {
            Volumes = new HashSet<Volume>();
        }

        #region Column Properties

        public Guid Id { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_DisplayName), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_DisplayNameRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [MaxLength(DbConstants.DbColMaxLen_DisplayName, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string DisplayName { get => _displayName; set => _displayName = value ?? ""; }

        [Display(Name = nameof(ModelResources.DisplayName_MachineIdentifer), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_MachineIdentiferRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [MaxLength(DbConstants.DbColMaxLen_MachineIdentifer, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_MachineIdentiferLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string MachineIdentifer { get => _machineIdentifier; set => _machineIdentifier = value ?? ""; }

        [Display(Name = nameof(ModelResources.DisplayName_MachineName), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_MachineNameRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [MaxLength(DbConstants.DbColMaxLen_MachineName, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_MachineNameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string MachineName { get => _machineName; set => _machineName = value ?? ""; }

        public Guid PlatformId { get; set; }

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

        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_Platform), ErrorMessageResourceType = typeof(ModelResources))]
        public HostPlatform Platform { get; set; }

        public HashSet<Volume> Volumes { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_CreatedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile CreatedBy { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ModifiedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile ModifiedBy { get; set; }

        #endregion

        #region Explicit Members

        IReadOnlyCollection<IUpstreamVolume> IHostDevice.Volumes => Volumes;

        IUserProfile IUpstreamTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IUpstreamTimeStampedEntity.ModifiedBy => ModifiedBy;

        IHostPlatform IHostDevice.Platform => Platform;

        #endregion
    }
}
