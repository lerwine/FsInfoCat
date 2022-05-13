using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for an entity that associated an <see cref="IUserGroup"/> with an <see cref="IUserProfile"/> entity.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IGroupMembershipRow : IUpstreamDbEntity
    {
        /// <summary>
        /// Indicates whether the user is a group administrator.
        /// </summary>
        /// <value><see langword="true"/> if the <see cref="User"/> can edit group membership or <see langword="false"/> if the <see cref="User"/> is a regular group member.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsGroupAdmin), ShortName = nameof(Properties.Resources.DisplayName_Admin), Description = nameof(Properties.Resources.Description_GroupMembership_IsGroupAdmin),
            ResourceType = typeof(Properties.Resources))]
        bool IsGroupAdmin { get; }
    }
}
