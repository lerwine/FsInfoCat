using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    // TODO: Document PhotoPropertiesRow class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract class PhotoPropertiesRow : PropertiesRow, ILocalPhotoPropertiesRow
    {
        #region Fields

        private string _cameraManufacturer = string.Empty;
        private string _cameraModel = string.Empty;
        private string _exifVersion = string.Empty;
        private string _orientationText = string.Empty;

        #endregion

        #region Properties

        [NotNull]
        [BackingField(nameof(_cameraManufacturer))]
        public string CameraManufacturer { get => _cameraManufacturer; set => _cameraManufacturer = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_cameraModel))]
        public string CameraModel { get => _cameraModel; set => _cameraModel = value.AsWsNormalizedOrEmpty(); }

        public DateTime? DateTaken { get; set; }

        public MultiStringValue Event { get; set; }

        [NotNull]
        [BackingField(nameof(_exifVersion))]
        public string EXIFVersion { get => _exifVersion; set => _exifVersion = value.AsWsNormalizedOrEmpty(); }

        public ushort? Orientation { get; set; }

        [NotNull]
        [BackingField(nameof(_orientationText))]
        public string OrientationText { get => _orientationText; set => _orientationText = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue PeopleNames { get; set; }

        #endregion

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalPhotoPropertiesRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ILocalPhotoPropertiesRow other) => ArePropertiesEqual((IPhotoPropertiesRow)other) &&
            EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
            LastSynchronizedOn == other.LastSynchronizedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IPhotoPropertiesRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] IPhotoPropertiesRow other) => ArePropertiesEqual((IPhotoProperties)other) &&
            CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IPhotoProperties" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] IPhotoProperties other) => _cameraManufacturer == other.CameraManufacturer &&
            _cameraModel == other.CameraModel &&
            _exifVersion == other.EXIFVersion &&
            _orientationText == other.OrientationText &&
            DateTaken == other.DateTaken &&
            EqualityComparer<MultiStringValue>.Default.Equals(Event, other.Event) &&
            Orientation == other.Orientation &&
            EqualityComparer<MultiStringValue>.Default.Equals(PeopleNames, other.PeopleNames);
        //EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
        //LastSynchronizedOn == other.LastSynchronizedOn &&
        //CreatedOn == other.CreatedOn &&
        //ModifiedOn == other.ModifiedOn;

        public abstract bool Equals(IPhotoPropertiesRow other);

        public abstract bool Equals(IPhotoProperties other);

        public override int GetHashCode()
        {
            if (TryGetId(out Guid id)) return id.GetHashCode();
            HashCode hash = new();
            hash.Add(_cameraManufacturer);
            hash.Add(_cameraModel);
            hash.Add(_exifVersion);
            hash.Add(_orientationText);
            hash.Add(DateTaken);
            hash.Add(Event);
            hash.Add(Orientation);
            hash.Add(PeopleNames);
            hash.Add(UpstreamId);
            hash.Add(LastSynchronizedOn);
            hash.Add(CreatedOn);
            hash.Add(ModifiedOn);
            return hash.ToHashCode();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
