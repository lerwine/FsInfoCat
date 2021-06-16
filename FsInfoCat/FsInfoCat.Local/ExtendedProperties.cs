using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Local
{
    public class ExtendedProperties : LocalDbEntity, ILocalExtendedProperties
    {
        #region Fields

        private HashSet<DbFile> _files = new();
        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _kind;
        private readonly IPropertyChangeTracker<ushort?> _width;
        private readonly IPropertyChangeTracker<ushort?> _height;
        private readonly IPropertyChangeTracker<ulong?> _duration;
        private readonly IPropertyChangeTracker<uint?> _frameCount;
        private readonly IPropertyChangeTracker<uint?> _trackNumber;
        private readonly IPropertyChangeTracker<uint?> _bitrate;
        private readonly IPropertyChangeTracker<uint?> _frameRate;
        private readonly IPropertyChangeTracker<ushort?> _samplesPerPixel;
        private readonly IPropertyChangeTracker<uint?> _pixelPerUnitX;
        private readonly IPropertyChangeTracker<uint?> _pixelPerUnitY;
        private readonly IPropertyChangeTracker<ushort?> _compression;
        private readonly IPropertyChangeTracker<uint?> _xResNumerator;
        private readonly IPropertyChangeTracker<uint?> _xResDenominator;
        private readonly IPropertyChangeTracker<uint?> _yResNumerator;
        private readonly IPropertyChangeTracker<uint?> _yResDenominator;
        private readonly IPropertyChangeTracker<ushort?> _resolutionXUnit;
        private readonly IPropertyChangeTracker<ushort?> _resolutionYUnit;
        private readonly IPropertyChangeTracker<ushort?> _jpegProc;
        private readonly IPropertyChangeTracker<ushort?> _jpegQuality;
        private readonly IPropertyChangeTracker<DateTime?> _dateTime;
        private readonly IPropertyChangeTracker<string> _title;
        private readonly IPropertyChangeTracker<string> _description;
        private readonly IPropertyChangeTracker<string> _copyright;
        private readonly IPropertyChangeTracker<string> _softwareUsed;
        private readonly IPropertyChangeTracker<string> _artist;
        private readonly IPropertyChangeTracker<string> _hostComputer;

        #endregion

        #region Properties

        public Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public string Kind { get => _kind.GetValue(); set => _kind.SetValue(value); }

        public ushort? Width { get => _width.GetValue(); set => _width.SetValue(value); }

        public ushort? Height { get => _height.GetValue(); set => _height.SetValue(value); }

        public ulong? Duration { get => _duration.GetValue(); set => _duration.SetValue(value); }

        public uint? FrameCount { get => _frameCount.GetValue(); set => _frameCount.SetValue(value); }

        public uint? TrackNumber { get => _trackNumber.GetValue(); set => _trackNumber.SetValue(value); }

        public uint? Bitrate { get => _bitrate.GetValue(); set => _bitrate.SetValue(value); }

        public uint? FrameRate { get => _frameRate.GetValue(); set => _frameRate.SetValue(value); }

        public ushort? SamplesPerPixel { get => _samplesPerPixel.GetValue(); set => _samplesPerPixel.SetValue(value); }

        public uint? PixelPerUnitX { get => _pixelPerUnitX.GetValue(); set => _pixelPerUnitX.SetValue(value); }

        public uint? PixelPerUnitY { get => _pixelPerUnitY.GetValue(); set => _pixelPerUnitY.SetValue(value); }

        public ushort? Compression { get => _compression.GetValue(); set => _compression.SetValue(value); }

        public uint? XResNumerator { get => _xResNumerator.GetValue(); set => _xResNumerator.SetValue(value); }

        public uint? XResDenominator { get => _xResDenominator.GetValue(); set => _xResDenominator.SetValue(value); }

        public uint? YResNumerator { get => _yResNumerator.GetValue(); set => _yResNumerator.SetValue(value); }

        public uint? YResDenominator { get => _yResDenominator.GetValue(); set => _yResDenominator.SetValue(value); }

        public ushort? ResolutionXUnit { get => _resolutionXUnit.GetValue(); set => _resolutionXUnit.SetValue(value); }

        public ushort? ResolutionYUnit { get => _resolutionYUnit.GetValue(); set => _resolutionYUnit.SetValue(value); }

        public ushort? JPEGProc { get => _jpegProc.GetValue(); set => _jpegProc.SetValue(value); }

        public ushort? JPEGQuality { get => _jpegQuality.GetValue(); set => _jpegQuality.SetValue(value); }

        public DateTime? DateTime { get => _dateTime.GetValue(); set => _dateTime.SetValue(value); }

        public string Title { get => _title.GetValue(); set => _title.SetValue(value); }

        public string Description { get => _description.GetValue(); set => _description.SetValue(value); }

        public string Copyright { get => _copyright.GetValue(); set => _copyright.SetValue(value); }

        public string SoftwareUsed { get => _softwareUsed.GetValue(); set => _softwareUsed.SetValue(value); }

        public string Artist { get => _artist.GetValue(); set => _artist.SetValue(value); }

        public string HostComputer { get => _hostComputer.GetValue(); set => _hostComputer.SetValue(value); }

        public HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        #endregion

        #region Explicit Members

        IEnumerable<IFile> IExtendedProperties.Files => Files.Cast<IFile>();

        IEnumerable<ILocalFile> ILocalExtendedProperties.Files => Files.Cast<ILocalFile>();

        #endregion

        public ExtendedProperties()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _kind = AddChangeTracker(nameof(Kind), "", TrimmedNonNullStringCoersion.Default);
            _width = AddChangeTracker(nameof(Width), (ushort?)null);
            _height = AddChangeTracker(nameof(Height), (ushort?)null);
            _duration = AddChangeTracker(nameof(Duration), (ulong?)null);
            _frameCount = AddChangeTracker(nameof(FrameCount), (uint?)null);
            _trackNumber = AddChangeTracker(nameof(TrackNumber), (uint?)null);
            _bitrate = AddChangeTracker(nameof(Bitrate), (uint?)null);
            _frameRate = AddChangeTracker(nameof(FrameRate), (uint?)null);
            _samplesPerPixel = AddChangeTracker(nameof(SamplesPerPixel), (ushort?)null);
            _pixelPerUnitX = AddChangeTracker(nameof(PixelPerUnitX), (uint?)null);
            _pixelPerUnitY = AddChangeTracker(nameof(PixelPerUnitY), (uint?)null);
            _compression = AddChangeTracker(nameof(Compression), (ushort?)null);
            _xResNumerator = AddChangeTracker(nameof(XResNumerator), (uint?)null);
            _xResDenominator = AddChangeTracker(nameof(XResDenominator), (uint?)null);
            _yResNumerator = AddChangeTracker(nameof(YResNumerator), (uint?)null);
            _yResDenominator = AddChangeTracker(nameof(YResDenominator), (uint?)null);
            _resolutionXUnit = AddChangeTracker(nameof(ResolutionXUnit), (ushort?)null);
            _resolutionYUnit = AddChangeTracker(nameof(ResolutionYUnit), (ushort?)null);
            _jpegProc = AddChangeTracker(nameof(JPEGProc), (ushort?)null);
            _jpegQuality = AddChangeTracker(nameof(JPEGQuality), (ushort?)null);
            _dateTime = AddChangeTracker(nameof(DateTime), (DateTime?)null);
            _title = AddChangeTracker(nameof(Title), (string)null);
            _description = AddChangeTracker(nameof(Description), (string)null);
            _copyright = AddChangeTracker(nameof(Copyright), (string)null);
            _softwareUsed = AddChangeTracker(nameof(SoftwareUsed), (string)null);
            _artist = AddChangeTracker(nameof(Artist), (string)null);
            _hostComputer = AddChangeTracker(nameof(HostComputer), (string)null);
        }
    }
}
