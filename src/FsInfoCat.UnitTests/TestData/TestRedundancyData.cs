using System;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestRedundancyData
    {
        public Guid UpstreamId { get; init; }

        public DateTime LastSynchronizedOn { get; init; }

        public string Reference { get; init; }

        public string Notes { get; init; }

        public Guid FileId { get; init; }

        public Guid RedundantSetId { get; init; }

        public DateTime CreatedOn { get; init; }

        public DateTime ModifiedOn { get; init; }
    }
}
