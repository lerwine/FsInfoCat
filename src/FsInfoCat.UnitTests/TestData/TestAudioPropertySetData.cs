using System;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestAudioPropertySetData
    {
        public Guid UpstreamId { get; init; }

        public DateTime LastSynchronizedOn { get; init; }

        public DateTime CreatedOn { get; init; }
        public DateTime ModifiedOn { get; init; }

        public Guid Id { get; init; }

        public string Compression { get; init; }

        public uint EncodingBitrate { get; init; }

        public string Format { get; init; }

        public bool IsVariableBitrate { get; init; }

        public uint SampleRate { get; init; }

        public uint SampleSize { get; init; }

        public string StreamName { get; init; }

        public ushort StreamNumber { get; init; }

        public static readonly TestAudioPropertySetData Item1 = new()
        {
            Id = new("046cace2-4475-41b5-bc5c-8c032d215034"),
            UpstreamId = new("c09a3c55-d267-4729-ae4e-b6377ef531da"),
            CreatedOn = new(637885454658434428L), // 2022-05-19T08:24:25.8434428
            ModifiedOn = new(637885454658434428L), // 2022-05-19T08:24:25.8434428
            LastSynchronizedOn = new(637885454658434428L) // 2022-05-19T08:24:25.8434428
        };

        public static readonly TestAudioPropertySetData Item2 = new()
        {
            Id = new("f7003b48-7190-4c7a-9c43-1445e6965e8a"),
            UpstreamId = new("c890ba87-07ad-486f-b90b-faf8e4edbb8f"),
            CreatedOn = new(637886422159704428L), // 2022-05-20T11:16:55.9704428
            ModifiedOn = new(637886422159704428L), // 2022-05-20T11:16:55.9704428
            LastSynchronizedOn = new(637886422159704428L) // 2022-05-20T11:16:55.9704428
        };
    }
}
