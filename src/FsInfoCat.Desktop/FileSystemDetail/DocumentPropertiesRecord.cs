using FsInfoCat.Collections;
using System;

namespace FsInfoCat.Desktop.FileSystemDetail
{
    public record DocumentPropertiesRecord : IDocumentProperties
    {
        public string ClientID { get; init; }

        public MultiStringValue Contributor { get; init; }

        public DateTime? DateCreated { get; init; }

        public string LastAuthor { get; init; }

        public string RevisionNumber { get; init; }

        public int? Security { get; init; }

        public string Division { get; init; }

        public string DocumentID { get; init; }

        public string Manager { get; init; }

        public string PresentationFormat { get; init; }

        public string Version { get; init; }
    }
}
