using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsInfoCat.Models.DB
{
    public class HostContributor
    {
        [Required()]
        [Display(Name = "User ID")]
        public Guid AccountID { get; set; }

        [Required()]
        [Display(Name = "Host ID")]
        public Guid HostID { get; set; }
    }
}
