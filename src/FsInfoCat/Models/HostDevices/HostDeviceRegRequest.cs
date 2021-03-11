using FsInfoCat.Models.DB;
using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models.HostDevices
{
    public class HostDeviceRegRequest : IHostDeviceReg
    {
        private string _displayName = "";
        private string _machineIdentifer = "";
        private string _machineName = "";
        private PlatformType _platform;

        [MaxLength(DB.HostDevice.Max_Length_DisplayName, ErrorMessage = DB.HostDevice.Error_Message_DisplayName)]
        [Display(Name = DB.HostDevice.DisplayName_DisplayName)]
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (value is null) ? "" : value; }
        }

        [MaxLength(DB.HostDevice.Max_Length_MachineIdentifer, ErrorMessage = DB.HostDevice.Error_Message_MachineIdentifer)]
        [Display(Name = DB.HostDevice.DisplayName_MachineIdentifer)]
        public string MachineIdentifer
        {
            get { return _machineIdentifer; }
            set { _machineIdentifer = (value is null) ? "" : value; }
        }

        [Required()]
        [MinLength(1)]
        [MaxLength(DB.HostDevice.Max_Length_MachineName, ErrorMessage = DB.HostDevice.Error_Message_MachineName_Length)]
        [Display(Name = DB.HostDevice.DisplayName_MachineName)]
        [RegularExpression(UriHelper.PATTERN_DNS_NAME, ErrorMessage = DB.HostDevice.Error_Message_MachineName_Invalid)]
        public string MachineName
        {
            get { return _machineName; }
            set { _machineName = (value is null) ? "" : value; }
        }

        [Display(Name = "Host OS Platform Type")]
        public PlatformType Platform { get => _platform; set => _platform = ModelHelper.ToPlatformType((byte)value); }

        [Display(Name = "Allow Local Crawl")]
        public bool AllowCrawl { get; set; }

        Guid IHostDeviceReg.HostDeviceID { get => Guid.Empty; set => throw new NotSupportedException(); }

        public void Normalize()
        {
            _machineIdentifer = _machineIdentifer.CoerceAsWsNormalized();
            _machineName = _machineName.Trim();
            if ((_displayName = _displayName.CoerceAsWsNormalized()).Length == 0)
                _displayName = _machineName;
        }

        private void OnValidateAll(List<ValidationResult> result)
        {
            Validate(result, HostDevice.PropertyName_DisplayName);
            Validate(result, HostDevice.PropertyName_MachineIdentifer);
            Validate(result, HostDevice.PropertyName_MachineName);
        }

        private void Validate(List<ValidationResult> result, string propertyName)
        {
            switch (propertyName)
            {
                case HostDevice.PropertyName_MachineIdentifer:
                case HostDevice.DisplayName_MachineIdentifer:
                    if (_machineIdentifer.Length > HostDevice.Max_Length_DisplayName)
                        result.Add(new ValidationResult(HostDevice.Error_Message_MachineIdentifer, new string[] { HostDevice.PropertyName_MachineIdentifer }));
                    break;
                case HostDevice.PropertyName_MachineName:
                case HostDevice.DisplayName_MachineName:
                    if (_machineName.Length == 0)
                        result.Add(new ValidationResult(HostDevice.Error_Message_MachineName_Empty, new string[] { HostDevice.PropertyName_MachineName }));
                    else if (_machineName.Length > HostDevice.Max_Length_DisplayName)
                        result.Add(new ValidationResult(HostDevice.Error_Message_MachineName_Length, new string[] { HostDevice.PropertyName_MachineName }));
                    else if (!UriHelper.HOST_NAME_REGEX.IsMatch(_machineName))
                        result.Add(new ValidationResult(HostDevice.Error_Message_MachineName_Invalid, new string[] { HostDevice.PropertyName_MachineName }));
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
