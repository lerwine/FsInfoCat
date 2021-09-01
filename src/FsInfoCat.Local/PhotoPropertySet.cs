using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class PhotoPropertiesListItem : PhotoPropertiesRow, ILocalPhotoPropertiesListItem
    {
        public const string VIEW_NAME = "vPhotoPropertiesListing";

        private readonly IPropertyChangeTracker<long> _existingFileCount;
        private readonly IPropertyChangeTracker<long> _totalFileCount;

        public long ExistingFileCount { get => _existingFileCount.GetValue(); set => _existingFileCount.SetValue(value); }

        public long TotalFileCount { get => _totalFileCount.GetValue(); set => _totalFileCount.SetValue(value); }

        public PhotoPropertiesListItem()
        {
            _existingFileCount = AddChangeTracker(nameof(ExistingFileCount), 0L);
            _totalFileCount = AddChangeTracker(nameof(TotalFileCount), 0L);
        }

        internal static void OnBuildEntity(EntityTypeBuilder<PhotoPropertiesListItem> builder) => builder.ToView(VIEW_NAME);
    }
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
    /// <summary>
    /// Class PhotoPropertySet.
    /// Implements the <see cref="LocalDbEntity" />
    /// Implements the <see cref="ILocalPhotoPropertySet" />
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalPhotoPropertySet" />
    public class PhotoPropertySet : PhotoPropertiesRow, ILocalPhotoPropertySet, ISimpleIdentityReference<PhotoPropertySet>
    {
        private HashSet<DbFile> _files = new();

        public HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        PhotoPropertySet IIdentityReference<PhotoPropertySet>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<PhotoPropertySet> builder)
        {
            if (builder is null)
                throw new ArgumentOutOfRangeException(nameof(builder));
            builder.Property(nameof(Event)).HasConversion(MultiStringValue.Converter);
            builder.Property(nameof(PeopleNames)).HasConversion(MultiStringValue.Converter);
        }

        internal static async Task RefreshAsync([DisallowNull] EntityEntry<DbFile> entry, [DisallowNull] IFileDetailProvider fileDetailProvider,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (entry is null)
                throw new ArgumentNullException(nameof(entry));
            if (fileDetailProvider is null)
                throw new ArgumentNullException(nameof(fileDetailProvider));
            switch (entry.State)
            {
                case EntityState.Detached:
                    throw new ArgumentOutOfRangeException(nameof(entry), $"{nameof(DbFile)} is detached");
                case EntityState.Deleted:
                    throw new ArgumentOutOfRangeException(nameof(entry), $"{nameof(DbFile)} is flagged for deletion");
            }
            if (entry.Context is not LocalDbContext dbContext)
                throw new ArgumentOutOfRangeException(nameof(entry), "Invalid database context");
            DbFile entity;
            PhotoPropertySet oldPropertySet = (entity = entry.Entity).PhotoPropertySetId.HasValue ?
                await entry.GetRelatedReferenceAsync(f => f.PhotoProperties, cancellationToken) : null;
            IPhotoProperties currentProperties = await fileDetailProvider.GetPhotoPropertiesAsync(cancellationToken);
            if (FilePropertiesComparer.Equals(oldPropertySet, currentProperties))
                return;
            if (currentProperties.IsNullOrAllPropertiesEmpty())
                entity.PhotoProperties = null;
            else
                entity.PhotoProperties = await dbContext.GetMatchingAsync(currentProperties, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (oldPropertySet is null)
                return;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    Guid id = entity.Id;
                    if (!(await dbContext.Entry(oldPropertySet).GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                        dbContext.PhotoPropertySets.Remove(oldPropertySet);
                    cancellationToken.ThrowIfCancellationRequested();
                    break;
            }
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers()
        {
            yield return Id;
        }
    }
}
