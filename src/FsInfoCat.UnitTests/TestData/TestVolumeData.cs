using System;
using System.IO;
using FsInfoCat.Model;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestVolumeData
    {
        public Guid UpstreamId { get; init; }

        public DateTime LastSynchronizedOn { get; init; }

        public string DisplayName { get; init; }

        public string VolumeName { get; init; }

        public VolumeIdentifier Identifier { get; init; }

        public bool ReadOnly { get; init; }

        public uint MaxNameLength { get; init; }

        public DriveType Type { get; init; }

        public string Notes { get; init; }

        public VolumeStatus Status { get; init; }

        public TestFileSystemData FileSystem { get; init; }

        public Guid FileSystemId => FileSystem.Id;

        public DateTime CreatedOn { get; init; }

        public DateTime ModifiedOn { get; init; }

        public Guid Id { get; init; }

        public static readonly TestVolumeData Item1 = new()
        {
            DisplayName = "My DVD",
            Identifier = new VolumeIdentifier(0x8ac1),
            MaxNameLength = 1023,
            Notes = "This is my backup",
            ReadOnly = true,
            Status = VolumeStatus.Controlled,
            Type = DriveType.CDRom,
            VolumeName = "BACKUP_1",
            FileSystem = TestFileSystemData.Item1,
            Id = new("2abf7a2e-b4eb-4252-8a9f-995f13d41931"),
            UpstreamId = new("7b5164f7-5c4f-40f7-8ee8-46af6a2df5c2"),
            CreatedOn = new(637883017793224428L), // 2022-05-16T12:42:59.3224428
            ModifiedOn = new(637889548497924557L), // 2022-05-24T02:07:29.7924557
            LastSynchronizedOn = new(637889548487752122L), // 2022-05-24T02:07:28.7752122
        };
    }
}
