using System;
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
            Name = "",
            Notes = "",
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
            Name = "",
            Notes = "",
            Options = FileCrawlOptions.DoNotCompare,
            Status = FileCorrelationStatus.Justified,
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
    }
}
