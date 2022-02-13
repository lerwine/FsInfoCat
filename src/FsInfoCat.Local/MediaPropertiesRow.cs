using FsInfoCat.Collections;

namespace FsInfoCat.Local
{
    public class MediaPropertiesRow : PropertiesRow, IMediaProperties
    {
        #region Fields

        private string _contentDistributor = string.Empty;
        private string _creatorApplication = string.Empty;
        private string _creatorApplicationVersion = string.Empty;
        private string _dateReleased = string.Empty;
        private string _dvdID = string.Empty;
        private string _protectionType = string.Empty;
        private string _providerRating = string.Empty;
        private string _providerStyle = string.Empty;
        private string _publisher = string.Empty;
        private string _subtitle = string.Empty;

        #endregion

        #region Properties

        public string ContentDistributor { get => _contentDistributor; set => _contentDistributor = value.AsWsNormalizedOrEmpty(); }

        public string CreatorApplication { get => _creatorApplication; set => _creatorApplication = value.AsWsNormalizedOrEmpty(); }

        public string CreatorApplicationVersion { get => _creatorApplicationVersion; set => _creatorApplicationVersion = value.AsWsNormalizedOrEmpty(); }

        public string DateReleased { get => _dateReleased; set => _dateReleased = value.AsWsNormalizedOrEmpty(); }

        public ulong? Duration { get; set; }

        public string DVDID { get => _dvdID; set => _dvdID = value.AsWsNormalizedOrEmpty(); }

        public uint? FrameCount { get; set; }

        public MultiStringValue Producer { get; set; }

        public string ProtectionType { get => _protectionType; set => _protectionType = value.AsWsNormalizedOrEmpty(); }

        public string ProviderRating { get => _providerRating; set => _providerRating = value.AsWsNormalizedOrEmpty(); }

        public string ProviderStyle { get => _providerStyle; set => _providerStyle = value.AsWsNormalizedOrEmpty(); }

        public string Publisher { get => _publisher; set => _publisher = value.AsWsNormalizedOrEmpty(); }

        public string Subtitle { get => _subtitle; set => _subtitle = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue Writer { get; set; }

        public uint? Year { get; set; }

        #endregion
    }
}
