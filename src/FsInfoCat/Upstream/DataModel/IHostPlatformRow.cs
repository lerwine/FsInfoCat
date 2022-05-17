using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    // TODO: Document IHostPlatformRow interface
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IHostPlatformRow : IUpstreamDbEntity
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Gets the display name of the platform.
        /// </summary>
        /// <value>The user-friendly display name of the platform.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayName), ShortName = nameof(Properties.Resources.DisplayName_Name),
            Description = nameof(Properties.Resources.Description_HostPlatform_DisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; }

        /// <summary>
        /// Gets the generalized host platform type.
        /// </summary>
        /// <value>The <see cref="PlatformType"/> that indicates the generalized platform type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_PlatformType), ShortName = nameof(Properties.Resources.DisplayName_Type),
            Description = nameof(Properties.Resources.Description_HostPlatform_PlatformType), ResourceType = typeof(Properties.Resources))]
        PlatformType Type { get; }

        /// <summary>
        /// Gets the notes for the platform.
        /// </summary>
        /// <value>The custom notes to associate with the current platform.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ShortName = nameof(Properties.Resources.DisplayName_Notes),
            Description = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>
        /// Indicates whether the current platform record is inactive.
        /// </summary>
        /// <value><see langword="true"/> if the current platform record is inactive (archived); otherwise, <see langword="false"/> if it is active.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsInactive), ShortName = nameof(Properties.Resources.DisplayName_Inactive),
            Description = nameof(Properties.Resources.Description_HostPlatform_IsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }
    }
}
