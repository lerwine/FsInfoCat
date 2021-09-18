using FsInfoCat.Collections;
using System;

namespace FsInfoCat.Upstream
{
    public interface IGroupMemberListItem : IGroupMembershipRow
    {
        Guid UserId { get; }

        Guid GroupId { get; }

        string DisplayName { get; }

        string FirstName { get; }

        string LastName { get; }

        int? DbPrincipalId { get; }

        ByteValues SID { get; }
    }
}
