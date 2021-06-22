using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUserGroup : IUpstreamDbEntity
    {
        string Name { get; set; }

        UserRole Roles { get; set; }

        string Notes { get; set; }

        bool IsInactive { get; set; }

        IEnumerable<IGroupMembership> Members { get; }

        IEnumerable<IMitigationTask> Tasks { get; }
    }
}
