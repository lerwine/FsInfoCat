using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.RemoteDb
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
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedHostPlatforms).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedHostPlatforms).IsRequired();
            throw new NotImplementedException();
        }

        public HostPlatform()
        {
            HostDevices = new HashSet<HostDevice>();
        }

        #region Column Properties

        public Guid Id { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_DISPLAY_NAME)]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.ERROR_MESSAGE_DISPAY_NAME_REQUIRED)]
        [MaxLength(Constants.MAX_LENGTH_DISPLAY_NAME, ErrorMessage = Constants.ERROR_MESSAGE_DISPAY_NAME_LENGTH)]
        public string DisplayName { get => _displayName; set => _displayName = value ?? ""; }

        public PlatformType Type { get; set; }

        public Guid? DefaultFsTypeId { get; set; }

        public string Notes { get => _notes; set => _notes = value ?? ""; }

        [DisplayName(Constants.DISPLAY_NAME_IS_INACTIVE)]
        public bool IsInactive { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CREATED_ON)]
        [Required]
        public DateTime CreatedOn { get; set; }

        public Guid CreatedById { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        [Required]
        public DateTime ModifiedOn { get; set; }

        public Guid ModifiedById { get; set; }

        #endregion

        #region Navigation Properties

        public FileSystem DefaultFSType { get; set; }

        public HashSet<HostDevice> HostDevices { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CREATED_BY)]
        [Required]
        public UserProfile CreatedBy { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_BY)]
        [Required]
        public UserProfile ModifiedBy { get; set; }

        #endregion

        #region Explicit Members

        IRemoteFileSystem IHostPlatform.DefaultFsType => DefaultFSType;

        IReadOnlyCollection<IHostDevice> IHostPlatform.HostDevices => HostDevices;

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => ModifiedBy;

        #endregion
    }
}
