using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public interface ILocalDbContext : IDbContext
    {
        new IEnumerable<ILocalComparison> Comparisons { get; }

        new IEnumerable<ILocalBinaryPropertySet> BinaryPropertySets { get; }

        new IEnumerable<ILocalSummaryProperties> SummaryPropertySets { get; }

        new IEnumerable<ILocalDocumentPropertySet> DocumentPropertySets { get; }

        new IEnumerable<ILocalAudioPropertySet> AudioPropertySets { get; }

        new IEnumerable<ILocalDRMPropertySet> DRMPropertySets { get; }

        new IEnumerable<ILocalGPSPropertySet> GPSPropertySets { get; }

        new IEnumerable<ILocalImagePropertySet> ImagePropertySets { get; }

        new IEnumerable<ILocalMediaPropertySet> MediaPropertySets { get; }

        new IEnumerable<ILocalMusicPropertySet> MusicPropertySets { get; }

        new IEnumerable<ILocalPhotoPropertySet> PhotoPropertySets { get; }

        new IEnumerable<ILocalRecordedTVPropertySet> RecordedTVPropertySets { get; }

        new IEnumerable<ILocalVideoPropertySet> VideoPropertySets { get; }

        new IEnumerable<IAccessError<ILocalFile>> FileAccessErrors { get; }

        new IEnumerable<ILocalFile> Files { get; }

        new IEnumerable<ILocalFileSystem> FileSystems { get; }

        new IEnumerable<ILocalRedundancy> Redundancies { get; }

        new IEnumerable<ILocalRedundantSet> RedundantSets { get; }

        new IEnumerable<ILocalSubdirectory> Subdirectories { get; }

        new IEnumerable<IAccessError<ILocalSubdirectory>> SubdirectoryAccessErrors { get; }

        new IEnumerable<ILocalSymbolicName> SymbolicNames { get; }

        new IEnumerable<IAccessError<ILocalVolume>> VolumeAccessErrors { get; }

        new IEnumerable<ILocalVolume> Volumes { get; }

        new IEnumerable<ILocalCrawlConfiguration> CrawlConfigurations { get; }

        void ForceDeleteBinaryProperties(ILocalBinaryPropertySet target);

        void ForceDeleteRedundantSet(ILocalRedundantSet target);

        void ForceDeleteFileSystem(ILocalFileSystem target);

        Task ImportAsync(XDocument document);
    }
}
