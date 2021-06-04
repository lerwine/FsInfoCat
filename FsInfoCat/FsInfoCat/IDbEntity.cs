using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public interface IDbEntity : IValidatableObject, IRevertibleChangeTracking
    {
        DateTime CreatedOn { get; set; }

        DateTime ModifiedOn { get; set; }

        [Obsolete("Probaby need to use change tracking, instead")]
        bool IsNew();

        [Obsolete("Probaby need to include change tracking, instead")]
        bool IsSameDbRow(IDbEntity other);
    }
}
