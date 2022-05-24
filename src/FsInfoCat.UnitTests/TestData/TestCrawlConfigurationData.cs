using System;
using System.Collections.ObjectModel;
using FsInfoCat.Model;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestCrawlConfigurationData
    {
        public Guid UpstreamId { get; init; }

        public DateTime LastSynchronizedOn { get; init; }

        public string DisplayName { get; init; }

        public string Notes { get; init; }

        public CrawlStatus StatusValue { get; init; }

        public DateTime LastCrawlStart { get; init; }

        public DateTime LastCrawlEnd { get; init; }

        public DateTime NextScheduledStart { get; init; }

        public long RescheduleInterval { get; init; }

        public bool RescheduleFromJobEnd { get; init; }

        public bool RescheduleAfterFail { get; init; }

        public Guid RootId { get; init; }

        public DateTime CreatedOn { get; init; }

        public DateTime ModifiedOn { get; init; }

        public ushort MaxRecursionDepth { get; init; }

        public ulong MaxTotalItems { get; init; }

        public long TTL { get; init; }

        public Guid Id { get; init; }
    }
}
