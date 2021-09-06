using FsInfoCat.Collections;

namespace FsInfoCat.Local
{
    public class VideoPropertiesRow : PropertiesRow, IVideoProperties
    {
        #region Fields

        private readonly IPropertyChangeTracker<string> _compression;
        private readonly IPropertyChangeTracker<MultiStringValue> _director;
        private readonly IPropertyChangeTracker<uint?> _encodingBitrate;
        private readonly IPropertyChangeTracker<uint?> _frameHeight;
        private readonly IPropertyChangeTracker<uint?> _frameRate;
        private readonly IPropertyChangeTracker<uint?> _frameWidth;
        private readonly IPropertyChangeTracker<uint?> _horizontalAspectRatio;
        private readonly IPropertyChangeTracker<string> _streamName;
        private readonly IPropertyChangeTracker<ushort?> _streamNumber;
        private readonly IPropertyChangeTracker<uint?> _verticalAspectRatio;

        #endregion

        #region Properties

        public string Compression { get => _compression.GetValue(); set => _compression.SetValue(value); }
        public MultiStringValue Director { get => _director.GetValue(); set => _director.SetValue(value); }
        public uint? EncodingBitrate { get => _encodingBitrate.GetValue(); set => _encodingBitrate.SetValue(value); }
        public uint? FrameHeight { get => _frameHeight.GetValue(); set => _frameHeight.SetValue(value); }
        public uint? FrameRate { get => _frameRate.GetValue(); set => _frameRate.SetValue(value); }
        public uint? FrameWidth { get => _frameWidth.GetValue(); set => _frameWidth.SetValue(value); }
        public uint? HorizontalAspectRatio { get => _horizontalAspectRatio.GetValue(); set => _horizontalAspectRatio.SetValue(value); }
        public string StreamName { get => _streamName.GetValue(); set => _streamName.SetValue(value); }
        public ushort? StreamNumber { get => _streamNumber.GetValue(); set => _streamNumber.SetValue(value); }
        public uint? VerticalAspectRatio { get => _verticalAspectRatio.GetValue(); set => _verticalAspectRatio.SetValue(value); }

        #endregion

        public VideoPropertiesRow()
        {
            _compression = AddChangeTracker(nameof(Compression), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _director = AddChangeTracker<MultiStringValue>(nameof(Director), null);
            _encodingBitrate = AddChangeTracker<uint?>(nameof(EncodingBitrate), null);
            _frameHeight = AddChangeTracker<uint?>(nameof(FrameHeight), null);
            _frameRate = AddChangeTracker<uint?>(nameof(FrameRate), null);
            _frameWidth = AddChangeTracker<uint?>(nameof(FrameWidth), null);
            _horizontalAspectRatio = AddChangeTracker<uint?>(nameof(HorizontalAspectRatio), null);
            _streamName = AddChangeTracker(nameof(StreamName), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _streamNumber = AddChangeTracker<ushort?>(nameof(StreamNumber), null);
            _verticalAspectRatio = AddChangeTracker<uint?>(nameof(VerticalAspectRatio), null);
        }
    }
}
