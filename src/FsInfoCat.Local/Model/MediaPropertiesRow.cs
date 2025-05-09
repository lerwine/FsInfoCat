using FsInfoCat.Model;
using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Base class for entities containing extended file properties for media files.
    /// </summary>
    /// <seealso cref="MediaPropertiesListItem" />
    /// <seealso cref="MediaPropertySet" />
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected virtual string PropertiesToString() => $@"Subtitle=""{ExtensionMethods.EscapeCsString(_subtitle)}"", Duration={Duration}, FrameCount={FrameCount},
    Year={Year}, DateReleased=""{ExtensionMethods.EscapeCsString(_dateReleased)}"", DVDID=""{ExtensionMethods.EscapeCsString(_dvdID)}"",
    ProtectionType=""{ExtensionMethods.EscapeCsString(_protectionType)}"", ProviderRating=""{ExtensionMethods.EscapeCsString(_providerRating)}"", ProviderStyle=""{ExtensionMethods.EscapeCsString(_providerStyle)}"",
    Writer={Writer.ToCsString()}, Producer={Producer.ToCsString()}, Publisher=""{ExtensionMethods.EscapeCsString(_publisher)}"",
    ContentDistributor=""{ExtensionMethods.EscapeCsString(_contentDistributor)}"", CreatorApplication=""{ExtensionMethods.EscapeCsString(_creatorApplication)}"", CreatorApplicationVersion=""{ExtensionMethods.EscapeCsString(_creatorApplicationVersion)}""";

        public override string ToString() => $@"{{ Id={(TryGetId(out Guid id) ? id : null)}, {PropertiesToString()},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId} }}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

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

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalMediaPropertiesRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ILocalMediaPropertiesRow other) => ArePropertiesEqual((IMediaPropertiesRow)other) &&
            EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
            LastSynchronizedOn == other.LastSynchronizedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IMediaPropertiesRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] IMediaPropertiesRow other) => ArePropertiesEqual((IMediaProperties)other) &&
            CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IMediaProperties" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] IMediaProperties other) => _contentDistributor == other.ContentDistributor &&
            _creatorApplication == other.CreatorApplication &&
            _creatorApplicationVersion == other.CreatorApplicationVersion &&
            _dateReleased == other.DateReleased &&
            _dvdID == other.DVDID &&
            _protectionType == other.ProtectionType &&
            _providerRating == other.ProviderRating &&
            _providerStyle == other.ProviderStyle &&
            _publisher == other.Publisher &&
            _subtitle == other.Subtitle &&
            Duration == other.Duration &&
            FrameCount == other.FrameCount &&
            EqualityComparer<MultiStringValue>.Default.Equals(Producer, other.Producer) &&
            EqualityComparer<MultiStringValue>.Default.Equals(Writer, other.Writer) &&
            Year == other.Year;
        //EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
        //LastSynchronizedOn == other.LastSynchronizedOn &&
        //CreatedOn == other.CreatedOn &&
        //ModifiedOn == other.ModifiedOn;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public abstract bool Equals(IMediaPropertiesRow other);

        public abstract bool Equals(IMediaProperties other);

        public override int GetHashCode()
        {
            if (TryGetId(out Guid id)) return id.GetHashCode();
            HashCode hash = new();
            hash.Add(_contentDistributor);
            hash.Add(_creatorApplication);
            hash.Add(_creatorApplicationVersion);
            hash.Add(_dateReleased);
            hash.Add(_dvdID);
            hash.Add(_protectionType);
            hash.Add(_providerRating);
            hash.Add(_providerStyle);
            hash.Add(_publisher);
            hash.Add(_subtitle);
            hash.Add(Duration);
            hash.Add(FrameCount);
            hash.Add(Producer);
            hash.Add(Writer);
            hash.Add(Year);
            hash.Add(UpstreamId);
            hash.Add(LastSynchronizedOn);
            hash.Add(CreatedOn);
            hash.Add(ModifiedOn);
            return hash.ToHashCode();
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
