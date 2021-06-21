using System;
using System.Collections.Generic;

namespace FsInfoCat.PS.Export
{
    public class GPSProperties : ExportSet.ExtendedPropertiesBase
    {
        /// <summary>
        /// Represents the name of the GPS area
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemGPS.AreaInformation"/>
        public string AreaInformation { get; set; }

        /// <summary>
        /// Indicates the latitude degrees.
        /// </summary>
        /// <remarks>This is the value at index 0 from an array of three values.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemGPS.Latitude"/>
        public double? LatitudeDegrees { get; set; }

        /// <summary>
        /// Indicates the latitude minutes.
        /// </summary>
        /// <remarks>This is the value at index 1 from an array of three values.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemGPS.Latitude"/>
        public double? LatitudeMinutes { get; set; }

        /// <summary>
        /// Indicates the latitude seconds.
        /// </summary>
        /// <remarks>This is the value at index 2 from an array of three values.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemGPS.Latitude"/>
        public double? LatitudeSeconds { get; set; }

        /// <summary>
        /// Indicates whether latitude is north or south latitude
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemGPS.LatitudeRef"/>
        public string LatitudeRef { get; set; }

        /// <summary>
        /// Indicates the longitude degrees.
        /// </summary>
        /// <remarks>This is the value at index 0 from an array of three values.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemGPS.Longitude"/>
        public double? LongitudeDegrees { get; set; }

        /// <summary>
        /// Indicates the longitude minutes.
        /// </summary>
        /// <remarks>This is the value at index 1 from an array of three values.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemGPS.Longitude"/>
        public double? LongitudeMinutes { get; set; }

        /// <summary>
        /// Indicates the longitude seconds.
        /// </summary>
        /// <remarks>This is the value at index 2 from an array of three values.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemGPS.Longitude"/>
        public double? LongitudeSeconds { get; set; }

        /// <summary>
        /// Indicates whether longitude is east or west longitude
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemGPS.LongitudeRef"/>
        public string LongitudeRef { get; set; }

        /// <summary>
        /// Indicates the GPS measurement mode.
        /// </summary>
        /// <remarks>eg: 2-dimensional, 3-dimensional</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemGPS.MeasureMode"/>
        public string MeasureMode { get; set; }

        /// <summary>
        /// Indicates the name of the method used for location finding
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemGPS.ProcessingMethod"/>
        public string ProcessingMethod { get; set; }

        /// <summary>
        /// Indicates the version of the GPS information
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemGPS.VersionID"/>
        public byte[] VersionID { get; set; }

        public IEnumerable<File> GetFiles() => throw new NotImplementedException();

        internal static GPSProperties Create(System.IO.FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }
    }
}
