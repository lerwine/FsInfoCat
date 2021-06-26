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

        FileCorrelationStatus Status { get; set; }

        DateTime? LastHashCalculation { get; set; }

        IBinaryPropertySet BinaryProperties { get; set; }

        ISummaryPropertySet SummaryProperties { get; set; }

        IDocumentPropertySet DocumentProperties { get; set; }

        IAudioPropertySet AudioProperties { get; set; }

        IDRMPropertySet DRMProperties { get; set; }

        IGPSPropertySet GPSProperties { get; set; }

        IImagePropertySet ImageProperties { get; set; }

        IMediaPropertySet MediaProperties { get; set; }

        IMusicPropertySet MusicProperties { get; set; }

        IPhotoPropertySet PhotoProperties { get; set; }

        IRecordedTVPropertySet RecordedTVProperties { get; set; }

        IVideoPropertySet VideoProperties { get; set; }

        IRedundancy Redundancy { get; }

        IEnumerable<IComparison> ComparisonSources { get; }

        IEnumerable<IComparison> ComparisonTargets { get; }

        new IEnumerable<IAccessError<IFile>> AccessErrors { get; }
    }
}
