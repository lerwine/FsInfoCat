using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Represent the status of a task.
    /// </summary>
    public enum TaskStatus : byte
    {
        /// <summary>
        /// Task has not been acknowledged.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.New), ShortName = nameof(Properties.Resources.New),
            Description = nameof(Properties.Resources.TaskNew), ResourceType = typeof(Properties.Resources))]
        New = 0,

        /// <summary>
        /// Task has been acknowledged, but not yet started.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Open), ShortName = nameof(Properties.Resources.Open),
            Description = nameof(Properties.Resources.TaskOpen), ResourceType = typeof(Properties.Resources))]
        Open = 1,

        /// <summary>
        /// Task is currently being addressed.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Active), ShortName = nameof(Properties.Resources.Active),
            Description = nameof(Properties.Resources.Description_Active), ResourceType = typeof(Properties.Resources))]
        Active = 2,

        /// <summary>
        /// Task is in a paused state, pending some other factor.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Pending), ShortName = nameof(Properties.Resources.Pending),
            Description = nameof(Properties.Resources.Description_Pending), ResourceType = typeof(Properties.Resources))]
        Pending = 3,

        /// <summary>
        /// Task has been canceled.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Canceled), ShortName = nameof(Properties.Resources.Canceled),
            Description = nameof(Properties.Resources.TaskCanceled), ResourceType = typeof(Properties.Resources))]
        Canceled = 4,

        /// <summary>
        /// Task has been completed.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Completed), ShortName = nameof(Properties.Resources.Completed),
            Description = nameof(Properties.Resources.Description_Completed), ResourceType = typeof(Properties.Resources))]
        Completed = 5
    }
}
