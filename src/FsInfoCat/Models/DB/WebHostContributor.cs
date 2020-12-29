using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models.DB
{
    public class WebHostContributor
    {
        [Required()]
        [Key()]
        [Display(Name = "User ID")]
        Guid AccountID { get; set; }

        [Required()]
        [Key()]
        [Display(Name = "Host ID")]
        Guid HostID { get; set; }
    }
}
