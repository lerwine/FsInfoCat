using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public interface IHostDevice : IHostDeviceReg, IModficationAuditable
    {
        [Display(Name = "Is Inactive")]
        bool IsInactive { get; set; }

        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        string Notes { get; set; }
    }
}
