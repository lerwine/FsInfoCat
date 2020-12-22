using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public class MediaHostNameRef
    {
        public string MachineName { get; set; }
        public bool IsWindows { get; set; }
    }
}
