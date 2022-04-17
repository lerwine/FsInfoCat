using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
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

        protected bool ArePropertiesEqual([DisallowNull] IPhotoProperties other)
        {
            throw new NotImplementedException();
        }

        public abstract bool Equals(IPhotoPropertiesRow other);

        public abstract bool Equals(IPhotoProperties other);
    }
}
