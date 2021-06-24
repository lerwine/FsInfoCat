namespace FsInfoCat.Desktop
{
    public record GPSPropertiesRecord : IGPSProperties
    {
        public string AreaInformation { get; init; }

        public double? LatitudeDegrees { get; init; }

        public double? LatitudeMinutes { get; init; }

        public double? LatitudeSeconds { get; init; }

        public string LatitudeRef { get; init; }

        public double? LongitudeDegrees { get; init; }

        public double? LongitudeMinutes { get; init; }

        public double? LongitudeSeconds { get; init; }

        public string LongitudeRef { get; init; }

        public string MeasureMode { get; init; }

        public string ProcessingMethod { get; init; }

        public byte[] VersionID { get; init; }
    }
}
