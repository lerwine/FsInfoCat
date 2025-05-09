using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Base class for entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="DRMPropertiesListItem" />
    /// <seealso cref="DRMPropertySet" />
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected virtual string PropertiesToString() => $@"IsProtected={IsProtected}, PlayCount={PlayCount}, DatePlayStarts={DatePlayStarts:yyyy-mm-ddTHH:mm:ss.fffffff}, DatePlayExpires={DatePlayExpires:yyyy-mm-ddTHH:mm:ss.fffffff},
    _description=""{ExtensionMethods.EscapeCsString(_description)}""";

        public override string ToString() => $@"{{ Id={(TryGetId(out Guid id) ? id : null)}, {PropertiesToString()},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId} }}";

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalDRMPropertiesRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ILocalDRMPropertiesRow other) => ArePropertiesEqual((IDRMPropertiesRow)other) &&
            EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
            LastSynchronizedOn == other.LastSynchronizedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IDRMPropertiesRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] IDRMPropertiesRow other) => ArePropertiesEqual((IDRMProperties)other) &&
            CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IDRMProperties" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
