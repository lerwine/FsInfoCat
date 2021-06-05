using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public interface IDbEntity : IValidatableObject, IRevertibleChangeTracking
    {
        DateTime CreatedOn { get; set; }

        DateTime ModifiedOn { get; set; }

        // TODO: Deprecate IsNew(), use change tracking, instead
        [Obsolete("Probaby need to use change tracking, instead")]
        bool IsNew();

        // TODO: Deprecate IsSameDbRow(IDbEntity), use change tracking, instead
        [Obsolete("Probaby need to include change tracking, instead")]
        bool IsSameDbRow(IDbEntity other);
    }
}
