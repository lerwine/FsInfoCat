using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IGroupMembership : IUpstreamDbEntity
    {
        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsGroupAdmin), ResourceType = typeof(Properties.Resources))]
        bool IsGroupAdmin { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Group), ResourceType = typeof(Properties.Resources))]
        IUserGroup Group { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_User), ResourceType = typeof(Properties.Resources))]
        IUserProfile User { get; }
    }
}
