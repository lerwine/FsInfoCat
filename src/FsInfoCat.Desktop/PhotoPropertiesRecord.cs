using System;

namespace FsInfoCat.Desktop
{
    public record PhotoPropertiesRecord : IPhotoProperties
    {
        public string CameraManufacturer { get; init; }

        public string CameraModel { get; init; }

        public DateTime? DateTaken { get; init; }

        public string[] Event { get; init; }

        public string EXIFVersion { get; init; }

        public ushort? Orientation { get; init; }

        public string OrientationText { get; init; }

        public string[] PeopleNames { get; init; }
    }
}
