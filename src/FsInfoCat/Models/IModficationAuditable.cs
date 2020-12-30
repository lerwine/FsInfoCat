using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public interface IModficationAuditable : IValidatableModel
    {
        DateTime CreatedOn { get; set; }

        Guid CreatedBy { get; set; }

        DateTime ModifiedOn { get; set; }

        Guid ModifiedBy { get; set; }
    }
}
