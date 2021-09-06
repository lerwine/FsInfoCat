namespace FsInfoCat.Local
{
    public class AudioPropertiesRow : PropertiesRow, IAudioProperties
    {
        #region Fields

        private readonly IPropertyChangeTracker<string> _compression;
        private readonly IPropertyChangeTracker<uint?> _encodingBitrate;
        private readonly IPropertyChangeTracker<string> _format;
        private readonly IPropertyChangeTracker<bool?> _isVariableBitrate;
        private readonly IPropertyChangeTracker<uint?> _sampleRate;
        private readonly IPropertyChangeTracker<uint?> _sampleSize;
        private readonly IPropertyChangeTracker<string> _streamName;
        private readonly IPropertyChangeTracker<ushort?> _streamNumber;

        #endregion

        #region Properties

        public string Compression { get => _compression.GetValue(); set => _compression.SetValue(value); }
        public uint? EncodingBitrate { get => _encodingBitrate.GetValue(); set => _encodingBitrate.SetValue(value); }
        public string Format { get => _format.GetValue(); set => _format.SetValue(value); }
        public bool? IsVariableBitrate { get => _isVariableBitrate.GetValue(); set => _isVariableBitrate.SetValue(value); }
        public uint? SampleRate { get => _sampleRate.GetValue(); set => _sampleRate.SetValue(value); }
        public uint? SampleSize { get => _sampleSize.GetValue(); set => _sampleSize.SetValue(value); }
        public string StreamName { get => _streamName.GetValue(); set => _streamName.SetValue(value); }
        public ushort? StreamNumber { get => _streamNumber.GetValue(); set => _streamNumber.SetValue(value); }

        #endregion

        public AudioPropertiesRow()
        {
            _compression = AddChangeTracker(nameof(Compression), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _encodingBitrate = AddChangeTracker<uint?>(nameof(EncodingBitrate), null);
            _format = AddChangeTracker(nameof(Format), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _isVariableBitrate = AddChangeTracker<bool?>(nameof(IsVariableBitrate), null);
            _sampleRate = AddChangeTracker<uint?>(nameof(SampleRate), null);
            _sampleSize = AddChangeTracker<uint?>(nameof(SampleSize), null);
            _streamName = AddChangeTracker(nameof(StreamName), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _streamNumber = AddChangeTracker<ushort?>(nameof(StreamNumber), null);
        }
    }
}
