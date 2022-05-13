using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    public interface IMitigationTaskRow : IUpstreamDbEntity
    {
        /// <summary>
        /// Gets the task's short description.
        /// </summary>
        /// <value>The short description of task.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ShortDescription), ResourceType = typeof(Properties.Resources))]
        string ShortDescription { get; }

        /// <summary>
        /// Gets the task notes.
        /// </summary>
        /// <value>The custom notes for the task.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>
        /// Gets the task status.
        /// </summary>
        /// <value>The status value for the current task.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Status), ResourceType = typeof(Properties.Resources))]
        TaskStatus Status { get; }

        /// <summary>
        /// Gets the task priority.
        /// </summary>
        /// <value>The priority value for the current task.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Priority), ResourceType = typeof(Properties.Resources))]
        PriorityLevel Priority { get; }
    }
}
