using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IUserGroup : IUpstreamDbEntity
    {
        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Name), ResourceType = typeof(Properties.Resources))]
        string Name { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Roles), ResourceType = typeof(Properties.Resources))]
        UserRole Roles { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Members), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IGroupMembership> Members { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Tasks), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IMitigationTask> Tasks { get; }
    }
}
