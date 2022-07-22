using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Represents the priority level of a task.
    /// </summary>
    public enum PriorityLevel : byte
    {
        /// <summary>
        /// Task has been deferred to a later date.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Deferred), ShortName = nameof(Properties.Resources.Deferred),
            Description = nameof(Properties.Resources.TaskDeferred), ResourceType = typeof(Properties.Resources))]
        Deferred = 0,

        /// <summary>
        /// Task has the highest priority.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.CriticalUrgency), ShortName = nameof(Properties.Resources.Critical),
            Description = nameof(Properties.Resources.TaskCriticalUrgency), ResourceType = typeof(Properties.Resources))]
        Critical = 1,

        /// <summary>
        /// Task has elevated priority.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.HighPriority), ShortName = nameof(Properties.Resources.High),
            Description = nameof(Properties.Resources.TaskHighPriority), ResourceType = typeof(Properties.Resources))]
        High = 2,

        /// <summary>
        /// Task has normal priority.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.NormalPriority), ShortName = nameof(Properties.Resources.Normal),
            Description = nameof(Properties.Resources.TaskNormalPriority), ResourceType = typeof(Properties.Resources))]
        Normal = 3,

        /// <summary>
        /// Task has low priority.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.LowPriority), ShortName = nameof(Properties.Resources.Low),
            Description = nameof(Properties.Resources.TaskLowPriority), ResourceType = typeof(Properties.Resources))]
        Low = 4
    }
}
