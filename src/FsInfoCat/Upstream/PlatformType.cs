using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>Represents a host platform type.</summary>
    public enum PlatformType : byte
    {
        /// <summary>Host platform type is unknown.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PlatformType_Unknown), ShortName = nameof(Properties.Resources.DisplayName_Unknown), Description = nameof(Properties.Resources.Description_PlatformType_Unknown), ResourceType = typeof(Properties.Resources))]
        Unknown = 0,

        /// <summary>Host machine runs on a Windows-based operating system.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PlatformType_Windows), ShortName = nameof(Properties.Resources.DisplayName_PlatformType_Windows), Description = nameof(Properties.Resources.Description_PlatformType_Windows), ResourceType = typeof(Properties.Resources))]
        Windows = 1,

        /// <summary>Host machine runs on a Linux-based operating system.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PlatformType_Linux), ShortName = nameof(Properties.Resources.DisplayName_PlatformType_Linux), Description = nameof(Properties.Resources.Description_PlatformType_Linux), ResourceType = typeof(Properties.Resources))]
        Linux = 2,

        /// <summary>Host machine runs on a OSX-based operating system.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PlatformType_OSX), ShortName = nameof(Properties.Resources.DisplayName_PlatformType_OSX), Description = nameof(Properties.Resources.Description_PlatformType_OSX), ResourceType = typeof(Properties.Resources))]
        OSX = 3,

        /// <summary>Host is an android device.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PlatformType_Android), ShortName = nameof(Properties.Resources.DisplayName_PlatformType_Android), Description = nameof(Properties.Resources.Description_PlatformType_Android), ResourceType = typeof(Properties.Resources))]
        Android = 4,

        /// <summary>Host is an IOS device.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PlatformType_IOS), ShortName = nameof(Properties.Resources.DisplayName_PlatformType_IOS), Description = nameof(Properties.Resources.Description_PlatformType_IOS), ResourceType = typeof(Properties.Resources))]
        IOS = 5
    }
}
