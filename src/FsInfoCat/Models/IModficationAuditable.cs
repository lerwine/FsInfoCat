using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public interface IModficationAuditable : IValidatableModel
    {
        [Editable(false)]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        DateTime CreatedOn { get; set; }

        [Editable(false)]
        Guid CreatedBy { get; set; }

        [Editable(false)]
        [Display(Name = "Modified On")]
        [DataType(DataType.DateTime)]
        DateTime ModifiedOn { get; set; }

        [Editable(false)]
        Guid ModifiedBy { get; set; }
    }
}
