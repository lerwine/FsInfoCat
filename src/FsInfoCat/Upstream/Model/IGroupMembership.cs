using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Defines a user's membership into a group.
    /// </summary>
    /// <seealso cref="IGroupMembershipRow" />
    /// <seealso cref="IEquatable{IGroupMembership}" />
    public interface IGroupMembership : IGroupMembershipRow, IEquatable<IGroupMembership>
    {
        /// <summary>
        /// Gets the target group.
        /// </summary>
        /// <value>The target <see cref="IUserGroup"/> that this specifies the member of.</value>
        [Display(Name = nameof(Properties.Resources.Group), ShortName = nameof(Properties.Resources.Group),
            Description = nameof(Properties.Resources.Description_TargetGroup), ResourceType = typeof(Properties.Resources))]
        IUserGroup Group { get; }

        /// <summary>
        /// Gets group member.
        /// </summary>
        /// <value>The <see cref="IUserProfile"/> of the group member.</value>
        [Display(Name = nameof(Properties.Resources.User), ShortName = nameof(Properties.Resources.User),
            Description = nameof(Properties.Resources.Description_GroupMember), ResourceType = typeof(Properties.Resources))]
        IUserProfile User { get; }
    }
}
