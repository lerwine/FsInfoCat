using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>Defines a user's membership into a group.</summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IGroupMembership : IUpstreamDbEntity
    {
        /// <summary>Indicates whether the user is a group administrator.</summary>
        /// <value><see langword="true"/> if the <see cref="User"/> can edit group membership or <see langword="false"/> if the <see cref="User"/> is a regular group member.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsGroupAdmin), ShortName= nameof(Properties.Resources.DisplayName_Admin), Description = nameof(Properties.Resources.Description_GroupMembership_IsGroupAdmin),
            ResourceType = typeof(Properties.Resources))]
        bool IsGroupAdmin { get; }

        /// <summary>Gets the target group.</summary>
        /// <value>The target <see cref="IUserGroup"/> that this specifies the member of.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Group), ShortName = nameof(Properties.Resources.DisplayName_Group), Description = nameof(Properties.Resources.Description_GroupMembership_Group),
            ResourceType = typeof(Properties.Resources))]
        IUserGroup Group { get; }

        /// <summary>Gets group member.</summary>
        /// <value>The <see cref="IUserProfile"/> of the group member.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_User), ShortName = nameof(Properties.Resources.DisplayName_User), Description = nameof(Properties.Resources.Description_GroupMembership_User),
            ResourceType = typeof(Properties.Resources))]
        IUserProfile User { get; }
    }
}
