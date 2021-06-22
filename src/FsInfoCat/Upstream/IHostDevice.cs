using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IHostDevice : IUpstreamDbEntity
    {
        string DisplayName { get; }

        string MachineIdentifer { get; }

        string MachineName { get; }

        IHostPlatform Platform { get; }

        string Notes { get; }

        bool IsInactive { get; }

        IEnumerable<IUpstreamVolume> Volumes { get; }

        IEnumerable<IHostCrawlConfiguration> CrawlConfigurations { get; }
    }
}
