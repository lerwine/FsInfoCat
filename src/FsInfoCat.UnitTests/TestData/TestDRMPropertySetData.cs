using System;
using System.Collections.ObjectModel;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestDRMPropertySetData
    {
        public Guid UpstreamId { get; init; }

        public DateTime LastSynchronizedOn { get; init; }

        public DateTime CreatedOn { get; init; }
        public DateTime ModifiedOn { get; init; }

        public Guid Id { get; init; }

        public DateTime DatePlayExpires { get; init; }

        public DateTime DatePlayStarts { get; init; }

        public string Description { get; init; }

        public bool IsProtected { get; init; }

        public uint PlayCount { get; init; }

        public static readonly Collection<TestDRMPropertySetData> Data = new();

        public static readonly TestDRMPropertySetData Item1 = new()
        {
            Id = new("833a1e59-1839-40e7-b039-f68e736730c7"),
            UpstreamId = new("a3361446-0d6b-4e6e-a6ff-acf69dd0cf84"),
            CreatedOn = new(637885611218414428L), // 2022-05-19T12:45:21.8414428
            ModifiedOn = new(637885611218414428L), // 2022-05-19T12:45:21.8414428
            LastSynchronizedOn = new(637885611218414428L) // 2022-05-19T12:45:21.8414428
        };

        public static readonly TestDRMPropertySetData Item2 = new()
        {
            Id = new("6fdb5c2b-aa87-4d43-b896-17808a02bbec"),
            UpstreamId = new("6e40365a-8d17-4f69-874f-ff0e93c7554c"),
            CreatedOn = new(637885776152124428L), // 2022-05-19T17:20:15.2124428
            ModifiedOn = new(637885776152124428L), // 2022-05-19T17:20:15.2124428
            LastSynchronizedOn = new(637885776152124428L) // 2022-05-19T17:20:15.2124428
        };
    }
}
