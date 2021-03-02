using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;

namespace FsInfoCat.Models.DB
{
    [DisplayColumn("VolumeID", "DisplayName", false)]
    public class Volume : IVolume
    {
        #region Properties

        public string Name
        {
            get
            {
                string n = _displayName;
                if (string.IsNullOrWhiteSpace(n))
                {
                    n = _volumeName;
                    if (string.IsNullOrWhiteSpace(n))
                        n = _rootPathName;
                }
                return n;
            }
        }

        [Required()]
        [Key()]
        [Display(Name = "ID")]
        public Guid VolumeID { get; set; }

        public Guid? HostDeviceID { get; set; }

        public HostDevice Host { get; set; }

        public string HostName
        {
            get
            {
                HostDevice host = Host;
                return (host is null) ? "" : host.Name;
            }
        }

        #region DisplayName

        public const int Max_Length_DisplayName = 128;
        public const string DisplayName_DisplayName = "Display name";
        public const string PropertyName_DisplayName = "DisplayName";
        public const string Error_Message_DisplayName = "Display name too long.";
        private string _displayName = "";

        [MaxLength(Max_Length_DisplayName, ErrorMessage = Error_Message_DisplayName)]
        [Display(Name = DisplayName_DisplayName)]
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (value is null) ? "" : value; }
        }

        #endregion

        public FileUri RootUri { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #region RootPathName

        public const string DisplayName_RootPathName = "Root Path Name";
        public const string PropertyName_RootPathName = "RootPathName";
        public const int Max_Length_RootPathName = 1024;
        public const string Error_Message_RootPathName_Empty = "Root path name cannot be empty.";
        public const string Error_Message_RootPathName_Length = "Root path name too long.";
        public const string Error_Message_RootPathName_Invalid = "Invalid path or url.";
        private string _rootPathName = "";

        [Required(ErrorMessage = Error_Message_RootPathName_Empty)]
        [MaxLength(Max_Length_RootPathName, ErrorMessage = Error_Message_RootPathName_Length)]
        [RegularExpression(ModelHelper.PATTERN_PATH_OR_URL, ErrorMessage = Error_Message_RootPathName_Invalid)]
        [Display(Name = DisplayName_RootPathName, Description = "Enter a file URI or a windows file path.")]
        public string RootPathName
        {
            get { return _rootPathName; }
            set { _rootPathName = (value is null) ? "" : value; }
        }

        #endregion

        #region DriveFormat

        public const string DisplayName_DriveFormat = "Name of filesystem type";
        public const string PropertyName_DriveFormat = "DriveFormat";
        public const int Max_Length_DriveFormat = 128;
        public const string Error_Message_DriveFormat_Empty = "Name of filesystem cannot be empty.";
        public const string Error_Message_DriveFormat_Length = "Name of filesystem type too long.";
        private string _driveFormat = "";

        [Required(ErrorMessage = Error_Message_DriveFormat_Empty)]
        [MaxLength(Max_Length_DriveFormat, ErrorMessage = Error_Message_DriveFormat_Length)]
        [Display(Name = DisplayName_DriveFormat)]
        public string DriveFormat
        {
            get { return _driveFormat; }
            set { _driveFormat = (value is null) ? "" : value; }
        }

        #endregion

        #region VolumeName

        public const string DisplayName_VolumeName = "Volume name";
        public const string PropertyName_VolumeName = "VolumeName";
        public const int Max_Length_Volume = 128;
        public const string Error_Message_VolumeName_Empty = "Volume name cannot be empty.";
        public const string Error_Message_VolumeName_Length = "Volume name length too long.";
        private string _volumeName = "";

        [Required(ErrorMessage = Error_Message_VolumeName_Empty)]
        [MaxLength(Max_Length_Volume, ErrorMessage = Error_Message_VolumeName_Length)]
        [Display(Name = DisplayName_VolumeName)]
        public string VolumeName
        {
            get { return _volumeName; }
            set { _volumeName = (value is null) ? "" : value; }
        }

        #endregion

        [Required()]
        [Display(Name = "Volume Identifier")]
        public VolumeIdentifier Identifier { get; set; }

        [Required()]
        [Display(Name = "Max Name Length")]
        public uint MaxNameLength { get; set; }

        [Required()]
        [Display(Name = "Flags")]
        [EnumDataType(typeof(FileSystemFeature))]
        public FileSystemFeature Flags { get; set; }

        [Display(Name = "Is Inactive")]
        public bool IsInactive { get; set; }

        #region Notes

        private string _notes = "";

        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string Notes
        {
            get { return _notes; }
            set { _notes = (value is null) ? "" : value; }
        }

        #endregion

        #region Audit

        [Editable(false)]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [Editable(false)]
        [Display(Name = "Created By")]
        public Guid CreatedBy { get; set; }

        public Account Creator { get; set; }

        public string CreatorName
        {
            get
            {
                Account account = Creator;
                return (account is null) ? "" : account.Name;
            }
        }

        [Editable(false)]
        [Display(Name = "Modified On")]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; }

        [Editable(false)]
        [Display(Name = "Modified By")]
        public Guid ModifiedBy { get; set; }

        public Account Modifier { get; set; }

        public string ModifierName
        {
            get
            {
                Account account = Modifier;
                return (account is null) ? "" : account.Name;
            }
        }

        public bool CaseSensitive { get; set; }

        #endregion

        #endregion

        #region Constructors

        public Volume()
        {
            CreatedOn = ModifiedOn = DateTime.UtcNow;
        }

        public Volume(string displayName, HostDevice host, Guid createdBy) : this()
        {
            VolumeID = Guid.NewGuid();
            DisplayName = displayName;
            if (host is null)
                HostDeviceID = null;
            else
                HostDeviceID = host.HostDeviceID;
            CreatedBy = ModifiedBy = createdBy;
        }

        public Volume(string displayName, HostDevice host, Account createdBy) : this()
        {
            if (createdBy is null)
                throw new ArgumentNullException("createdBy");
            VolumeID = Guid.NewGuid();
            DisplayName = displayName;
            if (host is null)
                HostDeviceID = null;
            else
                HostDeviceID = host.HostDeviceID;
            Creator = Modifier = createdBy;
            CreatedBy = ModifiedBy = createdBy.AccountID;
        }

        #endregion

        public void Normalize()
        {
            if (VolumeID.Equals(Guid.Empty))
                VolumeID = Guid.NewGuid();
            _rootPathName = _rootPathName.Trim();
            _driveFormat = _driveFormat.Trim();
            _rootPathName = _rootPathName.Trim();
            _volumeName = _volumeName.Trim();
            if ((_displayName = ModelHelper.CoerceAsWsNormalized(_displayName)).Length == 0)
                _displayName = _volumeName;
            _notes = _notes.Trim();
            CreatedOn = ModelHelper.CoerceAsLocalTime(CreatedOn);
            ModifiedOn = ModelHelper.CoerceAsLocalTime(ModifiedOn);
            if (null != Creator)
                CreatedBy = Creator.AccountID;
            if (null != Modifier)
                ModifiedBy = Modifier.AccountID;
            if (null != Host)
                HostDeviceID = Host.HostDeviceID;
        }

        private void OnValidateAll(List<ValidationResult> result)
        {
            Validate(result, PropertyName_DisplayName);
            Validate(result, PropertyName_DriveFormat);
            Validate(result, PropertyName_RootPathName);
            Validate(result, PropertyName_VolumeName);
        }

        private void Validate(List<ValidationResult> result, string propertyName)
        {
            switch (propertyName)
            {
                case PropertyName_DisplayName:
                case DisplayName_DisplayName:
                    if (_displayName.Length > Max_Length_DisplayName)
                        result.Add(new ValidationResult(Error_Message_DisplayName, new string[] { PropertyName_DisplayName }));
                    break;
                case PropertyName_DriveFormat:
                case DisplayName_DriveFormat:
                    if (_driveFormat.Length == 0)
                        result.Add(new ValidationResult(Error_Message_DriveFormat_Empty, new string[] { PropertyName_DriveFormat }));
                    else if (_driveFormat.Length > Max_Length_DriveFormat)
                        result.Add(new ValidationResult(Error_Message_DriveFormat_Length, new string[] { PropertyName_DriveFormat }));
                    break;
                case PropertyName_RootPathName:
                case DisplayName_RootPathName:
                    if (_rootPathName.Length == 0)
                        result.Add(new ValidationResult(Error_Message_RootPathName_Empty, new string[] { PropertyName_RootPathName }));
                    else if (_rootPathName.Length > Max_Length_DisplayName)
                        result.Add(new ValidationResult(Error_Message_RootPathName_Length, new string[] { PropertyName_RootPathName }));
                    else if (!ModelHelper.PathOrUrlRegex.IsMatch(_rootPathName))
                        result.Add(new ValidationResult(Error_Message_RootPathName_Invalid, new string[] { PropertyName_RootPathName }));
                    break;
                case PropertyName_VolumeName:
                case DisplayName_VolumeName:
                    if (_volumeName.Length == 0)
                        result.Add(new ValidationResult(Error_Message_VolumeName_Empty, new string[] { PropertyName_VolumeName }));
                    else if (_volumeName.Length > Max_Length_DisplayName)
                        result.Add(new ValidationResult(Error_Message_VolumeName_Length, new string[] { PropertyName_VolumeName }));
                    break;
            }
        }

        public IList<ValidationResult> ValidateAll()
        {
            List<ValidationResult> result = new List<ValidationResult>();
            Normalize();
            OnValidateAll(result);
            return result;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();
            Normalize();
            if (string.IsNullOrEmpty(validationContext.DisplayName))
                OnValidateAll(result);
            else
                Validate(result, validationContext.DisplayName);
            return result;
        }
    }
}
