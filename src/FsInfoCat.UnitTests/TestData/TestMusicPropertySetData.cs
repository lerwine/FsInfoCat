using System;
using FsInfoCat.Collections;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestMusicPropertySetData
    {
        public Guid UpstreamId { get; init; }

        public DateTime LastSynchronizedOn { get; init; }

        public DateTime CreatedOn { get; init; }
        public DateTime ModifiedOn { get; init; }

        public Guid Id { get; init; }

        public string AlbumArtist { get; init; }

        public string AlbumTitle { get; init; }

        public MultiStringValue Artist { get; init; }

        public uint ChannelCount { get; init; }

        public MultiStringValue Composer { get; init; }

        public MultiStringValue Conductor { get; init; }

        public string DisplayArtist { get; init; }

        public MultiStringValue Genre { get; init; }

        public string PartOfSet { get; init; }

        public string Period { get; init; }

        public uint TrackNumber { get; init; }

        public static readonly TestMusicPropertySetData Item1 = new()
        {
            Id = new("4bff15ae-7363-460a-84bc-f4a566ab9aad"),
            UpstreamId = new("250dd7db-1a11-41a3-84c7-962d5c148fd3"),
            CreatedOn = new(637883735707224428L), // 2022-05-17T08:39:30.7224428
            ModifiedOn = new(637883735707224428L), // 2022-05-17T08:39:30.7224428
            LastSynchronizedOn = new(637883735707224428L) // 2022-05-17T08:39:30.7224428
        };

        public static readonly TestMusicPropertySetData Item2 = new()
        {
            Id = new("1e76f66e-6d43-4292-96a4-747795990237"),
            UpstreamId = new("4d6d7f7e-32c0-4155-9128-cfd1f95131aa"),
            CreatedOn = new(637886533855324428L), // 2022-05-20T14:23:05.5324428
            ModifiedOn = new(637886533855324428L), // 2022-05-20T14:23:05.5324428
            LastSynchronizedOn = new(637886533855324428L) // 2022-05-20T14:23:05.5324428
        };
    }
}
