using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public abstract class DRMPropertiesRow : PropertiesRow, ILocalDRMPropertiesRow
    {
        private string _description = string.Empty;

        public DateTime? DatePlayExpires { get; set; }

        public DateTime? DatePlayStarts { get; set; }

        [NotNull]
        [BackingField(nameof(_description))]
        public string Description { get => _description; set => _description = value.EmptyIfNullOrWhiteSpace(); }

        public bool? IsProtected { get; set; }

        public uint? PlayCount { get; set; }

        protected bool ArePropertiesEqual([DisallowNull] ILocalDRMPropertiesRow other) => ArePropertiesEqual((IDRMPropertiesRow)other) &&
            EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
            LastSynchronizedOn == other.LastSynchronizedOn;

        protected bool ArePropertiesEqual([DisallowNull] IDRMPropertiesRow other) => ArePropertiesEqual((IDRMProperties)other) &&
            CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn;

        protected virtual bool ArePropertiesEqual([DisallowNull] IDRMProperties other) => _description == other.Description &&
            DatePlayExpires == other.DatePlayExpires &&
            DatePlayStarts == other.DatePlayStarts &&
            IsProtected == other.IsProtected &&
            PlayCount == other.PlayCount;
        //EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
        //LastSynchronizedOn == other.LastSynchronizedOn &&
        //CreatedOn == other.CreatedOn &&
        //ModifiedOn == other.ModifiedOn;

        public abstract bool Equals(IDRMPropertiesRow other);

        public abstract bool Equals(IDRMProperties other);

        public override int GetHashCode()
        {
            if (TryGetId(out Guid id)) return id.GetHashCode();
            HashCode hash = new();
            hash.Add(_description);
            hash.Add(DatePlayExpires);
            hash.Add(DatePlayStarts);
            hash.Add(IsProtected);
            hash.Add(PlayCount);
            hash.Add(UpstreamId);
            hash.Add(LastSynchronizedOn);
            hash.Add(CreatedOn);
            hash.Add(ModifiedOn);
            return hash.ToHashCode();
        }
    }
}
