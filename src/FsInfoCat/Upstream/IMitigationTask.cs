using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>Compliance/Redundancy mitigation task.</summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IMitigationTask : IUpstreamDbEntity
    {
        /// <summary>Gets the task's short description.</summary>
        /// <value>The short description of task.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ShortDescription), ResourceType = typeof(Properties.Resources))]
        string ShortDescription { get; }

        /// <summary>Gets the task notes.</summary>
        /// <value>The custom notes for the task.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>Gets the task status.</summary>
        /// <value>The status value for the current task.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Status), ResourceType = typeof(Properties.Resources))]
        TaskStatus Status { get; }

        /// <summary>Gets the task priority.</summary>
        /// <value>The priority value for the current task.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Priority), ResourceType = typeof(Properties.Resources))]
        PriorityLevel Priority { get; }

        /// <summary>Gets the task's assignment group.</summary>
        /// <value>The <see cref="IUserGroup"/> responsible for task mitigation.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AssignmentGroup), ResourceType = typeof(Properties.Resources))]
        IUserGroup AssignmentGroup { get; }

        /// <summary>Gets the user that the task is assigned to.</summary>
        /// <value>The <see cref="IUserProfile"/> of the person accountable for task mitigation.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AssignedTo), ResourceType = typeof(Properties.Resources))]
        IUserProfile AssignedTo { get; }

        /// <summary>Gets the actions to be taken on files.</summary>
        /// <value>The list of actions to be carried out on specific files.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileActions), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IFileAction> FileActions { get; }

        /// <summary>Gets the actions to be taken on subdirectories.</summary>
        /// <value>The list of actions to be carried out on entire subdirectories.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SubdirectoryActions), ResourceType = typeof(Properties.Resources))]
        IEnumerable<ISubdirectoryAction> SubdirectoryActions { get; }
    }
}
