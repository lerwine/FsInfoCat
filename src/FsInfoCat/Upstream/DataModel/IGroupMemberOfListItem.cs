using System;

namespace FsInfoCat.Upstream
{
    public interface IGroupMemberOfListItem : IGroupMembershipRow
    {
        Guid UserId { get; }

        Guid GroupId { get; }

        string Name { get; }

        UserRole Roles { get; }

        bool IsInactive { get; }
    }
}
