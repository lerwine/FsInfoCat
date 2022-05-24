using System;
using System.Collections.ObjectModel;
using FsInfoCat.Collections;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestSummaryPropertySetData
    {
        public Guid UpstreamId { get; init; }

        public DateTime LastSynchronizedOn { get; init; }

        public DateTime CreatedOn { get; init; }

        public DateTime ModifiedOn { get; init; }

        public Guid Id { get; init; }

        public string ApplicationName { get; init; }

        public MultiStringValue Author { get; init; }

        public string Comment { get; init; }

        public MultiStringValue Keywords { get; init; }

        public string Subject { get; init; }

        public string Title { get; init; }

        public string Company { get; init; }

        public string ContentType { get; init; }

        public string Copyright { get; init; }

        public string ParentalRating { get; init; }

        public uint Rating { get; init; }

        public MultiStringValue ItemAuthors { get; init; }

        public string ItemType { get; init; }

        public string ItemTypeText { get; init; }

        public MultiStringValue Kind { get; init; }

        public string MIMEType { get; init; }

        public string ParentalRatingReason { get; init; }

        public string ParentalRatingsOrganization { get; init; }

        public ushort Sensitivity { get; init; }

        public string SensitivityText { get; init; }

        public uint SimpleRating { get; init; }

        public string Trademarks { get; init; }

        public string ProductName { get; init; }

        public static readonly Collection<TestSummaryPropertySetData> Data = new();

        public static readonly TestSummaryPropertySetData Item1 = new()
        {
            Id = new("9da52d52-5943-4794-9659-04bc99c51520"),
            UpstreamId = new("d818c1b5-70d0-4201-99aa-e6c38b26b6ba"),
            CreatedOn = new(637884837491574428L), // 2022-05-18T15:15:49.1574428
            ModifiedOn = new(637884837491574428L), // 2022-05-18T15:15:49.1574428
            LastSynchronizedOn = new(637884837491574428L), // 2022-05-18T15:15:49.1574428
            ApplicationName = "",
            Author = new MultiStringValue(new string[] { "", "" }),
            Comment = "",
            Company = "",
            ContentType = "",
            Copyright = "",
            ItemAuthors = new MultiStringValue(new string[] { "", "" }),
            ItemType = "",
            ItemTypeText = "",
            Keywords = new MultiStringValue(new string[] { "", "" }),
            Kind = new MultiStringValue(new string[] { "", "" }),
            MIMEType = "",
            ParentalRating = "",
            ParentalRatingReason = "",
            ParentalRatingsOrganization = "",
            ProductName = "",
            Rating = 5,
            Sensitivity = 3,
            SensitivityText = "",
            SimpleRating = 5,
            Subject = "",
            Title = "",
            Trademarks = ""
        };

        public static readonly TestSummaryPropertySetData Item2 = new()
        {
            Id = new("1a4f7743-aed7-4e6e-b861-1ce325d8d920"),
            UpstreamId = new("9469d975-6ef8-4fee-a40a-8b9006fd4559"),
            CreatedOn = new(637886647257914428L), // 2022-05-20T17:32:05.7914428
            ModifiedOn = new(637886647257914428L), // 2022-05-20T17:32:05.7914428
            LastSynchronizedOn = new(637886647257914428L) // 2022-05-20T17:32:05.7914428
        };
    }
}
