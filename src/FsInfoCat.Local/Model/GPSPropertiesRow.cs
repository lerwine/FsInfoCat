using FsInfoCat.Model;
using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Base class for entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="GPSPropertiesListItem" />
    /// <seealso cref="GPSPropertySet" />
    public abstract class GPSPropertiesRow : PropertiesRow, ILocalGPSPropertiesRow
    {
        #region Fields

        private string _areaInformation = string.Empty;
        private string _latitudeRef = string.Empty;
        private string _longitudeRef = string.Empty;
        private string _measureMode = string.Empty;
        private string _processingMethod = string.Empty;

        #endregion

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        #region Properties

        [NotNull]
        [BackingField(nameof(_areaInformation))]
        public string AreaInformation { get => _areaInformation; set => _areaInformation = value.AsWsNormalizedOrEmpty(); }

        public double? LatitudeDegrees { get; set; }

        public double? LatitudeMinutes { get; set; }

        public double? LatitudeSeconds { get; set; }

        [NotNull]
        [BackingField(nameof(_latitudeRef))]
        public string LatitudeRef { get => _latitudeRef; set => _latitudeRef = value.AsWsNormalizedOrEmpty(); }

        public double? LongitudeDegrees { get; set; }

        public double? LongitudeMinutes { get; set; }

        public double? LongitudeSeconds { get; set; }

        [NotNull]
        [BackingField(nameof(_longitudeRef))]
        public string LongitudeRef { get => _longitudeRef; set => _longitudeRef = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_measureMode))]
        public string MeasureMode { get => _measureMode; set => _measureMode = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_processingMethod))]
        public string ProcessingMethod { get => _processingMethod; set => _processingMethod = value.AsWsNormalizedOrEmpty(); }

        public ByteValues VersionID { get; set; }

        #endregion

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalGPSPropertiesRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ILocalGPSPropertiesRow other) => ArePropertiesEqual((IGPSPropertiesRow)other) &&
            EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
            LastSynchronizedOn == other.LastSynchronizedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IGPSPropertiesRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] IGPSPropertiesRow other) => ArePropertiesEqual((IGPSProperties)other) &&
            CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IGPSProperties" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected virtual bool ArePropertiesEqual([DisallowNull] IGPSProperties other) => _areaInformation == other.AreaInformation &&
            _latitudeRef == other.LatitudeRef &&
            _longitudeRef == other.LongitudeRef &&
            _measureMode == other.MeasureMode &&
            _processingMethod == other.ProcessingMethod &&
            LatitudeDegrees == other.LatitudeDegrees &&
            LatitudeMinutes == other.LatitudeMinutes &&
            LatitudeSeconds == other.LatitudeSeconds &&
            LongitudeDegrees == other.LongitudeDegrees &&
            LongitudeMinutes == other.LongitudeMinutes &&
            LongitudeSeconds == other.LongitudeSeconds &&
            EqualityComparer<ByteValues>.Default.Equals(VersionID, other.VersionID);
        //EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
        //LastSynchronizedOn == other.LastSynchronizedOn &&
        //CreatedOn == other.CreatedOn &&
        //ModifiedOn == other.ModifiedOn

        public abstract bool Equals(IGPSPropertiesRow other);

        public abstract bool Equals(IGPSProperties other);

        public override int GetHashCode()
        {
            if (TryGetId(out Guid id)) return id.GetHashCode();
            HashCode hash = new();
            hash.Add(_areaInformation);
            hash.Add(_latitudeRef);
            hash.Add(_longitudeRef);
            hash.Add(_measureMode);
            hash.Add(_processingMethod);
            hash.Add(LatitudeDegrees);
            hash.Add(LatitudeMinutes);
            hash.Add(LatitudeSeconds);
            hash.Add(LongitudeDegrees);
            hash.Add(LongitudeMinutes);
            hash.Add(LongitudeSeconds);
            hash.Add(VersionID);
            hash.Add(UpstreamId);
            hash.Add(LastSynchronizedOn);
            hash.Add(CreatedOn);
            hash.Add(ModifiedOn);
            return hash.ToHashCode();
        }

        protected virtual string PropertiesToString() => $@"AreaInformation=""{ExtensionMethods.EscapeCsString(_areaInformation)}"", MeasureMode=""{ExtensionMethods.EscapeCsString(_measureMode)}"",
    LatitudeDegrees={LatitudeDegrees}, LatitudeMinutes={LatitudeMinutes}, LatitudeSeconds={LatitudeSeconds}, LatitudeRef=""{ExtensionMethods.EscapeCsString(_latitudeRef)}"",
    LongitudeDegrees={LongitudeDegrees}, LongitudeMinutes={LongitudeMinutes}, LongitudeSeconds={LongitudeSeconds}, LongitudeRef=""{ExtensionMethods.EscapeCsString(_longitudeRef)}"",
    ProcessingMethod=""{ExtensionMethods.EscapeCsString(_processingMethod)}"", VersionID={VersionID?.ToString()}";

        public override string ToString() => $@"{{ Id={(TryGetId(out Guid id) ? id : null)}, {PropertiesToString()},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId} }}";
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
