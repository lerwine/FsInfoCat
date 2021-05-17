using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.RemoteDb
{
    public class UserGroup : IUserGroup
    {
        private string _name = "";
        private string _notes = "";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        internal static void BuildEntity(EntityTypeBuilder<UserGroup> builder)
        {
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedUserGroups).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedUserGroups).IsRequired();
            throw new NotImplementedException();
        }

        public UserGroup()
        {
            Members = new HashSet<UserProfile>();
            DirectoryRelocationTasks = new HashSet<DirectoryRelocateTask>();
            FileRelocationTasks = new HashSet<FileRelocateTask>();
        }

        #region Column Properties

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.ERROR_MESSAGE_NAME_REQUIRED)]
        [MaxLength(Constants.MAX_LENGTH_DISPLAY_NAME, ErrorMessage = Constants.ERROR_MESSAGE_NAME_LENGTH)]
        public string Name { get => _name; set => _name = value ?? ""; }

        public UserRole Roles { get; set; }

        public string Notes { get => _notes; set => _notes = value ?? ""; }

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

        public HashSet<UserProfile> Members { get; set; }

        public HashSet<DirectoryRelocateTask> DirectoryRelocationTasks { get; set; }

        public HashSet<FileRelocateTask> FileRelocationTasks { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CREATED_BY)]
        [Required]
        public UserProfile CreatedBy { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_BY)]
        [Required]
        public UserProfile ModifiedBy { get; set; }

        #endregion

        #region Explicit Members

        IReadOnlyCollection<IUserProfile> IUserGroup.Members => Members;

        IReadOnlyCollection<IDirectoryRelocateTask> IUserGroup.DirectoryRelocationTasks => DirectoryRelocationTasks;

        IReadOnlyCollection<IFileRelocateTask> IUserGroup.FileRelocationTasks => FileRelocationTasks;

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => ModifiedBy;

        #endregion
    }
}
