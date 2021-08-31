using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    public interface IUserGroupRow : IUpstreamDbEntity
    {
        /// <summary>Gets the group's name.</summary>
        /// <value>The name of the group.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Name), ResourceType = typeof(Properties.Resources))]
        string Name { get; }

        /// <summary>Gets roles for the current group.</summary>
        /// <value>The <see cref="UserRole">roles</see> that are assigned to all members of the group.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Roles), ResourceType = typeof(Properties.Resources))]
        UserRole Roles { get; }

        /// <summary>Gets notes for the current group.</summary>
        /// <value>The custom notes to be associated with the current group.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>Indicates whether the current group record is inactive.</summary>
        /// <value><see langword="true"/> if the current group record is inactive (archived); otherwise, <see langword="false"/> if it is active.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }
    }
    public interface IUserGroup : IUserGroupRow
    {
        /// <summary>Gets the group membership.</summary>
        /// <value>A <see cref="IGroupMembership"/> record that defines a user's membership with a specific group.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Members), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IGroupMembership> Members { get; }

        /// <summary>Gets mitigation tasks for the curent group.</summary>
        /// <value>Mitigation tasks that have been assigned to the current group.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Tasks), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IMitigationTask> Tasks { get; }
    }
    public interface IUserGroupListItem : IUserGroupRow
    {
        long MemberCount { get; }

        long TaskCount { get; }
    }
}
