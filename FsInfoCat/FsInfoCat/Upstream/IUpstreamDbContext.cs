using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamDbContext : IDbContext
    {
        new IEnumerable<IUpstreamComparison> Comparisons { get; }

        new IEnumerable<IUpstreamBinaryPropertySet> BinaryPropertySets { get; }

        [System.Obsolete()]
        new IEnumerable<IUpstreamExtendedPropertySet> ExtendedProperties { get; }

        new IEnumerable<IUpstreamSummaryPropertySet> SummaryPropertySets { get; }

        new IEnumerable<IUpstreamDocumentPropertySet> DocumentPropertySets { get; }

        new IEnumerable<IUpstreamAudioPropertySet> AudioPropertySets { get; }

        new IEnumerable<IUpstreamDRMPropertySet> DRMPropertySets { get; }

        new IEnumerable<IUpstreamGPSPropertySet> GPSPropertySets { get; }

        new IEnumerable<IUpstreamImagePropertySet> ImagePropertySets { get; }

        new IEnumerable<IUpstreamMediaPropertySet> MediaPropertySets { get; }

        new IEnumerable<IUpstreamMusicPropertySet> MusicPropertySets { get; }

        new IEnumerable<IUpstreamPhotoPropertySet> PhotoPropertySets { get; }

        new IEnumerable<IUpstreamRecordedTVPropertySet> RecordedTVPropertySets { get; }

        new IEnumerable<IUpstreamVideoPropertySet> VideoPropertySets { get; }

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

        void ForceDeleteBinaryProperties(IUpstreamBinaryPropertySet target);

        void ForceDeleteRedundantSet(IUpstreamRedundantSet target);
    }
}
