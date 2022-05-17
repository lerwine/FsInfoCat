using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Base interface for user profile database entities.
    /// </summary>
    /// <seealso cref="IUserProfileRow" />
    /// <seealso cref="IEquatable{IUserProfile}" />
    public interface IUserProfile : IUserProfileRow, IEquatable<IUserProfile>
    {
        /// <summary>
        /// Gets the membership objects that determine what groups the current user belongs to.
        /// </summary>
        /// <value>The <see cref="IGroupMembership">membership objects</see> that determine what groups the current user belongs to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MemberOf), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IGroupMembership> MemberOf { get; }

        /// <summary>
        /// Gets the tasks that are assigned to the current user.
        /// </summary>
        /// <value>The <see cref="IMitigationTask">tasks</see> that are assigned to the current user.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Tasks), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IMitigationTask> Tasks { get; }
    }
}
