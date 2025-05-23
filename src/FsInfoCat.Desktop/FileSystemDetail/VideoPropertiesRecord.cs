using FsInfoCat.Collections;

namespace FsInfoCat.Desktop.FileSystemDetail
{
    public record VideoPropertiesRecord : Model.IVideoProperties
    {
        public string Compression { get; init; }

        public MultiStringValue Director { get; init; }

        public uint? EncodingBitrate { get; init; }

        public uint? FrameHeight { get; init; }

        public uint? FrameRate { get; init; }

        public uint? FrameWidth { get; init; }

        public uint? HorizontalAspectRatio { get; init; }

        public string StreamName { get; init; }

        public ushort? StreamNumber { get; init; }

        public uint? VerticalAspectRatio { get; init; }

        public bool Equals(Model.IVideoProperties other)
        {
            // TODO: Implement Equals(IVideoProperties);
            throw new System.NotImplementedException();
        }
    }
}
