using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public interface IDbEntity : IValidatableObject, IRevertibleChangeTracking
    {
        DateTime CreatedOn { get; set; }

        DateTime ModifiedOn { get; set; }

        void BeforeSave(ValidationContext validationContext);
    }
}
