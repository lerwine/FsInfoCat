using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Local
{
    public class ExtendedProperties : NotifyPropertyChanged, ILocalExtendedProperties
    {
        private HashSet<DbFile> _files = new();
        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<ushort> _width;
        private readonly IPropertyChangeTracker<ushort> _height;
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
        private readonly IPropertyChangeTracker<Guid?> _upstreamId;
        private readonly IPropertyChangeTracker<DateTime?> _lastSynchronizedOn;
        private readonly IPropertyChangeTracker<DateTime> _createdOn;
        private readonly IPropertyChangeTracker<DateTime> _modifiedOn;

        public Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }
        public ushort Width { get => _width.GetValue(); set => _width.SetValue(value); }
        public ushort Height { get => _height.GetValue(); set => _height.SetValue(value); }
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

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_UpstreamId), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Guid? UpstreamId { get => _upstreamId.GetValue(); set => _upstreamId.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_LastSynchronizedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime? LastSynchronizedOn { get => _lastSynchronizedOn.GetValue(); set => _lastSynchronizedOn.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CreatedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime CreatedOn { get => _createdOn.GetValue(); set => _createdOn.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ModifiedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime ModifiedOn { get => _modifiedOn.GetValue(); set => _modifiedOn.SetValue(value); }

        public HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        public ExtendedProperties()
        {
            _id = CreateChangeTracker(nameof(Id), Guid.Empty);
            _width = CreateChangeTracker(nameof(Width), (ushort)0);
            _height = CreateChangeTracker(nameof(Height), (ushort)0);
            _duration = CreateChangeTracker(nameof(Duration), (ulong?)null);
            _frameCount = CreateChangeTracker(nameof(FrameCount), (uint?)null);
            _trackNumber = CreateChangeTracker(nameof(TrackNumber), (uint?)null);
            _bitrate = CreateChangeTracker(nameof(Bitrate), (uint?)null);
            _frameRate = CreateChangeTracker(nameof(FrameRate), (uint?)null);
            _samplesPerPixel = CreateChangeTracker(nameof(SamplesPerPixel), (ushort?)null);
            _pixelPerUnitX = CreateChangeTracker(nameof(PixelPerUnitX), (uint?)null);
            _pixelPerUnitY = CreateChangeTracker(nameof(PixelPerUnitY), (uint?)null);
            _compression = CreateChangeTracker(nameof(Compression), (ushort?)null);
            _xResNumerator = CreateChangeTracker(nameof(XResNumerator), (uint?)null);
            _xResDenominator = CreateChangeTracker(nameof(XResDenominator), (uint?)null);
            _yResNumerator = CreateChangeTracker(nameof(YResNumerator), (uint?)null);
            _yResDenominator = CreateChangeTracker(nameof(YResDenominator), (uint?)null);
            _resolutionXUnit = CreateChangeTracker(nameof(ResolutionXUnit), (ushort?)null);
            _resolutionYUnit = CreateChangeTracker(nameof(ResolutionYUnit), (ushort?)null);
            _jpegProc = CreateChangeTracker(nameof(JPEGProc), (ushort?)null);
            _jpegQuality = CreateChangeTracker(nameof(JPEGQuality), (ushort?)null);
            _dateTime = CreateChangeTracker(nameof(DateTime), (DateTime?)null);
            _title = CreateChangeTracker(nameof(Title), (string)null);
            _description = CreateChangeTracker(nameof(Description), (string)null);
            _copyright = CreateChangeTracker(nameof(Copyright), (string)null);
            _softwareUsed = CreateChangeTracker(nameof(SoftwareUsed), (string)null);
            _artist = CreateChangeTracker(nameof(Artist), (string)null);
            _hostComputer = CreateChangeTracker(nameof(HostComputer), (string)null);
            _upstreamId = CreateChangeTracker<Guid?>(nameof(UpstreamId), null);
            _lastSynchronizedOn = CreateChangeTracker<DateTime?>(nameof(LastSynchronizedOn), null);
            _modifiedOn = CreateChangeTracker(nameof(ModifiedOn), (_createdOn = CreateChangeTracker(nameof(CreatedOn), System.DateTime.Now)).GetValue());
        }

        public bool IsNew()
        {
            throw new NotImplementedException();
        }

        public bool IsSameDbRow(IDbEntity other)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IFile> IExtendedProperties.Files => Files.Cast<IFile>();

        IEnumerable<ILocalFile> ILocalExtendedProperties.Files => Files.Cast<ILocalFile>();
    }
}
