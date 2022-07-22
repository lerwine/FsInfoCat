using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    // TODO: Document IUserGroupRow interface
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IUserGroupRow : IUpstreamDbEntity
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Gets the group's name.
        /// </summary>
        /// <value>The name of the group.</value>
        [Display(Name = nameof(Properties.Resources.Name), ResourceType = typeof(Properties.Resources))]
        string Name { get; }

        /// <summary>
        /// Gets roles for the current group.
        /// </summary>
        /// <value>The <see cref="UserRole">roles</see> that are assigned to all members of the group.</value>
        [Display(Name = nameof(Properties.Resources.Roles), ResourceType = typeof(Properties.Resources))]
        UserRole Roles { get; }

        /// <summary>
        /// Gets notes for the current group.
        /// </summary>
        /// <value>The custom notes to be associated with the current group.</value>
        [Display(Name = nameof(Properties.Resources.Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>
        /// Indicates whether the current group record is inactive.
        /// </summary>
        /// <value><see langword="true"/> if the current group record is inactive (archived); otherwise, <see langword="false"/> if it is active.</value>
        [Display(Name = nameof(Properties.Resources.IsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }
    }
}
