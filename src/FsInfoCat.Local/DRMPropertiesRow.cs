using System;

namespace FsInfoCat.Local
{
    public class DRMPropertiesRow : PropertiesRow, IDRMProperties
    {
        private string _description = string.Empty;

        public DateTime? DatePlayExpires { get; set; }

        public DateTime? DatePlayStarts { get; set; }

        public string Description { get => _description; set => _description = value.EmptyIfNullOrWhiteSpace(); }

        public bool? IsProtected { get; set; }

        public uint? PlayCount { get; set; }
    }
}
