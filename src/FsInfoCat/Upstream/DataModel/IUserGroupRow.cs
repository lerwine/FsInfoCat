﻿using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    public interface IUserGroupRow : IUpstreamDbEntity
    {
        /// <summary>
        /// Gets the group's name.
        /// </summary>
        /// <value>The name of the group.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Name), ResourceType = typeof(Properties.Resources))]
        string Name { get; }

        /// <summary>
        /// Gets roles for the current group.
        /// </summary>
        /// <value>The <see cref="UserRole">roles</see> that are assigned to all members of the group.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Roles), ResourceType = typeof(Properties.Resources))]
        UserRole Roles { get; }

        /// <summary>
        /// Gets notes for the current group.
        /// </summary>
        /// <value>The custom notes to be associated with the current group.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>
        /// Indicates whether the current group record is inactive.
        /// </summary>
        /// <value><see langword="true"/> if the current group record is inactive (archived); otherwise, <see langword="false"/> if it is active.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }
    }
}