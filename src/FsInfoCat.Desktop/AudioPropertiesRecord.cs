namespace FsInfoCat.Desktop
{
    public record AudioPropertiesRecord : IAudioProperties
    {
        public string Compression { get; init; }

        public uint? EncodingBitrate { get; init; }

        public string Format { get; init; }

        public bool? IsVariableBitrate { get; init; }

        public uint? SampleRate { get; init; }

        public uint? SampleSize { get; init; }

        public string StreamName { get; init; }

        public ushort? StreamNumber { get; init; }
    }
}
