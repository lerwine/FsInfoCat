using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public interface IDbEntity : IValidatableObject
    {
        Guid Id { get; set; }

        DateTime CreatedOn { get; set; }

        DateTime ModifiedOn { get; set; }

        bool IsNew();
    }
}
