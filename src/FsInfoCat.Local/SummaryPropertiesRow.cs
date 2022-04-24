using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        [BackingField(nameof(_applicationName))]
        public string ApplicationName { get => _applicationName; set => _applicationName = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue Author { get; set; }

        [NotNull]
        [BackingField(nameof(_comment))]
        public string Comment { get => _comment; set => _comment = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue Keywords { get; set; }

        [NotNull]
        [BackingField(nameof(_subject))]
        public string Subject { get => _subject; set => _subject = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_title))]
        public string Title { get => _title; set => _title = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_company))]
        public string Company { get => _company; set => _company = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_contentType))]
        public string ContentType { get => _contentType; set => _contentType = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_copyright))]
        public string Copyright { get => _copyright; set => _copyright = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_parentalRating))]
        public string ParentalRating { get => _parentalRating; set => _parentalRating = value.AsWsNormalizedOrEmpty(); }

        public uint? Rating { get; set; }

        public MultiStringValue ItemAuthors { get; set; }

        [NotNull]
        [BackingField(nameof(_itemType))]
        public string ItemType { get => _itemType; set => _itemType = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_itemTypeText))]
        public string ItemTypeText { get => _itemTypeText; set => _itemTypeText = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue Kind { get; set; }

        [NotNull]
        [BackingField(nameof(_mimeType))]
        public string MIMEType { get => _mimeType; set => _mimeType = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_parentalRatingReason))]
        public string ParentalRatingReason { get => _parentalRatingReason; set => _parentalRatingReason = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_parentalRatingsOrganization))]
        public string ParentalRatingsOrganization { get => _parentalRatingsOrganization; set => _parentalRatingsOrganization = value.AsWsNormalizedOrEmpty(); }

        public ushort? Sensitivity { get; set; }

        [NotNull]
        [BackingField(nameof(_sensitivityText))]
        public string SensitivityText { get => _sensitivityText; set => _sensitivityText = value.AsWsNormalizedOrEmpty(); }

        public uint? SimpleRating { get; set; }

        [NotNull]
        [BackingField(nameof(_trademarks))]
        public string Trademarks { get => _trademarks; set => _trademarks = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_productName))]
        public string ProductName { get => _productName; set => _productName = value.AsWsNormalizedOrEmpty(); }

        #endregion

        protected bool ArePropertiesEqual([DisallowNull] ILocalSummaryPropertiesRow other) => ArePropertiesEqual((ISummaryPropertiesRow)other) &&
            EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
            LastSynchronizedOn == other.LastSynchronizedOn;

        protected bool ArePropertiesEqual([DisallowNull] ISummaryPropertiesRow other) => ArePropertiesEqual((ISummaryProperties)other) &&
            CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn;

        protected bool ArePropertiesEqual([DisallowNull] ISummaryProperties other) => _applicationName == other.ApplicationName &&
            _comment == other.Comment &&
            _subject == other.Subject &&
            _title == other.Title &&
            _company == other.Company &&
            _contentType == other.ContentType &&
            _copyright == other.Copyright &&
            _parentalRating == other.ParentalRating &&
            _itemType == other.ItemType &&
            _itemTypeText == other.ItemTypeText &&
            _mimeType == other.MIMEType &&
            _parentalRatingReason == other.ParentalRatingReason &&
            _parentalRatingsOrganization == other.ParentalRatingsOrganization &&
            _sensitivityText == other.SensitivityText &&
            _trademarks == other.Trademarks &&
            _productName == other.ProductName &&
            EqualityComparer<MultiStringValue>.Default.Equals(Author, other.Author) &&
            EqualityComparer<MultiStringValue>.Default.Equals(Keywords, other.Keywords) &&
            Rating == other.Rating &&
            EqualityComparer<MultiStringValue>.Default.Equals(ItemAuthors, other.ItemAuthors) &&
            EqualityComparer<MultiStringValue>.Default.Equals(Kind, other.Kind) &&
            Sensitivity == other.Sensitivity &&
            SimpleRating == other.SimpleRating;
        //EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
        //LastSynchronizedOn == other.LastSynchronizedOn &&
        //CreatedOn == other.CreatedOn &&
        //ModifiedOn == other.ModifiedOn;

        public abstract bool Equals(ISummaryPropertiesRow other);

        public abstract bool Equals(ISummaryProperties other);

        public override int GetHashCode()
        {
            if (TryGetId(out Guid id)) return id.GetHashCode();
            HashCode hash = new();
            hash.Add(_applicationName);
            hash.Add(_comment);
            hash.Add(_subject);
            hash.Add(_title);
            hash.Add(_company);
            hash.Add(_contentType);
            hash.Add(_copyright);
            hash.Add(_parentalRating);
            hash.Add(_itemType);
            hash.Add(_itemTypeText);
            hash.Add(_mimeType);
            hash.Add(_parentalRatingReason);
            hash.Add(_parentalRatingsOrganization);
            hash.Add(_sensitivityText);
            hash.Add(_trademarks);
            hash.Add(_productName);
            hash.Add(Author);
            hash.Add(Keywords);
            hash.Add(Rating);
            hash.Add(ItemAuthors);
            hash.Add(Kind);
            hash.Add(Sensitivity);
            hash.Add(SimpleRating);
            hash.Add(UpstreamId);
            hash.Add(LastSynchronizedOn);
            hash.Add(CreatedOn);
            hash.Add(ModifiedOn);
            return hash.ToHashCode();
        }
    }
}
