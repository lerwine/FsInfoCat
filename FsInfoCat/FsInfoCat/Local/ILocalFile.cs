using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalFile : IFile, ILocalDbFsItem
    {
        new ILocalBinaryProperties BinaryProperties { get; set; }

        [System.Obsolete]
        new ILocalExtendedProperties ExtendedProperties { get; set; }

        new ILocalSummaryProperties SummaryProperties { get; set; }

        new ILocalDocumentProperties DocumentProperties { get; set; }

        new ILocalAudioProperties AudioProperties { get; set; }

        new ILocalDRMProperties DRMProperties { get; set; }

        new ILocalGPSProperties GPSProperties { get; set; }

        new ILocalImageProperties ImageProperties { get; set; }

        new ILocalMediaProperties MediaProperties { get; set; }

        new ILocalMusicProperties MusicProperties { get; set; }

        new ILocalPhotoProperties PhotoProperties { get; set; }

        new ILocalRecordedTVProperties RecordedTVProperties { get; set; }

        new ILocalVideoProperties VideoProperties { get; set; }

        new ILocalSubdirectory Parent { get; set; }

        new ILocalRedundancy Redundancy { get; }

        new IEnumerable<ILocalComparison> ComparisonSources { get; }

        new IEnumerable<ILocalComparison> ComparisonTargets { get; }

        new IEnumerable<IAccessError<ILocalFile>> AccessErrors { get; }
    }
}
