using System;

namespace FsInfoCat.Desktop.FileSystemDetail
{
    public record RecordedTVPropertiesRecord : Model.IRecordedTVProperties
    {
        public uint? ChannelNumber { get; init; }

        public string EpisodeName { get; init; }

        public bool? IsDTVContent { get; init; }

        public bool? IsHDContent { get; init; }

        public string NetworkAffiliation { get; init; }

        public DateTime? OriginalBroadcastDate { get; init; }

        public string ProgramDescription { get; init; }

        public string StationCallSign { get; init; }

        public string StationName { get; init; }

        public bool Equals(Model.IRecordedTVProperties other)
        {
            // TODO: Implement Equals(IRecordedTVProperties);
            throw new NotImplementedException();
        }
    }
}
