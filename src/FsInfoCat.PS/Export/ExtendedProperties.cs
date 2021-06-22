using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    [Obsolete]
    public class ExtendedProperties : ExportSet.ExtendedPropertySetBase
    {
        public string Kind { get; set; }
        public ushort? Width { get; set; }
        public ushort? Height { get; set; }
        public ulong? Duration { get; set; }
        public uint? FrameCount { get; set; }
        public uint? TrackNumber { get; set; }
        public uint? Bitrate { get; set; }
        public uint? FrameRate { get; set; }
        public ushort? SamplesPerPixel { get; set; }
        public uint? PixelPerUnitX { get; set; }
        public uint? PixelPerUnitY { get; set; }
        public ushort? Compression { get; set; }
        public uint? XResNumerator { get; set; }
        public uint? XResDenominator { get; set; }
        public uint? YResNumerator { get; set; }
        public uint? YResDenominator { get; set; }
        public ushort? ResolutionXUnit { get; set; }
        public ushort? ResolutionYUnit { get; set; }
        public ushort? JPEGProc { get; set; }
        public ushort? JPEGQuality { get; set; }

        [XmlAttribute(nameof(DateTime))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_DateTime { get => DateTime.ToDateTimeXml(); set => DateTime = value.FromXmlDateTime(); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public DateTime? DateTime { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }
        public string SoftwareUsed { get; set; }
        public string Artist { get; set; }
        public string HostComputer { get; set; }

        public IEnumerable<File> GetFiles() => throw new NotImplementedException();

        internal static ExtendedProperties Create(System.IO.FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }
    }
}
