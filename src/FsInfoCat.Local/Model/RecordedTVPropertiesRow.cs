using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Base class for entities containing extended file properties for recorded TV files.
    /// </summary>
    /// <seealso cref="RecordedTVPropertiesListItem" />
    /// <seealso cref="RecordedTVPropertySet" />
    public abstract class RecordedTVPropertiesRow : PropertiesRow, ILocalRecordedTVPropertiesRow
    {
        #region Fields

        private string _episodeName;
        private string _networkAffiliation;
        private string _programDescription;
        private string _stationCallSign;
        private string _stationName;

        #endregion

        #region Properties

        public uint? ChannelNumber { get; set; }

        [NotNull]
        [BackingField(nameof(_episodeName))]
        public string EpisodeName { get => _episodeName; set => _episodeName = value.AsWsNormalizedOrEmpty(); }

        public bool? IsDTVContent { get; set; }

        public bool? IsHDContent { get; set; }

        [NotNull]
        [BackingField(nameof(_networkAffiliation))]
        public string NetworkAffiliation { get => _networkAffiliation; set => _networkAffiliation = value.AsWsNormalizedOrEmpty(); }

        public DateTime? OriginalBroadcastDate { get; set; }

        [NotNull]
        [BackingField(nameof(_programDescription))]
        public string ProgramDescription { get => _programDescription; set => _programDescription = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_stationCallSign))]
        public string StationCallSign { get => _stationCallSign; set => _stationCallSign = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_stationName))]
        public string StationName { get => _stationName; set => _stationName = value.AsWsNormalizedOrEmpty(); }

        #endregion

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalRecordedTVPropertiesRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ILocalRecordedTVPropertiesRow other) => ArePropertiesEqual((IRecordedTVPropertiesRow)other) &&
            EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
            LastSynchronizedOn == other.LastSynchronizedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IRecordedTVPropertiesRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] IRecordedTVPropertiesRow other) => ArePropertiesEqual((IRecordedTVProperties)other) &&
            CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IRecordedTVProperties" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] IRecordedTVProperties other) => _episodeName == other.EpisodeName &&
            _networkAffiliation == other.NetworkAffiliation &&
            _programDescription == other.ProgramDescription &&
            _stationCallSign == other.StationCallSign &&
            _stationName == other.StationName &&
            ChannelNumber == other.ChannelNumber &&
            IsDTVContent == other.IsDTVContent &&
            IsHDContent == other.IsHDContent &&
            OriginalBroadcastDate == other.OriginalBroadcastDate;
        //EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
        //LastSynchronizedOn == other.LastSynchronizedOn &&
        //CreatedOn == other.CreatedOn &&
        //ModifiedOn == other.ModifiedOn;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public abstract bool Equals(IRecordedTVPropertiesRow other);

        public abstract bool Equals(IRecordedTVProperties other);

        public override int GetHashCode()
        {
            if (TryGetId(out Guid id)) return id.GetHashCode();
            HashCode hash = new();
            hash.Add(_episodeName);
            hash.Add(_networkAffiliation);
            hash.Add(_programDescription);
            hash.Add(_stationCallSign);
            hash.Add(_stationName);
            hash.Add(ChannelNumber);
            hash.Add(IsDTVContent);
            hash.Add(IsHDContent);
            hash.Add(OriginalBroadcastDate);
            hash.Add(UpstreamId);
            hash.Add(LastSynchronizedOn);
            hash.Add(CreatedOn);
            hash.Add(ModifiedOn);
            return hash.ToHashCode();
        }

        protected virtual string PropertiesToString() => $@"EpisodeName=""{ExtensionMethods.EscapeCsString(_episodeName)}"", ProgramDescription=""{ExtensionMethods.EscapeCsString(_programDescription)}"",
    IsDTVContent={IsDTVContent}, IsHDContent={IsHDContent}, OriginalBroadcastDate={OriginalBroadcastDate:yyyy-mm-ddTHH:mm:ss.fffffff}, ChannelNumber={ChannelNumber},
    StationName=""{ExtensionMethods.EscapeCsString(_stationName)}"", StationCallSign=""{ExtensionMethods.EscapeCsString(_stationCallSign)}"", NetworkAffiliation=""{ExtensionMethods.EscapeCsString(_networkAffiliation)}""";

        public override string ToString() => $@"{{ Id={(TryGetId(out Guid id) ? id : null)}, {PropertiesToString()},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId} }}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
