using System;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestRedundantSetData
    {
        public Guid UpstreamId { get; init; }

        public DateTime LastSynchronizedOn { get; init; }

        public string Reference { get; init; }

        public string Notes { get; init; }

        public RedundancyRemediationStatus Status { get; init; }

        public TestBinaryPropertySetData BinaryProperties { get; init; }

        public Guid BinaryPropertiesId => BinaryProperties.Id;

        public DateTime CreatedOn { get; init; }

        public DateTime ModifiedOn { get; init; }

        public Guid Id { get; init; }
    }
}
