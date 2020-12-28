using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public interface IHostDeviceReg
    {
        [Required()]
        [Key()]
        [Display(Name = "ID")]
        Guid HostID { get; set; }

        [MaxLength(DB.HostDevice.Max_Length_DisplayName, ErrorMessage = DB.HostDevice.Error_Message_DisplayName)]
        [Display(Name = DB.HostDevice.DisplayName_DisplayName)]
        string DisplayName { get; set; }

        // TODO: Add this to db script and view
        [MaxLength(DB.HostDevice.Max_Length_MachineIdentifer, ErrorMessage = DB.HostDevice.Error_Message_MachineIdentifer)]
        [Display(Name = DB.HostDevice.DisplayName_MachineIdentifer)]
        string MachineIdentifer { get; set; }

        [Required()]
        [MinLength(1)]
        [MaxLength(DB.HostDevice.Max_Length_MachineName, ErrorMessage = DB.HostDevice.Error_Message_MachineName_Length)]
        [Display(Name = DB.HostDevice.DisplayName_MachineName)]
        [RegularExpression(ModelHelper.PATTERN_MACHINE_NAME, ErrorMessage = DB.HostDevice.Error_Message_MachineName_Invalid)]
        string MachineName { get; set; }

        [Display(Name = "Is Windows OS")]
        bool IsWindows { get; set; }
    }
}
