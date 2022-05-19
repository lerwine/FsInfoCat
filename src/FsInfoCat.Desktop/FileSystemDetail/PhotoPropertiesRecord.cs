using FsInfoCat.Collections;
using System;

namespace FsInfoCat.Desktop.FileSystemDetail
{
    public record PhotoPropertiesRecord : Model.IPhotoProperties
    {
        public string CameraManufacturer { get; init; }

        public string CameraModel { get; init; }

        public DateTime? DateTaken { get; init; }

        public MultiStringValue Event { get; init; }

        public string EXIFVersion { get; init; }

        public ushort? Orientation { get; init; }

        public string OrientationText { get; init; }

        public MultiStringValue PeopleNames { get; init; }

        public bool Equals(Model.IPhotoProperties other)
        {
            // TODO: Implement Equals(IPhotoProperties);
            throw new NotImplementedException();
        }
    }
}
