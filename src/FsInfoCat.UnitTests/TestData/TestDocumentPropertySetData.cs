using System;
using System.Collections.ObjectModel;
using FsInfoCat.Collections;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestDocumentPropertySetData
    {
        public Guid UpstreamId { get; init; }

        public DateTime LastSynchronizedOn { get; init; }

        public DateTime CreatedOn { get; init; }
        public DateTime ModifiedOn { get; init; }

        public Guid Id { get; init; }

        public string ClientID { get; init; }

        public MultiStringValue Contributor { get; init; }

        public DateTime DateCreated { get; init; }

        public string LastAuthor { get; init; }

        public string RevisionNumber { get; init; }

        public int Security { get; init; }

        public string Division { get; init; }

        public string DocumentID { get; init; }

        public string Manager { get; init; }

        public string PresentationFormat { get; init; }

        public string Version { get; init; }

        public static readonly Collection<TestDocumentPropertySetData> Data = new();

        public static readonly TestDocumentPropertySetData Item1 = new()
        {
            Id = new("217a5039-3b01-4e0b-9e94-d0e06488a52f"),
            UpstreamId = new("96338fb6-f250-483a-8470-18d02c026912"),
            CreatedOn = new(637883126013814428L), // 2022-05-16T15:43:21.3814428
            ModifiedOn = new(637883126013814428L), // 2022-05-16T15:43:21.3814428
            LastSynchronizedOn = new(637883126013814428L) // 2022-05-16T15:43:21.3814428
        };

        public static readonly TestDocumentPropertySetData Item2 = new()
        {
            Id = new("48493f0f-e1d5-400c-a74c-b680f62ce83b"),
            UpstreamId = new("dc23f1fc-7d1f-45bb-a532-7c1123404163"),
            CreatedOn = new(637884590171594428L), // 2022-05-18T08:23:37.1594428
            ModifiedOn = new(637884590171594428L), // 2022-05-18T08:23:37.1594428
            LastSynchronizedOn = new(637884590171594428L) // 2022-05-18T08:23:37.1594428
        };
    }
}
