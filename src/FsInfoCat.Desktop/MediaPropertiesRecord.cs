namespace FsInfoCat.Desktop
{
    public record MediaPropertiesRecord : IMediaProperties
    {
        public string ContentDistributor { get; init; }

        public string CreatorApplication { get; init; }

        public string CreatorApplicationVersion { get; init; }

        public string DateReleased { get; init; }

        public ulong? Duration { get; init; }

        public string DVDID { get; init; }

        public uint? FrameCount { get; init; }

        public MultiStringValue Producer { get; init; }

        public string ProtectionType { get; init; }

        public string ProviderRating { get; init; }

        public string ProviderStyle { get; init; }

        public string Publisher { get; init; }

        public string Subtitle { get; init; }

        public MultiStringValue Writer { get; init; }

        public uint? Year { get; init; }
    }
}
