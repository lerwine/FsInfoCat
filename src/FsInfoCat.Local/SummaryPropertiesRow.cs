using FsInfoCat.Collections;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public abstract class SummaryPropertiesRow : PropertiesRow, ILocalSummaryPropertiesRow
    {
        #region Fields

        private string _applicationName = string.Empty;
        private string _comment = string.Empty;
        private string _subject = string.Empty;
        private string _title = string.Empty;
        private string _company = string.Empty;
        private string _contentType = string.Empty;
        private string _copyright = string.Empty;
        private string _parentalRating = string.Empty;
        private string _itemType = string.Empty;
        private string _itemTypeText = string.Empty;
        private string _mimeType = string.Empty;
        private string _parentalRatingReason = string.Empty;
        private string _parentalRatingsOrganization = string.Empty;
        private string _sensitivityText = string.Empty;
        private string _trademarks = string.Empty;
        private string _productName = string.Empty;

        #endregion

        #region Properties

        [NotNull]
        public string ApplicationName { get => _applicationName; set => _applicationName = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue Author { get; set; }

        [NotNull]
        public string Comment { get => _comment; set => _comment = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue Keywords { get; set; }

        [NotNull]
        public string Subject { get => _subject; set => _subject = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string Title { get => _title; set => _title = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string Company { get => _company; set => _company = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string ContentType { get => _contentType; set => _contentType = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string Copyright { get => _copyright; set => _copyright = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string ParentalRating { get => _parentalRating; set => _parentalRating = value.AsWsNormalizedOrEmpty(); }

        public uint? Rating { get; set; }

        public MultiStringValue ItemAuthors { get; set; }

        [NotNull]
        public string ItemType { get => _itemType; set => _itemType = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string ItemTypeText { get => _itemTypeText; set => _itemTypeText = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue Kind { get; set; }

        [NotNull]
        public string MIMEType { get => _mimeType; set => _mimeType = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string ParentalRatingReason { get => _parentalRatingReason; set => _parentalRatingReason = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string ParentalRatingsOrganization { get => _parentalRatingsOrganization; set => _parentalRatingsOrganization = value.AsWsNormalizedOrEmpty(); }

        public ushort? Sensitivity { get; set; }

        [NotNull]
        public string SensitivityText { get => _sensitivityText; set => _sensitivityText = value.AsWsNormalizedOrEmpty(); }

        public uint? SimpleRating { get; set; }

        [NotNull]
        public string Trademarks { get => _trademarks; set => _trademarks = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string ProductName { get => _productName; set => _productName = value.AsWsNormalizedOrEmpty(); }

        #endregion

        protected bool ArePropertiesEqual([DisallowNull] ISummaryProperties other)
        {
            throw new NotImplementedException();
        }

        public abstract bool Equals(ISummaryPropertiesRow other);

        public abstract bool Equals(ISummaryProperties other);
    }
}
