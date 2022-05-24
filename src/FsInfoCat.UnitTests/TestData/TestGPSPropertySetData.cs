using System;
using System.Collections.ObjectModel;
using FsInfoCat.Collections;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestGPSPropertySetData
    {
        public Guid UpstreamId { get; init; }

        public DateTime LastSynchronizedOn { get; init; }

        public DateTime CreatedOn { get; init; }
        public DateTime ModifiedOn { get; init; }

        public Guid Id { get; init; }

        public string AreaInformation { get; init; }

        public double LatitudeDegrees { get; init; }

        public double LatitudeMinutes { get; init; }

        public double LatitudeSeconds { get; init; }

        public string LatitudeRef { get; init; }

        public double LongitudeDegrees { get; init; }

        public double LongitudeMinutes { get; init; }

        public double LongitudeSeconds { get; init; }

        public string LongitudeRef { get; init; }

        public string MeasureMode { get; init; }

        public string ProcessingMethod { get; init; }

        public ByteValues VersionID { get; init; }

        public static readonly Collection<TestGPSPropertySetData> Data = new();

        public static readonly TestGPSPropertySetData Item1 = new()
        {
            Id = new("d869afb4-3745-4add-b68d-12def32d4541"),
            UpstreamId = new("ab6f5ebe-7264-4e30-8b89-63f74663e90f"),
            CreatedOn = new(637884608797474428L), // 2022-05-18T08:54:39.7474428
            ModifiedOn = new(637884608797474428L), // 2022-05-18T08:54:39.7474428
            LastSynchronizedOn = new(637884608797474428L) // 2022-05-18T08:54:39.7474428
        };

        public static readonly TestGPSPropertySetData Item2 = new()
        {
            Id = new("89833860-d3c6-46e8-8eeb-831c878c6825"),
            UpstreamId = new("f55555fd-6586-4ac2-bdf8-6751618c01a8"),
            CreatedOn = new(637885577079174428L), // 2022-05-19T11:48:27.9174428
            ModifiedOn = new(637885577079174428L), // 2022-05-19T11:48:27.9174428
            LastSynchronizedOn = new(637885577079174428L) // 2022-05-19T11:48:27.9174428
        };
    }
}
