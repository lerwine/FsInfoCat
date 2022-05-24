using System;
using System.Collections.ObjectModel;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestImagePropertySetData
    {
        public Guid UpstreamId { get; init; }

        public DateTime LastSynchronizedOn { get; init; }

        public DateTime CreatedOn { get; init; }
        public DateTime ModifiedOn { get; init; }

        public Guid Id { get; init; }

        public uint BitDepth { get; init; }

        public ushort ColorSpace { get; init; }

        public double CompressedBitsPerPixel { get; init; }

        public ushort Compression { get; init; }

        public string CompressionText { get; init; }

        public double HorizontalResolution { get; init; }

        public uint HorizontalSize { get; init; }

        public string ImageID { get; init; }

        public short ResolutionUnit { get; init; }

        public double VerticalResolution { get; init; }

        public uint VerticalSize { get; init; }

        public static readonly Collection<TestImagePropertySetData> Data = new();

        public static readonly TestImagePropertySetData Item1 = new()
        {
            Id = new("807292ad-4984-4b77-af03-eed638e53d08"),
            UpstreamId = new("fdea4dd9-1e01-4949-a235-1945240de11c"),
            CreatedOn = new(637883917066944428L), // 2022-05-17T13:41:46.6944428
            ModifiedOn = new(637883917066944428L), // 2022-05-17T13:41:46.6944428
            LastSynchronizedOn = new(637883917066944428L) // 2022-05-17T13:41:46.6944428
        };

        public static readonly TestImagePropertySetData Item2 = new()
        {
            Id = new("eaa59d7e-bd21-478e-b233-df895a826229"),
            UpstreamId = new("740b1d7d-f6b6-4886-8326-2cb540aeda07"),
            CreatedOn = new(637883997071324428L), // 2022-05-17T15:55:07.1324428
            ModifiedOn = new(637883997071324428L), // 2022-05-17T15:55:07.1324428
            LastSynchronizedOn = new(637883997071324428L) // 2022-05-17T15:55:07.1324428
        };
    }
}
