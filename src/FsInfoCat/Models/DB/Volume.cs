using FsInfoCat.Models.HostDevices;
using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace FsInfoCat.Models.DB
{
    [DisplayColumn(PropertyName_VolumeID, PropertyName_DisplayName, false)]
    public class Volume : IVolume
    {
        public const int Max_Length_DisplayName = 128;
        public const string PropertyName_VolumeID = nameof(VolumeID);
        public const string PropertyName_HostDeviceID = nameof(HostDeviceID);
        public const string PropertyName_Host = nameof(Host);
        public const string DisplayName_DisplayName = "Display name";
        public const string PropertyName_DisplayName = nameof(DisplayName);
        public const string Error_Message_DisplayName = "Display name too long.";
        public const string DisplayName_RootPathName = "Root Path Name";
        public const string PropertyName_RootPathName = nameof(RootPathName);
        public const int Max_Length_RootPathName = 1024;
        public const string Error_Message_RootPathName_Empty = "Root path name cannot be empty.";
        public const string Error_Message_RootPathName_Length = "Root path name too long.";
        public const string Error_Message_RootPathName_Invalid = "Invalid path or url.";
        public const string DisplayName_DriveFormat = "Name of filesystem type";
        public const string PropertyName_DriveFormat = nameof(DriveFormat);
        public const int Max_Length_DriveFormat = 128;
        public const string Error_Message_DriveFormat_Empty = "Name of filesystem cannot be empty.";
        public const string Error_Message_DriveFormat_Length = "Name of filesystem type too long.";
        public const string DisplayName_VolumeName = "Volume name";
        public const string PropertyName_VolumeName = nameof(VolumeName);
        public const int Max_Length_Volume = 128;
        public const string Error_Message_VolumeName_Empty = "Volume name cannot be empty.";
        public const string Error_Message_VolumeName_Length = "Volume name length too long.";

        private StringComparer _segmentNameComparer;
        private Guid _volumeID;
        private Guid? _hostDeviceID;
        private HostDevice _host;
        private string _displayName = "";
        private FileUri _rootUri = new FileUri("");
        private string _rootPathName = "";
        private string _driveFormat = "";
        private string _volumeName = "";
        private VolumeIdentifier _identifier;
        private uint _maxNameLength;
        private bool _caseSensitive;
        private bool _isInactive;
        private string _notes = "";
        private DateTime _createdOn;
        private Guid _createdBy;
        private Account _creator;
        private DateTime _modifiedOn;
        private Guid _modifiedBy;
        private Account _modifier;

        public event PropertyValueChangeEventHandler PropertyValueChanging;
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyValueChangeEventHandler PropertyValueChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        public string Name => CalculateName(_displayName, _volumeName, _rootPathName);

        [Required()]
        [Key()]
        [Display(Name = "ID")]
        public Guid VolumeID
        {
            get => _volumeID;
            set
            {
                Monitor.Enter(this);
                try
                {
                    if (value == _volumeID)
                        return;
                    RaisePropertyValueChanging(nameof(VolumeID), _volumeID, value);
                    Guid oldValue = _volumeID;
                    _volumeID = value;
                    RaisePropertyValueChanged(nameof(VolumeID), oldValue, _volumeID);
                }
                finally { Monitor.Exit(this); }
            }
        }

        public Guid? HostDeviceID
        {
            get => _hostDeviceID;
            set
            {
                Monitor.Enter(this);
                try
                {
                    if ((value.HasValue) ? _hostDeviceID.HasValue && value.Value.Equals(_hostDeviceID.Value) : !_hostDeviceID.HasValue)
                        return;
                    RaisePropertyValueChanging(nameof(HostDeviceID), _hostDeviceID, value);
                    Guid? oldValue = _hostDeviceID;
                    _hostDeviceID = value;
                    RaisePropertyValueChanged(nameof(HostDeviceID), oldValue, _volumeID);
                }
                finally { Monitor.Exit(this); }
            }
        }

        public HostDevice Host
        {
            get => _host;
            set
            {
                Monitor.Enter(this);
                try
                {
                    if ((value is null) ? _host is null : ReferenceEquals(value, _host))
                        return;
                    string oldHostName = HostName;
                    string newHostName = (value is null) ? "" : value.Name;
                    RaisePropertyValueChanging(nameof(Host), _host, value);
                    if (!((oldHostName is null) ? newHostName is null : oldHostName.Equals(newHostName)))
                        RaisePropertyValueChanging(nameof(HostName), oldHostName, newHostName);
                    Guid? newHostDeviceId = (value is null) ? null : (Guid?)value.HostDeviceID;
                    if ((_hostDeviceID.HasValue) ? !(newHostDeviceId.HasValue && _hostDeviceID.Value.Equals(newHostDeviceId.Value)) : newHostDeviceId.HasValue)
                        RaisePropertyValueChanging(nameof(HostDeviceID), _hostDeviceID, newHostDeviceId);
                    HostDevice oldValue = _host;
                    _host = value;
                    Guid? oldHostDeviceId = _hostDeviceID;
                    _hostDeviceID = (value is null) ? null : (Guid?)value.HostDeviceID;
                    try { RaisePropertyValueChanged(nameof(Host), oldValue, _host); }
                    finally
                    {
                        try
                        {
                            newHostName = (value is null) ? "" : value.Name;
                            if (!((oldHostName is null) ? newHostName is null : oldHostName.Equals(newHostName)))
                                RaisePropertyValueChanged(nameof(HostName), oldHostName, newHostName);
                        }
                        finally
                        {
                            if ((oldHostDeviceId.HasValue) ? !(_hostDeviceID.HasValue && oldHostDeviceId.Value.Equals(_hostDeviceID.Value)) : _hostDeviceID.HasValue)
                                RaisePropertyValueChanged(nameof(HostDeviceID), oldHostDeviceId, _hostDeviceID);
                        }
                    }
                }
                finally { Monitor.Exit(this); }
            }
        }

        public string HostName
        {
            get
            {
                HostDevice host = Host;
                return (host is null) ? "" : host.Name;
            }
        }

        [MaxLength(Max_Length_DisplayName, ErrorMessage = Error_Message_DisplayName)]
        [Display(Name = DisplayName_DisplayName)]
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                Monitor.Enter(this);
                try
                {
                    string newValue = value ?? "";
                    if (_displayName.Equals(newValue))
                        return;
                    RaisePropertyValueChanging(nameof(DisplayName), _displayName, newValue);
                    string oldName = CalculateName(_displayName, _volumeName, _rootPathName);
                    string newName = CalculateName(newValue, _volumeName, _rootPathName);
                    if (!oldName.Equals(newName))
                        RaisePropertyValueChanging(nameof(Name), oldName, newName);
                    string oldValue = _displayName;
                    _displayName = newValue;
                    try { RaisePropertyValueChanged(nameof(DisplayName), oldValue, _displayName); }
                    finally
                    {
                        newName = CalculateName(_displayName, _volumeName, _rootPathName);
                        if (!oldName.Equals(newName))
                            RaisePropertyValueChanged(nameof(Name), oldName, newName);
                    }
                }
                finally { Monitor.Exit(this); }
            }
        }

        public FileUri RootUri
        {
            get => _rootUri;
            set
            {
                Monitor.Enter(this);
                try
                {
                    FileUri newValue;
                    if (value is null)
                    {
                        if (_rootUri.IsEmpty())
                            return;
                        newValue = new FileUri("");
                    }
                    else
                    {
                        if (ReferenceEquals(value, _rootUri))
                            return;
                        newValue = value;
                    }
                    RaisePropertyValueChanging(nameof(RootUri), _rootUri, newValue);
                    string oldName = CalculateName(_displayName, _volumeName, _rootPathName);
                    string newName = CalculateName(_rootUri.ToLocalPath(), _volumeName, _rootPathName);
                    if (!oldName.Equals(newName))
                        RaisePropertyValueChanging(nameof(Name), oldName, newName);
                    string newRootPathName = _rootUri.ToLocalPath();
                    if (!_rootPathName.Equals(newRootPathName))
                        RaisePropertyValueChanging(nameof(RootPathName), _rootPathName, newRootPathName);
                    FileUri oldValue = _rootUri;
                    _rootUri = newValue;
                    string oldRootPathName = _rootPathName;
                    _rootPathName = _rootUri.ToLocalPath();
                    try { RaisePropertyValueChanged(nameof(RootUri), oldValue, _rootUri); }
                    finally
                    {
                        try
                        {
                            if (!oldRootPathName.Equals(_rootPathName))
                                RaisePropertyValueChanged(nameof(RootPathName), oldRootPathName, _rootPathName);
                        }
                        finally
                        {
                            newName = CalculateName(_displayName, _volumeName, _rootPathName);
                            if (!oldName.Equals(newName))
                                RaisePropertyValueChanged(nameof(Name), oldName, newName);
                        }
                    }
                }
                finally { Monitor.Exit(this); }
            }
        }

        [Required(ErrorMessage = Error_Message_RootPathName_Empty)]
        [MaxLength(Max_Length_RootPathName, ErrorMessage = Error_Message_RootPathName_Length)]
        [RegularExpression(FileUriConverter.PATTERN_ANY_ABS_FILE_URI_OR_LOCAL_LAX, ErrorMessage = Error_Message_RootPathName_Invalid)]
        [Display(Name = DisplayName_RootPathName, Description = "Enter a file system path.")]
        public string RootPathName
        {
            get => _rootPathName;
            set
            {
                Monitor.Enter(this);
                try
                {
                    string rootPathName = value ?? "";
                    if (_rootPathName == rootPathName)
                        return;
                    RaisePropertyValueChanging(nameof(RootPathName), _rootPathName, rootPathName);
                    string oldName = CalculateName(_displayName, _volumeName, _rootPathName);
                    string newName = CalculateName(_displayName, _volumeName, rootPathName);
                    if (!oldName.Equals(newName))
                        RaisePropertyValueChanging(nameof(Name), oldName, newName);
                    PlatformType platformType = Host.GetPlatformType();
                    if (!FileUriConverter.TryFromLocalPathOrUri(value, platformType, false, out FileUri newRootUri))
                    {
                        switch (platformType)
                        {
                            case PlatformType.Windows:
                            case PlatformType.Xbox:
                                platformType = PlatformType.Linux;
                                break;
                            default:
                                platformType = PlatformType.Windows;
                                break;
                        }
                        if (!FileUriConverter.TryFromLocalPathOrUri(value, platformType, false, out newRootUri))
                            newRootUri = new FileUri("");
                    }
                    if (_rootUri.ToString().Equals(newRootUri.ToString()))
                        newRootUri = _rootUri;
                    else
                        RaisePropertyValueChanging(nameof(RootUri), _rootUri, newRootUri);
                    string oldPathName = _rootPathName;
                    _rootPathName = rootPathName;
                    FileUri oldRootUri = _rootUri;
                    _rootUri = newRootUri;
                    try { RaisePropertyValueChanged(nameof(RootUri), oldPathName, _rootPathName); }
                    finally
                    {
                        try
                        {
                            if (!ReferenceEquals(oldRootUri, _rootUri))
                                RaisePropertyValueChanged(nameof(RootUri), oldRootUri, _rootUri);
                        }
                        finally
                        {
                            newName = CalculateName(_displayName, _volumeName, _rootPathName);
                            if (!oldName.Equals(newName))
                                RaisePropertyValueChanged(nameof(Name), oldName, newName);
                        }
                    }
                }
                finally { Monitor.Exit(this); }
            }
        }

        [Required(ErrorMessage = Error_Message_DriveFormat_Empty)]
        [MaxLength(Max_Length_DriveFormat, ErrorMessage = Error_Message_DriveFormat_Length)]
        [Display(Name = DisplayName_DriveFormat)]
        public string DriveFormat
        {
            get { return _driveFormat; }
            set
            {
                Monitor.Enter(this);
                try
                {
                    string newValue = value ?? "";
                    if (_driveFormat.Equals(newValue))
                        return;
                    RaisePropertyValueChanging(nameof(DriveFormat), _driveFormat, newValue);
                    string oldValue = _driveFormat;
                    _driveFormat = newValue;
                    RaisePropertyValueChanged(nameof(DriveFormat), oldValue, _driveFormat);
                }
                finally { Monitor.Exit(this); }
            }
        }

        [Required(ErrorMessage = Error_Message_VolumeName_Empty)]
        [MaxLength(Max_Length_Volume, ErrorMessage = Error_Message_VolumeName_Length)]
        [Display(Name = DisplayName_VolumeName)]
        public string VolumeName
        {
            get { return _volumeName; }
            set
            {
                Monitor.Enter(this);
                try
                {
                    string newValue = value ?? "";
                    if (_volumeName.Equals(newValue))
                        return;
                    RaisePropertyValueChanging(nameof(VolumeName), _volumeName, newValue);
                    string oldName = CalculateName(_displayName, _volumeName, _rootPathName);
                    string newName = CalculateName(_displayName, newValue, _rootPathName);
                    if (!oldName.Equals(newName))
                        RaisePropertyValueChanging(nameof(Name), oldName, newName);
                    string oldValue = _volumeName;
                    _volumeName = newValue;
                    try { RaisePropertyValueChanged(nameof(VolumeName), oldValue, _volumeName); }
                    finally
                    {
                        newName = CalculateName(_displayName, _volumeName, _rootPathName);
                        if (!oldName.Equals(newName))
                            RaisePropertyValueChanged(nameof(Name), oldName, newName);
                    }
                }
                finally { Monitor.Exit(this); }
            }
        }

        public const string PropertyName_Identifier = nameof(Identifier);
        [Required()]
        [Display(Name = "Volume Identifier")]
        public VolumeIdentifier Identifier
        {
            get => _identifier;
            set
            {
                Monitor.Enter(this);
                try
                {
                    if (value.Equals(_identifier))
                        return;
                    RaisePropertyValueChanging(nameof(Identifier), _identifier, value);
                    VolumeIdentifier oldValue = _identifier;
                    _identifier = value;
                    RaisePropertyValueChanged(nameof(Identifier), oldValue, _identifier);
                }
                finally { Monitor.Exit(this); }
            }
        }

        public const string PropertyName_MaxNameLength = nameof(MaxNameLength);
        [Required()]
        [Display(Name = "Max Name Length")]
        public uint MaxNameLength
        {
            get => _maxNameLength;
            set
            {
                Monitor.Enter(this);
                try
                {
                    if (value == _maxNameLength)
                        return;
                    RaisePropertyValueChanging(nameof(MaxNameLength), _maxNameLength, value);
                    uint oldValue = _maxNameLength;
                    _maxNameLength = value;
                    RaisePropertyValueChanged(nameof(MaxNameLength), oldValue, _maxNameLength);
                }
                finally { Monitor.Exit(this); }
            }
        }

        public bool CaseSensitive
        {
            get => _caseSensitive;
            set
            {
                Monitor.Enter(this);
                try
                {
                    if (_caseSensitive == value)
                        return;
                    RaisePropertyValueChanging(nameof(CaseSensitive), _caseSensitive, value);
                    _caseSensitive = value;
                    _segmentNameComparer = null;
                    RaisePropertyValueChanged(nameof(CaseSensitive), !_caseSensitive, _caseSensitive);
                }
                finally { Monitor.Exit(this); }
            }
        }

        [Display(Name = "Is Inactive")]
        public bool IsInactive
        {
            get => _isInactive;
            set
            {
                Monitor.Enter(this);
                try
                {
                    if (_isInactive == value)
                        return;
                    RaisePropertyValueChanging(nameof(IsInactive), _isInactive, value);
                    _isInactive = value;
                    RaisePropertyValueChanged(nameof(IsInactive), !_isInactive, _isInactive);
                }
                finally { Monitor.Exit(this); }
            }
        }

        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string Notes
        {
            get { return _notes; }
            set
            {
                Monitor.Enter(this);
                try
                {
                    string newValue = value ?? "";
                    if (_notes.Equals(newValue))
                        return;
                    RaisePropertyValueChanging(nameof(Notes), _notes, newValue);
                    string oldValue = _notes;
                    _notes = newValue;
                    RaisePropertyValueChanged(nameof(Notes), oldValue, _notes);
                }
                finally { Monitor.Exit(this); }
                _notes = (value is null) ? "" : value;
            }
        }

        [Editable(false)]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn
        {
            get => _createdOn;
            set
            {
                Monitor.Enter(this);
                try
                {
                    if (value.Equals(_createdOn))
                        return;
                    RaisePropertyValueChanging(nameof(CreatedOn), _createdOn, value);
                    DateTime oldValue = _createdOn;
                    _createdOn = value;
                    RaisePropertyValueChanged(nameof(CreatedOn), oldValue, _createdOn);
                }
                finally { Monitor.Exit(this); }
            }
        }

        [Editable(false)]
        [Display(Name = "Created By")]
        public Guid CreatedBy
        {
            get => _createdBy;
            set
            {
                Monitor.Enter(this);
                try
                {
                    if (value.Equals(_createdBy))
                        return;
                    RaisePropertyValueChanging(nameof(CreatedBy), _createdBy, value);
                    bool clearModifier = !(_creator is null || _creator.AccountID.Equals(value));
                    string oldName = CreatorName;
                    if (clearModifier)
                    {
                        RaisePropertyValueChanging(nameof(Creator), _creator, null);
                        if (oldName.Length > 0)
                            RaisePropertyValueChanging(nameof(CreatorName), oldName, "");
                    }
                    Guid oldValue = _createdBy;
                    _createdBy = value;
                    Account oldModifier = _creator;
                    if (clearModifier)
                        _creator = null;
                    RaisePropertyValueChanged(nameof(CreatedBy), oldValue, _createdBy);
                    if (clearModifier)
                    {
                        RaisePropertyValueChanging(nameof(Creator), oldModifier, _creator);
                        if (oldName.Length > 0)
                            RaisePropertyValueChanging(nameof(CreatorName), oldName, "");
                    }
                }
                finally { Monitor.Exit(this); }
            }
        }

        public Account Creator
        {
            get => _creator;
            set
            {
                Monitor.Enter(this);
                try
                {
                    if ((value is null) ? _creator is null : ReferenceEquals(value, _creator))
                        return;
                    string oldName = CreatorName;
                    string newName = (value is null) ? "" : value.Name;
                    RaisePropertyValueChanging(nameof(Creator), _creator, value);
                    if (!((oldName is null) ? newName is null : oldName.Equals(newName)))
                        RaisePropertyValueChanging(nameof(CreatorName), oldName, newName);
                    Guid newId = (value is null) ? Guid.Empty : value.AccountID;
                    if (!_createdBy.Equals(newId))
                        RaisePropertyValueChanging(nameof(CreatedBy), _createdBy, newId);
                    Account oldValue = _creator;
                    _creator = value;
                    Guid oldId = _createdBy;
                    _createdBy = newId;
                    try { RaisePropertyValueChanged(nameof(Creator), oldValue, _creator); }
                    finally
                    {
                        try
                        {
                            newName = (value is null) ? "" : value.Name;
                            if (!((oldName is null) ? newName is null : oldName.Equals(newName)))
                                RaisePropertyValueChanged(nameof(CreatorName), oldName, newName);
                        }
                        finally
                        {
                            if (!_createdBy.Equals(oldId))
                                RaisePropertyValueChanged(nameof(CreatedBy), oldId, _createdBy);
                        }
                    }
                }
                finally { Monitor.Exit(this); }
            }
        }

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
        public DateTime ModifiedOn
        {
            get => _modifiedOn;
            set
            {
                Monitor.Enter(this);
                try
                {
                    if (value.Equals(_modifiedOn))
                        return;
                    RaisePropertyValueChanging(nameof(ModifiedOn), _modifiedOn, value);
                    DateTime oldValue = _modifiedOn;
                    _modifiedOn = value;
                    RaisePropertyValueChanged(nameof(ModifiedOn), oldValue, _modifiedOn);
                }
                finally { Monitor.Exit(this); }
            }
        }

        [Editable(false)]
        [Display(Name = "Modified By")]
        public Guid ModifiedBy
        {
            get => _modifiedBy;
            set
            {
                Monitor.Enter(this);
                try
                {
                    if (value.Equals(_modifiedBy))
                        return;
                    RaisePropertyValueChanging(nameof(ModifiedBy), _modifiedBy, value);
                    bool clearModifier = !(_modifier is null || _modifier.AccountID.Equals(value));
                    string oldName = ModifierName;
                    if (clearModifier)
                    {
                        RaisePropertyValueChanging(nameof(Modifier), _modifier, null);
                        if (oldName.Length > 0)
                            RaisePropertyValueChanging(nameof(Modifier), oldName, "");
                    }
                    Guid oldValue = _modifiedBy;
                    _modifiedBy = value;
                    Account oldModifier = _modifier;
                    if (clearModifier)
                        _modifier = null;
                    RaisePropertyValueChanged(nameof(ModifiedBy), oldValue, _modifiedBy);
                    if (clearModifier)
                    {
                        RaisePropertyValueChanging(nameof(Modifier), oldModifier, _modifier);
                        if (oldName.Length > 0)
                            RaisePropertyValueChanging(nameof(ModifierName), oldName, "");
                    }
                }
                finally { Monitor.Exit(this); }
            }
        }

        public Account Modifier
        {
            get => _modifier;
            set
            {
                Monitor.Enter(this);
                try
                {
                    if ((value is null) ? _modifier is null : ReferenceEquals(value, _modifier))
                        return;
                    string oldName = ModifierName;
                    string newName = (value is null) ? "" : value.Name;
                    RaisePropertyValueChanging(nameof(Modifier), _modifier, value);
                    if (!((oldName is null) ? newName is null : oldName.Equals(newName)))
                        RaisePropertyValueChanging(nameof(ModifierName), oldName, newName);
                    Guid newId = (value is null) ? Guid.Empty : value.AccountID;
                    if (!_modifiedBy.Equals(newId))
                        RaisePropertyValueChanging(nameof(ModifiedBy), _modifiedBy, newId);
                    Account oldValue = _modifier;
                    _modifier = value;
                    Guid oldId = _modifiedBy;
                    _modifiedBy = newId;
                    try { RaisePropertyValueChanged(nameof(Modifier), oldValue, _modifier); }
                    finally
                    {
                        try
                        {
                            newName = (value is null) ? "" : value.Name;
                            if (!((oldName is null) ? newName is null : oldName.Equals(newName)))
                                RaisePropertyValueChanged(nameof(ModifierName), oldName, newName);
                        }
                        finally
                        {
                            if (!_modifiedBy.Equals(oldId))
                                RaisePropertyValueChanged(nameof(ModifiedBy), oldId, _modifiedBy);
                        }
                    }
                }
                finally { Monitor.Exit(this); }
            }
        }

        public string ModifierName
        {
            get
            {
                Account account = Modifier;
                return (account is null) ? "" : account.Name;
            }
        }

        [Obsolete("Use GetPathComparer()")]
        public IEqualityComparer<string> PathComparer
        {
            get
            {
                StringComparer comparer = _segmentNameComparer;
                if (comparer is null)
                    _segmentNameComparer = comparer = (_caseSensitive) ? ComponentHelper.CASE_SENSITIVE_COMPARER : ComponentHelper.IGNORE_CASE_COMPARER;
                return comparer;
            }
        }

        public IEqualityComparer<string> GetPathComparer()
        {
            StringComparer comparer = _segmentNameComparer;
            if (comparer is null)
                _segmentNameComparer = comparer = (_caseSensitive) ? ComponentHelper.CASE_SENSITIVE_COMPARER : ComponentHelper.IGNORE_CASE_COMPARER;
            return comparer;
        }

        IHostDevice IVolume.Host { get => Host; set => Host = (HostDevice)value; }

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
                throw new ArgumentNullException(nameof(createdBy));
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
            _driveFormat = _driveFormat.Trim();
            _volumeName = _volumeName.Trim();
            if ((_displayName = _displayName.CoerceAsWsNormalized()).Length == 0)
                _displayName = _volumeName;
            _notes = _notes.Trim();
            CreatedOn = CreatedOn.CoerceAsLocalTime();
            ModifiedOn = ModifiedOn.CoerceAsLocalTime();
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
                    if (_rootUri.IsEmpty())
                        result.Add(new ValidationResult(Error_Message_RootPathName_Empty, new string[] { PropertyName_RootPathName }));
                    else if (_rootUri.ToLocalPath().Length > Max_Length_DisplayName)
                        result.Add(new ValidationResult(Error_Message_RootPathName_Length, new string[] { PropertyName_RootPathName }));
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

        private void RaisePropertyValueChanging<T>(string propertyName, T oldValue, T newValue)
        {
            PropertyValueChangingEventArgs<T> args = new PropertyValueChangingEventArgs<T>(propertyName, oldValue, newValue);
            PropertyValueChanging?.Invoke(this, args);
            PropertyChanging?.Invoke(this, args);
        }

        private void RaisePropertyValueChanged<T>(string propertyName, T oldValue, T newValue)
        {
            PropertyValueChangedEventArgs<T> args = new PropertyValueChangedEventArgs<T>(propertyName, oldValue, newValue);
            try { PropertyValueChanged?.Invoke(this, args); }
            finally { PropertyChanged?.Invoke(this, args); }
        }

        private static string CalculateName(string displayName, string volumeName, string rootPathName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                return (string.IsNullOrWhiteSpace(volumeName)) ? rootPathName : volumeName;
            return displayName;
        }
    }
}
