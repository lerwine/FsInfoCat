using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IMitigationTask : IUpstreamDbEntity
    {
        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ShortDescription), ResourceType = typeof(Properties.Resources))]
        string ShortDescription { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Status), ResourceType = typeof(Properties.Resources))]
        TaskStatus Status { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Priority), ResourceType = typeof(Properties.Resources))]
        PriorityLevel Priority { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AssignmentGroup), ResourceType = typeof(Properties.Resources))]
        IUserGroup AssignmentGroup { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AssignedTo), ResourceType = typeof(Properties.Resources))]
        IUserProfile AssignedTo { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileActions), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IFileAction> FileActions { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SubdirectoryActions), ResourceType = typeof(Properties.Resources))]
        IEnumerable<ISubdirectoryAction> SubdirectoryActions { get; }
    }
}
