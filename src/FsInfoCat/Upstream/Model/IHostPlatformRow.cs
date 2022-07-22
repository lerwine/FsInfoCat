using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
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
        [Display(Name = nameof(Properties.Resources.DisplayName), ShortName = nameof(Properties.Resources.Name),
            Description = nameof(Properties.Resources.UserFriendlyDisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; }

        /// <summary>
        /// Gets the generalized host platform type.
        /// </summary>
        /// <value>The <see cref="PlatformType"/> that indicates the generalized platform type.</value>
        [Display(Name = nameof(Properties.Resources.PlatformType), ShortName = nameof(Properties.Resources.Type),
            Description = nameof(Properties.Resources.Description_PlatformType), ResourceType = typeof(Properties.Resources))]
        PlatformType Type { get; }

        /// <summary>
        /// Gets the notes for the platform.
        /// </summary>
        /// <value>The custom notes to associate with the current platform.</value>
        [Display(Name = nameof(Properties.Resources.Notes), ShortName = nameof(Properties.Resources.Notes),
            Description = nameof(Properties.Resources.Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>
        /// Indicates whether the current platform record is inactive.
        /// </summary>
        /// <value><see langword="true"/> if the current platform record is inactive (archived); otherwise, <see langword="false"/> if it is active.</value>
        [Display(Name = nameof(Properties.Resources.IsInactive), ShortName = nameof(Properties.Resources.Inactive),
            Description = nameof(Properties.Resources.Description_HostIsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }
    }
}
