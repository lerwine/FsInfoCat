using System;
using System.Collections.Generic;
using FsInfoCat.Local.Model;
using FsInfoCat.Model;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestFileData
    {
        public Guid UpstreamId { get; init; }

        public DateTime LastSynchronizedOn { get; init; }

        public FileCrawlOptions Options { get; init; }

        public FileCorrelationStatus Status { get; init; }

        public DateTime LastHashCalculation { get; init; }

        public string Name { get; init; }

        public DateTime LastAccessed { get; init; }

        public string Notes { get; init; }

        public DateTime CreationTime { get; init; }

        public DateTime LastWriteTime { get; init; }

        public DateTime CreatedOn { get; init; }

        public DateTime ModifiedOn { get; init; }

        public Guid Id { get; init; }

        public TestSubdirectoryData Parent { get; init; }

        public Guid ParentId => Parent.Id;

        public TestBinaryPropertySetData BinaryProperties { get; init; }

        public Guid BinaryPropertySetId => BinaryProperties.Id;

        public TestAudioPropertySetData AudioProperties { get; init; }

        public Guid? AudioPropertySetId => AudioProperties?.Id;

        public TestSummaryPropertySetData SummaryProperties { get; init; }

        public Guid? SummaryPropertySetId => SummaryProperties?.Id;

        public TestDocumentPropertySetData DocumentProperties { get; init; }

        public Guid? DocumentPropertySetId => DocumentProperties?.Id;

        public TestDRMPropertySetData DRMProperties { get; init; }

        public Guid? DRMPropertySetId => DRMProperties?.Id;

        public TestGPSPropertySetData GPSProperties { get; init; }

        public Guid? GPSPropertySetId => GPSProperties?.Id;

        public TestImagePropertySetData ImageProperties { get; init; }

        public Guid? ImagePropertySetId => ImageProperties?.Id;

        public TestMediaPropertySetData MediaProperties { get; init; }

        public Guid? MediaPropertySetId => MediaProperties?.Id;

        public TestMusicPropertySetData MusicProperties { get; init; }

        public Guid? MusicPropertySetId => MusicProperties?.Id;

        public TestPhotoPropertySetData PhotoProperties { get; init; }

        public Guid? PhotoPropertySetId => PhotoProperties?.Id;

        public TestRecordedTVPropertySetData RecordedTVProperties { get; init; }

        public Guid? RecordedTVPropertySetId => RecordedTVProperties?.Id;

        public TestVideoPropertySetData VideoProperties { get; init; }

        public Guid? VideoPropertySetId => VideoProperties?.Id;

        public static readonly TestFileData Item1 = new()
        {
            CreationTime = new(637885463072018206L), // 2022-05-19T08:38:27.2018206
            LastWriteTime = new(637886329247008315L), // 2022-05-20T08:42:04.7008315
            LastAccessed = new(637889535366161120L), // 2022-05-24T01:45:36.6161120
            LastHashCalculation = new(637889535366161120L), // 2022-05-24T01:45:36.6161120
            Name = "File1.txt",
            Notes = "Notes for first file",
            Options = FileCrawlOptions.DoNotCompare,
            Status = FileCorrelationStatus.Justified,
            Parent = TestSubdirectoryData.Item1,
            BinaryProperties = TestBinaryPropertySetData.Item1,
            AudioProperties = TestAudioPropertySetData.Item1,
            SummaryProperties = TestSummaryPropertySetData.Item1,
            DocumentProperties = TestDocumentPropertySetData.Item1,
            DRMProperties = TestDRMPropertySetData.Item1,
            GPSProperties = TestGPSPropertySetData.Item1,
            ImageProperties = TestImagePropertySetData.Item1,
            MediaProperties = TestMediaPropertySetData.Item1,
            MusicProperties = TestMusicPropertySetData.Item1,
            PhotoProperties = TestPhotoPropertySetData.Item1,
            RecordedTVProperties = TestRecordedTVPropertySetData.Item1,
            VideoProperties = TestVideoPropertySetData.Item1,
            Id = new("f51a39f6-8676-43b7-a563-2dc9741a6ded"),
            UpstreamId = new("9d584188-f82f-4fa5-8c15-f6624fd3f6f8"),
            CreatedOn = new(637886347352914428L), // 2022-05-20T09:12:15.2914428
            ModifiedOn = new(637889535366161120L), // 2022-05-24T01:45:36.6161120
            LastSynchronizedOn = new(637889535366161120L), // 2022-05-24T01:45:36.6161120
        };

        public static readonly TestFileData Item2 = new()
        {
            CreationTime = new(637883369492186819L), // 2022-05-16T22:29:09.2186819
            LastWriteTime = new(637889546900921092L), // 2022-05-24T02:04:50.0921092
            LastAccessed = new(637889547440368744L), // 2022-05-24T02:05:44.0368744
            LastHashCalculation = new(637889547429297264L), // 2022-05-24T02:05:42.9297264
            Name = "File2.xlsx",
            Notes = "Notes for second file",
            Options = FileCrawlOptions.FlaggedForRescan,
            Status = FileCorrelationStatus.Deferred,
            Parent = TestSubdirectoryData.Item2,
            BinaryProperties = TestBinaryPropertySetData.Item2,
            AudioProperties = TestAudioPropertySetData.Item2,
            SummaryProperties = TestSummaryPropertySetData.Item2,
            DocumentProperties = TestDocumentPropertySetData.Item2,
            DRMProperties = TestDRMPropertySetData.Item2,
            GPSProperties = TestGPSPropertySetData.Item2,
            ImageProperties = TestImagePropertySetData.Item2,
            MediaProperties = TestMediaPropertySetData.Item2,
            MusicProperties = TestMusicPropertySetData.Item2,
            PhotoProperties = TestPhotoPropertySetData.Item2,
            RecordedTVProperties = TestRecordedTVPropertySetData.Item2,
            VideoProperties = TestVideoPropertySetData.Item2,
            Id = new("ef54f15a-2c9f-470b-b835-21561812c920"),
            UpstreamId = new("787818d0-ab3b-4b5a-9657-2f8cd8940417"),
            CreatedOn = new(637886669012570556L), // 2022-05-20T18:08:21.2570556
            ModifiedOn = new(637889547784553818L), // 2022-05-24T02:06:18.4553818
            LastSynchronizedOn = new(637889547875623815L), // 2022-05-24T02:06:27.5623815
        };

        public static IEnumerable<(DbFile target, DbFile other, bool expectedResult)> GetEqualsTestData()
        {
            yield return
            (
                new DbFile
                {
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    LastHashCalculation = Item2.LastHashCalculation,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    LastHashCalculation = Item2.LastHashCalculation,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    LastSynchronizedOn = Item1.LastSynchronizedOn, UpstreamId = Item1.UpstreamId,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    LastSynchronizedOn = Item1.LastSynchronizedOn, UpstreamId = Item1.UpstreamId,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Name = Item2.Name,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    Name = Item2.Name,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Notes = Item1.Notes,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Notes = Item1.Notes,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Options = Item2.Options,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    Options = Item2.Options,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Status = Item1.Status,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Status = Item1.Status,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    AudioProperties = new() { Id = Item2.AudioProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    AudioProperties = new() { Id = Item2.AudioProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    DocumentProperties = new() { Id = Item1.DocumentProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    DocumentProperties = new() { Id = Item1.DocumentProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    DRMProperties = new() { Id = Item2.DRMProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    DRMProperties = new() { Id = Item2.DRMProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            // index: 10
            yield return
            (
                new DbFile
                {
                    GPSProperties = new() { Id = Item1.GPSProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    GPSProperties = new() { Id = Item1.GPSProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    ImageProperties = new() { Id = Item2.ImageProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    ImageProperties = new() { Id = Item2.ImageProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    MediaProperties = new() { Id = Item1.MediaProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    MediaProperties = new() { Id = Item1.MediaProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    MusicProperties = new() { Id = Item2.MusicProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    MusicProperties = new() { Id = Item2.MusicProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    PhotoProperties = new() { Id = Item1.PhotoProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    PhotoProperties = new() { Id = Item1.PhotoProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    RecordedTVProperties = new() { Id = Item2.RecordedTVProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    RecordedTVProperties = new() { Id = Item2.RecordedTVProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    SummaryProperties = new() { Id = Item1.SummaryProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    SummaryProperties = new() { Id = Item1.SummaryProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    VideoProperties = new() { Id = Item2.VideoProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    VideoProperties = new() { Id = Item2.VideoProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                false
            );
            // index: 20
            yield return
            (
                new DbFile
                {
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    LastHashCalculation = Item1.LastHashCalculation,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    LastHashCalculation = Item2.LastHashCalculation,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    LastSynchronizedOn = Item2.LastSynchronizedOn, UpstreamId = Item1.UpstreamId,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    LastSynchronizedOn = Item1.LastSynchronizedOn, UpstreamId = Item1.UpstreamId,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    LastSynchronizedOn = Item1.LastSynchronizedOn, UpstreamId = Item2.UpstreamId,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    LastSynchronizedOn = Item1.LastSynchronizedOn, UpstreamId = Item1.UpstreamId,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    Name = Item1.Name,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    Name = Item2.Name,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    Notes = Item2.Notes,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Notes = Item1.Notes,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                false
            );
            // index: 30
            yield return
            (
                new DbFile
                {
                    Options = Item1.Options,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    Options = Item2.Options,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    Status = Item2.Status,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Status = Item1.Status,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    AudioProperties = new() { Id = Item1.AudioProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    AudioProperties = new() { Id = Item2.AudioProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    DocumentProperties = new() { Id = Item2.DocumentProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    DocumentProperties = new() { Id = Item1.DocumentProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    DRMProperties = new() { Id = Item1.DRMProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    DRMProperties = new() { Id = Item2.DRMProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    GPSProperties = new() { Id = Item2.GPSProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    GPSProperties = new() { Id = Item1.GPSProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    ImageProperties = new() { Id = Item1.ImageProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    ImageProperties = new() { Id = Item2.ImageProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    MediaProperties = new() { Id = Item2.MediaProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    MediaProperties = new() { Id = Item1.MediaProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    MusicProperties = new() { Id = Item1.MusicProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    MusicProperties = new() { Id = Item2.MusicProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    PhotoProperties = new() { Id = Item2.PhotoProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    PhotoProperties = new() { Id = Item1.PhotoProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    RecordedTVProperties = new() { Id = Item1.RecordedTVProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    RecordedTVProperties = new() { Id = Item2.RecordedTVProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    SummaryProperties = new() { Id = Item2.SummaryProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    SummaryProperties = new() { Id = Item1.SummaryProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    VideoProperties = new() { Id = Item1.VideoProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    VideoProperties = new() { Id = Item2.VideoProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile
                {
                    Id = Item1.Id,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Id = Item2.Id,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                false
            );
            yield return
            (
                new DbFile { Id = Item2.Id },
                new DbFile { Id = Item2.Id },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item1.Id,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Id = Item1.Id,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item2.Id,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    Id = Item2.Id,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item1.Id,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Id = Item1.Id,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item2.Id,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    Id = Item2.Id,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item1.Id,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Id = Item1.Id,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item2.Id,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Id = Item2.Id,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item1.Id,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Id = Item1.Id,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item2.Id,
                    LastHashCalculation = Item1.LastHashCalculation,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    Id = Item2.Id,
                    LastHashCalculation = Item2.LastHashCalculation,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item1.Id,
                    LastSynchronizedOn = Item2.LastSynchronizedOn, UpstreamId = Item1.UpstreamId,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Id = Item1.Id,
                    LastSynchronizedOn = Item1.LastSynchronizedOn, UpstreamId = Item1.UpstreamId,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item1.Id,
                    LastSynchronizedOn = Item1.LastSynchronizedOn, UpstreamId = Item2.UpstreamId,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Id = Item1.Id,
                    LastSynchronizedOn = Item1.LastSynchronizedOn, UpstreamId = Item1.UpstreamId,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item2.Id,
                    Name = Item1.Name,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    Id = Item2.Id,
                    Name = Item2.Name,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item1.Id,
                    Notes = Item2.Notes,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Id = Item1.Id,
                    Notes = Item1.Notes,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item2.Id,
                    Options = Item1.Options,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    Id = Item2.Id,
                    Options = Item2.Options,
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item1.Id,
                    Status = Item2.Status,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Id = Item1.Id,
                    Status = Item1.Status,
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item2.Id,
                    AudioProperties = new() { Id = Item1.AudioProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    Id = Item2.Id,
                    AudioProperties = new() { Id = Item2.AudioProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item1.Id,
                    DocumentProperties = new() { Id = Item2.DocumentProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Id = Item1.Id,
                    DocumentProperties = new() { Id = Item1.DocumentProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item2.Id,
                    DRMProperties = new() { Id = Item1.DRMProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    Id = Item2.Id,
                    DRMProperties = new() { Id = Item2.DRMProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item1.Id,
                    GPSProperties = new() { Id = Item2.GPSProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Id = Item1.Id,
                    GPSProperties = new() { Id = Item1.GPSProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item2.Id,
                    ImageProperties = new() { Id = Item1.ImageProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    Id = Item2.Id,
                    ImageProperties = new() { Id = Item2.ImageProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item1.Id,
                    MediaProperties = new() { Id = Item2.MediaProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Id = Item1.Id,
                    MediaProperties = new() { Id = Item1.MediaProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item2.Id,
                    MusicProperties = new() { Id = Item1.MusicProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    Id = Item2.Id,
                    MusicProperties = new() { Id = Item2.MusicProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item1.Id,
                    PhotoProperties = new() { Id = Item2.PhotoProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Id = Item1.Id,
                    PhotoProperties = new() { Id = Item1.PhotoProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item2.Id,
                    RecordedTVProperties = new() { Id = Item1.RecordedTVProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    Id = Item2.Id,
                    RecordedTVProperties = new() { Id = Item2.RecordedTVProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item1.Id,
                    SummaryProperties = new() { Id = Item2.SummaryProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                new DbFile
                {
                    Id = Item1.Id,
                    SummaryProperties = new() { Id = Item1.SummaryProperties.Id },
                    BinaryProperties = new() { Id = Item1.BinaryPropertySetId },
                    Parent = new() { Id = Item1.ParentId },
                    CreationTime = Item1.CreationTime,
                    LastWriteTime = Item1.LastWriteTime,
                    LastAccessed = Item1.LastAccessed,
                    CreatedOn = Item1.CreatedOn,
                    ModifiedOn = Item1.ModifiedOn
                },
                true
            );
            yield return
            (
                new DbFile
                {
                    Id = Item2.Id,
                    VideoProperties = new() { Id = Item1.VideoProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                new DbFile
                {
                    Id = Item2.Id,
                    VideoProperties = new() { Id = Item2.VideoProperties.Id },
                    BinaryProperties = new() { Id = Item2.BinaryPropertySetId },
                    Parent = new() { Id = Item2.ParentId },
                    CreationTime = Item2.CreationTime,
                    LastWriteTime = Item2.LastWriteTime,
                    LastAccessed = Item2.LastAccessed,
                    CreatedOn = Item2.CreatedOn,
                    ModifiedOn = Item2.ModifiedOn
                },
                true
            );
        }
    }
}
