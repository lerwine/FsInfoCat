using FsInfoCat.Collections;
using System;

namespace FsInfoCat.Local
{
    public class PhotoPropertiesRow : PropertiesRow, IPhotoProperties
    {
        #region Fields

        private readonly IPropertyChangeTracker<string> _cameraManufacturer;
        private readonly IPropertyChangeTracker<string> _cameraModel;
        private readonly IPropertyChangeTracker<DateTime?> _dateTaken;
        private readonly IPropertyChangeTracker<MultiStringValue> _event;
        private readonly IPropertyChangeTracker<string> _exifVersion;
        private readonly IPropertyChangeTracker<ushort?> _orientation;
        private readonly IPropertyChangeTracker<string> _orientationText;
        private readonly IPropertyChangeTracker<MultiStringValue> _peopleNames;

        #endregion

        #region Properties

        public string CameraManufacturer { get => _cameraManufacturer.GetValue(); set => _cameraManufacturer.SetValue(value); }
        public string CameraModel { get => _cameraModel.GetValue(); set => _cameraModel.SetValue(value); }
        public DateTime? DateTaken { get => _dateTaken.GetValue(); set => _dateTaken.SetValue(value); }
        public MultiStringValue Event { get => _event.GetValue(); set => _event.SetValue(value); }
        public string EXIFVersion { get => _exifVersion.GetValue(); set => _exifVersion.SetValue(value); }
        public ushort? Orientation { get => _orientation.GetValue(); set => _orientation.SetValue(value); }
        public string OrientationText { get => _orientationText.GetValue(); set => _orientationText.SetValue(value); }
        public MultiStringValue PeopleNames { get => _peopleNames.GetValue(); set => _peopleNames.SetValue(value); }

        #endregion

        public PhotoPropertiesRow()
        {
            _cameraManufacturer = AddChangeTracker(nameof(CameraManufacturer), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _cameraModel = AddChangeTracker(nameof(CameraModel), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _dateTaken = AddChangeTracker<DateTime?>(nameof(DateTaken), null);
            _event = AddChangeTracker<MultiStringValue>(nameof(Event), null);
            _exifVersion = AddChangeTracker(nameof(EXIFVersion), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _orientation = AddChangeTracker<ushort?>(nameof(Orientation), null);
            _orientationText = AddChangeTracker(nameof(OrientationText), null, FilePropertiesComparer.StringValueCoersion);
            _peopleNames = AddChangeTracker<MultiStringValue>(nameof(PeopleNames), null);
        }
    }
}
