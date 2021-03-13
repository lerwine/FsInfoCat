using FsInfoCat.Models.HostDevices;
using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models.DB
{
    [DisplayColumn("HostDeviceID", "DisplayName", false)]
    public class HostDevice : IHostDevice
    {
        #region Properties

        public string Name
        {
            get
            {
                string n = _displayName;
                if (string.IsNullOrWhiteSpace(n))
                {
                    n = _machineName;
                    if (string.IsNullOrWhiteSpace(n))
                        n = _machineIdentifer;
                }
                return n;
            }
        }

        [Required()]
        [Key()]
        [Display(Name = "ID")]
        public Guid HostDeviceID { get; set; }

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

        #region MachineIdentifer

        public const string DisplayName_MachineIdentifer = "Machine Unique Identifier";
        public const string PropertyName_MachineIdentifer = "MachineIdentifer";
        public const int Max_Length_MachineIdentifer = 128;
        public const string Error_Message_MachineIdentifer = "Machine identifier too long.";
        private string _machineIdentifer = "";

        [MaxLength(Max_Length_MachineIdentifer, ErrorMessage = Error_Message_MachineIdentifer)]
        [Display(Name = DisplayName_MachineIdentifer)]
        public string MachineIdentifer
        {
            get { return _machineIdentifer; }
            set { _machineIdentifer = (value is null) ? "" : value; }
        }

        #endregion

        #region MachineName

        public const string DisplayName_MachineName = "Machine name";
        public const string PropertyName_MachineName = "MachineName";
        public const int Max_Length_MachineName = 128;
        public const string Error_Message_MachineName_Empty = "Machine name cannot be empty";
        public const string Error_Message_MachineName_Length = "Machine name too long.";
        public const string Error_Message_MachineName_Invalid = "Invalid machine name";
        private string _machineName = "";

        [Required()]
        [MinLength(1)]
        [MaxLength(Max_Length_MachineName, ErrorMessage = Error_Message_MachineName_Length)]
        [Display(Name = DisplayName_MachineName)]
        [RegularExpression(FileUriFactory.PATTERN_HOST_NAME, ErrorMessage = Error_Message_MachineName_Invalid)]
        public string MachineName
        {
            get { return _machineName; }
            set { _machineName = (value is null) ? "" : value; }
        }

        #endregion

        #region Platform

        public const string DisplayName_Platform = "Host OS Platform Type";
        private PlatformType _platform;

        [Display(Name = DisplayName_Platform)]
        public PlatformType Platform { get => _platform; set => _platform = ModelHelper.ToPlatformType((byte)value); }

        #endregion

        [Display(Name = "Allow Local Crawl")]
        public bool AllowCrawl { get; set; }

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

        #endregion

        #endregion

        #region Constructors

        public HostDevice()
        {
            CreatedOn = ModifiedOn = DateTime.UtcNow;
        }

        protected HostDevice(HostDevice host)
        {
            if (host is null)
                throw new ArgumentNullException(nameof(host));
            HostDeviceID = host.HostDeviceID;
            DisplayName = host.DisplayName;
            MachineIdentifer = host.MachineIdentifer;
            MachineName = host.MachineName;
            AllowCrawl = host.AllowCrawl;
            Platform = host.Platform;
            CreatedOn = host.CreatedOn;
            CreatedBy = host.CreatedBy;
            ModifiedOn = host.ModifiedOn;
            ModifiedBy = host.ModifiedBy;
        }

        public HostDevice(HostDeviceRegRequest request, Guid createdBy) : this()
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));
            HostDeviceID = Guid.NewGuid();
            MachineIdentifer = request.MachineIdentifer;
            MachineName = request.MachineName;
            AllowCrawl = request.AllowCrawl;
            Platform = request.Platform;
            CreatedBy = ModifiedBy = createdBy;
        }

        public HostDevice(HostDeviceRegRequest request, Account createdBy) : this()
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));
            HostDeviceID = Guid.NewGuid();
            MachineIdentifer = request.MachineIdentifer;
            MachineName = request.MachineName;
            AllowCrawl = request.AllowCrawl;
            Platform = request.Platform;
            Creator = Modifier = createdBy;
            CreatedBy = ModifiedBy = createdBy.AccountID;
        }

        #endregion

        public void Normalize()
        {
            if (HostDeviceID.Equals(Guid.Empty))
                HostDeviceID = Guid.NewGuid();
            _machineIdentifer = _machineIdentifer.CoerceAsWsNormalized();
            _machineName = _machineName.Trim();
            if ((_displayName = _displayName.CoerceAsWsNormalized()).Length == 0)
                _displayName = _machineName;
            _notes = _notes.Trim();
            CreatedOn = CreatedOn.CoerceAsLocalTime();
            ModifiedOn = ModifiedOn.CoerceAsLocalTime();
            if (null != Creator)
                CreatedBy = Creator.AccountID;
            if (null != Modifier)
                ModifiedBy = Modifier.AccountID;
        }

        public IList<ValidationResult> ValidateAll()
        {
            List<ValidationResult> result = new List<ValidationResult>();
            Normalize();
            OnValidateAll(result);
            return result;
        }

        private void OnValidateAll(List<ValidationResult> result)
        {
            Validate(result, PropertyName_DisplayName);
            Validate(result, PropertyName_MachineIdentifer);
            Validate(result, PropertyName_MachineName);
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
                case PropertyName_MachineIdentifer:
                case DisplayName_MachineIdentifer:
                    if (_machineIdentifer.Length > Max_Length_DisplayName)
                        result.Add(new ValidationResult(Error_Message_MachineIdentifer, new string[] { PropertyName_MachineIdentifer }));
                    break;
                case PropertyName_MachineName:
                case DisplayName_MachineName:
                    if (_machineName.Length == 0)
                        result.Add(new ValidationResult(Error_Message_MachineName_Empty, new string[] { PropertyName_MachineName }));
                    else if (_machineName.Length > Max_Length_DisplayName)
                        result.Add(new ValidationResult(Error_Message_MachineName_Length, new string[] { PropertyName_MachineName }));
                    else if (!FileUriFactory.HOST_NAME_REGEX.IsMatch(_machineName))
                        result.Add(new ValidationResult(Error_Message_MachineName_Invalid, new string[] { PropertyName_MachineName }));
                    break;
            }
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
