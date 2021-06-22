using FsInfoCat.Model.Upstream;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.UpstreamDb
{
    public class UserGroupMembership : IUserGroupMembership
    {
        internal static void BuildEntity(EntityTypeBuilder<UserGroupMembership> builder)
        {
            builder.HasOne(d => d.User).WithMany(u => u.AssignmentGroups).HasForeignKey(nameof(UserId)).IsRequired();
            builder.HasOne(d => d.Group).WithMany(u => u.Members).HasForeignKey(nameof(GroupId)).IsRequired();
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedMemberships).HasForeignKey(nameof(CreatedById)).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedMemberships).HasForeignKey(nameof(ModifiedById)).IsRequired();
        }

        public Guid UserId { get; set; }

        public Guid GroupId { get; set; }

        public UserProfile User { get; set; }

        public UserGroup Group { get; set; }

        public Guid CreatedById { get; set; }

        public Guid ModifiedById { get; set; }

        public UserProfile CreatedBy { get; set; }

        public UserProfile ModifiedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        IUserProfile IUserGroupMembership.User => throw new NotImplementedException();

        IUserGroup IUserGroupMembership.Group => throw new NotImplementedException();

        IUserProfile IUpstreamTimeStampedEntity.CreatedBy => throw new NotImplementedException();

        IUserProfile IUpstreamTimeStampedEntity.ModifiedBy => throw new NotImplementedException();
    }
}
