using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public interface ILocalDbContext : IDbContext
    {
        new IEnumerable<ILocalComparison> Comparisons { get; }

        new IEnumerable<ILocalBinaryProperties> BinaryProperties { get; }

        [System.Obsolete]
        new IEnumerable<ILocalExtendedProperties> ExtendedProperties { get; }

        new IEnumerable<ILocalSummaryProperties> SummaryProperties { get; }

        new IEnumerable<ILocalDocumentProperties> DocumentProperties { get; }

        new IEnumerable<ILocalAudioProperties> AudioProperties { get; }

        new IEnumerable<ILocalDRMProperties> DRMProperties { get; }

        new IEnumerable<ILocalGPSProperties> GPSProperties { get; }

        new IEnumerable<ILocalImageProperties> ImageProperties { get; }

        new IEnumerable<ILocalMediaProperties> MediaProperties { get; }

        new IEnumerable<ILocalMusicProperties> MusicProperties { get; }

        new IEnumerable<ILocalPhotoProperties> PhotoProperties { get; }

        new IEnumerable<ILocalRecordedTVProperties> RecordedTVProperties { get; }

        new IEnumerable<ILocalVideoProperties> VideoProperties { get; }

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

        void ForceDeleteBinaryProperties(ILocalBinaryProperties target);

        void ForceDeleteRedundantSet(ILocalRedundantSet target);

        void ForceDeleteFileSystem(ILocalFileSystem target);

        Task ImportAsync(XDocument document);
    }
}
