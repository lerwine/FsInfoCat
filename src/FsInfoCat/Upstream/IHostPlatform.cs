using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IHostPlatform : IUpstreamDbEntity
    {
        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Type), ResourceType = typeof(Properties.Resources))]
        PlatformType Type { get; }

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
        [Display(Name = nameof(Properties.Resources.DisplayName_DefaultFsType), ResourceType = typeof(Properties.Resources))]
        IUpstreamFileSystem DefaultFsType { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_HostDevices), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IHostDevice> HostDevices { get; }
    }
}
