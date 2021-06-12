using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public interface ILocalDbContext : IDbContext
    {
        new IEnumerable<ILocalComparison> Comparisons { get; }
        new IEnumerable<ILocalContentInfo> ContentInfos { get; }
        new IEnumerable<ILocalExtendedProperties> ExtendedProperties { get; }
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
        void ForceDeleteContentInfo(ILocalContentInfo target);
        void ForceDeleteRedundantSet(ILocalRedundantSet target);
        void ForceDeleteFileSystem(ILocalFileSystem target);
        Task ImportAsync(XDocument document);
    }
}
