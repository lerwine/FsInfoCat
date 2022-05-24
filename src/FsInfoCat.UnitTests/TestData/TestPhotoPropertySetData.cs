using System;
using System.Collections.ObjectModel;
using FsInfoCat.Collections;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestPhotoPropertySetData
    {
        public Guid UpstreamId { get; init; }

        public DateTime LastSynchronizedOn { get; init; }

        public DateTime CreatedOn { get; init; }
        public DateTime ModifiedOn { get; init; }

        public Guid Id { get; init; }

        public string CameraManufacturer { get; init; }

        public string CameraModel { get; init; }

        public DateTime DateTaken { get; init; }

        public MultiStringValue Event { get; init; }

        public string EXIFVersion { get; init; }

        public ushort Orientation { get; init; }

        public string OrientationText { get; init; }

        public MultiStringValue PeopleNames { get; init; }

        public static readonly Collection<TestPhotoPropertySetData> Data = new();

        public static readonly TestPhotoPropertySetData Item1 = new()
        {
            Id = new("da5abd01-fb63-406e-887d-035f13459587"),
            UpstreamId = new("780245d5-bea1-40cc-b20b-25bc5e8ac5af"),
            CreatedOn = new(637883031133574428L), // 2022-05-16T13:05:13.3574428
            ModifiedOn = new(637883031133574428L), // 2022-05-16T13:05:13.3574428
            LastSynchronizedOn = new(637883031133574428L) // 2022-05-16T13:05:13.3574428
        };

        public static readonly TestPhotoPropertySetData Item2 = new()
        {
            Id = new("69adb7ca-e59e-460c-9727-72099d90dd42"),
            UpstreamId = new("4ef5946e-6b82-4607-8e08-ed3263ed9437"),
            CreatedOn = new(637885431439424428L), // 2022-05-19T07:45:43.9424428
            ModifiedOn = new(637885431439424428L), // 2022-05-19T07:45:43.9424428
            LastSynchronizedOn = new(637885431439424428L) // 2022-05-19T07:45:43.9424428
        };
    }
}
