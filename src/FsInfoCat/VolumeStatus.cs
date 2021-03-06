using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public enum VolumeStatus : byte
    {
        /// <summary>
        /// The status of the volume is uknown or unspecified.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_VolumeStatus_Unknown), Description = nameof(Properties.Resources.Description_VolumeStatus_Unknown), ResourceType = typeof(Properties.Resources))]
        Unknown = 0,

        /// <summary>
        /// Volume is under corporate control / ownership.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_VolumeStatus_Controlled), Description = nameof(Properties.Resources.Description_VolumeStatus_Controlled), ResourceType = typeof(Properties.Resources))]
        Controlled = 1,

        /// <summary>
        /// An error occurred while trying to access the volume.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_VolumeStatus_AccessError), Description = nameof(Properties.Resources.Description_VolumeStatus_AccessError), ResourceType = typeof(Properties.Resources))]
        AccessError = 2,

        /// <summary>
        /// Volume is being tracked, but is not under corporate control / ownership.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_VolumeStatus_Uncontrolled), Description = nameof(Properties.Resources.Description_VolumeStatus_Uncontrolled), ResourceType = typeof(Properties.Resources))]
        Uncontrolled = 3,

        /// <summary>
        /// Volume is offline or unavailable.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_VolumeStatus_Offline), Description = nameof(Properties.Resources.Description_VolumeStatus_Offline), ResourceType = typeof(Properties.Resources))]
        Offline = 4,

        /// <summary>
        /// Ownership / control of volume has been relinquished to another entity.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_VolumeStatus_Relinquished), Description = nameof(Properties.Resources.Description_VolumeStatus_Relinquished), ResourceType = typeof(Properties.Resources))]
        Relinquished = 5,

        /// <summary>
        /// Volume has been destroyed.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_VolumeStatus_Destroyed), Description = nameof(Properties.Resources.Description_VolumeStatus_Destroyed), ResourceType = typeof(Properties.Resources))]
        Destroyed = 6
    }
}
