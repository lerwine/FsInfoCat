using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IHostDevice : IUpstreamDbEntity
    {
        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MachineIdentifer), ResourceType = typeof(Properties.Resources))]
        string MachineIdentifer { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MachineName), ResourceType = typeof(Properties.Resources))]
        string MachineName { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Platform), ResourceType = typeof(Properties.Resources))]
        IHostPlatform Platform { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Volumes), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IUpstreamVolume> Volumes { get; }
    }
}
