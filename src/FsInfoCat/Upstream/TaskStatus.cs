using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>Represent the status of a task.</summary>
    public enum TaskStatus : byte
    {
        /// <summary>Task has not been acknowledged.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_TaskStatus_New), ShortName = nameof(Properties.Resources.DisplayName_TaskStatus_New), Description = nameof(Properties.Resources.Description_TaskStatus_New),
            ResourceType = typeof(Properties.Resources))]
        New = 0,

        /// <summary>Task has been acknowledged, but not yet started.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_TaskStatus_Open), ShortName = nameof(Properties.Resources.DisplayName_TaskStatus_Open), Description = nameof(Properties.Resources.Description_TaskStatus_Open),
            ResourceType = typeof(Properties.Resources))]
        Open = 1,

        /// <summary>Task is currently being addressed.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_TaskStatus_Active), ShortName = nameof(Properties.Resources.DisplayName_TaskStatus_Active), Description = nameof(Properties.Resources.Description_TaskStatus_Active),
            ResourceType = typeof(Properties.Resources))]
        Active = 2,

        /// <summary>Task is in a paused state, pending some other factor.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_Pending), ShortName = nameof(Properties.Resources.DisplayName_Pending), Description = nameof(Properties.Resources.Description_TaskStatus_Pending),
            ResourceType = typeof(Properties.Resources))]
        Pending = 3,

        /// <summary>Task has been canceled.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_Canceled), ShortName = nameof(Properties.Resources.DisplayName_Canceled), Description = nameof(Properties.Resources.Description_TaskStatus_Canceled),
            ResourceType = typeof(Properties.Resources))]
        Canceled = 4,

        /// <summary>Task has been completed.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_Completed), ShortName = nameof(Properties.Resources.DisplayName_Completed), Description = nameof(Properties.Resources.Description_TaskStatus_Completed),
            ResourceType = typeof(Properties.Resources))]
        Completed = 5
    }
}
