using FsInfoCat.Collections;

namespace FsInfoCat.Desktop.FileSystemDetail
{
    public record SummaryPropertiesRecord : ISummaryProperties
    {
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
        public uint? Rating { get; init; }
        public MultiStringValue ItemAuthors { get; init; }
        public string ItemType { get; init; }
        public string ItemTypeText { get; init; }
        public MultiStringValue Kind { get; init; }
        public string MIMEType { get; init; }
        public string ParentalRatingReason { get; init; }
        public string ParentalRatingsOrganization { get; init; }
        public ushort? Sensitivity { get; init; }
        public string SensitivityText { get; init; }
        public uint? SimpleRating { get; init; }
        public string Trademarks { get; init; }
        public string ProductName { get; init; }
    }
}
