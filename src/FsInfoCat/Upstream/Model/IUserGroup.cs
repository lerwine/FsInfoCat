using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    // TODO: Document IUserGroup interface
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    /// <seealso cref="IEquatable{IUserGroup}" />
    public interface IUserGroup : IUserGroupRow, IEquatable<IUserGroup>
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Gets the group membership.
        /// </summary>
        /// <value>An <see cref="IGroupMembership"/> record that defines a user's membership with a specific group.</value>
        [Display(Name = nameof(Properties.Resources.Members), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IGroupMembership> Members { get; }

        /// <summary>
        /// Gets mitigation tasks for the curent group.
        /// </summary>
        /// <value>Mitigation tasks that have been assigned to the current group.</value>
        [Display(Name = nameof(Properties.Resources.Tasks), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IMitigationTask> Tasks { get; }
    }
}
