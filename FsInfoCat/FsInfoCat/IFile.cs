using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    /// <summary>
    /// Represents a file in its hierarchical structure.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    public interface IFile : IDbFsItem
    {
        FileCrawlOptions Options { get; set; }

        DateTime? LastHashCalculation { get; set; }

        bool Deleted { get; }

        IBinaryProperties BinaryProperties { get; set; }

        [Obsolete]
        IExtendedProperties ExtendedProperties { get; set; }

        ISummaryProperties SummaryProperties { get; set; }

        IDocumentProperties DocumentProperties { get; set; }

        IAudioProperties AudioProperties { get; set; }

        IDRMProperties DRMProperties { get; set; }

        IGPSProperties GPSProperties { get; set; }

        IImageProperties ImageProperties { get; set; }

        IMediaProperties MediaProperties { get; set; }

        IMusicProperties MusicProperties { get; set; }

        IPhotoProperties PhotoProperties { get; set; }

        IRecordedTVProperties RecordedTVProperties { get; set; }

        IVideoProperties VideoProperties { get; set; }

        IRedundancy Redundancy { get; }

        IEnumerable<IComparison> ComparisonSources { get; }

        IEnumerable<IComparison> ComparisonTargets { get; }

        new IEnumerable<IAccessError<IFile>> AccessErrors { get; }
    }
}
