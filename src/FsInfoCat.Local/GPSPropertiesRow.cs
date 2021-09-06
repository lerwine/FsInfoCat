using FsInfoCat.Collections;

namespace FsInfoCat.Local
{
    public class GPSPropertiesRow : PropertiesRow, IGPSProperties
    {
        #region Fields

        private readonly IPropertyChangeTracker<string> _areaInformation;
        private readonly IPropertyChangeTracker<double?> _latitudeDegrees;
        private readonly IPropertyChangeTracker<double?> _latitudeMinutes;
        private readonly IPropertyChangeTracker<double?> _latitudeSeconds;
        private readonly IPropertyChangeTracker<string> _latitudeRef;
        private readonly IPropertyChangeTracker<double?> _longitudeDegrees;
        private readonly IPropertyChangeTracker<double?> _longitudeMinutes;
        private readonly IPropertyChangeTracker<double?> _longitudeSeconds;
        private readonly IPropertyChangeTracker<string> _longitudeRef;
        private readonly IPropertyChangeTracker<string> _measureMode;
        private readonly IPropertyChangeTracker<string> _processingMethod;
        private readonly IPropertyChangeTracker<ByteValues> _versionID;

        #endregion

        #region Properties

        public string AreaInformation { get => _areaInformation.GetValue(); set => _areaInformation.SetValue(value); }
        public double? LatitudeDegrees { get => _latitudeDegrees.GetValue(); set => _latitudeDegrees.SetValue(value); }
        public double? LatitudeMinutes { get => _latitudeMinutes.GetValue(); set => _latitudeMinutes.SetValue(value); }
        public double? LatitudeSeconds { get => _latitudeSeconds.GetValue(); set => _latitudeSeconds.SetValue(value); }
        public string LatitudeRef { get => _latitudeRef.GetValue(); set => _latitudeRef.SetValue(value); }
        public double? LongitudeDegrees { get => _longitudeDegrees.GetValue(); set => _longitudeDegrees.SetValue(value); }
        public double? LongitudeMinutes { get => _longitudeMinutes.GetValue(); set => _longitudeMinutes.SetValue(value); }
        public double? LongitudeSeconds { get => _longitudeSeconds.GetValue(); set => _longitudeSeconds.SetValue(value); }
        public string LongitudeRef { get => _longitudeRef.GetValue(); set => _longitudeRef.SetValue(value); }
        public string MeasureMode { get => _measureMode.GetValue(); set => _measureMode.SetValue(value); }
        public string ProcessingMethod { get => _processingMethod.GetValue(); set => _processingMethod.SetValue(value); }
        public ByteValues VersionID { get => _versionID.GetValue(); set => _versionID.SetValue(value); }

        #endregion

        public GPSPropertiesRow()
        {
            _areaInformation = AddChangeTracker(nameof(AreaInformation), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _latitudeDegrees = AddChangeTracker<double?>(nameof(LatitudeDegrees), null);
            _latitudeMinutes = AddChangeTracker<double?>(nameof(LatitudeMinutes), null);
            _latitudeSeconds = AddChangeTracker<double?>(nameof(LatitudeSeconds), null);
            _latitudeRef = AddChangeTracker(nameof(LatitudeRef), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _longitudeDegrees = AddChangeTracker<double?>(nameof(LongitudeDegrees), null);
            _longitudeMinutes = AddChangeTracker<double?>(nameof(LongitudeMinutes), null);
            _longitudeSeconds = AddChangeTracker<double?>(nameof(LongitudeSeconds), null);
            _longitudeRef = AddChangeTracker(nameof(LongitudeRef), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _measureMode = AddChangeTracker(nameof(MeasureMode), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _processingMethod = AddChangeTracker(nameof(ProcessingMethod), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _versionID = AddChangeTracker<ByteValues>(nameof(VersionID), null);
        }
    }
}
