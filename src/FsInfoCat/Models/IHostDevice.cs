using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public interface IHostDevice : IHostDeviceReg, IModficationAuditable
    {
        bool IsInactive { get; set; }

        string Notes { get; set; }
    }
}
