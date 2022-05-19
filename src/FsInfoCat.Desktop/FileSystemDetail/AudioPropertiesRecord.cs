namespace FsInfoCat.Desktop
{
    public record AudioPropertiesRecord : Model.IAudioProperties
    {
        public string Compression { get; init; }

        public uint? EncodingBitrate { get; init; }

        public string Format { get; init; }

        public bool? IsVariableBitrate { get; init; }

        public uint? SampleRate { get; init; }

        public uint? SampleSize { get; init; }

        public string StreamName { get; init; }

        public ushort? StreamNumber { get; init; }

        public bool Equals(Model.IAudioProperties other)
        {
            // TODO: Implement Equals(IAudioProperties);
            throw new System.NotImplementedException();
        }
    }
}
