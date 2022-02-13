namespace FsInfoCat.Local
{
    public class AudioPropertiesRow : PropertiesRow, IAudioProperties
    {
        #region Fields

        private string _compression = string.Empty;
        private string _format = string.Empty;
        private string _streamName = string.Empty;

        #endregion

        #region Properties

        public string Compression { get => _compression; set => _compression = value.AsWsNormalizedOrEmpty(); }

        public uint? EncodingBitrate { get; set; }

        public string Format { get => _format; set => _format = value.AsWsNormalizedOrEmpty(); }

        public bool? IsVariableBitrate { get; set; }

        public uint? SampleRate { get; set; }

        public uint? SampleSize { get; set; }

        public string StreamName { get => _streamName; set => _streamName = value.AsWsNormalizedOrEmpty(); }

        public ushort? StreamNumber { get; set; }

        #endregion
    }
}
