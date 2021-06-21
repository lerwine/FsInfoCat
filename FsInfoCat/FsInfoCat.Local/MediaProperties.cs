using System;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Local
{
    public class MediaProperties : LocalDbEntity, ILocalMediaProperties
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _contentDistributor;
        private readonly IPropertyChangeTracker<string> _creatorApplication;
        private readonly IPropertyChangeTracker<string> _creatorApplicationVersion;
        private readonly IPropertyChangeTracker<string> _dateReleased;
        private readonly IPropertyChangeTracker<ulong?> _duration;
        private readonly IPropertyChangeTracker<string> _dvdID;
        private readonly IPropertyChangeTracker<uint?> _frameCount;
        private readonly IPropertyChangeTracker<string[]> _producer;
        private readonly IPropertyChangeTracker<string> _protectionType;
        private readonly IPropertyChangeTracker<string> _providerRating;
        private readonly IPropertyChangeTracker<string> _providerStyle;
        private readonly IPropertyChangeTracker<string> _publisher;
        private readonly IPropertyChangeTracker<string> _subtitle;
        private readonly IPropertyChangeTracker<string[]> _writer;
        private readonly IPropertyChangeTracker<uint?> _year;
        private HashSet<DbFile> _files = new();

        #endregion

        #region Properties

        public Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public string ContentDistributor { get => _contentDistributor.GetValue(); set => _contentDistributor.SetValue(value); }
        public string CreatorApplication { get => _creatorApplication.GetValue(); set => _creatorApplication.SetValue(value); }
        public string CreatorApplicationVersion { get => _creatorApplicationVersion.GetValue(); set => _creatorApplicationVersion.SetValue(value); }
        public string DateReleased { get => _dateReleased.GetValue(); set => _dateReleased.SetValue(value); }
        public ulong? Duration { get => _duration.GetValue(); set => _duration.SetValue(value); }
        public string DVDID { get => _dvdID.GetValue(); set => _dvdID.SetValue(value); }
        public uint? FrameCount { get => _frameCount.GetValue(); set => _frameCount.SetValue(value); }
        public string[] Producer { get => _producer.GetValue(); set => _producer.SetValue(value); }
        public string ProtectionType { get => _protectionType.GetValue(); set => _protectionType.SetValue(value); }
        public string ProviderRating { get => _providerRating.GetValue(); set => _providerRating.SetValue(value); }
        public string ProviderStyle { get => _providerStyle.GetValue(); set => _providerStyle.SetValue(value); }
        public string Publisher { get => _publisher.GetValue(); set => _publisher.SetValue(value); }
        public string Subtitle { get => _subtitle.GetValue(); set => _subtitle.SetValue(value); }
        public string[] Writer { get => _writer.GetValue(); set => _writer.SetValue(value); }
        public uint? Year { get => _year.GetValue(); set => _year.SetValue(value); }

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

        public MediaProperties()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _contentDistributor = AddChangeTracker<string>(nameof(ContentDistributor), null);
            _creatorApplication = AddChangeTracker<string>(nameof(CreatorApplication), null);
            _creatorApplicationVersion = AddChangeTracker<string>(nameof(CreatorApplicationVersion), null);
            _dateReleased = AddChangeTracker<string>(nameof(DateReleased), null);
            _duration = AddChangeTracker<ulong?>(nameof(Duration), null);
            _dvdID = AddChangeTracker<string>(nameof(DVDID), null);
            _frameCount = AddChangeTracker<uint?>(nameof(FrameCount), null);
            _producer = AddChangeTracker<string[]>(nameof(Producer), null);
            _protectionType = AddChangeTracker<string>(nameof(ProtectionType), null);
            _providerRating = AddChangeTracker<string>(nameof(ProviderRating), null);
            _providerStyle = AddChangeTracker<string>(nameof(ProviderStyle), null);
            _publisher = AddChangeTracker<string>(nameof(Publisher), null);
            _subtitle = AddChangeTracker<string>(nameof(Subtitle), null);
            _writer = AddChangeTracker<string[]>(nameof(Writer), null);
            _year = AddChangeTracker<uint?>(nameof(Year), null);
        }
    }
}
