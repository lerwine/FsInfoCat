using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamFile : IFile, IUpstreamDbFsItem
    {
        new IUpstreamBinaryPropertySet BinaryProperties { get; set; }

        new IUpstreamSummaryPropertySet SummaryProperties { get; set; }

        new IUpstreamDocumentPropertySet DocumentProperties { get; set; }

        new IUpstreamAudioPropertySet AudioProperties { get; set; }

        new IUpstreamDRMPropertySet DRMProperties { get; set; }

        new IUpstreamGPSPropertySet GPSProperties { get; set; }

        new IUpstreamImagePropertySet ImageProperties { get; set; }

        new IUpstreamMediaPropertySet MediaProperties { get; set; }

        new IUpstreamMusicPropertySet MusicProperties { get; set; }

        new IUpstreamPhotoPropertySet PhotoProperties { get; set; }

        new IUpstreamRecordedTVPropertySet RecordedTVProperties { get; set; }

        new IUpstreamVideoPropertySet VideoProperties { get; set; }

        new IUpstreamSubdirectory Parent { get; set; }

        new IUpstreamRedundancy Redundancy { get; }

        IEnumerable<IFileAction> FileActions { get; }

        new IEnumerable<IUpstreamComparison> ComparisonSources { get; }

        new IEnumerable<IUpstreamComparison> ComparisonTargets { get; }

        new IEnumerable<IAccessError<IUpstreamFile>> AccessErrors { get; }
    }
}
