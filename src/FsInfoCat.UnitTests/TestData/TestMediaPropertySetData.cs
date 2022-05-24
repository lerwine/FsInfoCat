using System;
using System.Collections.ObjectModel;
using FsInfoCat.Collections;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestMediaPropertySetData
    {
        public Guid UpstreamId { get; init; }

        public DateTime LastSynchronizedOn { get; init; }

        public DateTime CreatedOn { get; init; }
        public DateTime ModifiedOn { get; init; }

        public Guid Id { get; init; }

        public string ContentDistributor { get; init; }

        public string CreatorApplication { get; init; }

        public string CreatorApplicationVersion { get; init; }

        public string DateReleased { get; init; }

        public ulong Duration { get; init; }

        public string DVDID { get; init; }

        public uint FrameCount { get; init; }

        public MultiStringValue Producer { get; init; }

        public string ProtectionType { get; init; }

        public string ProviderRating { get; init; }

        public string ProviderStyle { get; init; }

        public string Publisher { get; init; }

        public string Subtitle { get; init; }

        public MultiStringValue Writer { get; init; }

        public uint Year { get; init; }

        public static readonly TestMediaPropertySetData Item1 = new()
        {
            Id = new("cddede4e-c2bd-461c-9d1b-d7bfef5c0f7c"),
            UpstreamId = new("316395d6-7805-46fd-8dc7-033d0f365331"),
            CreatedOn = new(637884516391574428L), // 2022-05-18T06:20:39.1574428
            ModifiedOn = new(637884516391574428L), // 2022-05-18T06:20:39.1574428
            LastSynchronizedOn = new(637884516391574428L) // 2022-05-18T06:20:39.1574428
        };

        public static readonly TestMediaPropertySetData Item2 = new()
        {
            Id = new("32f9bf10-ebbb-45f2-8ba3-4d1fad21766b"),
            UpstreamId = new("93be15b3-7feb-4265-9eb6-04afbc758e78"),
            CreatedOn = new(637886372446114428L), // 2022-05-20T09:54:04.6114428
            ModifiedOn = new(637886372446114428L), // 2022-05-20T09:54:04.6114428
            LastSynchronizedOn = new(637886372446114428L) // 2022-05-20T09:54:04.6114428
        };
    }
}
