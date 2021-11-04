using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public enum AsyncJobStatus
    {
        [Display(Name = nameof(Properties.Resources.DisplayName_Pending), ShortName = nameof(Properties.Resources.DisplayName_Pending), Description = nameof(Properties.Resources.DisplayName_Pending), ResourceType = typeof(Properties.Resources))]
        WaitingToRun,

        [Display(Name = nameof(Properties.Resources.DisplayName_Started), ShortName = nameof(Properties.Resources.DisplayName_Started), Description = nameof(Properties.Resources.DisplayName_Started), ResourceType = typeof(Properties.Resources))]
        Running,

        [Display(Name = nameof(Properties.Resources.DisplayName_Cancelling), ShortName = nameof(Properties.Resources.DisplayName_Cancelling), Description = nameof(Properties.Resources.DisplayName_Cancelling), ResourceType = typeof(Properties.Resources))]
        Cancelling,

        [Display(Name = nameof(Properties.Resources.DisplayName_Canceled), ShortName = nameof(Properties.Resources.DisplayName_Canceled), Description = nameof(Properties.Resources.DisplayName_Canceled), ResourceType = typeof(Properties.Resources))]
        Canceled,

        [Display(Name = nameof(Properties.Resources.DisplayName_Failed), ShortName = nameof(Properties.Resources.DisplayName_Failed), Description = nameof(Properties.Resources.DisplayName_Failed), ResourceType = typeof(Properties.Resources))]
        Faulted,

        [Display(Name = nameof(Properties.Resources.DisplayName_Completed), ShortName = nameof(Properties.Resources.DisplayName_Completed), Description = nameof(Properties.Resources.DisplayName_Completed), ResourceType = typeof(Properties.Resources))]
        Succeeded
    }
}
