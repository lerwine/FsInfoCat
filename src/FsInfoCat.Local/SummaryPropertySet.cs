using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Local
{
    public class SummaryPropertySet : LocalDbEntity, ILocalSummaryPropertySet
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _applicationName;
        private readonly IPropertyChangeTracker<MultiStringValue> _author;
        private readonly IPropertyChangeTracker<string> _comment;
        private readonly IPropertyChangeTracker<MultiStringValue> _keywords;
        private readonly IPropertyChangeTracker<string> _subject;
        private readonly IPropertyChangeTracker<string> _title;
        private readonly IPropertyChangeTracker<string> _company;
        private readonly IPropertyChangeTracker<string> _contentType;
        private readonly IPropertyChangeTracker<string> _copyright;
        private readonly IPropertyChangeTracker<string> _parentalRating;
        private readonly IPropertyChangeTracker<uint?> _rating;
        private readonly IPropertyChangeTracker<MultiStringValue> _itemAuthors;
        private readonly IPropertyChangeTracker<string> _itemType;
        private readonly IPropertyChangeTracker<string> _itemTypeText;
        private readonly IPropertyChangeTracker<MultiStringValue> _kind;
        private readonly IPropertyChangeTracker<string> _mimeType;
        private readonly IPropertyChangeTracker<string> _parentalRatingReason;
        private readonly IPropertyChangeTracker<string> _parentalRatingsOrganization;
        private readonly IPropertyChangeTracker<ushort?> _sensitivity;
        private readonly IPropertyChangeTracker<string> _sensitivityText;
        private readonly IPropertyChangeTracker<uint?> _simpleRating;
        private readonly IPropertyChangeTracker<string> _trademarks;
        private readonly IPropertyChangeTracker<string> _productName;
        private HashSet<DbFile> _files = new();

        #endregion

        #region Properties

        public Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public string ApplicationName { get => _applicationName.GetValue(); set => _applicationName.SetValue(value); }
        public MultiStringValue Author { get => _author.GetValue(); set => _author.SetValue(value); }
        public string Comment { get => _comment.GetValue(); set => _comment.SetValue(value); }
        public MultiStringValue Keywords { get => _keywords.GetValue(); set => _keywords.SetValue(value); }
        public string Subject { get => _subject.GetValue(); set => _subject.SetValue(value); }
        public string Title { get => _title.GetValue(); set => _title.SetValue(value); }
        public string Company { get => _company.GetValue(); set => _company.SetValue(value); }
        public string ContentType { get => _contentType.GetValue(); set => _contentType.SetValue(value); }
        public string Copyright { get => _copyright.GetValue(); set => _copyright.SetValue(value); }
        public string ParentalRating { get => _parentalRating.GetValue(); set => _parentalRating.SetValue(value); }
        public uint? Rating { get => _rating.GetValue(); set => _rating.SetValue(value); }
        public MultiStringValue ItemAuthors { get => _itemAuthors.GetValue(); set => _itemAuthors.SetValue(value); }
        public string ItemType { get => _itemType.GetValue(); set => _itemType.SetValue(value); }
        public string ItemTypeText { get => _itemTypeText.GetValue(); set => _itemTypeText.SetValue(value); }
        public MultiStringValue Kind { get => _kind.GetValue(); set => _kind.SetValue(value); }
        public string MIMEType { get => _mimeType.GetValue(); set => _mimeType.SetValue(value); }
        public string ParentalRatingReason { get => _parentalRatingReason.GetValue(); set => _parentalRatingReason.SetValue(value); }
        public string ParentalRatingsOrganization { get => _parentalRatingsOrganization.GetValue(); set => _parentalRatingsOrganization.SetValue(value); }
        public ushort? Sensitivity { get => _sensitivity.GetValue(); set => _sensitivity.SetValue(value); }
        public string SensitivityText { get => _sensitivityText.GetValue(); set => _sensitivityText.SetValue(value); }
        public uint? SimpleRating { get => _simpleRating.GetValue(); set => _simpleRating.SetValue(value); }
        public string Trademarks { get => _trademarks.GetValue(); set => _trademarks.SetValue(value); }
        public string ProductName { get => _productName.GetValue(); set => _productName.SetValue(value); }

        public HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        #endregion

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        #endregion

        public SummaryPropertySet()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _applicationName = AddChangeTracker<string>(nameof(ApplicationName), null);
            _author = AddChangeTracker<MultiStringValue>(nameof(Author), null);
            _comment = AddChangeTracker<string>(nameof(Comment), null);
            _keywords = AddChangeTracker<MultiStringValue>(nameof(Keywords), null);
            _subject = AddChangeTracker<string>(nameof(Subject), null);
            _title = AddChangeTracker<string>(nameof(Title), null);
            _company = AddChangeTracker<string>(nameof(Company), null);
            _contentType = AddChangeTracker<string>(nameof(ContentType), null);
            _copyright = AddChangeTracker<string>(nameof(Copyright), null);
            _parentalRating = AddChangeTracker<string>(nameof(ParentalRating), null);
            _rating = AddChangeTracker<uint?>(nameof(Rating), null);
            _itemAuthors = AddChangeTracker<MultiStringValue>(nameof(ItemAuthors), null);
            _itemType = AddChangeTracker<string>(nameof(ItemType), null);
            _itemTypeText = AddChangeTracker<string>(nameof(ItemTypeText), null);
            _kind = AddChangeTracker<MultiStringValue>(nameof(Kind), null);
            _mimeType = AddChangeTracker<string>(nameof(MIMEType), null);
            _parentalRatingReason = AddChangeTracker<string>(nameof(ParentalRatingReason), null);
            _parentalRatingsOrganization = AddChangeTracker<string>(nameof(ParentalRatingsOrganization), null);
            _sensitivity = AddChangeTracker<ushort?>(nameof(Sensitivity), null);
            _sensitivityText = AddChangeTracker<string>(nameof(SensitivityText), null);
            _simpleRating = AddChangeTracker<uint?>(nameof(SimpleRating), null);
            _trademarks = AddChangeTracker<string>(nameof(Trademarks), null);
            _productName = AddChangeTracker<string>(nameof(ProductName), null);
        }

        internal static void BuildEntity(EntityTypeBuilder<SummaryPropertySet> obj)
        {
            obj.Property(nameof(Author)).HasConversion(MultiStringValue.Converter);
            obj.Property(nameof(Keywords)).HasConversion(MultiStringValue.Converter);
            obj.Property(nameof(ItemAuthors)).HasConversion(MultiStringValue.Converter);
            obj.Property(nameof(Kind)).HasConversion(MultiStringValue.Converter);
        }
    }
}
