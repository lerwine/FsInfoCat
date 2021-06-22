using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalFile : IFile, ILocalDbFsItem
    {
        new ILocalBinaryPropertySet BinaryProperties { get; set; }

        new ILocalSummaryProperties SummaryProperties { get; set; }

        new ILocalDocumentPropertySet DocumentProperties { get; set; }

        new ILocalAudioPropertySet AudioProperties { get; set; }

        new ILocalDRMPropertySet DRMProperties { get; set; }

        new ILocalGPSPropertySet GPSProperties { get; set; }

        new ILocalImagePropertySet ImageProperties { get; set; }

        new ILocalMediaPropertySet MediaProperties { get; set; }

        new ILocalMusicPropertySet MusicProperties { get; set; }

        new ILocalPhotoPropertySet PhotoProperties { get; set; }

        new ILocalRecordedTVPropertySet RecordedTVProperties { get; set; }

        new ILocalVideoPropertySet VideoProperties { get; set; }

        new ILocalSubdirectory Parent { get; set; }

        new ILocalRedundancy Redundancy { get; }

        new IEnumerable<ILocalComparison> ComparisonSources { get; }

        new IEnumerable<ILocalComparison> ComparisonTargets { get; }

        new IEnumerable<IAccessError<ILocalFile>> AccessErrors { get; }
    }
}
