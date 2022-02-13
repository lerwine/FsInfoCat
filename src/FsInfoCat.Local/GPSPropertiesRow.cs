using FsInfoCat.Collections;

namespace FsInfoCat.Local
{
    public class GPSPropertiesRow : PropertiesRow, IGPSProperties
    {
        #region Fields

        private string _areaInformation = string.Empty;
        private string _latitudeRef = string.Empty;
        private string _longitudeRef = string.Empty;
        private string _measureMode = string.Empty;
        private string _processingMethod = string.Empty;

        #endregion

        #region Properties

        public string AreaInformation { get => _areaInformation; set => _areaInformation = value.AsWsNormalizedOrEmpty(); }

        public double? LatitudeDegrees { get; set; }

        public double? LatitudeMinutes { get; set; }

        public double? LatitudeSeconds { get; set; }

        public string LatitudeRef { get => _latitudeRef; set => _latitudeRef = value.AsWsNormalizedOrEmpty(); }

        public double? LongitudeDegrees { get; set; }

        public double? LongitudeMinutes { get; set; }

        public double? LongitudeSeconds { get; set; }

        public string LongitudeRef { get => _longitudeRef; set => _longitudeRef = value.AsWsNormalizedOrEmpty(); }

        public string MeasureMode { get => _measureMode; set => _measureMode = value.AsWsNormalizedOrEmpty(); }

        public string ProcessingMethod { get => _processingMethod; set => _processingMethod = value.AsWsNormalizedOrEmpty(); }

        public ByteValues VersionID { get; set; }

        #endregion
    }
}
