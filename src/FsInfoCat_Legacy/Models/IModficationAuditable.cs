using FsInfoCat.Models.DB;
using System;

namespace FsInfoCat.Models
{
    public interface IModficationAuditable : IValidatableModel
    {
        DateTime CreatedOn { get; set; }

        Guid CreatedBy { get; set; }

        Account Creator { get; set; }

        string CreatorName { get; }

        DateTime ModifiedOn { get; set; }

        Guid ModifiedBy { get; set; }

        Account Modifier { get; set; }

        string ModifierName { get; }
    }
}
