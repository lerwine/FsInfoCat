using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public abstract class MediaPropertiesRow : PropertiesRow, ILocalMediaPropertiesRow
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

        [NotNull]
        [BackingField(nameof(_contentDistributor))]
        public string ContentDistributor { get => _contentDistributor; set => _contentDistributor = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_creatorApplication))]
        public string CreatorApplication { get => _creatorApplication; set => _creatorApplication = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_creatorApplicationVersion))]
        public string CreatorApplicationVersion { get => _creatorApplicationVersion; set => _creatorApplicationVersion = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_dateReleased))]
        public string DateReleased { get => _dateReleased; set => _dateReleased = value.AsWsNormalizedOrEmpty(); }

        public ulong? Duration { get; set; }

        [NotNull]
        [BackingField(nameof(_dvdID))]
        public string DVDID { get => _dvdID; set => _dvdID = value.AsWsNormalizedOrEmpty(); }

        public uint? FrameCount { get; set; }

        public MultiStringValue Producer { get; set; }

        [NotNull]
        [BackingField(nameof(_protectionType))]
        public string ProtectionType { get => _protectionType; set => _protectionType = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_providerRating))]
        public string ProviderRating { get => _providerRating; set => _providerRating = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_providerStyle))]
        public string ProviderStyle { get => _providerStyle; set => _providerStyle = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_publisher))]
        public string Publisher { get => _publisher; set => _publisher = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_subtitle))]
        public string Subtitle { get => _subtitle; set => _subtitle = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue Writer { get; set; }

        public uint? Year { get; set; }

        #endregion

        protected bool ArePropertiesEqual([DisallowNull] IMediaProperties other)
        {
            throw new NotImplementedException();
        }

        public abstract bool Equals(IMediaPropertiesRow other);

        public abstract bool Equals(IMediaProperties other);
    }
}
