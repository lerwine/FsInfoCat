using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Represents a host platform type.
    /// </summary>
    public enum PlatformType : byte
    {
        /// <summary>
        /// Host platform type is unknown.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.UnknownPlatformType), ShortName = nameof(Properties.Resources.Unknown),
            Description = nameof(Properties.Resources.Description_UnknownPlatformType), ResourceType = typeof(Properties.Resources))]
        Unknown = 0,

        /// <summary>
        /// Host machine runs on a Windows-based operating system.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Windows), ShortName = nameof(Properties.Resources.Windows),
            Description = nameof(Properties.Resources.WindowsPlatformType), ResourceType = typeof(Properties.Resources))]
        Windows = 1,

        /// <summary>
        /// Host machine runs on a Linux-based operating system.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Linux), ShortName = nameof(Properties.Resources.Linux),
            Description = nameof(Properties.Resources.LinuxPlatformType), ResourceType = typeof(Properties.Resources))]
        Linux = 2,

        /// <summary>
        /// Host machine runs on a OSX-based operating system.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.OSX), ShortName = nameof(Properties.Resources.OSX),
            Description = nameof(Properties.Resources.OSXPlatformType), ResourceType = typeof(Properties.Resources))]
        OSX = 3,

        /// <summary>
        /// Host is an android device.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Android), ShortName = nameof(Properties.Resources.Android),
            Description = nameof(Properties.Resources.AndroidPlatformType), ResourceType = typeof(Properties.Resources))]
        Android = 4,

        /// <summary>
        /// Host is an IOS device.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.IOS), ShortName = nameof(Properties.Resources.IOS),
            Description = nameof(Properties.Resources.IOSPlatformType), ResourceType = typeof(Properties.Resources))]
        IOS = 5
    }
}
