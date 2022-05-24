using System;
using FsInfoCat.Collections;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestVideoPropertySetData
    {
        public Guid UpstreamId { get; init; }

        public DateTime LastSynchronizedOn { get; init; }

        public DateTime CreatedOn { get; init; }
        public DateTime ModifiedOn { get; init; }

        public Guid Id { get; init; }

        public string Compression { get; init; }

        public MultiStringValue Director { get; init; }

        public uint EncodingBitrate { get; init; }

        public uint FrameHeight { get; init; }

        public uint FrameRate { get; init; }

        public uint FrameWidth { get; init; }

        public uint HorizontalAspectRatio { get; init; }

        public ushort StreamNumber { get; init; }

        public string StreamName { get; init; }

        public uint VerticalAspectRatio { get; init; }

        public static readonly TestVideoPropertySetData Item1 = new()
        {
            Id = new("8dbad6c4-b589-4fab-afd0-75533d5166b6"),
            UpstreamId = new("b8890991-57fe-4645-b326-5698f19b3fb1"),
            CreatedOn = new(637883725118904428L), // 2022-05-17T08:21:51.8904428
            ModifiedOn = new(637883725118904428L), // 2022-05-17T08:21:51.8904428
            LastSynchronizedOn = new(637883725118904428L) // 2022-05-17T08:21:51.8904428
        };

        public static readonly TestVideoPropertySetData Item2 = new()
        {
            Id = new("1db1fe56-6746-4f41-b1bf-cf25150fa1a8"),
            UpstreamId = new("c422c5a3-6497-4447-a2df-14cd93b49bb1"),
            CreatedOn = new(637883855680734428L), // 2022-05-17T11:59:28.0734428
            ModifiedOn = new(637883855680734428L), // 2022-05-17T11:59:28.0734428
            LastSynchronizedOn = new(637883855680734428L) // 2022-05-17T11:59:28.0734428
        };
    }
}
