using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public interface IDbEntity : IValidatableObject, IRevertibleChangeTracking
    {
        DateTime CreatedOn { get; set; }

        DateTime ModifiedOn { get; set; }
    }
}
