using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Represents a volume status.
    /// </summary>
    public enum VolumeStatus : byte
    {
        /// <summary>
        /// The status of the volume is uknown or unspecified.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Unknown), ShortName = nameof(Properties.Resources.Unknown), Description = nameof(Properties.Resources.VolumeStatusUnknown),
            ResourceType = typeof(Properties.Resources))]
        Unknown = 0,

        /// <summary>
        /// Volume is under corporate control / ownership.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Controlled), ShortName = nameof(Properties.Resources.Controlled),
            Description = nameof(Properties.Resources.VolumeControlled), ResourceType = typeof(Properties.Resources))]
        Controlled = 1,

        /// <summary>
        /// An error occurred while trying to access the volume.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.AccessError), ShortName = nameof(Properties.Resources.Error), Description = nameof(Properties.Resources.VolumeAccessError),
            ResourceType = typeof(Properties.Resources))]
        AccessError = 2,

        /// <summary>
        /// Volume is being tracked, but is not under corporate control / ownership.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Uncontrolled), ShortName = nameof(Properties.Resources.Uncontrolled),
            Description = nameof(Properties.Resources.VolumeUncontrolled), ResourceType = typeof(Properties.Resources))]
        Uncontrolled = 3,

        /// <summary>
        /// Volume is offline or unavailable.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Offline), ShortName = nameof(Properties.Resources.Offline),
            Description = nameof(Properties.Resources.VolumeOffline), ResourceType = typeof(Properties.Resources))]
        Offline = 4,

        /// <summary>
        /// Ownership / control of volume has been relinquished to another entity.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Relinquished), ShortName = nameof(Properties.Resources.Relinquished),
            Description = nameof(Properties.Resources.VolumeRelinquished), ResourceType = typeof(Properties.Resources))]
        Relinquished = 5,

        /// <summary>
        /// Volume has been destroyed.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Destroyed), ShortName = nameof(Properties.Resources.Destroyed),
            Description = nameof(Properties.Resources.VolumeDestroyed), ResourceType = typeof(Properties.Resources))]
        Destroyed = 6,

        /// <summary>
        /// Volume entity marked as deleted.
        /// </summary>
        Deleted = 7
    }
}

