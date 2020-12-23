using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public class MediaHostRegRequest
    {
        public string DisplayName { get; set; }
        public string MachineName { get; set; }
        public bool IsWindows { get; set; }
    }
}
