using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public interface IAppUser : ILogin, IModficationAuditable
    {
        Guid AccountID { get; set; }

        string DisplayName { get; set; }

        /// <summary>
        /// Gets the role of the user.
        /// </summary>
        UserRole Role { get; set; }

        string Notes { get; set; }
    }
}
