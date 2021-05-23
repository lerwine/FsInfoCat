using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public interface IDbEntity : IValidatableObject
    {
        DateTime CreatedOn { get; set; }

        DateTime ModifiedOn { get; set; }

        [Obsolete("Probaby need to use change tracking, instead")]
        bool IsNew();

        [Obsolete("Probaby need to include change tracking, instead")]
        bool IsSameDbRow(IDbEntity other);
    }
}
