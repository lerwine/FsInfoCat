using System;

namespace FsInfoCat.Desktop.FileSystemDetail
{
    public record DRMPropertiesRecord : IDRMProperties
    {
        public DateTime? DatePlayExpires { get; init; }

        public DateTime? DatePlayStarts { get; init; }

        public string Description { get; init; }

        public bool? IsProtected { get; init; }

        public uint? PlayCount { get; init; }

        public bool Equals(IDRMProperties other)
        {
            // TODO: Implement Equals(IDRMProperties);
            throw new NotImplementedException();
        }
    }
}
