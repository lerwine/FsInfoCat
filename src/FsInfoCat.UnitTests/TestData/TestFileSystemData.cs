using System;
using System.IO;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestFileSystemData
    {
        public Guid UpstreamId { get; init; }

        public DateTime LastSynchronizedOn { get; init; }

        public string Notes { get; init; }

        public bool IsInactive { get; init; }

        public string DisplayName { get; init; }

        public bool ReadOnly { get; init; }

        public uint MaxNameLength { get; init; }

        public DriveType DefaultDriveType { get; init; }

        public DateTime CreatedOn { get; init; }
        public DateTime ModifiedOn { get; init; }

        public Guid Id { get; init; }

        public static readonly TestFileSystemData Item1 = new()
        {
            DefaultDriveType = DriveType.CDRom,
            DisplayName = "CDFS",
            IsInactive = true,
            MaxNameLength = 64,
            Notes = "My File System Note",
            ReadOnly = true,
            Id = new("3b93adc6-e632-4630-93ff-969cff734405"),
            UpstreamId = new("35755cc7-d4d4-4b96-ae10-414ab35b08d4"),
            CreatedOn = new(637882981639614428L), // 2022-05-16T11:42:43.9614428
            ModifiedOn = new(637889549073663975L), // 2022-05-24T02:08:27.3663975
            LastSynchronizedOn = new(637889549058372670L), // 2022-05-24T02:08:25.8372670
        };
    }
}
