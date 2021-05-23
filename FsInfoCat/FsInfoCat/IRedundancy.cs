using System;

namespace FsInfoCat
{
    public interface IRedundancy : IDbEntity
    {
        Guid FileId { get; set; }

        Guid RedundantSetId { get; set; }

        string Reference { get; set; }

        FileRedundancyStatus Status { get; set; }

        string Notes { get; set; }

        IFile File { get; set; }

        IRedundantSet RedundantSet { get; set; }
    }
}
