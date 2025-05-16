using FsInfoCat.Model;
using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Base class for entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="GPSPropertiesListItem" />
    /// <seealso cref="GPSPropertySet" />
    /// <seealso cref="LocalDbContext.GPSPropertySets" />
    /// <seealso cref="LocalDbContext.GPSPropertiesListing" />
    public abstract class GPSPropertiesRow : PropertiesRow, ILocalGPSPropertiesRow
    {
        #region Fields

        private string _areaInformation = string.Empty;
        private string _latitudeRef = string.Empty;
        private string _longitudeRef = string.Empty;
        private string _measureMode = string.Empty;
        private string _processingMethod = string.Empty;

        #endregion
        #region Properties

        /// <summary>
        /// Gets the name of the GPS area.
        /// </summary>
        /// <value>The name of the GPS area.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Area Information</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{972E333E-AC7E-49F1-8ADF-A70D07A9BCAB} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-areainformation">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.AreaInformation), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_areaInformation))]
        public string AreaInformation { get => _areaInformation; set => _areaInformation = value.AsWsNormalizedOrEmpty(); }

        /// <summary>
        /// Indicates the latitude degrees.
        /// </summary>
        /// <value>This is the value at index 0 from an array of three values.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Latitude Degrees</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{8727CFFF-4868-4EC6-AD5B-81B98521D1AB} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-latitudedegrees">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.LatitudeDegrees), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public double? LatitudeDegrees { get; set; }

        /// <summary>
        /// Indicates the latitude minutes.
        /// </summary>
        /// <value>This is the value at index 1 from an array of three values.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Latitude Minutes</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{8727CFFF-4868-4EC6-AD5B-81B98521D1AB} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-latitudeminutes">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.LatitudeMinutes), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public double? LatitudeMinutes { get; set; }

        /// <summary>
        /// Indicates the latitude seconds.
        /// </summary>
        /// <value>This is the value at index 2 from an array of three values.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Latitude Seconds</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{8727CFFF-4868-4EC6-AD5B-81B98521D1AB} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-latitude">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.LatitudeSeconds), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public double? LatitudeSeconds { get; set; }

        /// <summary>
        /// Indicates whether latitude is north or south latitude.
        /// </summary>
        /// <value>Indicates whether latitude is north or south.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Latitude Reference</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{029C0252-5B86-46C7-ACA0-2769FFC8E3D4} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-latituderef">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.LatitudeRef), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_latitudeRef))]
        public string LatitudeRef { get => _latitudeRef; set => _latitudeRef = value.AsWsNormalizedOrEmpty(); }

        /// <summary>
        /// Indicates the longitude degrees.
        /// </summary>
        /// <value>This is the value at index 0 from an array of three values.</value>
        /// <remarks>
        /// Indicates whether latitude is north or south.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Longitude Degrees</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{C4C4DBB2-B593-466B-BBDA-D03D27D5E43A} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-longitudedegrees">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.LongitudeDegrees), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public double? LongitudeDegrees { get; set; }

        /// <summary>
        /// Indicates the longitude minutes.
        /// </summary>
        /// <value>This is the value at index 1 from an array of three values.</value>
        /// <remarks>
        /// Indicates whether latitude is north or south.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Longitude Minutes</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{C4C4DBB2-B593-466B-BBDA-D03D27D5E43A} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-longitudeminutes">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.LongitudeMinutes), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public double? LongitudeMinutes { get; set; }

        /// <summary>
        /// Indicates the longitude seconds.
        /// </summary>
        /// <value>This is the value at index 2 from an array of three values.</value>
        /// <remarks>
        /// Indicates whether latitude is north or south.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Longitude Seconds</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{C4C4DBB2-B593-466B-BBDA-D03D27D5E43A} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-longitudeseconds">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.LongitudeSeconds), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public double? LongitudeSeconds { get; set; }

        /// <summary>
        /// Indicates whether longitude is east or west longitude.
        /// </summary>
        /// <value>Indicates whether longitude is east or west.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Longitude Reference</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{33DCF22B-28D5-464C-8035-1EE9EFD25278} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-longituderef">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.LongitudeRef), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_longitudeRef))]
        public string LongitudeRef { get => _longitudeRef; set => _longitudeRef = value.AsWsNormalizedOrEmpty(); }

        /// <summary>
        /// Indicates the GPS measurement mode.
        /// </summary>
        /// <value>eg: 2-dimensional, 3-dimensional Indicates the GPS measurement mode (for example, two-dimensional, three-dimensional).</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Measure Mode</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{A015ED5D-AAEA-4D58-8A86-3C586920EA0B} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-measuremode">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.MeasureMode), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_measureMode))]
        public string MeasureMode { get => _measureMode; set => _measureMode = value.AsWsNormalizedOrEmpty(); }

        /// <summary>
        /// Indicates the name of the method used for location finding.
        /// </summary>
        /// <value>Indicates the name of the method used for finding locations.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Processing Method</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{59D49E61-840F-4AA9-A939-E2099B7F6399} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-processingmethod">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.ProcessingMethod), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_processingMethod))]
        public string ProcessingMethod { get => _processingMethod; set => _processingMethod = value.AsWsNormalizedOrEmpty(); }

        /// <summary>
        /// Indicates the version of the GPS information.
        /// </summary>
        /// <value>Indicates the version of the GPS information.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Version ID</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{22704DA4-C6B2-4A99-8E56-F16DF8C92599} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <see href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-versionid">[Reference Link]</see>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.VersionID), ResourceType = typeof(FsInfoCat.Properties.Resources))]
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


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
    ProcessingMethod=""{ExtensionMethods.EscapeCsString(_processingMethod)}"", VersionID={VersionID}";

        public override string ToString() => $@"{{ Id={(TryGetId(out Guid id) ? id : null)}, {PropertiesToString()},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId} }}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
