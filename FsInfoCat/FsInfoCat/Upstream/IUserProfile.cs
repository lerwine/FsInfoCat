using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUserProfile : IUpstreamDbEntity
    {
        string DisplayName { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string MI { get; set; }

        string Suffix { get; set; }

        string Title { get; set; }

        int? DbPrincipalId { get; set; }

        IList<byte> SID { get; set; }

        string LoginName { get; set; }

        UserRole ExplicitRoles { get; set; }

        string Notes { get; set; }

        bool IsInactive { get; set; }

        IEnumerable<IGroupMembership> MemberOf { get; }

        IEnumerable<IMitigationTask> Tasks { get; }
    }
}
