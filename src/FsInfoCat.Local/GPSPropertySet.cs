using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class GPSPropertySet : LocalDbEntity, ILocalGPSPropertySet
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _areaInformation;
        private readonly IPropertyChangeTracker<double?> _latitudeDegrees;
        private readonly IPropertyChangeTracker<double?> _latitudeMinutes;
        private readonly IPropertyChangeTracker<double?> _latitudeSeconds;
        private readonly IPropertyChangeTracker<string> _latitudeRef;
        private readonly IPropertyChangeTracker<double?> _longitudeDegrees;
        private readonly IPropertyChangeTracker<double?> _longitudeMinutes;
        private readonly IPropertyChangeTracker<double?> _longitudeSeconds;
        private readonly IPropertyChangeTracker<string> _longitudeRef;
        private readonly IPropertyChangeTracker<string> _measureMode;
        private readonly IPropertyChangeTracker<string> _processingMethod;
        private readonly IPropertyChangeTracker<ByteValues> _versionID;
        private HashSet<DbFile> _files = new();

        #endregion

        #region Properties

        public Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public string AreaInformation { get => _areaInformation.GetValue(); set => _areaInformation.SetValue(value); }
        public double? LatitudeDegrees { get => _latitudeDegrees.GetValue(); set => _latitudeDegrees.SetValue(value); }
        public double? LatitudeMinutes { get => _latitudeMinutes.GetValue(); set => _latitudeMinutes.SetValue(value); }
        public double? LatitudeSeconds { get => _latitudeSeconds.GetValue(); set => _latitudeSeconds.SetValue(value); }
        public string LatitudeRef { get => _latitudeRef.GetValue(); set => _latitudeRef.SetValue(value); }
        public double? LongitudeDegrees { get => _longitudeDegrees.GetValue(); set => _longitudeDegrees.SetValue(value); }
        public double? LongitudeMinutes { get => _longitudeMinutes.GetValue(); set => _longitudeMinutes.SetValue(value); }
        public double? LongitudeSeconds { get => _longitudeSeconds.GetValue(); set => _longitudeSeconds.SetValue(value); }
        public string LongitudeRef { get => _longitudeRef.GetValue(); set => _longitudeRef.SetValue(value); }
        public string MeasureMode { get => _measureMode.GetValue(); set => _measureMode.SetValue(value); }
        public string ProcessingMethod { get => _processingMethod.GetValue(); set => _processingMethod.SetValue(value); }
        public ByteValues VersionID { get => _versionID.GetValue(); set => _versionID.SetValue(value); }

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
        public GPSPropertySet()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _areaInformation = AddChangeTracker<string>(nameof(AreaInformation), null);
            _latitudeDegrees = AddChangeTracker<double?>(nameof(LatitudeDegrees), null);
            _latitudeMinutes = AddChangeTracker<double?>(nameof(LatitudeMinutes), null);
            _latitudeSeconds = AddChangeTracker<double?>(nameof(LatitudeSeconds), null);
            _latitudeRef = AddChangeTracker<string>(nameof(LatitudeRef), null);
            _longitudeDegrees = AddChangeTracker<double?>(nameof(LongitudeDegrees), null);
            _longitudeMinutes = AddChangeTracker<double?>(nameof(LongitudeMinutes), null);
            _longitudeSeconds = AddChangeTracker<double?>(nameof(LongitudeSeconds), null);
            _longitudeRef = AddChangeTracker<string>(nameof(LongitudeRef), null);
            _measureMode = AddChangeTracker<string>(nameof(MeasureMode), null);
            _processingMethod = AddChangeTracker<string>(nameof(ProcessingMethod), null);
            _versionID = AddChangeTracker<ByteValues>(nameof(VersionID), null);
        }

        internal static void BuildEntity(EntityTypeBuilder<GPSPropertySet> obj)
        {
            obj.Property(nameof(VersionID)).HasConversion(ByteValues.Converter);
        }

        internal static async Task RefreshAsync(EntityEntry<DbFile> entry, IFileDetailProvider fileDetailProvider, CancellationToken cancellationToken)
        {
            GPSPropertySet oldGPSPropertySet = entry.Entity.GPSPropertySetId.HasValue ? await entry.GetRelatedReferenceAsync(f => f.GPSProperties, cancellationToken) : null;
            IGPSProperties currentGPSProperties = await fileDetailProvider.GetGPSPropertiesAsync(cancellationToken);
            // TODO: Implement RefreshAsync(EntityEntry<DbFile>, IFileDetailProvider, CancellationToken)
            throw new NotImplementedException();
        }
    }
}
