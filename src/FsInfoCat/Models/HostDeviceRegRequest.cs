using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public class HostDeviceRegRequest
    {
        [MaxLength(256)]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Required()]
        [MinLength(1)]
        [MaxLength(256)]
        [Display(Name = "Machine Name")]
        [RegularExpression(HostDevice.PATTERN_MACHINE_NAME, ErrorMessage = "Invalid host name")]
        public string MachineName { get; set; }

        [Display(Name = "Is Windows OS")]
        public bool IsWindows { get; set; }
    }
}
