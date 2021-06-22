using FsInfoCat.Model;
using FsInfoCat.Model.Upstream;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FsInfoCat.UpstreamDb
{
    public class Volume : IUpstreamVolume
    {
        private string _displayName = "";
        private string _identifier = "";
        private string _volumeName = "";
        private string _notes = "";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(DisplayName, new ValidationContext(this, null, null) { MemberName = nameof(DisplayName) }, results);
            Validator.TryValidateProperty(VolumeName, new ValidationContext(this, null, null) { MemberName = nameof(VolumeName) }, results);
            Validator.TryValidateProperty(Identifier, new ValidationContext(this, null, null) { MemberName = nameof(Identifier) }, results);
            Validator.TryValidateProperty(MaxNameLength, new ValidationContext(this, null, null) { MemberName = nameof(MaxNameLength) }, results);
            Validator.TryValidateProperty(FileSystem, new ValidationContext(this, null, null) { MemberName = nameof(FileSystem) }, results);
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(ModelResources.ErrorMessage_ModifiedOn, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        internal static void BuildEntity(EntityTypeBuilder<Volume> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(DisplayName)).HasMaxLength(DbConstants.DbColMaxLen_DisplayName).IsRequired();
            builder.Property(nameof(VolumeName)).HasMaxLength(DbConstants.DbColMaxLen_VolumeName).IsRequired();
            builder.Property(nameof(Identifier)).HasMaxLength(DbConstants.DbColMaxLen_VolumeIdentifier).IsRequired();
            builder.Property(nameof(CaseSensitiveSearch)).HasDefaultValue(false);
            builder.Property(nameof(ReadOnly)).HasDefaultValue(false);
            builder.Property(nameof(MaxNameLength)).HasDefaultValue(DbConstants.DefaultValue_MaxFileSystemNameLength);
            builder.Property(nameof(Notes)).HasDefaultValue("").HasColumnType("nvarchar(max)").IsRequired();
            builder.HasOne(p => p.FileSystem).WithMany(d => d.Volumes).IsRequired();
            builder.HasOne(p => p.RootDirectory).WithOne(d => d.Volume);
            builder.HasOne(v => v.HostDevice).WithMany(h => h.Volumes);
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedVolumes).HasForeignKey(nameof(CreatedById)).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedVolumes).HasForeignKey(nameof(ModifiedById)).IsRequired();
        }

        #region Column Properties

        public Guid Id { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CaseSensitiveSearch), ResourceType = typeof(ModelResources))]
        public bool? CaseSensitiveSearch { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ReadOnly), ResourceType = typeof(ModelResources))]
        public bool? ReadOnly { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_DisplayName), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_DisplayNameRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [MaxLength(DbConstants.DbColMaxLen_DisplayName, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string DisplayName { get => _displayName; set => _displayName = value ?? ""; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_IsInactive), ResourceType = typeof(ModelResources))]
        public bool IsInactive { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_MaxNameLength), ResourceType = typeof(ModelResources))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_MaxNameLengthNegative), ErrorMessageResourceType = typeof(ModelResources))]
        public long? MaxNameLength { get; set; }

        public Guid FileSystemId { get; set; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_DriveType), ResourceType = typeof(ModelResources))]
        public DriveType Type { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Notes { get => _notes; set => _notes = value ?? ""; }

        [Display(Name = nameof(ModelResources.DisplayName_VolumeName), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_VolumeNameRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [MaxLength(DbConstants.DbColMaxLen_VolumeName, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string VolumeName { get => _volumeName; set => _volumeName = value ?? ""; }

        [Display(Name = nameof(ModelResources.DisplayName_VolumeIdentifier), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_VolumeIdentifierRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [MaxLength(DbConstants.DbColMaxLen_VolumeIdentifier, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string Identifier { get => _identifier; set => _identifier = value ?? ""; }

        public Guid? HostDeviceId { get; set; }

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

        [Display(Name = nameof(ModelResources.DisplayName_RootDirectory), ResourceType = typeof(ModelResources))]
        public FsDirectory RootDirectory { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_FileSystem), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_FileSystemRequired), ErrorMessageResourceType = typeof(ModelResources))]
        public FileSystem FileSystem { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_HostDevice), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_HostDeviceRequired), ErrorMessageResourceType = typeof(ModelResources))]
        public HostDevice HostDevice { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_CreatedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile CreatedBy { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ModifiedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile ModifiedBy { get; set; }

        #endregion

        #region Explicit Members

        ISubDirectory IVolume.RootDirectory => RootDirectory;

        IUpstreamSubDirectory IUpstreamVolume.RootDirectory => RootDirectory;

        IFileSystem IVolume.FileSystem => FileSystem;

        IUpstreamFileSystem IUpstreamVolume.FileSystem => FileSystem;

        IUserProfile IUpstreamTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IUpstreamTimeStampedEntity.ModifiedBy => ModifiedBy;

        IHostDevice IUpstreamVolume.HostDevice => HostDevice;

        #endregion
    }
}
