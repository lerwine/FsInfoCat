using System;
using FsInfoCat.Model;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestSubdirectoryData
    {
        public Guid? UpstreamId { get; init; }

        public DateTime? LastSynchronizedOn { get; init; }

        public DirectoryCrawlOptions Options { get; init; }

        public DirectoryStatus Status { get; init; }

        public TestVolumeData Volume { get; init; }

        public TestSubdirectoryData Parent { get; init; }

        public Guid? ParentId => Parent?.Id;

        public Guid? VolumeId => Volume?.Id;

        public string Name { get; init; }

        public DateTime LastAccessed { get; init; }

        public string Notes { get; init; }

        public DateTime CreationTime { get; init; }

        public DateTime LastWriteTime { get; init; }

        public DateTime CreatedOn { get; init; }

        public DateTime ModifiedOn { get; init; }

        public Guid Id { get; init; }

        public static readonly TestSubdirectoryData Item1 = new()
        {
            CreationTime = new(637879289534571762L), // 2022-05-12T05:09:13.4571762
            LastWriteTime = new(637882331883461247L), // 2022-05-15T17:39:48.3461247
            LastAccessed = new(637889546318906351L), // 2022-05-24T02:03:51.8906351
            Name = "First Folder",
            Notes = "Note for first folder",
            Options = DirectoryCrawlOptions.Skip,
            Status = DirectoryStatus.AccessError,
            Volume = TestVolumeData.Item1,
            Id = new("da88c8a6-8b5f-4031-9641-f359fdf19632"),
            UpstreamId = new("83c18e60-5ec0-49fc-ad39-063e9f1d39fc"),
            CreatedOn = new(637886238968484428L), // 2022-05-20T06:11:36.8484428
            ModifiedOn = new(637889546387242936L), // 2022-05-24T02:03:58.7242936
            LastSynchronizedOn = new(637889546465454599L), // 2022-05-24T02:04:06.5454599
        };

        public static readonly TestSubdirectoryData Item2 = new()
        {
            CreationTime = new(637882331883171247L), // 2022-05-15T17:39:48.3171247
            LastWriteTime = new(637886669012830556L), // 2022-05-20T18:08:21.2830556
            LastAccessed = new(637889545066377083), // 2022-05-24T02:01:46.6377083
            Name = "FolderTwo",
            Notes = "Folder note #2",
            Options = DirectoryCrawlOptions.SkipSubdirectories,
            Status = DirectoryStatus.Complete,
            Parent = Item1,
            Id = new("5aaa25e7-5d26-4d5c-80fe-933d94c4fe25"),
            UpstreamId = new("7d863b0c-ca83-4fc3-99d9-1e883e576701"),
            CreatedOn = new(637886669012743423L), // 2022-05-20-T18:08:21.2743423
            ModifiedOn = new(637889544876464438L), // 2022-05-24T02:01:27.6464438
            LastSynchronizedOn = new(637889545067907083L), // 2022-05-24T02:01:46.7907083
        };
    }
}
