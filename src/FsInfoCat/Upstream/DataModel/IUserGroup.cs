using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    public interface IUserGroup : IUserGroupRow, IEquatable<IUserGroup>
    {
        /// <summary>
        /// Gets the group membership.
        /// </summary>
        /// <value>An <see cref="IGroupMembership"/> record that defines a user's membership with a specific group.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Members), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IGroupMembership> Members { get; }

        /// <summary>
        /// Gets mitigation tasks for the curent group.
        /// </summary>
        /// <value>Mitigation tasks that have been assigned to the current group.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Tasks), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IMitigationTask> Tasks { get; }
    }
}
