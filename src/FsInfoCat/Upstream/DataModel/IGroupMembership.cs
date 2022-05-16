using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Defines a user's membership into a group.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IGroupMembership : IGroupMembershipRow, IEquatable<IGroupMembership>
    {
        /// <summary>
        /// Gets the target group.
        /// </summary>
        /// <value>The target <see cref="IUserGroup"/> that this specifies the member of.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Group), ShortName = nameof(Properties.Resources.DisplayName_Group),
            Description = nameof(Properties.Resources.Description_GroupMembership_Group), ResourceType = typeof(Properties.Resources))]
        IUserGroup Group { get; }

        /// <summary>
        /// Gets group member.
        /// </summary>
        /// <value>The <see cref="IUserProfile"/> of the group member.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_User), ShortName = nameof(Properties.Resources.DisplayName_User),
            Description = nameof(Properties.Resources.Description_GroupMembership_User), ResourceType = typeof(Properties.Resources))]
        IUserProfile User { get; }
    }
}
