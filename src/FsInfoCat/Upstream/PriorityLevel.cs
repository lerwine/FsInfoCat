using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>Represents the priority level of a task.</summary>
    public enum PriorityLevel : byte
    {
        /// <summary>Task has been deferred to a later date.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_Deferred), ShortName = nameof(Properties.Resources.DisplayName_Deferred), Description = nameof(Properties.Resources.Description_PriorityLevel_Deferred),
            ResourceType = typeof(Properties.Resources))]
        Deferred = 0,

        /// <summary>Task has the highest priority.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PriorityLevel_Critical), ShortName = nameof(Properties.Resources.DisplayName_Critical), Description = nameof(Properties.Resources.Description_PriorityLevel_Critical),
            ResourceType = typeof(Properties.Resources))]
        Critical = 1,

        /// <summary>Task has elevated priority.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PriorityLevel_High), ShortName = nameof(Properties.Resources.DisplayName_High), Description = nameof(Properties.Resources.Description_PriorityLevel_High),
            ResourceType = typeof(Properties.Resources))]
        High = 2,

        /// <summary>Task has normal priority.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PriorityLevel_Normal), ShortName = nameof(Properties.Resources.DisplayName_Normal), Description = nameof(Properties.Resources.Description_PriorityLevel_Normal),
            ResourceType = typeof(Properties.Resources))]
        Normal = 3,

        /// <summary>Task has low priority.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PriorityLevel_Low), ShortName = nameof(Properties.Resources.DisplayName_Low), Description = nameof(Properties.Resources.Description_PriorityLevel_Low),
            ResourceType = typeof(Properties.Resources))]
        Low = 4
    }
}
