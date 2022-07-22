using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    // TODO: Document IHostDeviceRow interface
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IHostDeviceRow : IUpstreamDbEntity
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Gets the display name of the host device.
        /// </summary>
        /// <value>The user-friendly display name of the host device.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName), ShortName = nameof(Properties.Resources.Display),
            Description = nameof(Properties.Resources.Description_DisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; }

        /// <summary>
        /// Gets the machine identifier.
        /// </summary>
        /// <value>The programmatic host machine identifier string.</value>
        [Display(Name = nameof(Properties.Resources.MachineIdentifer), ShortName = nameof(Properties.Resources.Identifier),
            Description = nameof(Properties.Resources.Description_MachineIdentifer), ResourceType = typeof(Properties.Resources))]
        string MachineIdentifer { get; }

        /// <summary>
        /// Gets the machine name.
        /// </summary>
        /// <value>The machine's host name or IP address.</value>
        [Display(Name = nameof(Properties.Resources.MachineName), ShortName = nameof(Properties.Resources.Name),
            Description = nameof(Properties.Resources.Description_MachineName), ResourceType = typeof(Properties.Resources))]
        string MachineName { get; }

        /// <summary>
        /// Gets the notest for the current host device.
        /// </summary>
        /// <value>The custom notes to associate with the current host device.</value>
        [Display(Name = nameof(Properties.Resources.Notes), ShortName = nameof(Properties.Resources.Notes),
            Description = nameof(Properties.Resources.Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>
        /// Indicates whether the current host device record is inactive.
        /// </summary>
        /// <value><see langword="true"/> if the current host device record is inactive (archived); otherwise, <see langword="false"/> if it is active.</value>
        [Display(Name = nameof(Properties.Resources.IsInactive), ShortName = nameof(Properties.Resources.Inactive),
            Description = nameof(Properties.Resources.Description_HostIsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }
    }
}
