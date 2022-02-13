using FsInfoCat.Collections;
using System;

namespace FsInfoCat.Local
{
    public class PhotoPropertiesRow : PropertiesRow, IPhotoProperties
    {
        #region Fields

        private string _cameraManufacturer = string.Empty;
        private string _cameraModel = string.Empty;
        private string _exifVersion = string.Empty;
        private string _orientationText = string.Empty;

        #endregion

        #region Properties

        public string CameraManufacturer { get => _cameraManufacturer; set => _cameraManufacturer = value.AsWsNormalizedOrEmpty(); }

        public string CameraModel { get => _cameraModel; set => _cameraModel = value.AsWsNormalizedOrEmpty(); }

        public DateTime? DateTaken { get; set; }

        public MultiStringValue Event { get; set; }

        public string EXIFVersion { get => _exifVersion; set => _exifVersion = value.AsWsNormalizedOrEmpty(); }

        public ushort? Orientation { get; set; }

        public string OrientationText { get => _orientationText; set => _orientationText = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue PeopleNames { get; set; }

        #endregion
    }
}
