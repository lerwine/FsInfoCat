using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamDbContext : IDbContext
    {
        new IEnumerable<IUpstreamComparison> Comparisons { get; }
        new IEnumerable<IUpstreamContentInfo> ContentInfos { get; }
        new IEnumerable<IUpstreamExtendedProperties> ExtendedProperties { get; }
        new IEnumerable<IAccessError<IUpstreamFile>> FileAccessErrors { get; }
        new IEnumerable<IUpstreamFile> Files { get; }
        new IEnumerable<IUpstreamFileSystem> FileSystems { get; }
        new IEnumerable<IUpstreamRedundancy> Redundancies { get; }
        new IEnumerable<IUpstreamRedundantSet> RedundantSets { get; }
        new IEnumerable<IUpstreamSubdirectory> Subdirectories { get; }
        new IEnumerable<IAccessError<IUpstreamSubdirectory>> SubdirectoryAccessErrors { get; }
        new IEnumerable<IUpstreamSymbolicName> SymbolicNames { get; }
        new IEnumerable<IAccessError<IUpstreamVolume>> VolumeAccessErrors { get; }
        new IEnumerable<IVolume> Volumes { get; }
        new IEnumerable<IHostCrawlConfiguration> CrawlConfigurations { get; }
        IEnumerable<IFileAction> FileActions { get; }
        IEnumerable<IGroupMembership> GroupMemberships { get; }
        IEnumerable<IHostDevice> HostDevices { get; }
        IEnumerable<IHostPlatform> HostPlatforms { get; }
        IEnumerable<IMitigationTask> MitigationTasks { get; }
        IEnumerable<ISubdirectoryAction> SubdirectoryActions { get; }
        IEnumerable<IUserGroup> UserGroups { get; }
        IEnumerable<IUserProfile> UserProfiles { get; }
        void ForceDeleteContentInfo(IUpstreamContentInfo target);
        void ForceDeleteRedundantSet(IUpstreamRedundantSet target);
    }
}
