using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#if WINDOWS
using System.DirectoryServices;
using System.Security.Principal;
#else
using System.IO;
#endif
using FsInfoCat.Models.DB;

namespace FsInfoCat.Models.HostDevices
{
    public class HostDeviceRegRequest : IHostDeviceReg
    {
        private string _displayName = "";
        private string _machineIdentifer = "";
        private string _machineName = "";

#if CORE
        [MaxLength(DB.HostDevice.Max_Length_DisplayName, ErrorMessage = DB.HostDevice.Error_Message_DisplayName)]
        [Display(Name = DB.HostDevice.DisplayName_DisplayName)]
#endif
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (null == value) ? "" : value; }
        }

#if CORE
        [MaxLength(DB.HostDevice.Max_Length_MachineIdentifer, ErrorMessage = DB.HostDevice.Error_Message_MachineIdentifer)]
        [Display(Name = DB.HostDevice.DisplayName_MachineIdentifer)]
#endif
        public string MachineIdentifer
        {
            get { return _machineIdentifer; }
            set { _machineIdentifer = (null == value) ? "" : value; }
        }

#if CORE
        [Required()]
        [MinLength(1)]
        [MaxLength(DB.HostDevice.Max_Length_MachineName, ErrorMessage = DB.HostDevice.Error_Message_MachineName_Length)]
        [Display(Name = DB.HostDevice.DisplayName_MachineName)]
        [RegularExpression(ModelHelper.PATTERN_MACHINE_NAME, ErrorMessage = DB.HostDevice.Error_Message_MachineName_Invalid)]
#endif
        public string MachineName
        {
            get { return _machineName; }
            set { _machineName = (null == value) ? "" : value; }
        }

#if CORE
        [Display(Name = "Is Windows OS")]
#endif
        public bool IsWindows { get; set; }

#if CORE
        [Display(Name = "Allow Local Crawl")]
#endif
        public bool AllowCrawl { get; set; }

        Guid IHostDeviceReg.HostDeviceID { get => Guid.Empty; set => throw new NotSupportedException(); }

        public void Normalize()
        {
            _machineIdentifer = ModelHelper.CoerceAsWsNormalized(_machineIdentifer);
            _machineName = _machineName.Trim();
            if ((_displayName = ModelHelper.CoerceAsWsNormalized(_displayName)).Length == 0)
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
                    else if (!ModelHelper.MachineNameRegex.IsMatch(_machineName))
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

        public static string GetLocalMachineIdentifier()
        {
#if WINDOWS
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                throw new NotSupportedException("Platform " + Environment.OSVersion.Platform.ToString("F") + " is not supported.");
            try
            {
                using (DirectoryEntry directoryEntry = new DirectoryEntry("WinNT://" + Environment.MachineName + "/Administrator"))
                    return new SecurityIdentifier((byte[])directoryEntry.InvokeGet("objectSID"), 0).AccountDomainSid.ToString();
            }
            catch (Exception exc)
            {
                if (string.IsNullOrWhiteSpace(exc.Message))
                    throw;
                throw new Exception("Encountered an exception while trying to retrieve computer SID - " + exc.Message, exc);
            }
#elif LINUX
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                throw new NotSupportedException("Platform " + Environment.OSVersion.Platform.ToString("F") + " is not supported.");
            try
            {
                string s = File.ReadAllText("/etc/machine-id");
                return (Guid.TryParse(s.Trim(), out Guid id)) ? id.ToString("d") : id;
            }
            catch (Exception exc)
            {
                if (string.IsNullOrWhiteSpace(exc.Message))
                    throw;
                throw new Exception("Encountered an exception while trying to retrieve computer GUID - " + exc.Message, exc);
            }
#else
            throw new NotSupportedException("CreateForLocal is not supported.");
#endif
        }
        public static HostDeviceRegRequest CreateForLocal()
        {
            return new HostDeviceRegRequest
            {
#if WINDOWS
                IsWindows = true,
#else
                IsWindows = false,
#endif
                MachineName = Environment.MachineName,
                MachineIdentifer = GetLocalMachineIdentifier()
            };
        }
    }
}
