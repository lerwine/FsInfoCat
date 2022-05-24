using System;
using System.Collections.ObjectModel;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestRecordedTVPropertySetData
    {
        public Guid UpstreamId { get; init; }

        public DateTime LastSynchronizedOn { get; init; }

        public DateTime CreatedOn { get; init; }
        public DateTime ModifiedOn { get; init; }

        public Guid Id { get; init; }

        public uint ChannelNumber { get; init; }

        public string EpisodeName { get; init; }

        public bool IsDTVContent { get; init; }

        public bool IsHDContent { get; init; }

        public string NetworkAffiliation { get; init; }

        public DateTime OriginalBroadcastDate { get; init; }

        public string ProgramDescription { get; init; }

        public string StationCallSign { get; init; }

        public string StationName { get; init; }

        public static readonly Collection<TestRecordedTVPropertySetData> Data = new();

        public static readonly TestRecordedTVPropertySetData Item1 = new()
        {
            Id = new("cb02ef2d-a193-4cfc-aa6f-9b0369a98415"),
            UpstreamId = new("d8313143-5f94-453d-9339-165aadafd9fe"),
            CreatedOn = new(637885392782794428L), // 2022-05-19T06:41:18.2794428
            ModifiedOn = new(637885392782794428L), // 2022-05-19T06:41:18.2794428
            LastSynchronizedOn = new(637885392782794428L) // 2022-05-19T06:41:18.2794428
        };

        public static readonly TestRecordedTVPropertySetData Item2 = new()
        {
            Id = new("556c3b90-cfc5-4c43-ae31-32e1bee4cda0"),
            UpstreamId = new("3e041939-877e-4044-ae8d-cc3b7d518845"),
            CreatedOn = new(637885704217534428L), // 2022-05-19T15:20:21.7534428
            ModifiedOn = new(637885704217534428L), // 2022-05-19T15:20:21.7534428
            LastSynchronizedOn = new(637885704217534428L) // 2022-05-19T15:20:21.7534428
        };
    }
}
