using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public class FileSystem : LocalDbEntity, ILocalFileSystem
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _displayName;
        private readonly IPropertyChangeTracker<bool> _caseSensitiveSearch;
        private readonly IPropertyChangeTracker<bool> _readOnly;
        private readonly IPropertyChangeTracker<int> _maxNameLength;
        private readonly IPropertyChangeTracker<DriveType?> _defaultDriveType;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<bool> _isInactive;
        private HashSet<Volume> _volumes = new();
        private HashSet<SymbolicName> _symbolicNames = new();

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_LongName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string DisplayName { get => _displayName.GetValue(); set => _displayName.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CaseSensitiveSearch), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool CaseSensitiveSearch { get => _caseSensitiveSearch.GetValue(); set => _caseSensitiveSearch.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ReadOnly), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool ReadOnly { get => _readOnly.GetValue(); set => _readOnly.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MaxNameLength), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual int MaxNameLength { get => _maxNameLength.GetValue(); set => _maxNameLength.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DefaultDriveType), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DriveType? DefaultDriveType { get => _defaultDriveType.GetValue(); set => _defaultDriveType.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool IsInactive { get => _isInactive.GetValue(); set => _isInactive.SetValue(value); }

        public virtual HashSet<Volume> Volumes
        {
            get => _volumes;
            set => CheckHashSetChanged(_volumes, value, h => _volumes = h);
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_SymbolicNames), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<SymbolicName> SymbolicNames
        {
            get => _symbolicNames;
            set => CheckHashSetChanged(_symbolicNames, value, h => _symbolicNames = h);
        }

        #endregion

        #region Explicit Members

        IEnumerable<ILocalVolume> ILocalFileSystem.Volumes => _volumes.Cast<ILocalVolume>();

        IEnumerable<ILocalSymbolicName> ILocalFileSystem.SymbolicNames => _volumes.Cast<ILocalSymbolicName>();

        IEnumerable<IVolume> IFileSystem.Volumes => _volumes.Cast<IVolume>();

        IEnumerable<ISymbolicName> IFileSystem.SymbolicNames => _volumes.Cast<ISymbolicName>();

        #endregion

        public FileSystem()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _displayName = AddChangeTracker(nameof(DisplayName), "", TrimmedNonNullStringCoersion.Default);
            _caseSensitiveSearch = AddChangeTracker(nameof(CaseSensitiveSearch), false);
            _readOnly = AddChangeTracker(nameof(ReadOnly), false);
            _maxNameLength = AddChangeTracker(nameof(MaxNameLength), DbConstants.DbColDefaultValue_MaxNameLength);
            _defaultDriveType = AddChangeTracker<DriveType?>(nameof(DefaultDriveType), null);
            _notes = AddChangeTracker(nameof(Notes), "", NonNullStringCoersion.Default);
            _isInactive = AddChangeTracker(nameof(IsInactive), false);
        }

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (string.IsNullOrWhiteSpace(validationContext.MemberName))
            {
                ValidateDriveType(results);
                ValidateDisplayName(validationContext, results);
            }
            else
                switch (validationContext.MemberName)
                {
                    case nameof(DefaultDriveType):
                        ValidateDriveType(results);
                        break;
                    case nameof(DisplayName):
                        ValidateDisplayName(validationContext, results);
                        break;
                }
        }

        private void ValidateDriveType(List<ValidationResult> results)
        {
            var driveType = DefaultDriveType;
            if (driveType.HasValue && !Enum.IsDefined(driveType.Value))
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DriveTypeInvalid, new string[] { nameof(DefaultDriveType) }));
        }

        private XElement Export(IEnumerable<SymbolicName> symbolicNames, IEnumerable<Volume> volumes)
        {
            XElement result = new(nameof(FileSystem),
                new XAttribute(nameof(Id), XmlConvert.ToString(Id)),
                new XAttribute(nameof(DisplayName), DisplayName)
            );
            if (CaseSensitiveSearch)
                result.SetAttributeValue(nameof(CaseSensitiveSearch), CaseSensitiveSearch);
            if (ReadOnly)
                result.SetAttributeValue(nameof(ReadOnly), ReadOnly);
            if (IsInactive)
                result.SetAttributeValue(nameof(IsInactive), IsInactive);
            if (MaxNameLength != DbConstants.DbColDefaultValue_MaxNameLength)
                result.SetAttributeValue(nameof(MaxNameLength), MaxNameLength);
            DriveType? defaultDriveType = DefaultDriveType;
            if (defaultDriveType.HasValue)
                result.SetAttributeValue(nameof(DefaultDriveType), Enum.GetName(typeof(DriveType), defaultDriveType.Value));
            AddExportAttributes(result);
            foreach (SymbolicName symbolicName in symbolicNames)
                result.Add(symbolicName.Export());
            foreach (Volume volume in volumes)
                result.Add(volume.Export());
            return result;
        }

        public XElement Export(bool includeSymbolicNames = false, bool includeVolumes = false) => Export(includeSymbolicNames ? SymbolicNames.AsEnumerable() : Enumerable.Empty<SymbolicName>(),
            includeVolumes ? Volumes.AsEnumerable() : Enumerable.Empty<Volume>());

        public XElement Export(Func<Volume, bool> filter, bool includeSymbolicNames = false) => Export(includeSymbolicNames ? SymbolicNames.AsEnumerable() : Enumerable.Empty<SymbolicName>(), Volumes.Where(filter));

        public XElement Export(Func<SymbolicName, bool> filter, bool includeVolumes = false) => Export(SymbolicNames.Where(filter), includeVolumes ? Volumes.AsEnumerable() : Enumerable.Empty<Volume>());

        public XElement Export(Func<SymbolicName, bool> symbolicNameFilter, Func<Volume, bool> volumeFilter) => Export(SymbolicNames.Where(symbolicNameFilter), Volumes.Where(volumeFilter));

        internal static void Import(LocalDbContext dbContext, ILogger<LocalDbContext> logger, XElement fileSystemElement)
        {
            XName n = nameof(Id);
            Guid fileSystemId = fileSystemElement.GetAttributeGuid(n).Value;
            StringBuilder sql = new StringBuilder("INSERT INTO \"").Append(nameof(LocalDbContext.FileSystems)).Append("\" (\"").Append(nameof(Id)).Append('"');
            List<object> values = new();
            values.Add(fileSystemId);
            foreach (XAttribute attribute in fileSystemElement.Attributes().Where(a => a.Name != n))
            {
                sql.Append(", \"").Append(attribute.Name.LocalName).Append('"');
                switch (attribute.Name.LocalName)
                {
                    case nameof(DisplayName):
                    case nameof(Notes):
                        values.Add(attribute.Value);
                        break;
                    case nameof(CaseSensitiveSearch):
                    case nameof(ReadOnly):
                    case nameof(IsInactive):
                        values.Add(XmlConvert.ToBoolean(attribute.Value));
                        break;
                    case nameof(MaxNameLength):
                        values.Add(XmlConvert.ToInt32(attribute.Value));
                        break;
                    case nameof(DefaultDriveType):
                        if (string.IsNullOrWhiteSpace(attribute.Value))
                            values.Add(null);
                        else
                            values.Add(Enum.ToObject(typeof(DriveType), Enum.Parse<DriveType>(attribute.Value)));
                        break;
                    case nameof(CreatedOn):
                    case nameof(ModifiedOn):
                    case nameof(LastSynchronizedOn):
                        values.Add(XmlConvert.ToDateTime(attribute.Value, XmlDateTimeSerializationMode.RoundtripKind));
                        break;
                    case nameof(UpstreamId):
                        values.Add(XmlConvert.ToGuid(attribute.Value));
                        break;
                    default:
                        throw new NotSupportedException($"Attribute {attribute.Name} is not supported for {nameof(FileSystem)}");
                }
            }
            sql.Append(") Values({0}");
            for (int i = 1; i < values.Count; i++)
                sql.Append(", {").Append(i).Append('}');
            logger.LogInformation($"Inserting {nameof(FileSystem)} with Id {{Id}}", fileSystemId);
            dbContext.Database.ExecuteSqlRaw(sql.Append(')').ToString(), values.ToArray());
            foreach (XElement symbolicNameElement in fileSystemElement.Elements(nameof(SymbolicName)))
                SymbolicName.Import(dbContext, logger, fileSystemId, symbolicNameElement);
            foreach (XElement volumeElement in fileSystemElement.Elements(nameof(Volume)))
                Volume.Import(dbContext, logger, fileSystemId, volumeElement);
        }

        private void ValidateDisplayName(ValidationContext validationContext, List<ValidationResult> results)
        {
            string displayName = DisplayName;
            if (string.IsNullOrEmpty(displayName))
                return;
            Guid id = Id;
            LocalDbContext dbContext = validationContext.GetService<LocalDbContext>();
            if (dbContext is not null)
            {
                if (dbContext.FileSystems.Any(fs => id != fs.Id && fs.DisplayName == displayName))
                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateDisplayName, new string[] { nameof(DisplayName) }));
            }
        }
    }
}
