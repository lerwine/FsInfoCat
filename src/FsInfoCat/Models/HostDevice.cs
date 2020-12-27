using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace FsInfoCat.Models
{
    [DisplayColumn("HostID", "DisplayName", false)]
    public class HostDevice : IModficationAuditable
    {
        #region Properties

        [Required()]
        [Key()]
        [Display(Name = "ID")]
        public Guid HostID { get; set; }

        #region DisplayName

        public const int Max_Length_DisplayName = 256;
        public const string DisplayName_DisplayName = "Display name";
        public const string PropertyName_DisplayName = "DisplayName";
        public const string Error_Message_DisplayName = "Display name too long.";
        private string _displayName = "";

        [MaxLength(Max_Length_DisplayName, ErrorMessage = Error_Message_DisplayName)]
        [Display(Name = DisplayName_DisplayName)]
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (null == value) ? "" : value; }
        }

        #endregion

        #region MachineIdentifer

        public const string DisplayName_MachineIdentifer = "Machine Unique Identifier";
        public const string PropertyName_MachineIdentifer = "MachineIdentifer";
        public const int Max_Length_MachineIdentifer = 256;
        public const string Error_Message_MachineIdentifer = "Machine identifier too long.";
        private string _machineIdentifer = "";

        [MaxLength(Max_Length_MachineIdentifer, ErrorMessage = Error_Message_MachineIdentifer)]
        [Display(Name = DisplayName_MachineIdentifer)]
        public string MachineIdentifer
        {
            get { return _machineIdentifer; }
            set { _machineIdentifer = (null == value) ? "" : value; }
        }

        #endregion

        #region MachineName

        public const string DisplayName_MachineName = "Machine name";
        public const string PropertyName_MachineName = "MachineName";
        public const int Max_Length_MachineName = 256;
        public const string Error_Message_MachineName_Empty = "Machine name cannot be empty";
        public const string Error_Message_MachineName_Length = "Machine name too long.";
        public const string Error_Message_MachineName_Invalid = "Invalid machine name";
        private string _machineName = "";

        [Required()]
        [MinLength(1)]
        [MaxLength(Max_Length_MachineName, ErrorMessage = Error_Message_MachineName_Length)]
        [Display(Name = DisplayName_MachineName)]
        [RegularExpression(ModelHelper.PATTERN_MACHINE_NAME, ErrorMessage = Error_Message_MachineName_Invalid)]
        public string MachineName
        {
            get { return _machineName; }
            set { _machineName = (null == value) ? "" : value; }
        }

        #endregion

        [Display(Name = "Is Windows")]
        public bool IsWindows { get; set; }

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
            set { _notes = (null == value) ? "" : value; }
        }

        #endregion

        public ICollection<Volume> Volumes { get; set; }

        #region Audit

        public DateTime CreatedOn { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Guid ModifiedBy { get; set; }

        #endregion

        #endregion

        #region Constructors

        public HostDevice()
        {
            CreatedOn = ModifiedOn = DateTime.UtcNow;
        }

        public HostDevice(string machineIdentifer, string machineName, bool isWindows, Guid createdBy) : this()
        {
            HostID = Guid.NewGuid();
            MachineIdentifer = machineIdentifer;
            MachineName = machineName;
            IsWindows = isWindows;
            CreatedBy = ModifiedBy = createdBy;
        }

        public HostDevice(HostDeviceRegRequest request, Guid createdBy) : this()
        {
            if (null == request)
                throw new ArgumentNullException("request");
            HostID = Guid.NewGuid();
            MachineIdentifer = request.MachineIdentifer;
            MachineName = request.MachineName;
            IsWindows = request.IsWindows;
            CreatedBy = ModifiedBy = createdBy;
        }

        #endregion

        public void Normalize()
        {
            if (HostID.Equals(Guid.Empty))
                HostID = Guid.NewGuid();
            _machineIdentifer = ModelHelper.CoerceAsWsNormalized(_machineIdentifer);
            _machineName = _machineName.Trim();
            if ((_displayName = ModelHelper.CoerceAsWsNormalized(_displayName)).Length == 0)
                _displayName = _machineName;
            _notes = _notes.Trim();
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
                    else if (!ModelHelper.MachineNameRegex.IsMatch(_machineName))
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
