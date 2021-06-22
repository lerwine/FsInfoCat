using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class PhotoPropertySet : LocalDbEntity, ILocalPhotoPropertySet
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _cameraManufacturer;
        private readonly IPropertyChangeTracker<string> _cameraModel;
        private readonly IPropertyChangeTracker<DateTime?> _dateTaken;
        private readonly IPropertyChangeTracker<string[]> _event;
        private readonly IPropertyChangeTracker<string> _exifVersion;
        private readonly IPropertyChangeTracker<ushort?> _orientation;
        private readonly IPropertyChangeTracker<string> _orientationText;
        private readonly IPropertyChangeTracker<string[]> _peopleNames;
        private HashSet<DbFile> _files = new();

        #endregion

        #region Properties

        public Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public string CameraManufacturer { get => _cameraManufacturer.GetValue(); set => _cameraManufacturer.SetValue(value); }
        public string CameraModel { get => _cameraModel.GetValue(); set => _cameraModel.SetValue(value); }
        public DateTime? DateTaken { get => _dateTaken.GetValue(); set => _dateTaken.SetValue(value); }
        public string[] Event { get => _event.GetValue(); set => _event.SetValue(value); }
        public string EXIFVersion { get => _exifVersion.GetValue(); set => _exifVersion.SetValue(value); }
        public ushort? Orientation { get => _orientation.GetValue(); set => _orientation.SetValue(value); }
        public string OrientationText { get => _orientationText.GetValue(); set => _orientationText.SetValue(value); }
        public string[] PeopleNames { get => _peopleNames.GetValue(); set => _peopleNames.SetValue(value); }

        public HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        #endregion

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        #endregion

        public PhotoPropertySet()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _cameraManufacturer = AddChangeTracker<string>(nameof(CameraManufacturer), null);
            _cameraModel = AddChangeTracker<string>(nameof(CameraModel), null);
            _dateTaken = AddChangeTracker<System.DateTime?>(nameof(DateTaken), null);
            _event = AddChangeTracker<string[]>(nameof(Event), null);
            _exifVersion = AddChangeTracker<string>(nameof(EXIFVersion), null);
            _orientation = AddChangeTracker<ushort?>(nameof(Orientation), null);
            _orientationText = AddChangeTracker<string>(nameof(OrientationText), null);
            _peopleNames = AddChangeTracker<string[]>(nameof(PeopleNames), null);
        }

        public static async Task ApplyAsync(EntityEntry<DbFile> fileEntry, LocalDbContext dbContext, System.Threading.CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
