using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public abstract class DRMPropertiesRow : PropertiesRow, ILocalDRMPropertiesRow
    {
        private string _description = string.Empty;

        public DateTime? DatePlayExpires { get; set; }

        public DateTime? DatePlayStarts { get; set; }

        public string Description { get => _description; set => _description = value.EmptyIfNullOrWhiteSpace(); }

        public bool? IsProtected { get; set; }

        public uint? PlayCount { get; set; }

        protected virtual bool ArePropertiesEqual([DisallowNull] IDRMProperties other)
        {
            throw new NotImplementedException();
        }

        public abstract bool Equals(IDRMPropertiesRow other);

        public abstract bool Equals(IDRMProperties other);
    }
}
