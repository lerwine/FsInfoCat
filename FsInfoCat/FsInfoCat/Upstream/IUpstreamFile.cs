using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamFile : IFile, IUpstreamDbFsItem
    {
        new IUpstreamBinaryProperties BinaryProperties { get; set; }

        new IUpstreamSummaryProperties SummaryProperties { get; set; }

        new IUpstreamDocumentProperties DocumentProperties { get; set; }

        new IUpstreamAudioProperties AudioProperties { get; set; }

        new IUpstreamDRMProperties DRMProperties { get; set; }

        new IUpstreamGPSProperties GPSProperties { get; set; }

        new IUpstreamImageProperties ImageProperties { get; set; }

        new IUpstreamMediaProperties MediaProperties { get; set; }

        new IUpstreamMusicProperties MusicProperties { get; set; }

        new IUpstreamPhotoProperties PhotoProperties { get; set; }

        new IUpstreamRecordedTVProperties RecordedTVProperties { get; set; }

        new IUpstreamVideoProperties VideoProperties { get; set; }

        new IUpstreamSubdirectory Parent { get; set; }

        new IUpstreamRedundancy Redundancy { get; }

        IEnumerable<IFileAction> FileActions { get; }

        new IEnumerable<IUpstreamComparison> ComparisonSources { get; }

        new IEnumerable<IUpstreamComparison> ComparisonTargets { get; }

        new IEnumerable<IAccessError<IUpstreamFile>> AccessErrors { get; }
    }
}
